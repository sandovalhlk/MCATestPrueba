using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicMCA;
using DataLayerMCA;


namespace SistContabilidadMCA.Areas.AdministracionMCA.Controllers
{
    [Authorize]
    public class ModuloMCAController : Controller
    {
        #region Instancias
        private ModulosMCABL repoModulosMCA = new ModulosMCABL();
        private TipoUsuariosBL repoTipoUsuario = new TipoUsuariosBL();
        private UsuariosBL repoUsuario = new UsuariosBL();
        private UsuarioModulosMCABL repoUsuarioModulo = new UsuarioModulosMCABL();
        private ProyectosBL repoProyectos = new ProyectosBL();
        private MunicipiosBL repoMunicipios = new MunicipiosBL();
        private DepartamentosBL repoDepartamentos = new DepartamentosBL();
        private TipoExtensionesBL repoExtensionesMCA = new TipoExtensionesBL();
        private AcuerdoSupMCABL repoAcuerdoSuplmentario = new AcuerdoSupMCABL();
        private UcrsBL repoUcrs = new UcrsBL();

        #endregion

        #region HTTP
        public ActionResult Index()
        {
            var usuario = User.Identity.Name;
            var toUsuario = repoUsuario.Get(filter: x=>x.AspNetUsers.UserName==usuario).FirstOrDefault();
            int ucrId = toUsuario == null ? 0 : toUsuario.Ucrs.ucrId;
            var toUcr = repoUcrs.Get(filter: x => x.ucrId == ucrId).SingleOrDefault();
            ViewBag.ucr = toUcr.ucr;
            ViewBag.ucrs = toUsuario==null ? 0 : toUsuario.Ucrs.ucrId;
            return View();
        }

       

        [HttpGet]
        public ActionResult CreateMCA()
        {
            var usuarioMCA = repoUsuario.Get(filter: x => x.AspNetUsers.UserName == User.Identity.Name).FirstOrDefault();
            int ucrId = usuarioMCA.ucrId.Value;

            ViewBag.tipoPersonas = new SelectList(repoTipoUsuario.GetAll().OrderBy(s => s.tipoUsuario).ToList(), "tipoUsuarioId", "tipoUsuario");
           
            ViewBag.proyectos = new SelectList(repoProyectos.Get(filter: x => x.estadoId == EstadosBL.KEY_ACTIVO && x.ucrId== ucrId).OrderBy(s => s.nombreProyecto).ToList(), "proyectoId", "nombreProyecto");
            ViewBag.departamentos = new SelectList(repoDepartamentos.GetAll().OrderBy(s => s.departamento).ToList(), "departamentoId", "departamento");
            ViewBag.municipios = new SelectList(repoMunicipios.GetAll().OrderBy(s => s.municipio).ToList(), "municipioId", "municipio");

            if (TempData["SolicitudGuardada"] != null)
            {
                ViewBag.SolicitudGuardada = TempData["SolicitudGuardada"];
                TempData.Remove("SolicitudGuardada");               
            }
            return View();
        }

        
      [HttpPost]
       public ActionResult CreateMCA(ModulosMCA toModulosMCA, List<UsuarioModulosMCA> listaFirmantes, List<Usuarios> listaUsuarios, float montoTotal)
        {

            var mensaje = "";
            bool ExisteModulo = ModulosMCABL.ExisteModulo(toModulosMCA);
            if (ExisteModulo == false)
            {
                toModulosMCA.monto = montoTotal;
                repoModulosMCA.Add(toModulosMCA);

                foreach (UsuarioModulosMCA toUserModulo in listaFirmantes)
                {
                    if (toUserModulo.usuarioModuloMCAId < 0) //? si el UsuarioModuloMCA no existe 
                    {
                        if (toUserModulo.usuarioId <= 0)
                        {
                            //repoUsuario.Add(toUsuario.);
                            Usuarios toUsuario = listaUsuarios.Where(x => x.usuarioId == toUserModulo.usuarioId).FirstOrDefault();

                            repoUsuario.Add(toUsuario);
                            toUserModulo.usuarioId = toUsuario.usuarioId;
                        }

                        toUserModulo.estadoId = 1;
                        toUserModulo.firmante = true;
                        toUserModulo.moduloMCAId = toModulosMCA.moduloMCAId;
                        toUserModulo.fecha = DateTime.Now;
                        //toUserModulo.usuarioId = toUsuario.usuarioId;


                        //toProyecto.monto = Convert.ToDouble(toProyecto.monto);
                        repoUsuarioModulo.Add(toUserModulo);

                    }

                }
                mensaje = "MCA guardado exitosamente!!";


            }
            else
            {
                mensaje = "El Módulo digitado, ya fue registrado.!!";
            }

           

            var usuarioMCA = repoUsuario.Get(filter: x => x.AspNetUsers.UserName == User.Identity.Name).FirstOrDefault();
            int ucrId = usuarioMCA.ucrId.Value;
            ViewBag.tipoPersonas = new SelectList(repoTipoUsuario.GetAll().OrderBy(s => s.tipoUsuario).ToList(), "tipoUsuarioId", "tipoUsuario");
            ViewBag.proyectos = new SelectList(repoProyectos.Get(filter: x => x.ucrId == ucrId).OrderBy(s => s.nombreProyecto).ToList(), "proyectoId", "nombreProyecto");
            ViewBag.departamentos = new SelectList(repoDepartamentos.GetAll().OrderBy(s => s.departamento).ToList(), "departamentoId", "departamento");
            ViewBag.municipios = new SelectList(repoMunicipios.GetAll().OrderBy(s => s.municipio).ToList(), "municipioId", "municipio");
            TempData["SolicitudGuardada"] = mensaje;
            ViewBag.SolicitudGuardada = TempData["SolicitudGuardada"];
            return View(toModulosMCA);





        }

        [HttpGet]
        public ActionResult EditMCA(int moduloMCAId)
        {            //repoModulosMCA.ProxyCreationEnabled();
            //bool convenioProyecto = ConveniosBL.ConvenioProyecto(convenioId);
          
           
            ViewBag.tipoPersonas = new SelectList(repoTipoUsuario.GetAll().ToList(), "tipoUsuarioId", "tipoUsuario");
            ViewBag.proyectos = new SelectList(repoProyectos.GetAll().ToList(), "proyectoId", "nombreProyecto");
            ViewBag.departamentos = new SelectList(repoDepartamentos.GetAll().ToList(), "departamentoId", "departamento");
            ViewBag.municipios = new SelectList(repoMunicipios.GetAll().ToList(), "municipioId", "municipio");
            bool Eval = ModulosMCABL.ModulosCuenta(moduloMCAId);
            ViewBag.Eval = Eval.ToString();

            var toModulosMCA = repoModulosMCA.GetByID(moduloMCAId);
            
            if (TempData["SolicitudGuardada"] != null)
            {
                ViewBag.SolicitudGuardada = TempData["SolicitudGuardada"];
                TempData.Remove("SolicitudGuardada");
            }
            return View(toModulosMCA);
            

        }

        
        [HttpPost]
        public ActionResult EditMCA(ModulosMCA toModulosMCA, List<UsuarioModulosMCA> listaFirmantes, List<Usuarios> listaUsuarios)
        {
            //Validar si corresponde a edicion valida
            bool Eval = ModulosMCABL.ModulosCuenta(toModulosMCA.moduloMCAId);
            if (Eval == false)
            {
                repoModulosMCA.Update(toModulosMCA);

            //Solo entra cuando se Ingresa un nuevo Firmante
            if (listaFirmantes!=null )
            {
            foreach (UsuarioModulosMCA toUserModulo in listaFirmantes)
            {
                if (toUserModulo.usuarioModuloMCAId < 0)
                {
                        Usuarios toUsuario = listaUsuarios.Where(x => x.usuarioId == toUserModulo.usuarioId).FirstOrDefault();
                    if (toUserModulo.usuarioId <= 0)
                    {
                        repoUsuario.Add(toUsuario);
                        }
                        toUserModulo.estadoId = 1;
                        toUserModulo.firmante = true;
                        toUserModulo.moduloMCAId = toModulosMCA.moduloMCAId;
                        toUserModulo.fecha = DateTime.Now;
                        toUserModulo.usuarioId = toUsuario.usuarioId;
                        repoUsuarioModulo.Add(toUserModulo);
                }
            }
            }
            //Agregar el listado de firmantes
          
            TempData["SolicitudGuardada"] = "MCA actualizado exitosamente!!";
            ViewBag.SolicitudGuardada = TempData["SolicitudGuardada"];

            }
            else
            {
                TempData["SolicitudGuardada"] = "El MCA no se puede modificar, porque ya tiene cuentas asociadas!!";
                ViewBag.SolicitudGuardada = TempData["SolicitudGuardada"];
            }
            ViewBag.tipoPersonas = new SelectList(repoTipoUsuario.GetAll().ToList(), "tipoUsuarioId", "tipoUsuario");
            ViewBag.proyectos = new SelectList(repoProyectos.GetAll().ToList(), "proyectoId", "nombreProyecto");
            ViewBag.municipios = new SelectList(repoMunicipios.GetAll().ToList(), "municipioId", "municipio");
            ViewBag.departamentos = new SelectList(repoDepartamentos.GetAll().ToList(), "departamentoId", "departamento");
            return View(toModulosMCA);
        }


        //MARJORIE Z
        [HttpGet]
       // [ValidateAntiForgeryToken]
        public ActionResult DetalleMCA(int? moduloMCAId )
        {

            repoModulosMCA.ProxyCreationEnabled();
            var toModulo = ModulosMCABL.GetListaModulos().Select(x => new { x.moduloMCAId, x.nombreModuloMCA, x.numContrato }).Where(s => s.moduloMCAId == moduloMCAId).FirstOrDefault();
            ViewBag.mcaProyecto = toModulo.nombreModuloMCA;
            ViewBag.mcaNumero = toModulo.numContrato;
            ViewBag.mca = repoModulosMCA.GetByID(moduloMCAId);

            //Combos
            ViewBag.TipoExtension = new SelectList(repoExtensionesMCA.Get(filter: x => x.estadoId == EstadosBL.KEY_ACTIVO).Select(x => new { x.tipoExtensionId, x.tipoExtencion }).OrderBy(s => s.tipoExtencion).ToList(), "tipoExtensionId", "tipoExtencion");
            return View();
        }


        #endregion

        #region Funciones AJAX

        public ActionResult GetListaFirmantes(int moduloMCAId)
        {
            //var  toModuloMCA= repoModulosMCA.Get(filter: x => x.moduloMCAId == moduloMCAId);
            var toListaFirmantes = UsuarioModulosMCABL.GetListaFirmantes(moduloMCAId);
            //GetListaEmpleados toEmpleado = SolicitudesBL.GetEmpleadoAsignadoVehiculoResponsables(vehiculo);

            return Json(new { toListaFirmantes }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUsuario(string cedula)
        {
            repoUsuario.ProxyCreationEnabled();
            var toPersona = repoUsuario.Get(filter: x=>x.cedula==cedula);
            //var toPersona = UsuarioModulosMCABL.GetListaFirmantes(moduloMCAId);
            return Json(new { toPersona = toPersona }, JsonRequestBehavior.AllowGet);
        }


        //[HttpGet]
        //public JsonResult ValidarFirmante(int moduloMCAId, int tipoUsuarioId)
        //{
        //    string mensaje = "";
        //    repoProyectos.ProxyCreationEnabled();
        //    bool firmante = UsuarioModulosMCABL.existeFirmante(moduloMCAId, tipoUsuarioId, out string mensaje);
        //    if (firmante == true)
        //    return Json(mensaje, JsonRequestBehavior.AllowGet);
        //}



        //MARJORIE Z

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAcuerdo(DataLayerMCA.AcuerdoSupMCA toAcuerdo, FormCollection collection)
        {
            ////Ejecucion del ajax
            //repoModulos.Add(toModulo);
            //return Json(new { }, JsonRequestBehavior.AllowGet);

            string mensaje = "";
            bool existeModulo = AcuerdoSupMCABL.ExisteAcuerdo(toAcuerdo, out mensaje);
            if (existeModulo == false)
            {
                toAcuerdo.numero = toAcuerdo.numero;
                toAcuerdo.moduloMACId = toAcuerdo.moduloMACId;
                toAcuerdo.descripcion = toAcuerdo.descripcion;
                toAcuerdo.tipoExtencionId = toAcuerdo.tipoExtencionId;
                toAcuerdo.monto = Convert.ToDouble(toAcuerdo.monto);
                toAcuerdo.loguitud = toAcuerdo.loguitud;
                toAcuerdo.fechaInicial = DateTime.Parse(collection["fechaInicial"].ToString());
                toAcuerdo.fechaFinal = DateTime.Parse(collection["fechaFinal"].ToString());
                toAcuerdo.estadoId = EstadosBL.KEY_ACTIVO;
                if (toAcuerdo.acuerdoSupMCAId == 0)
                {
                    repoAcuerdoSuplmentario.Add(toAcuerdo);
                }
                else
                {
                    repoAcuerdoSuplmentario.Update(toAcuerdo);
                }

            }

            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetAcuerdos(int acuerdoSupMCAId)
        {
            string mensaje = "";
            repoAcuerdoSuplmentario.ProxyCreationEnabled();
            var toAcuerdo = repoAcuerdoSuplmentario.GetByID(acuerdoSupMCAId);
            DateTime fechaAcuerdo = Convert.ToDateTime(toAcuerdo.fechaInicial).Date;
            int moduloMCAId = toAcuerdo.moduloMACId;
            bool ValidarAcuerdo = AcuerdoSupMCABL.ValidarAcuerdo(acuerdoSupMCAId, fechaAcuerdo, moduloMCAId);
            //AcuerdosSuplementarios
            ViewBag.acuerdoSupMCAId = repoAcuerdoSuplmentario.Get(filter: x => x.moduloMACId == moduloMCAId).Select(x => new { x.acuerdoSupMCAId }).FirstOrDefault();

            if (ValidarAcuerdo == false)            {

                var toAcuerdos = repoAcuerdoSuplmentario.Get(filter: x => x.acuerdoSupMCAId == acuerdoSupMCAId).Select(x => new { x.descripcion, x.numero, x.tipoExtencionId, x.monto, x.loguitud, fechaInicial = x.fechaInicial.ToString("dd/MM/yyyy"), fechaFinal = x.fechaFinal.ToString("dd/MM/yyyy"), x.acuerdoSupMCAId }).FirstOrDefault();
                //var toAcuerdos = repoAcuerdoSuplmentario.Get(filter: x => x.acuerdoSupMCAId == acuerdoSupMCAId).Select(x => new { x.descripcion, x.numero, x.tipoExtencionId, monto = "C$" + String.Format("{0:#,##0.00}", x.monto), x.loguitud, fechaInicial = x.fechaInicial.ToString("dd/MM/yyyy"), fechaFinal = x.fechaFinal.ToString("dd/MM/yyyy"), x.acuerdoSupMCAId }).FirstOrDefault();

                return Json(toAcuerdos, JsonRequestBehavior.AllowGet);
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult BajaAcuerdo(int acuerdoSupMCAId)
        {
            //string mensaje = "";
            repoAcuerdoSuplmentario.ProxyCreationEnabled();
            //bool ProyectoModulo = ProyectosBL.Proyectomodulo(proyectoId);
            //if (ProyectoModulo == false)
            //{
            var tacuerdo = repoAcuerdoSuplmentario.GetByID(acuerdoSupMCAId);
            tacuerdo.estadoId = EstadosBL.KEY_ANULADO;
            repoAcuerdoSuplmentario.Update(tacuerdo);
            return Json(tacuerdo, JsonRequestBehavior.AllowGet);
            //}
            //return Json(mensaje, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetMunicipioDep(int? departamentoId)
        {
            //repoMunicipio.ProxyCreationEnabled();
            var Municipios = repoMunicipios.Get(filter: x => x.departamentoId == departamentoId)
                .Select(x => new { x.municipioId, Municipio = x.municipio })
                .ToList();
            if (Municipios.Count() == 0)
            {
                Municipios = null;
            }
            return Json(new { Municipios }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarFirmantes(int UsuarioModuloId) ///Eliminacion logica de los usuarios
        {
            var msj = "1"; //controla mensajes de error que se presentaran al usuario
            try
            {
                var toUsuarioModuloId = repoUsuarioModulo.GetByID(UsuarioModuloId);
                toUsuarioModuloId.estadoId = EstadosBL.KEY_ANULADO;
                repoUsuarioModulo.Update(toUsuarioModuloId);
            }
            catch (Exception ex)
            {
                msj="El procedimiento no se ejecuto correctamente error -> " + ex.Message.ToString();
            }
            

            return Json(msj, JsonRequestBehavior.AllowGet);
        }



        public JsonResult ValdiarEdicion(int moduloMCAId)
        {
            string mensaje = "";
            bool ModulosCuenta = ModulosMCABL.ModulosCuenta(moduloMCAId);        
       
            if (ModulosCuenta == true)
            {
                mensaje = "El Módulo seleccionado, no se puede editar o actualizar porque tiene cuentas asociadas.";
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region Vistas Parciales
        public ActionResult GetListMCA(int ucrId)
        {
            var toModulosMCA= repoModulosMCA.Get(filter: x => x.Proyectos.ucrId == ucrId).Select(x => new { x.nombreModuloMCA, x.Proyectos.nombreProyecto, x.Municipios.municipio,x.Departamentos.departamento, x.longuitud, monto = "C$" + String.Format("{0:#,##0.00}", x.monto), x.moduloMCAId, x.Proyectos.ucrId}).ToList();
            var ucrs = repoUcrs.Get(filter: x => x.ucrId == ucrId).Select(x => new { x.ucr }).FirstOrDefault();
            ViewBag.ucr = ucrs.ucr;
            //var toModulosMCA = repoModulosMCA.Get(filter: x => x.Proyectos.ucrId == ucrId).ToList(); 
            return PartialView("_GetListMCA", toModulosMCA);
        }
        


        public ActionResult GetListaAcuerdos(int? moduloMCAId)
        {
            var listAcuerdos = repoAcuerdoSuplmentario.Get(filter: x => x.moduloMACId == moduloMCAId && x.estadoId == EstadosBL.KEY_ACTIVO).Select(x => new { x.numero, x.descripcion, x.TipoExtensiones.tipoExtencion, monto = "C$" + String.Format("{0:#,##0.00}", x.monto), x.loguitud, fechaInicial = x.fechaInicial.ToString("dd/MM/yyyy"), fechaFinal = x.fechaFinal.ToString("dd/MM/yyyy"), x.acuerdoSupMCAId, x.moduloMACId }).ToList();
            return PartialView("_GetListaAcuerdos", listAcuerdos);
        }

       

        #endregion

    }
}