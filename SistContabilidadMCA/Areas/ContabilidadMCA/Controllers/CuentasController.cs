using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicMCA;
using DataLayerMCA;


namespace SistContabilidadMCA.Areas.ContabilidadMCA.Controllers
{
    [Authorize]
    public class CuentasController : Controller
    {

        #region Instancias

        private CuentasBL repoCuentas = new CuentasBL();
        ComprobanteCuentasBL repoComprobantesCuentas = new ComprobanteCuentasBL();
        ComprobantesBL repoComprobantes = new ComprobantesBL();
        TipoComprobantesBL repoTipoComprobante = new TipoComprobantesBL();
        ComprobanteCuentasBL repoComprobanteCuentas = new ComprobanteCuentasBL();
        UsuarioModulosMCABL repoUsuarioModulosMCA = new UsuarioModulosMCABL();
        CierreCuentasBL repoCierreCuentas = new CierreCuentasBL();
        ModulosMCABL repoModulosMCA = new ModulosMCABL();
        CierresBL repoCierre = new CierresBL();
        ReinicioChequesBL repoReinicioCheque = new ReinicioChequesBL();
        private UsuarioModulosMCA UsermoduloMCA;

        #endregion

        #region HTTP
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Comprobantes()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddComprobante(int? comprobanteId)
        {
            ViewBag.SelectComprobanteTipo = new SelectList(repoTipoComprobante.Get(filter: x => x.tipoComprobanteId == 1 || x.tipoComprobanteId == 2), "tipoComprobanteId", "tipoComprobante");

            var toComprobante = repoComprobantes.GetByID(comprobanteId);
            //VERIFICACION DE EDICION DEL COMPROBANTE CON RESPECTO AL ULTIMO CIERRE
            bool editC = false;



            if (toComprobante != null)
                editC = ComprobantesBL.ValidarComprobanteCierre(comprobanteId.Value);
            else
                editC = true;
                
            

            ViewBag.editC = editC;
            return View(toComprobante);
        }

        public ActionResult CierresContables()
        {
            //OBTENER LA FECHA DEL ULTIMO CIERRE
            int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);
            var modulosMCA = repoModulosMCA.GetByID(moduloMCAId);
            repoCierre.ProxyCreationEnabled();
            var cierres = repoCierre.Get(filter: x => x.moduloMCAId == moduloMCAId && x.estadoId == EstadosBL.KEY_ASENTADO).LastOrDefault();

            DateTime ultimoCierre, cierreAnualActual;

            /*******BUG AOSH*****/
            cierreAnualActual = modulosMCA.fechaConstitucion;

            if (cierres != null) //si no existe ningun cierre se toma la fecha de constitucion del MCA
            {
                ultimoCierre = Convert.ToDateTime("01/" + cierres.fechaTransaccion.Month.ToString() + "/" + cierres.fechaTransaccion.Year.ToString()).AddMonths(1).AddDays(-1);
                ultimoCierre = ultimoCierre.AddMonths(1).AddDays(-1);
                cierreAnualActual = cierres.fechaTransaccion; //Se obtiene el Año para realizar el cierre Anual
            }
            else //obtner la fecha de constitucion de la empresa MCA la cual tiene que ser el primer cierre
            {
                ultimoCierre = Convert.ToDateTime("01/" + modulosMCA.fechaConstitucion.Month.ToString() + "/" + modulosMCA.fechaConstitucion.Year.ToString());
                ultimoCierre = ultimoCierre.AddMonths(1).AddDays(-1);
            }

            ViewBag.ultimoCierre = ultimoCierre;
            ViewBag.cierreAnual = cierreAnualActual;
            ViewBag.mesCierre = Global.GetMes(ultimoCierre.Month) + " " + ultimoCierre.Year.ToString();

            return View();
        }

        [HttpGet]
        public ActionResult ReversionCierre()
        { //CARGAR LOS CIERRES OFICIALES EN LA PANTALLADE REVERSION

            ViewBag.cierres = new SelectList(repoCierre.Get(filter: x => x.estadoId == EstadosBL.KEY_ASENTADO && x.tipoCierreId == 1).Select(s => new { cierreId = s.cierreId, cierre = "Asentado Mes: " + s.fechaTransaccion.Month.ToString() + " - Año: " + s.fechaTransaccion.Year.ToString() }), "cierreId", "cierre");
            ViewBag.SelectMCA = new SelectList(repoModulosMCA.GetAll(), "moduloMCAId", "nombreModuloMCA");
            return View();
        }
        #endregion

        #region Funciones AJAX

        public ActionResult CrearAnularCatalogo(int opcion) //Funcion para crear o anular un determinado catalogo de cuentas
        {
            var mensaje = "";
            bool band = false;
            int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);

            if (moduloMCAId > 0)
            {
                var toCuentas = repoCuentas.Get(filter: x => x.moduloMCAId == moduloMCAId).ToList();

                if (toCuentas.Count > 0)
                {
                    if (opcion == -1)//Anulacion del Catalogo
                    {
                        foreach (Cuentas cuenta in toCuentas)
                        {
                            cuenta.estadoId = EstadosBL.KEY_ANULADO;
                            repoCuentas.Update(cuenta);
                        }
                        mensaje = "El catalogo de cuentas fue actualizado exitosamente!!!";
                    }
                    else if (opcion == 1) // es una creaccion de catalogo
                        mensaje = "Ya existe un catalogo de cuentas asociado a este modulo";
                }
                else if (opcion == 1) // es una creaccion de catalogo
                {
                    mensaje = CuentasBL.CrearCatalogo(moduloMCAId);
                    mensaje = "El catalogo de cuentas fue creado exitosamente!!!";
                    band = true;
                }
                else
                    mensaje = "El catalogo de cuentas esta en uso comuniquese con el administrador";

            }
            else
                mensaje = "No posee permisos para crear un catalogo de cuentas";

            return Json(new { mensaje = mensaje, band = band }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditCuenta(int cuentaId) //Funcion de vista parcial del modal de addedit cuentas
        {
            var toCuenta = repoCuentas.Get(filter: x => x.cuentaId == cuentaId).Select(x => new
            {
                descripcionHijo = x.hijos.LastOrDefault()?.descripcion ?? null,
                codigoHijo = x.hijos.LastOrDefault()?.codigo ?? null,
                descripcionPadre = x.padre.descripcion,
                codigoPadre = x.padre.codigo,
                nivelPadre = x.padre.nivel,
                cuentaPadreId = x.padre.cuentaId,
                x.cuentaId,
                x.jerarquia,
                x.nivel,
                x.ModulosMCA.nombreModuloMCA,
                x.codigo,
                x.descripcion,
                x.TipoCuentas.tipoCuenta,
                x.naturaleza,
                x.Cuentabanco,
                comprobanes = x.ComprobanteCuentas.Count()
            }).ToList();
            var msj = "";
            if (toCuenta.Select(x => x.comprobanes).FirstOrDefault() > 0)
            {
                msj = "La cuenta seleccionada tiene comprobantes asociados, No se puede Editar";
            }

            return Json(new { toCuenta = toCuenta, msj = msj }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveCuenta(Cuentas toCuenta, int cuentaPadreId) //Funcion de vista parcial del modal de addedit cuentas
        {
            var toCuentaPadre = repoCuentas.GetByID(cuentaPadreId);

            toCuenta.jerarquia = cuentaPadreId;
            toCuenta.tipoCuentaId = 5;   //LAS CUENTAS QUE SE AGREGARAN SERAN UNICAMENTE DETALLE
            toCuenta.moduloMCAId = toCuentaPadre.moduloMCAId;
            toCuenta.naturaleza = toCuentaPadre.naturaleza;
            toCuenta.nivel = toCuentaPadre.nivel + 1;
            toCuenta.estadoId = EstadosBL.KEY_ACTIVO;

            if (toCuenta.cuentaId > 0)
                repoCuentas.Update(toCuenta);
            else
                repoCuentas.Add(toCuenta);

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NullCuentas(int cuentaId, string razon) //Funcion de vista parcial del modal de addedit cuentas
        {
            repoCuentas.ProxyCreationEnabled();
            var toCuenta = repoCuentas.GetByID(cuentaId);

            toCuenta.estadoId = EstadosBL.KEY_ANULADO;
            repoCuentas.Update(toCuenta);

            if (CuentasBL.NulidadCuenta(cuentaId)) //Comprobar Funcionamiento
                AnulacionesBL.Anular(razon, cuentaId, "Cuentas", "Usuario");

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListComprobantes(int comprobanteId = 0)
        {
            var toComprobante = repoComprobanteCuentas.Get(filter: x => x.comprobanteId == comprobanteId && x.estadoId != EstadosBL.KEY_ANULADO).Select(x => new { x.comprobanteCuentaId, x.comprobanteId, x.cuentaId, x.Cuentas.codigo, x.Cuentas.descripcion, x.credito, x.debito, x.Cuentas.Cuentabanco }).ToList();
            return Json(new { toComprobante }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListCuentas(string phrase) //Funcion para buscar las cuentas en el text de autocompletar
        {
            int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);
            repoCuentas.ProxyCreationEnabled();
            var data = repoCuentas.Get(filter: x => x.moduloMCAId == moduloMCAId && x.estadoId ==EstadosBL.KEY_ACTIVO && x.codigo.Contains(phrase)).Select(x => new { x.descripcion, x.codigo, x.cuentaId, x.Cuentabanco }).ToArray();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLastNumComprobante(int tipoComprobanteId, int? comprobanteId = 0, int? invertTipo = 0) //Nuevo cambio
        {
            int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);
            string numero = "";
            //Ultimo comprobante de su tipo
            var lastCompActual = new Comprobantes();
            Comprobantes LastComp = new Comprobantes();
            Comprobantes LastCompInv = new Comprobantes();

            if (comprobanteId > 0)
                lastCompActual = new ComprobantesBL().GetByID(comprobanteId);

            if (moduloMCAId > 0)
            {

                if (comprobanteId > 0)
                {
                    LastComp = new ComprobantesBL().Get(filter: x => x.tipoComprobanteId == tipoComprobanteId && x.estadoId != EstadosBL.KEY_ANULADO_DB && x.comprobanteId != comprobanteId && x.ComprobanteCuentas.Select(s => s.Cuentas.moduloMCAId).FirstOrDefault() == moduloMCAId).OrderBy(x => x.comprobanteId).LastOrDefault();
                    LastCompInv = new ComprobantesBL().Get(filter: x => x.tipoComprobanteId == invertTipo && x.estadoId != EstadosBL.KEY_ANULADO_DB && x.ComprobanteCuentas.Select(s => s.Cuentas.moduloMCAId).FirstOrDefault() == moduloMCAId).OrderBy(x => x.comprobanteId).LastOrDefault();
                }
                else
                    //Unicamente para los comprobantes de cheques**cuando es un comprobante nuevo
                    LastComp = new ComprobantesBL().Get(filter: x => x.tipoComprobanteId == tipoComprobanteId && x.estadoId != EstadosBL.KEY_ANULADO_DB && x.ComprobanteCuentas.Select(s => s.Cuentas.moduloMCAId).FirstOrDefault() == moduloMCAId).OrderBy(x => x.comprobanteId).LastOrDefault();
                //GENERAR NUMERO EN BASE AL ULTIMO COMPROBANTE
                if (LastComp == null)
                    numero = "1";
                else
                    numero = ((int.Parse(LastComp.numero) + 1).ToString());
            }
            if (lastCompActual != null && comprobanteId > 0)
            {
                if (lastCompActual.comprobanteId != LastCompInv.comprobanteId)
                    numero = numero = "";
            }

            return Json(numero, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveComprobante(Comprobantes toComprobante, ComprobanteCuentas[] movimientos, int? reinicio, string justificacion)
        {
            string msj = "";
            bool valCierre = false;
            int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);

            //Obtencion del num de reinicio maximo
            int numReinicio = 0;
            numReinicio = repoReinicioCheque.Filter(x => x.moduloMCAId == moduloMCAId).Select(s => s.numero).LastOrDefault();


            if (toComprobante.comprobanteId > 0) //Si es una actualizacion
            {  //Actualizar datos del comprobante
                toComprobante.fecha = DateTime.Now;
                //AGREGAR REINICIO DE NUM CHEQUE
                if (reinicio > 0)
                {
                    ReinicioChequesBL.AddReinicio(reinicio.Value, justificacion, toComprobante.comprobanteId,moduloMCAId);
                    toComprobante.numero = reinicio.ToString();

                    if (numReinicio > int.Parse(toComprobante.numero))
                        toComprobante.numero = (numReinicio + 1).ToString();

                }

                repoComprobantes.Update(toComprobante);



                //Actualizar datos de los movientos del comprobante 
                if (movimientos != null)
                {
                    foreach (ComprobanteCuentas cc in movimientos)
                    {
                        if (cc.comprobanteCuentaId > 0) //Es una actualizacion del comprobante
                        {//Estado del comprobante es aplicado ?? que pasaria con los eliminados!! --> estos no tendria que ser enviados
                            cc.estadoId = toComprobante.estadoId;
                            cc.fecha = DateTime.Now;
                            cc.fechaTransaccion = toComprobante.fechaComprobante;
                            repoComprobanteCuentas.Update(cc);
                        }
                        else
                        { //si es una nueva insercion de un movimiento en la actualizacion del comprobante
                            cc.comprobanteId = toComprobante.comprobanteId;
                            cc.estadoId = EstadosBL.KEY_EJECUTADO; //toComprobante.estadoId;
                            cc.fecha = DateTime.Now;
                            cc.fechaTransaccion = toComprobante.fechaComprobante;
                            repoComprobanteCuentas.Add(cc);
                        }
                    }
                }
            }
            else //CASO INSERCION
            {
              

                //ULTIMO CIERRE
                var ultimoCierre = repoCierre.Get(filter: x => x.moduloMCAId == moduloMCAId && x.estadoId == EstadosBL.KEY_ASENTADO).LastOrDefault();
                //ULTIMO COMPROBANTE
                var LastComp = new ComprobantesBL().Get(filter: x => x.tipoComprobanteId == toComprobante.tipoComprobanteId && x.estadoId != EstadosBL.KEY_ANULADO_DB && x.ComprobanteCuentas.Select(s => s.Cuentas.moduloMCAId).FirstOrDefault() == moduloMCAId).OrderBy(x => x.comprobanteId).LastOrDefault();

                //COMPROBAR QUE EL COMPROBANTE CORRESPONDA A SU CIERRE
                //VALIAR CIERRE
                if (ultimoCierre == null) //No existen cierres
                    valCierre = true; //Caso del ultimo cierre sea en dic
                else if (ultimoCierre.fechaTransaccion.Month == 12 && toComprobante.fechaComprobante.Month == 1 && (ultimoCierre.fechaTransaccion.Year + 1) == toComprobante.fechaComprobante.Year)
                    valCierre = true; //caso del cierre mesual normal
                else if ((ultimoCierre.fechaTransaccion.Month + 1) == toComprobante.fechaComprobante.Month && ultimoCierre.fechaTransaccion.Year == toComprobante.fechaComprobante.Year)
                    valCierre = true;

                //GENERAR NUMERO EN BASE AL ULTIMO COMPROBANTE
                if (reinicio > 0)
                {
                    toComprobante.numero = reinicio.ToString();
                    //Validacion del ultimo reinicio 
                    if (numReinicio > int.Parse(toComprobante.numero))
                        toComprobante.numero = (numReinicio + 1).ToString();
                }
                else
                {                
                if (LastComp == null)
                    toComprobante.numero = "1";
                else if ((LastComp.fechaComprobante.Month != toComprobante.fechaComprobante.Month) && toComprobante.tipoComprobanteId == 1)
                    toComprobante.numero = "1";
                else
                    toComprobante.numero = ((int.Parse(LastComp.numero) + 1).ToString()); //validar el caso de que sea el primer comprobante

                }

           
                if (toComprobante.numero != null && valCierre == true)
                {  //COMPROBACION DE COMPROBANTE 
                    toComprobante.fecha = DateTime.Now;
                    toComprobante.estadoId = EstadosBL.KEY_EJECUTADO;
                    repoComprobantes.Add(toComprobante);

                    if (reinicio > 0)
                        ReinicioChequesBL.AddReinicio(reinicio.Value, justificacion, toComprobante.comprobanteId, moduloMCAId);

                    foreach (ComprobanteCuentas cc in movimientos) //REGISTRO DE MOVIMIENTOS
                    {
                        cc.comprobanteId = toComprobante.comprobanteId;
                        cc.estadoId = EstadosBL.KEY_ACTIVO;
                        cc.fecha = DateTime.Now;
                        cc.fechaTransaccion = toComprobante.fechaComprobante;
                        repoComprobanteCuentas.Add(cc);
                    }
                }
                else
                {
                    msj = "La fecha indicada en el Comprobante, no corresponde con el periodo correcto, favor verificar la fecha del comprobante";
                    return Json(new { toComprobante = "", msj = msj }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { toComprobante = toComprobante, msj = msj }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarMovimiento(int movimientoId)
        {
            var toComprobanteCuentas = repoComprobanteCuentas.GetByID(movimientoId);
            toComprobanteCuentas.estadoId = EstadosBL.KEY_ANULADO;
            repoComprobanteCuentas.Update(toComprobanteCuentas);

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Funcion para ejecutar los cierres Mensual, Anuales y Total de las cuentas
        /// </summary>
        /// <param name="tipoCierreId">Tipo de cierre: Mensual Anual o Total</param>
        /// <param name="fechaTransaccion">Indica la fecha en base a las que se calcular los saldos</param>
        /// <param name="cierreOficial">Indica si es un cierre Oficial o un calculo de saldo </param> 
        /// <returns></returns>
        public ActionResult EjecutarCierre(int tipoCierreId, string fechaTransaccion, int cierreOficial = 3) //CIERRE 3 PARA EJECUTADO (NO ASENTADO) Y 4 PARA ASENTADO
        {
            string msj = "";
            DateTime fechaTrans;
            fechaTrans = DateTime.Parse(fechaTransaccion);

            int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);

            /************Validar Cierre************/
            var toModulo = repoModulosMCA.GetByID(moduloMCAId);
            var cierres = repoCierre.Get(filter: x => x.moduloMCAId == moduloMCAId); //SE OBTENDRAN TODOS LOS CIERRES DEL MCA
            //LA VALIDACION DE REPETICION DEL MES SOLO SERA PARA LOS CIERRRES MENSUALES, NO TENDRA EFECTO EN LOS CIERRES ANUALES Y TOTALES (♦♦♦EN PRUEBA♦♦♦)
            if (cierres.Where(x => x.tipoCierreId == 3 && x.estadoId == EstadosBL.KEY_ASENTADO).Count() > 0) //VERIFICAR QUE NO SEA UN CIERRE TOTAL DE CONTABILIDAD
                msj = "Esta operacion no se puede realizar, Debido a que ya la contabilidad se cerro";
            else if (cierres.Where(x => x.fechaTransaccion.Month == fechaTrans.Month && x.fechaTransaccion.Year == fechaTrans.Year && x.estadoId == EstadosBL.KEY_ASENTADO).Count() > 0 && tipoCierreId == 1) //SOLO VALIDAR CUANDO SEA MENSUAL
                msj = "Esta operacion no se puede realizar, Debido a que ya existe un cierre asentado para este mes ";
            else //SE PROCEDE A CREACION DE CIERRE 
            {
                int cierreId = 0; //VALIDACION DE CIERRE TOTAL *******

                switch (tipoCierreId) //TIPO DE CIERRE MENSUAL-ANUAL-TOTAL
                {
                    case 1: //CIERRE MENSUAL
                        if (cierres.Where(x => x.estadoId == EstadosBL.KEY_ASENTADO).Count() == 0) //VALIDAR SI ES EL PRIMER CIERRE MENSUAL.. sea Asentado o NO Asentado
                        {//Aunque sean un cierre sin asentar(culculo de saldos) la fecha debe ser la misma de la fecha de constitucion
                            if (toModulo.fechaConstitucion.Month == fechaTrans.Month && toModulo.fechaConstitucion.Year == fechaTrans.Year)
                            {
                                cierreId = CierresBL.CrearCierre(fechaTrans, tipoCierreId, cierreOficial, moduloMCAId);
                                if (cierreId > 0)
                                    ComprobanteCuentasBL.CerrarCuentas(fechaTrans, moduloMCAId, cierreId);
                                else
                                    msj = "OCURRIO UN ERROR EN EL PROCESO, EVENTO: CREACION CIERRE";

                            }
                            else
                                msj = "EL la fecha no corresponde, debe ser la misma fecha de la constitucion del MCA resgistrado en el sistema, para el primer Calculo de Saldos o Asentamiento";

                        }
                        else if (cierres.Where(s => s.estadoId == EstadosBL.KEY_ASENTADO).Max(s => s.fechaTransaccion).AddMonths(1).Month == fechaTrans.Month &&
                               cierres.Where(s => s.estadoId == EstadosBL.KEY_ASENTADO).Max(s => s.fechaTransaccion).AddMonths(1).Year == fechaTrans.Year) // YA EXISTE UN CIERRE MENSUAL.. SE SUMA UN MES PARA COMRPOBAR LA CORRECTA SECUENCIA
                        {//SE TENDRA QUE VALIDAR EL AÑO TAMBIEN 
                            cierreId = CierresBL.CrearCierre(fechaTrans, tipoCierreId, cierreOficial, moduloMCAId);
                            if (cierreId > 0)
                            {
                                ComprobanteCuentasBL.CerrarCuentas(fechaTrans, moduloMCAId, cierreId);
                                if (cierreOficial == EstadosBL.KEY_ASENTADO)
                                    CierreCuentasBL.CierreCuentaOficial(cierreId); //En teoria el unico lugar donde se deberia de replicar un Cierre Oficial
                            }
                            else
                                msj = "OCURRIO UN ERROR EN EL PROCESO, EVENTO: CREACION CIERRE";

                        }//Bug encontrado y reparado 14/05/2019 add OR para validar estado ejecutado y Asentado(en el caso q no exista un calculo de saldo previo)
                        else if (cierres.Where(s => s.estadoId == EstadosBL.KEY_EJECUTADO || s.estadoId == EstadosBL.KEY_ASENTADO).Max(x => x.fechaTransaccion).Month == fechaTrans.Month &&
                            cierres.Where(s => s.estadoId == EstadosBL.KEY_EJECUTADO || s.estadoId == EstadosBL.KEY_ASENTADO).Max(x => x.fechaTransaccion).Year == fechaTrans.Year
                            ) // YA EXISTE UN CIERRE MENSUAL PERO NO ESTA ASENTADO.. SE MANTIENE EL MISMO MES
                        {
                            cierreId = CierresBL.CrearCierre(fechaTrans, tipoCierreId, cierreOficial, moduloMCAId);
                            if (cierreId > 0)
                                ComprobanteCuentasBL.CerrarCuentas(fechaTrans, moduloMCAId, cierreId);
                            else
                                msj = "OCURRIO UN ERROR EN EL PROCESO, EVENTO: CREACION CIERRE";

                        }
                        else
                        {
                            msj = "La fecha de la operacion no corresponde, Deber ser igual al mes siguiente del ultimo cierre registrado";
                        }
                        break;

                    case 2: //CIERRE ANUAL
                        if (cierres.Where(x => x.tipoCierreId == 2 && x.estadoId == EstadosBL.KEY_ASENTADO && x.fechaTransaccion.Year == fechaTrans.Year).Count() > 0) //VALIDACION DE CIERRE ANUAL
                        {
                            msj = "El cierre de este periodo ya existe o la fecha de Cierre Anual no corresponde, El mes de Diciembre del año en seleccionado debe estar cerrado";
                        }
                        //else if (cierres.Select(x => x.fechaTransaccion.Year).LastOrDefault() == fechaTrans.Year) //Solo comprobar el año
                        else if (cierres.Where(x => x.fechaTransaccion.Year == fechaTrans.Year && x.fechaTransaccion.Month == 12 && x.tipoCierreId == 1 && x.estadoId == EstadosBL.KEY_ASENTADO).Count() > 0) //Comprobar que existe un cierre De Dic 
                        {

                            if (ComprobanteCuentasBL.CrearComprobante(moduloMCAId, fechaTrans, 3)) //CREA EL COMPROBANTE DE CIERRE TOTAL)
                            {
                                cierreId = CierresBL.CrearCierre(fechaTrans, tipoCierreId, cierreOficial, moduloMCAId); //CREA EL CIERRE TOTAL
                                if (cierreId > 0)
                                    ComprobanteCuentasBL.CerrarCuentas(fechaTrans, moduloMCAId, cierreId); //CIERRE DE CUENTAS TOTAL
                                else
                                    msj = "OCURRIO UN ERROR EN EL PROCESO, EVENTO: CREACION CIERRE";
                            }
                            else
                                msj = "OCURRIO UN ERROR EN EL PROCESO, EVENTO: COMPROBANTE AUTOMATICO";

                        }
                        else
                            msj = "OCURRIO UN ERROR EN EL PROCESO, EVENTO: COMPROVACION DE MES DICIEMBRE ASENTADO";

                        break;

                    case 3: //CIERRE TOTAL
                        if (cierres.Where(x => x.tipoCierreId == 3 && x.estadoId == EstadosBL.KEY_ASENTADO && x.fechaTransaccion.Year == fechaTrans.Year).Count() > 0) //VALIDACION DE CIERRE ANUAL
                        {
                            msj = "EL CIERRE DE ESTA CONTABILIDAD YA SE EJECUTO";
                        }
                        //else if (cierres.Select(x => x.fechaTransaccion.Year).LastOrDefault() == fechaTrans.Year ) //Solo comprobar el año
                        else if (toModulo.fechaFin.Year == fechaTrans.Year && toModulo.fechaFin.Month == fechaTrans.Month) //Solo comprobar el año
                        {
                            if (ComprobanteCuentasBL.CrearComprobante(moduloMCAId, fechaTrans, 2)) //CREA EL COMPROBANTE DE CIERRE TOTAL)
                            {
                                cierreId = CierresBL.CrearCierre(fechaTrans, tipoCierreId, cierreOficial, moduloMCAId); //CREA EL CIERRE TOTAL
                                if (cierreId > 0)
                                    ComprobanteCuentasBL.CerrarCuentas(fechaTrans, moduloMCAId, cierreId); //CIERRE DE CUENTAS TOTAL
                                else
                                    msj = "OCURRIO UN ERROR EN EL PROCESO, EVENTO: CREACION CIERRE";
                            }
                            else
                                msj = "OCURRIO UN ERROR EN EL PROCESO, EVENTO: COMPROBANTE AUTOMATICO";
                        }
                        else
                            msj = "OCURRIO UN ERROR EN EL PROCESO, VALIDACION: LA FECHA DE CIERRE DE LA CONTABILIDAD NO ES IGUAL A LA DE FIN DE MODULO";

                        break;
                }

            }
            return Json(msj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidarCodCuenta(string codigo, int nivel)
        {
            string msj = "";

            var toCuenta = repoCuentas.Get(filter: x => x.codigo == codigo && x.nivel == nivel);
            if (!(toCuenta.Count() > 0))
                msj = "La cuenta con el codigo " + codigo + " No existe en el catalogo de cuenta ó no es de nivel correspondiente al reporte, favor ingresar un codigo de cuenta valido";

            return Json(msj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult validarCierresExistentes(string fechaTrans, int? moduloMCAId = 0)
        {
            string msj = "";
            DateTime fechaTransaccion = DateTime.Parse(fechaTrans);

            moduloMCAId = moduloMCAId == 0 ? UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name) : moduloMCAId;

            var cierre = new CierresBL().Get(filter: x => x.moduloMCAId == moduloMCAId && (x.estadoId == EstadosBL.KEY_EJECUTADO || x.estadoId == EstadosBL.KEY_ASENTADO) && x.fechaTransaccion.Month == fechaTransaccion.Month && x.fechaTransaccion.Year == fechaTransaccion.Year).FirstOrDefault();

            if (cierre == null)
                msj = "NO EXISTEN CALCULO DE SALDOS/CIERRE REGISTRADOS";
            else
            {
                msj = new CierresBL().fechaCierreValidacion(moduloMCAId.Value, fechaTransaccion);

                //var toModulo = repoModulosMCA.GetByID(moduloMCAId);
                //var objAcuerdo = toModulo.AcuerdoSupMCA;

                //if (objAcuerdo != null)
                //    if (objAcuerdo.Count() > 0)
                //    {
                //        if (objAcuerdo.Max().fechaFinal.Month == (fechaTransaccion.Month + 1) && objAcuerdo.Max().fechaFinal.Year == fechaTransaccion.Year && cierre.tipoCierreId != 3)
                //            msj = "LA FECHA SELECCIONADA PERTENECE AL CIERRE CONTABLE SEGUN ACUERDOS SUPLEMENTARIO, REALICE CALCULO DE SALDOS PARA CIERRE CONTABLE";
                //    }
                //    else
                //    {
                //        if (toModulo.fechaFin.Month == (fechaTransaccion.Month+1) && toModulo.fechaFin.Year == fechaTransaccion.Year && cierre.tipoCierreId != 3)
                //            msj = "LA FECHA SELECCIONADA PERTENECE AL CIERRE CONTABLE, REALICE CALCULO DE SALDOS PARA CIERRE CONTABLE";
                //    }
            }

            return Json(msj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RevertirCierre(ReversionCierres reversionCierre, int moduloMCAId = 0)
        {
            string msj = "";
            //int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);
            // int moduloMCAId = 0;
            //  moduloMCAId = int.Parse(collection["moduloMCAId"]);
            if (moduloMCAId > 0)
            {
                int usuarioId = UsuariosASPBL.GetUsuario(User.Identity.Name.ToString()).usuarioId.Value;
                msj = CierresBL.RevertirCierre(reversionCierre, moduloMCAId, usuarioId);
            }
            else
                msj = "Seleccione un problema al realizar la reversion con el MCA seleccionado";


            //using (var contabilidadMCA = new ContabilidadMCA())
            //{
            //    using (var transaction = contabilidadMCA.Database.BeginTransaction())
            //    {
            //        try
            //        {
            //            var repoCierre = new CierresBL();

            //            var repoReversion = new ReversionCierreBL();
            //            var cierres = repoCierre.Get(filter: x => x.cierreId >= reversionCierre.cierreInicio && x.estadoId != 2);

            //            foreach (Cierres c in cierres) //ANULO TODOS LOS CIERRE DESPUES DEL CIERRE SELECCIONADO
            //            {

            //                c.estadoId = EstadosBL.KEY_ANULADO;
            //                c.fecha = DateTime.Now;
            //                repoCierre.Update(c);
            //                if (c.tipoCierreId == 2)//SE TIENEN QUE ELIMINAR LOS COMPROBANTES DE RESULTADO DEL EJERCICIO
            //                {
            //                    var repoComprobanteBL = new ComprobantesBL();
            //                    var comprobantes = repoComprobanteBL.Get(filter: x => x.tipoComprobanteId == 3 || x.tipoComprobanteId == 4).Where(s => s.ComprobanteCuentas.Select(k => k.Cuentas.moduloMCAId).FirstOrDefault() == moduloMCAId);

            //                    foreach (Comprobantes comp in comprobantes)
            //                    {
            //                        comp.estadoId = 2;
            //                        repoComprobanteBL.Update(comp);
            //                    }
            //                }
            //            }

            //            reversionCierre.cierreFin = cierres.LastOrDefault().cierreId;
            //            reversionCierre.estadoId = EstadosBL.KEY_EJECUTADO;
            //            reversionCierre.fecha = DateTime.Now;
            //            reversionCierre.fechaTransaccion = DateTime.Now;
            //            reversionCierre.razonReversion = reversionCierre.razonReversion;
            //            reversionCierre.solicitante = UsuariosASPBL.GetUsuario(User.Identity.Name.ToString()).usuarioId;

            //            repoReversion.Add(reversionCierre);
            //            msj = "LA REVERSION DEL CIERRE SE EJECUTO SATISFACTORIAMENTE";
            //            transaction.Commit();
            //        }
            //        catch (Exception exception)
            //        {
            //            transaction.Rollback();
            //            Console.WriteLine("Transaction Roll backed due to some exception");
            //            return false;
            //        }
            //    }
            //}

            return Json(msj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCierres(int moduloMCAId = 0)
        {
            var cierres = repoCierre.Get(filter: x => x.estadoId == EstadosBL.KEY_ASENTADO && x.tipoCierreId == 1 && x.moduloMCAId == moduloMCAId).Select(s => new { cierreId = s.cierreId, cierre = "Asentado Mes: " + s.fechaTransaccion.Month.ToString() + " - Año: " + s.fechaTransaccion.Year.ToString() });
            //var modulos = new SelectList(repoModulosMCA.GetAll(), "moduloMCAId", "nombreModuloMCA");

            return Json(cierres, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AnularComprobante(int comprobanteId = 0, string razonAnulacion="")
        {
            string msj = "";
            bool estado = false;


            estado = ComprobantesBL.ValidarComprobanteCierre(comprobanteId);

            if (estado == true)
                msj = ComprobantesBL.AnularComprobante(comprobanteId, razonAnulacion);
            else
                msj = "El comprobante pertenece a un periodo ya asentado, no se puede anular";

            
            return Json(msj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLastReinicioCheque(string reinicio)
        {
            string msj = "";
            int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);
            int numReinicio = 0;
            numReinicio = repoReinicioCheque.Filter(x => x.moduloMCAId == moduloMCAId).Select(s => s.numero).LastOrDefault();

            if (numReinicio >= int.Parse(reinicio))
                msj = "El numero ingredado " + reinicio + " es menor o igual al ultimo reinicio de cheque el cual es: " + numReinicio.ToString();

            return Json(msj, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Vitas Parciales
        public ActionResult TListCatalogoCuentas()
        {
            //Obtener el ID MCA correspondiente al usuario ingresado
            List<Cuentas> toCuentas = new List<Cuentas>();
            var user = UsuariosASPBL.GetUsuario(User.Identity.Name);
            int usuarioId = user.usuarioId.Value;

            if (usuarioId > 0)
            {
                UsermoduloMCA = repoUsuarioModulosMCA.Get(filter: x => x.usuarioId == usuarioId && x.estadoId == EstadosBL.KEY_ACTIVO).FirstOrDefault();
                if (UsermoduloMCA != null)
                    toCuentas = repoCuentas.Get(filter: x => x.moduloMCAId == UsermoduloMCA.moduloMCAId && x.estadoId ==EstadosBL.KEY_ACTIVO).ToList();

            }
            return PartialView("_TListCatalogoCuentas", toCuentas);
        }

        public ActionResult ListComprobantes()
        {

            int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);

            //if (UsermoduloMCA != null)
            //    moduloMCAId = UsermoduloMCA.moduloMCAId;

            //var toComprobante = repoComprobantes.Get(filter: x => x.ComprobanteCuentas.Select(s => s.Cuentas.moduloMCAId).FirstOrDefault() == moduloMCAId && x.tipoComprobanteId != 3 && x.tipoComprobanteId != 4 && x.estadoId==3);
            IEnumerable<Comprobantes> toComprobante;

            if (moduloMCAId > 0)
            {
                var listComp = repoComprobanteCuentas.Get(x => x.Cuentas.moduloMCAId == moduloMCAId);
                 toComprobante = listComp.Where(s => s.Comprobantes.tipoComprobanteId != 3 && s.Comprobantes.tipoComprobanteId != 4 && s.Comprobantes.estadoId == 3).Select(x => x.Comprobantes).Distinct();

                // toComprobante = repoComprobantes.Get(filter: x => x.ComprobanteCuentas.Select(s => s.Cuentas.moduloMCAId).FirstOrDefault() == moduloMCAId && x.tipoComprobanteId != 3 && x.tipoComprobanteId != 4 && x.estadoId == 3).OrderBy(x=>x.fechaComprobante).OrderBy(x => x.tipoComprobanteId); 
            }
            else
                toComprobante = null;

            return PartialView("_ListComprobantes", toComprobante);
        }
        #endregion

    }
}