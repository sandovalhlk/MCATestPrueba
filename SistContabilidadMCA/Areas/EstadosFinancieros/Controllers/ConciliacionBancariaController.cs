using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicMCA;
using DataLayerMCA;

namespace SistContabilidadMCA.Areas.EstadosFinancieros.Controllers
{
    [Authorize]
    public class ConciliacionBancariaController : Controller
    {
        #region Instancias
        private ModulosMCABL repoModulosMCA = new ModulosMCABL();
        private UcrsBL repoUcrs = new UcrsBL();
        private CuentasBL repoCuentas = new CuentasBL();
        private BancoBL repoBanco = new BancoBL();
        private ComprobantesBL repoComprobantes = new ComprobantesBL();
        private ComprobanteCuentasBL repoComprobantesCuentas = new ComprobanteCuentasBL();
        private ConciliacionBancariasBL repoConciliacion = new ConciliacionBancariasBL();
        private DetalleConciliacionBancariasBL repoConciliacionDetalle = new DetalleConciliacionBancariasBL();
        UsuarioModulosMCABL repoUsuarioModulosMCA = new UsuarioModulosMCABL();
        private UsuarioModulosMCA UsermoduloMCA;
        UsuariosBL repousuarios = new UsuariosBL();
        private Usuarios UsuarioUcr;
        #endregion

        #region HTTP

        public ActionResult Index()

        {

            //int usuarioId = Session["usuarioMCAId"] == null ? 0 : ((int)Session["usuarioMCAId"]);
            //UsermoduloMCA = repoUsuarioModulosMCA.Get(filter: x => x.usuarioId == usuarioId && x.estadoId == EstadosBL.KEY_ACTIVO).FirstOrDefault();
            int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);

            if (!(moduloMCAId > 0))//Validacion de existencia de MCA seleccionado
                {
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
                //int moduloMCAId = UsermoduloMCA.moduloMCAId;
                //var cuentas = repoCuentas.Get(filter: x => x.moduloMCAId == moduloMCAId && x.estadoId == EstadosBL.KEY_ACTIVO).FirstOrDefault();
                //ViewBag.numCuenta = cuentas.descripcion;
                return View();
        }

        public ActionResult Conciliacion(int cuentaId)
        {
            int usuarioId = Session["usuarioMCAId"] == null ? 0 : ((int)Session["usuarioMCAId"]);

            //UsermoduloMCA = repoUsuarioModulosMCA.Get(filter: x => x.usuarioId == usuarioId && x.estadoId == EstadosBL.KEY_ACTIVO).FirstOrDefault();
            //int moduloMCAId = UsermoduloMCA.moduloMCAId;
            //var tomodulo = repoModulosMCA.Get(filter: x => x.moduloMCAId == moduloMCAId).Select(x => new { x.nombreModuloMCA }).FirstOrDefault();
            //bool existeCuenta = ConciliacionBancariasBL.ExisteCuenta(moduloMCAId);
            //var tocuentas = repoCuentas.Get(filter: x => x.moduloMCAId == moduloMCAId && x.Cuentabanco == true && x.estadoId == EstadosBL.KEY_ACTIVO && x.cuentaId == cuentaId).Select(x => new { x.descripcion, x.bancoId, x.cuentaId }).FirstOrDefault();
            var tocuentas = repoCuentas.Get(filter: x => x.Cuentabanco == true && x.estadoId == EstadosBL.KEY_ACTIVO && x.cuentaId == cuentaId).Select(x => new { x.descripcion, x.bancoId, x.cuentaId, x.ModulosMCA.nombreModuloMCA }).FirstOrDefault();
            ViewBag.modulo = tocuentas.nombreModuloMCA;
            ViewBag.cuentaId = tocuentas.cuentaId;
            ViewBag.numCuenta = tocuentas.descripcion;



            return View();
        }

        [HttpGet]
        public ActionResult EditConciliacion(int conciliacionBancariaId)
        {    
            int usuarioId = Session["usuarioMCAId"] == null ? 0 : ((int)Session["usuarioMCAId"]);
            UsermoduloMCA = repoUsuarioModulosMCA.Get(filter: x => x.usuarioId == usuarioId && x.estadoId == EstadosBL.KEY_ACTIVO).FirstOrDefault();
            var toConciliacion = repoConciliacion.GetByID(conciliacionBancariaId);
            ViewBag.conciliacionBanId = toConciliacion.conciliacionBancariaId;
            int moduloMCAId = UsermoduloMCA.moduloMCAId;
            ViewBag.mca = repoModulosMCA.GetByID(moduloMCAId);
            var cuenta = repoCuentas.Get(filter: x => x.moduloMCAId == moduloMCAId && x.Cuentabanco == true).Select(x => new { x.descripcion, x.cuentaId, x.bancoId }).FirstOrDefault();
            ViewBag.ucr = cuenta.descripcion;
            ViewBag.cuentaId = cuenta.cuentaId;
            ViewBag.bancoId = cuenta.bancoId;
            //var toconciliaciones = repoConciliacion.Get(filter: x => x.conciliacionBancariaId == conciliacionBancariaId).Select(x => new { fecha = x.fechaTransaccion, x.partidaConsiliacion, x.saldoenLibro, x.saldoEstadoCuentaBanco, x.conciliacionBancariaId }).FirstOrDefault();
            ViewBag.fecha = toConciliacion.fechaTransaccion.ToString("dd/M/yyyy");
            ViewBag.partida = String.Format("{0:0,0.00}", toConciliacion.partidaConsiliacion);
            ViewBag.saldo =  String.Format("{0:0,0.00}", toConciliacion.saldoenLibro);
            ViewBag.saldoEstado = String.Format("{0:0,0.00}", toConciliacion.saldoEstadoCuentaBanco);
            //var toCheques = ComprobantesBL.GetListadoChequesEdit(conciliacionBanId).ToList();           
            return View();
        }      
          
        public ActionResult DetalleConciliacion(int cuentaId)
        {

            //int usuarioId = Session["usuarioMCAId"] == null ? 0 : ((int)Session["usuarioMCAId"]);
            //UsermoduloMCA = repoUsuarioModulosMCA.Get(filter: x => x.usuarioId == usuarioId && x.estadoId == EstadosBL.KEY_ACTIVO).FirstOrDefault();


            int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);
            ViewBag.mca = repoModulosMCA.GetByID(moduloMCAId);
            //var cuenta = repoCuentas.Get(filter: x => x.moduloMCAId == moduloMCAId && x.Cuentabanco == true).Select(x => new { x.descripcion, x.cuentaId, x.bancoId }).FirstOrDefault();
            var cuenta = repoCuentas.Get(filter: x => x.cuentaId == cuentaId).Select(x => new { x.descripcion, x.cuentaId, x.bancoId }).FirstOrDefault();
            ViewBag.ucr = cuenta.descripcion;
            ViewBag.cuentaId = cuentaId;
            //ViewBag.bancoId = cuenta.bancoId;
            return View();
        }
        #endregion

        #region Funciones AJAX
        public ActionResult GetListadoChequesEdit(int conciliacionBancariaId)
        {

            var toChequesE = ComprobantesBL.GetListadoChequesEdit(conciliacionBancariaId).ToList();
            return Json(toChequesE, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetListadoCheques(int cuentaId, DateTime fechaTransaccion)
        {
            //var toModulosMCA = ModulosMCABL.GetListadoMCA(ucrId).ToList();
            var toCheques = ComprobantesBL.GetListadoCheques(cuentaId, fechaTransaccion).ToList();
            return Json(toCheques, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListadoChequesDetalles(int[] comprobanteId)
        {            
                        
            var toListadoCheques = repoComprobantesCuentas.Get(filter: s => comprobanteId.Contains(s.comprobanteId) && s.Cuentas.Cuentabanco == true && s.Comprobantes.estadoId==3 &&( s.estadoId==3 || s.estadoId == 1) ).Select(s => new { cantidad = s.credito, s.Comprobantes.beneficiario, s.Comprobantes.numCheque }).OrderBy(o=>o.numCheque).ToList();           
            return Json(new { toListadoCheques}, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveConciliacion(ConciliacionBancarias toConciliacionBancaria, int[] comprobanteConciliadoId, int[] comprobanteId)
        {
            ConciliacionBancariasBL repoConciliacion = new ConciliacionBancariasBL();
            string mensaje = "";
            int conciliacionBId = 0;

            //Consultar si existe conciliacion pero para esa cuenta en un mes determinado --A*
            try
            {

            bool existeConciliacion = ConciliacionBancariasBL.Existeconciliacion(toConciliacionBancaria.cuentaId, toConciliacionBancaria.fechaTransaccion, out mensaje);
            if (existeConciliacion == false)
            {
                
                int mes = Convert.ToInt32(toConciliacionBancaria.fechaTransaccion.Month);
                int anio = Convert.ToInt32(toConciliacionBancaria.fechaTransaccion.Year);
                toConciliacionBancaria.fecha = DateTime.Now;
                toConciliacionBancaria.mes = mes;
                toConciliacionBancaria.anio = anio;
                toConciliacionBancaria.estadoId = EstadosBL.KEY_ACTIVO;

                if (toConciliacionBancaria.conciliacionBancariaId == 0)
                {
                    // registra conciliacion
                    repoConciliacion.Add(toConciliacionBancaria);
                    var conciliacion = repoConciliacion.Get(filter: x => x.conciliacionBancariaId == toConciliacionBancaria.conciliacionBancariaId).Select(x => new { x.conciliacionBancariaId }).FirstOrDefault();
                    int conciliacionBancariaId = Convert.ToInt32(conciliacion.conciliacionBancariaId);
                    conciliacionBId = conciliacionBancariaId;

                    if (comprobanteId != null)
                    { 
                        //registrar comprobantes no conciliados a la conciliacion
                        DetalleConciliacionBancarias toDetalle = new DetalleConciliacionBancarias();
                        toDetalle.conciliacionBancariaId = Convert.ToInt32(conciliacionBancariaId);
                        toDetalle.fecha = DateTime.Now;
                        toDetalle.estadoId = EstadosBL.KEY_ACTIVO;


                        for (int i = 0; i < comprobanteId.Length; i++)
                        {
                            toDetalle.comprobanteId = comprobanteId[i];
                            repoConciliacionDetalle.Add(toDetalle);
                        }

                    }

                    if (comprobanteConciliadoId != null)
                    {

                        //Actualizar Estado de los comprobantes conciliados
                        for (int i = 0; i < comprobanteConciliadoId.Length; i++)
                        {

                            var toComprobantes = repoComprobantes.GetByID(comprobanteConciliadoId[i]);
                            toComprobantes.conciliado = true;
                            repoComprobantes.Update(toComprobantes);
                        }
                    }

                }

                //else
                //repoConciliacion.Update(toConciliacionBancaria);           
            }

                return Json(new { mensaje, conciliacionBId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                mensaje = "Ocurrio un Error en el proceso interno " + e.Message;
                return Json(new { mensaje, conciliacionBId }, JsonRequestBehavior.AllowGet);
            }
            

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveConciliacionEdit(ConciliacionBancarias toConciliacionBancaria, int[] comprobanteeditId)
        {
            string mensaje = "";
    


            ConciliacionBancariasBL repoConciliacion = new ConciliacionBancariasBL();
            var conciliacion = repoConciliacion.Get(filter: x => x.conciliacionBancariaId == toConciliacionBancaria.conciliacionBancariaId).Select(x => new { x.conciliacionBancariaId }).FirstOrDefault();
            int conciliacionBancariaId = Convert.ToInt32(conciliacion.conciliacionBancariaId);
            var toconciliacion = repoConciliacion.GetByID(conciliacionBancariaId);
            toconciliacion.saldoEstadoCuentaBanco = toConciliacionBancaria.saldoEstadoCuentaBanco;
            toconciliacion.partidaConsiliacion = toConciliacionBancaria.partidaConsiliacion;
            toconciliacion.saldoenLibro = toConciliacionBancaria.saldoenLibro;
            repoConciliacion.Update(toconciliacion);

            if (comprobanteeditId != null)
            {
                var toDetalle = repoConciliacionDetalle.Get(filter: x => x.conciliacionBancariaId == conciliacionBancariaId && x.estadoId == EstadosBL.KEY_ACTIVO).FirstOrDefault();


                //toDetalle.conciliacionBancariaId = ;


                for (int i = 0; i < comprobanteeditId.Length; i++)
                {
                    //var toDetalle = repoConciliacionDetalle.GetByID(comprobanteeditId[i]);
                    //toDetalle.comprobanteId = comprobanteeditId[i];
                    toDetalle.fecha = DateTime.Now;
                    toDetalle.estadoId = EstadosBL.KEY_ANULADO;
                    repoConciliacionDetalle.Update(toDetalle);
                }

                //actualizar los comprobantes que cuando se registro se guardo como conciliado pero al editar pasa a no conciliado
                for (int i = 0; i < comprobanteeditId.Length; i++)
                {

                    var toComprobantes = repoComprobantes.GetByID(comprobanteeditId[i]);
                    toComprobantes.conciliado = false;
                    repoComprobantes.Update(toComprobantes);
                }
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public JsonResult GetConciliacion(int conciliacionBancariaId)
        {            
            var toConciliacion = repoConciliacion.GetByID(conciliacionBancariaId);

            var toListadoCheques = repoConciliacionDetalle.Get(filter: x => x.conciliacionBancariaId == conciliacionBancariaId).Select(x => new { x.comprobanteId, x.Comprobantes.beneficiario, x.Comprobantes.numCheque }).ToList();
                       
            ViewBag.acuerdoSupMCAId = repoConciliacion.Get(filter: x => x.conciliacionBancariaId == conciliacionBancariaId).Select(x => new { x.conciliacionBancariaId }).FirstOrDefault();
                        
            var toconciliaciones = repoConciliacion.Get(filter: x => x.conciliacionBancariaId == conciliacionBancariaId).Select(x => new { x.fechaTransaccion, x.partidaConsiliacion, x.saldoenLibro, x.saldoEstadoCuentaBanco, x.conciliacionBancariaId }).FirstOrDefault();

           return Json(new { toListadoCheques, toconciliaciones }, JsonRequestBehavior.AllowGet);
            
        }

        public JsonResult validarCuenta(string cuentaId)
        {
            string mensaje = "";
            var cuenta = Convert.ToInt32(cuentaId);
            bool validarC = ConciliacionBancariasBL.CierreCuenta(cuenta);

            if (validarC == false)
            {
                mensaje = "Para realizar la Conciliación Bancaria se requiere que este realizado el proceso de cierre, del periodo de la transaccion.";
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult EjecutarConciliacion(int[] comprobanteNcId, int conciliacionId)
        {
            string mensaje = "";
            var toConciliacion = repoConciliacion.GetByID(conciliacionId);
            toConciliacion.estadoId = EstadosBL.KEY_EJECUTADO;
            repoConciliacion.Update(toConciliacion);

            if (comprobanteNcId != null)
            {
                for (int i = 0; i < comprobanteNcId.Length; i++)
                {
                    int comprobanteId = Convert.ToInt16(comprobanteNcId[i]);
                    var toConciliacionDetalle = repoConciliacionDetalle.Get(filter: x => x.comprobanteId == comprobanteId).Select(x => new { x.detalleConcialicionBancariaId }).FirstOrDefault();
                    var todetalleConciliacion = repoConciliacionDetalle.GetByID(toConciliacionDetalle.detalleConcialicionBancariaId);
                    todetalleConciliacion.estadoId = EstadosBL.KEY_EJECUTADO;
                    repoConciliacionDetalle.Update(todetalleConciliacion);
                }
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarEdicion(int conciliacionBancariaId)
        {
            string mensaje = "";
            bool ValidaEdicion = ConciliacionBancariasBL.ValidarConciliacion(conciliacionBancariaId);

            if (ValidaEdicion == true)
            {
                mensaje = "La Conciliación seleccionada, no se puede editar porque se encuentra Ejecutada.";
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Vistas Parciales
        public ActionResult GetListadoMCA(int cuentaId)
        {
            //ViewBag.ucr = repoProyectos.Get(filter: x => x.ucrId == ucrId).Select(x => new { x.Ucrs.ucr }).FirstOrDefault()
            //int usuarioId = Session["usuarioMCAId"] == null ? 0 : ((int)Session["usuarioMCAId"]);
            //UsermoduloMCA = repoUsuarioModulosMCA.Get(filter: x => x.usuarioId == usuarioId && x.estadoId == EstadosBL.KEY_ACTIVO).FirstOrDefault();
            //int moduloMCAId = UsermoduloMCA.moduloMCAId;
            repoCuentas.ProxyCreationEnabled();
            var tocuenta = repoCuentas.Get(filter: x => x.cuentaId == cuentaId).Select(x => new { x.descripcion }).FirstOrDefault();
            //int cuentaId = tocuenta.cuentaId;
            ViewBag.numCuenta = tocuenta.descripcion;
            //repoConciliacion.ProxyCreationEnabled();
            //var toConciliacion = repoConciliacion.Get(filter: x => x.cuentaId == cuentaId).Select(x => new { x.conciliacionBancariaId, fechaInicial = x.fechaTransaccion.ToString("dd/MM/yyyy"), saldoEstadoCuentaBanco = "C$" + String.Format("{0:#,##0.00}", x.saldoEstadoCuentaBanco), partidaConsiliacion = "C$" + String.Format("{0:#,##0.00}",x.partidaConsiliacion), saldoenLibro = "C$" + String.Format("{0:#,##0.00}", x.saldoenLibro, x.conciliacionBancariaId)}).ToList();
            var toConciliacion = ConciliacionBancariasBL.GetListadoConciliacion(cuentaId).ToList();
            return PartialView("_GetListadoMCA", toConciliacion);

        }

        public ActionResult GetListadoCuentas()
        {
           // string rolUser = Session["Rol"].ToString();
            string DatosPartial = string.Empty;
            int usuarioId = Session["usuarioMCAId"] == null ? 0 : ((int)Session["usuarioMCAId"]);
            //if (rolUser == "CONTADOR")
            //{

            //UsermoduloMCA = repoUsuarioModulosMCA.Get(filter: x => x.usuarioId == usuarioId && x.estadoId == EstadosBL.KEY_ACTIVO).FirstOrDefault();
            //int moduloMCAId = UsermoduloMCA.moduloMCAId;
            //repoCuentas.ProxyCreationEnabled();
            int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);

            
                ViewBag.toCuenta = ConciliacionBancariasBL.GetListaCuentasProyecto(moduloMCAId);
                DatosPartial = "_GetListadoCuentas";


            //}
            //else
            //{
            //UsuarioUcr = repousuarios.Get(filter: x => x.usuarioId == usuarioId).FirstOrDefault();              
            //int? ucrId = UsuarioUcr.ucrId;
            //ViewBag.toCuentaUcr = ConciliacionBancariasBL.GetListaCuentasProyectoUcr(ucrId);
            //DatosPartial = "_GetListadoCuentasUcr";

            //var tocuenta = repoCuentas.Get(filter: x => x.moduloMCAId == moduloMCAId && x.Cuentabanco == true && x.estadoId == EstadosBL.KEY_ACTIVO).ToList();  
            //return PartialView("_GetListadoCuentas", tocuenta);
            return PartialView(DatosPartial);

        }

        #endregion

    }

        }