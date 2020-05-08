using SistContabilidadMCA.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicMCA;
using DataLayerMCA;
using System.Globalization;

namespace SistContabilidadMCA.Areas.Informes.Controllers
{
    [Authorize]
    public class TipoReportesController : Controller
    {

        #region Instancias
        UsuarioModulosMCABL repoUsuarioModulosMCA = new UsuarioModulosMCABL();
        ModulosMCABL repoModulosMCA = new ModulosMCABL();
        CuentasBL repoCuentas = new CuentasBL();
        ConciliacionBancariasBL repoConciliacion = new ConciliacionBancariasBL();
        CierreCuentasBL repoCierreCuenta = new CierreCuentasBL();
        CierresBL repoCierre = new CierresBL();

        #endregion

        #region HTTP

        // GET: Informes/TipoReportes
        public ActionResult Index(int? keyReport)
        {
            var valores = SelectionVista(keyReport ?? 0);

            ViewBag.keyReporte = keyReport ?? 0;
            ViewBag.vista = valores.First().Key;
            ViewBag.js = valores.First().Value;
  
            //puedo obtener el modulo
            int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);

            
            //Se obtien el tipo de Usuario para realizar el filtro en el rol consulta
            var userModulos = UsuarioModulosMCABL.GetUsuariosModulos(User.Identity.Name); //.Select(x=>x.tipoUsuarioId).FirstOrDefault();
            

            if (User.IsInRole("CONSULTA") || User.IsInRole("PROMOTOR") || User.IsInRole("ADMIN-MCA"))
            {
                if (userModulos.Count() >0)
                {
                    if (userModulos.Select(x => x.tipoUsuarioId).FirstOrDefault() == 1 || userModulos.Select(x => x.tipoUsuarioId).FirstOrDefault() == 3)
                        ViewBag.SelectMCA = new SelectList(repoModulosMCA.GetAll().Where(x => x.moduloMCAId == moduloMCAId), "moduloMCAId", "nombreModuloMCA");
                }else
                    ViewBag.SelectMCA = new SelectList(repoModulosMCA.GetAll(), "moduloMCAId", "nombreModuloMCA");
            }

            if (moduloMCAId == 0 && User.IsInRole("CONTADOR"))
                return RedirectToAction("Index", "Home", new { Area = "" });


            var cierres = repoCierre.Get(filter: x=>x.estadoId!=EstadosBL.KEY_ANULADO && x.mes==12 && x.tipoCierreId==2 && x.moduloMCAId==moduloMCAId).LastOrDefault(); //Cierre Anual
                ViewBag.cierreAnual = (cierres != null)  ? 2 : 1;


            return View();
        }


        private Dictionary<string, string> SelectionVista(int keyReport)
        {
            /*** Seleccionar cual vista parcial se mandar a llamar , junto con su archivo javascript si lo requiere ***/
            var value = new Dictionary<string, string>(); /*** Nombre de Vista Parcial, Ruta de arvhivo javascritp a usar ***/
            string pathJs = "~/Areas/Informes/Scripts/";

            switch (keyReport)
            {
                case 0: value.Add("_BalanceGeneral", pathJs + "BalanceGeneral.js");
                    ViewBag.titulo = "BALANCE GENERAL";
                    break; /* Vista de _BalanceGeneral */
                case 1: value.Add("_EstadoResultado", pathJs + "EstadoResultado.js");
                    ViewBag.titulo = "ESTADO DE RESULTADOS";
                    break; /* Vista de _EstadoResultado */
                case 2: value.Add("_SituacionFinanciera", pathJs + "SituacionFinanciera.js");
                    ViewBag.titulo = "ESTADO DE LA SITUACION FINANCIERA";
                    break; /* Vista de _SituacionFinanciera */
                case 3: value.Add("_BalanzaComprobacion", pathJs + "BalanzaComprobacion.js");
                    ViewBag.titulo = "BALANZA DE COMPROBACIÓN";
                    break; /* Vista de _LicenciasxActividad */
                case 4:
                    value.Add("_AuxiliarDetalle", pathJs + "AuxiliarDetalle.js");
                    ViewBag.titulo = "REPORTE DE AUXILIAR";
                    break;
                case 5:
                    value.Add("_AuxiliarMayor", pathJs + "AuxiliarMayor.js");
                    ViewBag.titulo = "REPORTE DE AUXILIAR MAYOR";
                    break;
                case 6:
                    value.Add("_LibroDiario", pathJs + "LibroDiario.js");
                    ViewBag.titulo = "REPORTE DE LIBRO DIARIO";
                    break;
                case 8:
                    value.Add("_ConciliacionBancaria", pathJs + "ConciliacionBancaria.js");
                    ViewBag.titulo = "REPORTE DE CONCILIACION BANCARIA";
                    break;

                case 9:
                    value.Add("_SituacionFinanciera", pathJs + "SituacionFinanciera.js");
                    ViewBag.titulo = "REPORTE DE SITUACION FINANCIERA";
                    break;
            }
            return value;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection collection)
        {
            /** Obtener cual vista parcial ejecuta la accion para llamar al reporte correspondiente segun parametros **/
            int keyReporte = int.Parse(collection["keyReporte"]);
            int tipoExp = int.Parse(collection["tipoExportacion"]);

            
            int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);

            if (User.IsInRole("CONSULTA") || User.IsInRole("PROMOTOR") || User.IsInRole("ADMIN-MCA"))
            {
                moduloMCAId = int.Parse(collection["moduloMCAId"]);
            }
            var tousuarioContador = repoUsuarioModulosMCA.Get(filter: x => x.moduloMCAId == moduloMCAId && x.estadoId == EstadosBL.KEY_ACTIVO && x.tipoUsuarioId == 2).Select(x => new { nombres = x.Usuarios.nombres + " " + x.Usuarios.apellidos }).FirstOrDefault();
            var tousuarioTesorero = repoUsuarioModulosMCA.Get(filter: x => x.moduloMCAId == moduloMCAId && x.estadoId == EstadosBL.KEY_ACTIVO && x.tipoUsuarioId == 3).Select(x => new { nombres = x.Usuarios.nombres + " " + x.Usuarios.apellidos }).FirstOrDefault();
            var tousuarioPresidente = repoUsuarioModulosMCA.Get(filter: x => x.moduloMCAId == moduloMCAId && x.estadoId == EstadosBL.KEY_ACTIVO && x.tipoUsuarioId == 1).Select(x => new { nombres = x.Usuarios.nombres + " " + x.Usuarios.apellidos }).FirstOrDefault();

            var toModulo = repoModulosMCA.GetByID(moduloMCAId);
            var tocuenta = repoCuentas.Get(filter: x => x.moduloMCAId == moduloMCAId && x.Cuentabanco == true).Select(x => new { x.descripcion }).FirstOrDefault();
            var toFechas = ModulosMCABL.GetFechas(DateTime.Parse(collection["fecha1"].ToString())).Select(x => new { x.fechaInicial, x.fechaFinal, x.dia, x.mesMov,x.mes, x.ano}).FirstOrDefault();

            DateTime fechaIni = DateTime.Parse(collection["fecha1"].ToString());
            //TIPO DE REPORTE
            
            int tipoCierreId = int.Parse(collection["cierreAnualInput"].ToString());


            if (fechaIni.Month != 12) 
                tipoCierreId = 1;

            //preguntar si el el mes a ejecutar es el mismo del cierre de la contabilidad, **********Validacion Estara en analisis.********************
            //if (toModulo.fechaFin.Month == fechaIni.Month && toModulo.fechaFin.Year == fechaIni.Year)
            //    tipoCierreId = 3;

            var cierre = repoCierre.Get(filter: x => x.moduloMCAId == moduloMCAId && x.fechaTransaccion.Month == fechaIni.Month && x.fechaTransaccion.Year == fechaIni.Year && x.estadoId!=EstadosBL.KEY_ANULADO && x.tipoCierreId==tipoCierreId).Select(x => new { x.cierreId }).LastOrDefault();
            //Nuevo COD
            int cierreId = cierre.cierreId;
            //int cierreId = 0;
            //cierreId = cierre != null ? cierre.cierreId : 0;

            


            switch (keyReporte)
            {
                case 0:
                    var rptBalanceGeneral = new RepBalanceGeneralA();

                    //CALCULAR SALDO PATRIMONIO
                    double ingresoMes = 0, gastosMes = 0, ingresoInicial = 0, gastosInicial = 0, ingresoFinal = 0, gastosFinal = 0;

                    var objCierreIngreso = repoCierreCuenta.Get(filter: x => x.cierreId == cierreId && x.Cuentas.nivel == 1 && x.Cuentas.codigo == "4"); //repoComprobanteCuenta.Get(filter: x => x.Cuentas.moduloMCAId == moduloMCAId && x.Cuentas.nivel == 5 && x.Cuentas.codigo.Split('-')[0] == "4"); //4
                    var objCierreGasto = repoCierreCuenta.Get(filter: x => x.cierreId == cierreId && x.Cuentas.nivel == 1 && x.Cuentas.codigo == "5");
                    //SE UNIFICARAN LOS SALDOS DE INGRESO Y PATRIMONIO PARA OBTENER EL DEL MES Y EL ACUMULADO
                 
                    ingresoMes = objCierreIngreso.Sum(x => x.credito) - objCierreIngreso.Sum(x => x.debito);
                    gastosMes =  objCierreGasto.Sum(x => x.debito)- objCierreGasto.Sum(x => x.credito);

                    ingresoInicial = objCierreIngreso.Sum(x => x.saldoInicialCredito) - objCierreIngreso.Sum(x => x.saldoInicialDebito);
                    gastosInicial = objCierreGasto.Sum(x => x.saldoInicialDebito) - objCierreGasto.Sum(x => x.saldoInicialCredito);

                    ingresoFinal = objCierreIngreso.Sum(x => x.saldoFinalCredito) - objCierreIngreso.Sum(x => x.saldoFinalDebito);
                    gastosFinal = objCierreGasto.Sum(x => x.saldoFinalDebito) - objCierreGasto.Sum(x => x.saldoFinalCredito);

                    /**********************************************************
                    *OBTENER EL PATRIMONIO
                    ***********************************************************/
                
                    double ResultadoEjerAntMes=0, ResultadoEjerAntInicio=0, ResultadoEjerAntFin=0,saldoPatrimonioD = 0, saldoPatrimonioC = 0, saldoPatrimonioDAcum = 0, saldoPatrimonioCAcum = 0, saldoPatrimonioDFin = 0, saldoPatrimonioCFin = 0; ;
                    var patrimonioG = new CierreCuentasBL().Get(filter: x => x.cierreId == cierreId && x.Cuentas.nivel == 5 && (x.Cuentas.codigo == "3-1-1-01-01" || x.Cuentas.codigo == "3-1-1-02-01"));

                    var patrimonioAnt = new CierreCuentasBL().Get(filter: x => x.cierreId == cierreId && x.Cuentas.nivel == 5 && (x.Cuentas.codigo == "3-1-1-02-01")).LastOrDefault();

                    if (patrimonioAnt != null)
                    {
                         ResultadoEjerAntMes = patrimonioAnt.credito - patrimonioAnt.debito;
                         ResultadoEjerAntInicio = patrimonioAnt.saldoInicialCredito - patrimonioAnt.saldoInicialDebito;
                         ResultadoEjerAntFin = patrimonioAnt.saldoFinalCredito - patrimonioAnt.saldoFinalDebito;
                    }


                    if (patrimonioG!=null)
                    {
                        saldoPatrimonioC = patrimonioG.Sum(x=>x.credito);
                        saldoPatrimonioD= patrimonioG.Sum(x => x.debito);
                     
                        saldoPatrimonioCAcum = patrimonioG.Sum(x => x.saldoInicialCredito);
                        saldoPatrimonioDAcum = patrimonioG.Sum(x => x.saldoInicialDebito);

                        saldoPatrimonioCFin = patrimonioG.Sum(x => x.saldoFinalCredito);
                        saldoPatrimonioDFin = patrimonioG.Sum(x => x.saldoFinalDebito);
                    }

                    //VALIDAR BIEN ESTAS FORMULAS
                    double vPatrimonioMes = (ingresoMes - saldoPatrimonioD) - (gastosMes - saldoPatrimonioC);
                    double vPatrimonioInicio = (ingresoInicial - saldoPatrimonioDAcum) - (gastosInicial - saldoPatrimonioCAcum);
                    double vPatrimonioFin = (ingresoFinal - saldoPatrimonioDFin) - (gastosFinal - saldoPatrimonioCFin);                     ///************************* PENDIENTE **************************

                    double ResultadoEjercicioMes = ingresoMes - gastosMes;
                    double ResultadoEjercicioInicio = ingresoInicial  - gastosInicial;
                    double ResultadoEjercicioFin = ingresoFinal - gastosFinal;
                    
                    var objCierreIngresoAcum = repoCierreCuenta.Get(filter: x => x.cierreId == cierreId && x.Cuentas.nivel == 1 && x.Cuentas.codigo == "4"); //repoComprobanteCuenta.Get(filter: x => x.Cuentas.moduloMCAId == moduloMCAId && x.Cuentas.nivel == 5 && x.Cuentas.codigo.Split('-')[0] == "4"); //4
                    var objCierreGastoAcum = repoCierreCuenta.Get(filter: x => x.cierreId == cierreId && x.Cuentas.nivel == 1 && x.Cuentas.codigo == "5");



                    /**********************************************************
                    *CALCULO DE INGRESO Y GASTOS ANTERIORES PARA OBTENER EL PATRIMONIO
                    ***********************************************************/

                    rptBalanceGeneral.lblucr.Text = "MODULO COMUNITARIO DE ADOQUINADO MTI- " + toModulo.Proyectos.Ucrs.organismo;
                    rptBalanceGeneral.lblproyecto.Text = toModulo.Proyectos.nombreProyecto;
                    rptBalanceGeneral.lblTitulo.Text = toModulo.nombreModuloMCA;
                    rptBalanceGeneral.lblModulo.Text = "CONTRATO N° " + toModulo.numContrato;
                    //rptBalanceGeneral.lblMes.Text = "Mes " + toFechas.mesMov;
                    rptBalanceGeneral.lblUsuario.Text = User.Identity.Name;
                    //if (tipoCierreId == 2)
                    //{
                    //    rptBalanceGeneral.lblFechaInicial.Text = toFechas.fechaFinal;
                    //    rptBalanceGeneral.lblMesMov.Text = "Cierre Anual";
                    //    rptBalanceGeneral.lblFechaFinal.Text = "Cierre Anual";
                    //}
                    if (tipoCierreId == 2 || tipoCierreId == 3)
                    {
                        rptBalanceGeneral.lblFechaInicial.Text = toFechas.fechaFinal;
                        rptBalanceGeneral.lblMesMov.Text = tipoCierreId == 2 ? "Cierre Anual" : "Cierre Contable";
                        rptBalanceGeneral.lblFechaFinal.Text = tipoCierreId == 2 ? "Cierre Anual" : "Cierre Contable";

                    }
                    else
                    {
                        rptBalanceGeneral.lblFechaInicial.Text = toFechas.fechaInicial;
                        rptBalanceGeneral.lblMesMov.Text = toFechas.mesMov;
                        rptBalanceGeneral.lblFechaFinal.Text = toFechas.fechaFinal;
                    }
                    rptBalanceGeneral.lblContador.Text = tousuarioContador.nombres;
                    rptBalanceGeneral.lblTesorero.Text = tousuarioTesorero.nombres;
                    rptBalanceGeneral.lblPresidente.Text = tousuarioPresidente.nombres;
                    rptBalanceGeneral.lblFechas.Text = "Al  " + string.Format("{0}  de  {1} del {2}", toFechas.dia, toFechas.mes, toFechas.ano);
                    rptBalanceGeneral.Parameters["moduloMCAId"].Value = moduloMCAId;
                    rptBalanceGeneral.Parameters["fechaInicial"].Value = DateTime.Parse(collection["fecha1"].ToString());
                    rptBalanceGeneral.Parameters["fechaFinal"].Value = DateTime.Parse(collection["fecha1"].ToString());
                    rptBalanceGeneral.Parameters["cierreId"].Value = cierreId;
                    rptBalanceGeneral.Parameters["cierreAnual"].Value = tipoCierreId == 2 ? " (CON CIERRE ANUAL) " : "";
                    rptBalanceGeneral.Parameters["vPatrimonioMes"].Value = vPatrimonioMes; 
                    rptBalanceGeneral.Parameters["vPatrimonioInicio"].Value = vPatrimonioInicio; 
                    rptBalanceGeneral.Parameters["vPatrimonioFin"].Value = vPatrimonioFin;

                    rptBalanceGeneral.Parameters["ResultadoEjercicioMes"].Value = ResultadoEjercicioMes;
                    rptBalanceGeneral.Parameters["ResultadoEjercicioInicio"].Value = ResultadoEjercicioInicio;
                    rptBalanceGeneral.Parameters["ResultadoEjercicioFin"].Value = ResultadoEjercicioFin;

                    rptBalanceGeneral.Parameters["ResultadoEjerAntMes"].Value = ResultadoEjerAntMes;
                    rptBalanceGeneral.Parameters["ResultadoEjerAntInicio"].Value = ResultadoEjerAntInicio;
                    rptBalanceGeneral.Parameters["ResultadoEjerAntFin"].Value = ResultadoEjerAntFin;
                                        
                    Session["reporte"] = rptBalanceGeneral;
                    switch (tipoExp)
                    {
                        case 1: /***  Excel ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 1 });
                        case 2: /*** PDF ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 0 });
                    }
                    break;

                case 1:
                    var RepEstadoResultadoA = new RepEstadoResultadoA();
                  
                    RepEstadoResultadoA.lblucr.Text = "MODULO COMUNITARIO DE ADOQUINADO MTI- " + toModulo.Proyectos.Ucrs.organismo;
                    RepEstadoResultadoA.lblNombreProyecto.Text = toModulo.Proyectos.nombreProyecto;
                    RepEstadoResultadoA.lblTitulo.Text = "Del " + string.Format("{0}  al  {1}", DateTime.Parse(collection["fecha1"].ToString()).ToString("dd/MM/yyyy"), DateTime.Parse(collection["fecha2"].ToString()).ToString("dd/MM/yyyy"));
                    RepEstadoResultadoA.lblNombreModulo.Text = toModulo.nombreModuloMCA;
                    RepEstadoResultadoA.lblNumModulo.Text = toModulo.numContrato;
                    RepEstadoResultadoA.lblUsuario.Text = User.Identity.Name;
                    RepEstadoResultadoA.Parameters["moduloMCAId"].Value = moduloMCAId;
                    if(tipoCierreId == 2 || tipoCierreId == 3)
                    {
                        RepEstadoResultadoA.lblFechaInicial.Text = toFechas.fechaFinal;
                        RepEstadoResultadoA.lblMesMov.Text = tipoCierreId == 2 ? "Cierre Anual" : "Cierre Contable";
                        RepEstadoResultadoA.lblFechaFinal.Text = tipoCierreId==2 ? "Cierre Anual" : "Cierre Contable";

                    }
                    else
                    {
                        RepEstadoResultadoA.lblFechaInicial.Text = toFechas.fechaInicial;
                        RepEstadoResultadoA.lblMesMov.Text = toFechas.mesMov;
                        RepEstadoResultadoA.lblFechaFinal.Text = toFechas.fechaFinal;

                    }

                    RepEstadoResultadoA.lblContador.Text = tousuarioContador.nombres;
                    RepEstadoResultadoA.lblTesorero.Text = tousuarioTesorero.nombres;
                    RepEstadoResultadoA.lblPresidente.Text = tousuarioPresidente.nombres;
                    RepEstadoResultadoA.Parameters["fechaInicial"].Value = tipoCierreId==2 ? DateTime.Parse(collection["fecha2"].ToString()) : DateTime.Parse(collection["fecha1"].ToString());
                    RepEstadoResultadoA.Parameters["cierreAnual"].Value = tipoCierreId == 2 ? " (CON CIERRE ANUAL) " : "";
                    RepEstadoResultadoA.Parameters["fechaFinal"].Value = DateTime.Parse(collection["fecha2"].ToString());
                    RepEstadoResultadoA.Parameters["cierreId"].Value = cierreId;
                    Session["reporte"] = RepEstadoResultadoA;
                    switch (tipoExp)
                    {
                        case 1: /*** Microsoft Excel ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 1 });
                        case 2: /*** PDF ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 0 });
                    }
                    break;

               
                case 3:
                   
                    var rptBalanzaComprobacion = new rptBalanzaComprobacion();

                    var toTotalesBalanza = BalanzaComprobacionBL.GetTotalesBalanza(moduloMCAId, DateTime.Parse(collection["fecha1"].ToString())).Select(x => new { x.TotalsaldoInicialDebito, x.TotalsaldoInicialCredito, x.Totaldebito, x.Totalcredito, x.TotalsaldoFinalDebito, x.TotalsaldoFinalCredito }).FirstOrDefault();

                    rptBalanzaComprobacion.lblfecha.Text = "BALANZA DE COMPROBACION " + (tipoCierreId == 2 ? " (CON CIERRE ANUAL) " : " AL " +  toFechas.fechaFinal);
                    rptBalanzaComprobacion.lblucr.Text = "MODULO COMUNITARIO DE ADOQUINADO MTI- " + toModulo.Proyectos.Ucrs.organismo;
                    rptBalanzaComprobacion.lblproyecto.Text = toModulo.Proyectos.nombreProyecto;
                    rptBalanzaComprobacion.lblTitulo.Text = toModulo.nombreModuloMCA;
                    rptBalanzaComprobacion.lbldepto.Text = toModulo.Departamentos.departamento;
                    rptBalanzaComprobacion.lblmunicipio.Text = toModulo.Municipios.municipio;
                    rptBalanzaComprobacion.lblModulo.Text = toModulo.numContrato;
                    rptBalanzaComprobacion.lblUsuario.Text = User.Identity.Name;

                    if (tipoCierreId == 2 || tipoCierreId == 3)
                    {
                        rptBalanzaComprobacion.lblFechaInicial.Text = toFechas.fechaFinal;
                        rptBalanzaComprobacion.lblMesMov.Text = tipoCierreId == 2 ? "Cierre Anual" : "Cierre Contable";
                        rptBalanzaComprobacion.lblFechaFinal.Text = tipoCierreId == 2 ? "Cierre Anual" : "Cierre Contable";

                    }
                    else
                    {
                        rptBalanzaComprobacion.lblFechaInicial.Text = toFechas.fechaInicial;
                        rptBalanzaComprobacion.lblMesMov.Text = toFechas.mesMov;
                        rptBalanzaComprobacion.lblFechaFinal.Text = toFechas.fechaFinal;
                    }

                    rptBalanzaComprobacion.lblContador.Text = tousuarioContador.nombres;
                    rptBalanzaComprobacion.lblTesorero.Text = tousuarioTesorero.nombres;
                    rptBalanzaComprobacion.lblPresidente.Text = tousuarioPresidente.nombres;
                    rptBalanzaComprobacion.Parameters["moduloMCAId"].Value = moduloMCAId;
                    rptBalanzaComprobacion.Parameters["fecha"].Value = DateTime.Parse(collection["fecha1"].ToString());
                    rptBalanzaComprobacion.Parameters["cierreId"].Value = cierreId;
                                      

                    Session["reporte"] = rptBalanzaComprobacion;
                    switch (tipoExp)
                    {
                        case 1: /*** Microsoft Excel ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 1 });
                        case 2: /*** PDF ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 0 });
                    }
                    break;

                case 4: ///REP AUXILIAR DETALLE --*AS

                    var repAuxiliarDetalle = new RepAuxiliarDetalle();
                    
                    repAuxiliarDetalle.lblUsuario.Text = User.Identity.Name;
                 

                    repAuxiliarDetalle.Parameters["moduloMCAId"].Value = moduloMCAId;

                    if (tipoCierreId == 2 || tipoCierreId == 3)
                    {
                        repAuxiliarDetalle.lblFechaInicial.Text = toFechas.fechaFinal;
                        repAuxiliarDetalle.lblFechaFinal.Text = tipoCierreId == 2 ? "Cierre Anual" : "Cierre Contable";
                        repAuxiliarDetalle.lblCierreAnual.Text = tipoCierreId == 2 ? "(con Cierre Anual)" : "(con Cierre Contable)";
                    }
                    else
                    {
                        repAuxiliarDetalle.lblFechaInicial.Text = DateTime.Parse(collection["fecha1"].ToString()).ToString("dd/MM/yyyy");
                        repAuxiliarDetalle.lblFechaFinal.Text = DateTime.Parse(collection["fecha2"].ToString()).ToString("dd/MM/yyyy");
                    }

                        repAuxiliarDetalle.Parameters["fechaInicial"].Value = DateTime.Parse(collection["fecha1"].ToString());
                    repAuxiliarDetalle.Parameters["fechaFinal"].Value = DateTime.Parse(collection["fecha2"].ToString());//DateTime.Parse(collection["fecha2"].ToString()).ToString("dd/MM/yyyy");

                    repAuxiliarDetalle.Parameters["codigo1"].Value =collection["codigo1"].ToString();
                    repAuxiliarDetalle.Parameters["codigo2"].Value = collection["codigo2"].ToString();
                    repAuxiliarDetalle.lblContador.Text = tousuarioContador.nombres;
                    repAuxiliarDetalle.lblTesorero.Text = tousuarioTesorero.nombres;
                    repAuxiliarDetalle.lblPresidente.Text = tousuarioPresidente.nombres;
                    if (collection["todas"].ToString()=="1")
                    {
                        repAuxiliarDetalle.Parameters["todas"].Value = true;
                    }
                    else
                    {
                        repAuxiliarDetalle.Parameters["todas"].Value = false;
                        repAuxiliarDetalle.Parameters["cuentasDelAl"].Value = "De la Cuenta " + collection["codigo1"].ToString() + " A la Cuenta " + collection["codigo2"].ToString();
                    }
                   

                    Session["reporte"] = repAuxiliarDetalle;
                    switch (tipoExp)
                    {
                        case 1: /*** Microsoft Excel ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 1 });
                        case 2: /*** PDF ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 0 });
                    }
                    break;

                case 5: ///REP AUXILIAR DETALLE --*AS

                    var repAuxiliarMayor = new RepAuxiliarMayor();

                    repAuxiliarMayor.lblUsuario.Text = User.Identity.Name;
                 

                    if (tipoCierreId == 2 || tipoCierreId == 3)
                    {
                        repAuxiliarMayor.lblFechaInicial.Text = toFechas.fechaFinal;
                        repAuxiliarMayor.lblFechaFinal.Text = tipoCierreId == 2 ? "Cierre Anual" : "Cierre Contable";
                        repAuxiliarMayor.lblCierreAnual.Text = tipoCierreId == 2 ? "(con Cierre Anual)" : "(con Cierre Contable)";

                    }
                    else
                    {
                        repAuxiliarMayor.lblFechaInicial.Text = DateTime.Parse(collection["fecha1"].ToString()).ToString("dd/MM/yyyy");
                        repAuxiliarMayor.lblFechaFinal.Text = DateTime.Parse(collection["fecha2"].ToString()).ToString("dd/MM/yyyy");

                    }

                    repAuxiliarMayor.Parameters["fechaInicial"].Value = DateTime.Parse(collection["fecha1"].ToString());
                    repAuxiliarMayor.Parameters["fechaFinal"].Value = DateTime.Parse(collection["fecha2"].ToString());

                    repAuxiliarMayor.Parameters["moduloMCAId"].Value = moduloMCAId;
                    repAuxiliarMayor.Parameters["codigo1"].Value = collection["codigo1"].ToString();
                    repAuxiliarMayor.Parameters["codigo2"].Value = collection["codigo2"].ToString();
                    repAuxiliarMayor.lblContador.Text = tousuarioContador.nombres;
                    repAuxiliarMayor.lblTesorero.Text = tousuarioTesorero.nombres;
                    repAuxiliarMayor.lblPresidente.Text = tousuarioPresidente.nombres;
                   
                    if (collection["todas"].ToString() == "1")
                    {
                        repAuxiliarMayor.Parameters["todas"].Value = true;
                    }
                    else
                    {
                        repAuxiliarMayor.Parameters["todas"].Value = false;
                        repAuxiliarMayor.Parameters["cuentasDelAl"].Value = "De la Cuenta " + collection["codigo1"].ToString() + " A la Cuenta " + collection["codigo2"].ToString();
                    }
                    Session["reporte"] = repAuxiliarMayor;
                    switch (tipoExp)
                    {
                        case 1: /*** Microsoft Excel ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 1 });
                        case 2: /*** PDF ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 0 });
                    }
                    break;

                case 6: ///REP AUXILIAR DETALLE --*AS

                    var repLibroDiario = new RepLibroDiario();

                    repLibroDiario.lblUsuario.Text = User.Identity.Name;

                    if (tipoCierreId == 2 || tipoCierreId == 3)
                    {
                        repLibroDiario.lblFechaInicial.Text = toFechas.fechaFinal;
                        repLibroDiario.lblFechaFinal.Text = tipoCierreId == 2 ? "Cierre Anual" : "Cierre Contable";
                        repLibroDiario.lblCierreAnual.Text = tipoCierreId == 2 ? "(con Cierre Anual)" : "(con Cierre Contable)";

                    }
                    else
                    {
                        repLibroDiario.lblFechaInicial.Text = DateTime.Parse(collection["fecha1"].ToString()).ToString("dd/MM/yyyy");
                        repLibroDiario.lblFechaFinal.Text = DateTime.Parse(collection["fecha2"].ToString()).ToString("dd/MM/yyyy");
                    }
                    
                    repLibroDiario.lblUcr.Text = toModulo.Proyectos.Ucrs.organismo;
                    repLibroDiario.lblModuloMCA.Text = toModulo.nombreModuloMCA;
                    repLibroDiario.lblProyecto.Text = toModulo.Proyectos.nombreProyecto;
                    repLibroDiario.lblContador.Text = tousuarioContador.nombres;
                    repLibroDiario.lblTesorero.Text = tousuarioTesorero.nombres;
                    repLibroDiario.lblPresidente.Text = tousuarioPresidente.nombres;

                    repLibroDiario.Parameters["fechaInicial"].Value = DateTime.Parse(collection["fecha1"].ToString());
                    repLibroDiario.Parameters["fechaFinal"].Value = DateTime.Parse(collection["fecha2"].ToString());
                    repLibroDiario.Parameters["moduloMCAId"].Value = moduloMCAId;

                    Session["reporte"] = repLibroDiario;
                    switch (tipoExp)
                    {
                        case 1: /*** Microsoft Excel ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 1 });
                        case 2: /*** PDF ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 0 });
                    }
                    break;

                case 7: ///REP AUXILIAR DETALLE --*AS

                    var repComprobanteDiario = new RepComprobanteDiario();

                    repComprobanteDiario.lblUsuario.Text = User.Identity.Name;
                    
                    repComprobanteDiario.lblProyecto.Text = toModulo.Proyectos.nombreProyecto;
                    repComprobanteDiario.lblUcr.Text = toModulo.Proyectos.Ucrs.organismo;
                    repComprobanteDiario.lblModuloMCA.Text = toModulo.nombreModuloMCA;
                  
                        repComprobanteDiario.Parameters["fechaInicial"].Value = DateTime.Parse(collection["fecha1"].ToString());
                    repComprobanteDiario.Parameters["fechaFinal"].Value = DateTime.Parse(collection["fecha2"].ToString());
                    repComprobanteDiario.Parameters["moduloMCAId"].Value = moduloMCAId;


                    Session["reporte"] = repComprobanteDiario;
                    switch (tipoExp)
                    {
                        case 1: /*** Microsoft Excel ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 1 });
                        case 2: /*** PDF ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 0 });
                    }
                    break;

                case 8:

                    var rptConciliacionBancaria = new rptConciliacionBancaria();

                    rptConciliacionBancaria.lblTitulo.Text = toModulo.nombreModuloMCA;
                    rptConciliacionBancaria.lbldepto.Text = toModulo.Departamentos.departamento;
                    rptConciliacionBancaria.lblmunicipio.Text = toModulo.Municipios.municipio;
                    rptConciliacionBancaria.lblfechas.Text = toFechas.fechaFinal;
                    rptConciliacionBancaria.lbmodulo.Text = toModulo.numContrato;
                    rptConciliacionBancaria.lblCuenta.Text = tocuenta.descripcion;
                    rptConciliacionBancaria.lblUsuario.Text = User.Identity.Name;
                    rptConciliacionBancaria.Parameters["moduloMCAId"].Value = moduloMCAId;
                    Session["reporte"] = rptConciliacionBancaria;
                    switch (tipoExp)
                    {
                        case 1: /*** Microsoft Excel ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 1 });
                        case 2: /*** PDF ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 0 });
                    }
                    break;

                case 9: ///REP SITUACION FINANCIERA

                    var RepSituacionFinanciera = new RepSituacionFinanciera();

                    RepSituacionFinanciera.lblUsuario.Text = User.Identity.Name;
                  

                    RepSituacionFinanciera.Parameters["moduloMCAId"].Value = moduloMCAId;
                    if (tipoCierreId == 2 || tipoCierreId == 3)
                    {
                        RepSituacionFinanciera.lblFechaInicial.Text = toFechas.fechaFinal;
                        RepSituacionFinanciera.lblFechaFinal.Text = tipoCierreId == 2 ? "Cierre Anual" : "Cierre Contable";
                        RepSituacionFinanciera.lblCierreAnual.Text = tipoCierreId == 2 ? "(con Cierre Anual)" : "(con Cierre Contable)";

                    }
                    else
                    {
                        RepSituacionFinanciera.lblFechaInicial.Text = DateTime.Parse(collection["fecha1"].ToString()).ToString("dd/MM/yyyy");
                        RepSituacionFinanciera.lblFechaFinal.Text = DateTime.Parse(collection["fecha2"].ToString()).ToString("dd/MM/yyyy");
                    }

                    RepSituacionFinanciera.Parameters["fechaInicial"].Value = DateTime.Parse(collection["fecha1"].ToString());
                    RepSituacionFinanciera.Parameters["fechaFinal"].Value = DateTime.Parse(collection["fecha2"].ToString());

                    
                    DateTime fecha1 = DateTime.Parse(collection["fecha1"].ToString());
                   var saldoIniV = new ComprobanteCuentasBL().Get(filter: x => x.Cuentas.padre.padre.codigo == "1-1-1" && x.estadoId != 2
                    && x.estadoId != 5 && x.estadoId != 0 && x.Comprobantes.estadoId == EstadosBL.KEY_EJECUTADO && x.Cuentas.moduloMCAId == moduloMCAId && x.fechaTransaccion < fecha1).Sum(s=>s.debito-s.credito);

                    RepSituacionFinanciera.Parameters["saldoAnteriorC"].Value = saldoIniV;

                    

                    RepSituacionFinanciera.lblUcr.Text = toModulo.Proyectos.Ucrs.organismo;
                    RepSituacionFinanciera.lblModuloMCA.Text = toModulo.nombreModuloMCA;
                    RepSituacionFinanciera.lblProyecto.Text = toModulo.Proyectos.nombreProyecto;
                    RepSituacionFinanciera.lblContador.Text = tousuarioContador.nombres;
                    RepSituacionFinanciera.lblTesorero.Text = tousuarioTesorero.nombres;
                    RepSituacionFinanciera.lblPresidente.Text = tousuarioPresidente.nombres;

                    Session["reporte"] = RepSituacionFinanciera;
                    switch (tipoExp)
                    {
                        case 1: /*** Microsoft Excel ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 1 });
                        case 2: /*** PDF ***/
                            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 0 });
                    }
                    break;
                    
            }
            if (User.IsInRole("CONSULTA") || User.IsInRole("PROMOTOR"))
            {
                ViewBag.SelectMCA = new SelectList(repoModulosMCA.GetAll(), "moduloMCAId", "nombreModuloMCA");
            }
            return RedirectToAction("DownloadReport", "Reportes", new { Area = "" });
        }


       // [HttpPost]
     //   [ValidateAntiForgeryToken]
        public ActionResult GenerarComp(int tipoComprobanteId, int comprobanteId)
        {
            /** Obtener cual vista parcial ejecuta la accion para llamar al reporte correspondiente segun parametros **/
           
            int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);

            var toModulo = repoModulosMCA.GetByID(moduloMCAId);
         
            var tousuarioContador = repoUsuarioModulosMCA.Get(filter: x => x.moduloMCAId == moduloMCAId && x.estadoId == EstadosBL.KEY_ACTIVO && x.tipoUsuarioId == 2).Select(x => new { nombres = x.Usuarios.nombres + " " + x.Usuarios.apellidos }).FirstOrDefault();
            var tousuarioTesorero = repoUsuarioModulosMCA.Get(filter: x => x.moduloMCAId == moduloMCAId && x.estadoId == EstadosBL.KEY_ACTIVO && x.tipoUsuarioId == 3).Select(x => new { nombres = x.Usuarios.nombres + " " + x.Usuarios.apellidos }).FirstOrDefault();
            var tousuarioPresidente = repoUsuarioModulosMCA.Get(filter: x => x.moduloMCAId == moduloMCAId && x.estadoId == EstadosBL.KEY_ACTIVO && x.tipoUsuarioId == 1).Select(x => new { nombres = x.Usuarios.nombres + " " + x.Usuarios.apellidos }).FirstOrDefault();
         
            switch (tipoComprobanteId)
            {
                case 1: ///REP Comprobante de Diario

                    var repComprobanteDiario = new RepComprobanteDiario();

                    repComprobanteDiario.lblUsuario.Text = User.Identity.Name;
                    repComprobanteDiario.lblUcr.Text = toModulo.Proyectos.Ucrs.organismo;
                    repComprobanteDiario.lblProyecto.Text = toModulo.Proyectos.nombreProyecto;
                    repComprobanteDiario.lblModuloMCA.Text = toModulo.nombreModuloMCA;
                    repComprobanteDiario.Parameters["comprobanteId"].Value = comprobanteId;
                    repComprobanteDiario.lblContador.Text = tousuarioContador.nombres;
                    repComprobanteDiario.lblTesorero.Text = tousuarioTesorero.nombres;
                    repComprobanteDiario.lblPresidente.Text = tousuarioPresidente.nombres;
                    Session["reporte"] = repComprobanteDiario;              

                    break;

                case 2: ///REP Comprobante de Pago
                    var toComprobantes = ComprobantesBL.getComprobantePago(comprobanteId).Select(x => new { x.municipio, x.dia, x.mes, x.ano, x.beneficiario, x.numCheque, x.cantidad }).FirstOrDefault(); //, x.cantidaletra
                    var repComprobantePago = new RepComprobantePago();

                    repComprobantePago.lblUsuario.Text = User.Identity.Name;
                    repComprobantePago.lblUcr.Text = toModulo.Proyectos.Ucrs.organismo;
                    repComprobantePago.lblProyecto.Text = toModulo.Proyectos.nombreProyecto;
                    repComprobantePago.lblModuloMCA.Text = toModulo.nombreModuloMCA;
                    repComprobantePago.Parameters["comprobanteId"].Value = comprobanteId;
                    repComprobantePago.lblContador.Text = tousuarioContador.nombres;
                    repComprobantePago.lblTesorero.Text = tousuarioTesorero.nombres;
                    repComprobantePago.lblPresidente.Text = tousuarioPresidente.nombres;
                   // repComprobantePago.lblPresidente2.Text = tousuarioPresidente.nombres;
                    repComprobantePago.lblMunicipio.Text = toComprobantes.municipio;
                    //repComprobantePago.lblvalor.Text = toComprobantes.cantidaletra;
                    repComprobantePago.lblvalor.Text = Global.enletras(toComprobantes.cantidad.ToString());
                    repComprobantePago.lbldiac.Text = toComprobantes.dia;
                    repComprobantePago.lblmesc.Text = toComprobantes.mes;
                    repComprobantePago.lblanioc.Text = toComprobantes.ano;
                    repComprobantePago.lblBeneficiario.Text = toComprobantes.beneficiario;
                    repComprobantePago.lblmonto.Text = String.Format("{0:0,0.00}", toComprobantes.cantidad);
                    Session["reporte"] = repComprobantePago;

                    break;
            }
            return RedirectToAction("DownloadReport", "Reportes", new { Area = "" });
        }

        public ActionResult GenerarCatalogoCuenta(int moduloMCAId=0)
        {
            /** Obtener cual vista parcial ejecuta la accion para llamar al reporte correspondiente segun parametros **/

            try
            {

            
            moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);

            var toModulo = repoModulosMCA.GetByID(moduloMCAId);

            var repCatalogoCuentas = new RepCatalogoCuentas();

            repCatalogoCuentas.lblUsuario.Text = User.Identity.Name;
            repCatalogoCuentas.lblucr.Text = "MINISTERIO DE TRANSPORTE E INFRAESTRUCTURA UCR " + toModulo.Proyectos.Ucrs.ucr;
            repCatalogoCuentas.lblproyecto.Text = toModulo.Proyectos.nombreProyecto;
            repCatalogoCuentas.lblMCA.Text = toModulo.nombreModuloMCA;
            repCatalogoCuentas.Parameters["moduloMCAId"].Value = moduloMCAId;

            Session["reporte"] = repCatalogoCuentas;
            }
            catch (Exception)
            {
                                
            }

            return RedirectToAction("DownloadReport", "Reportes", new { Area = "" });
        }


        public ActionResult GenerarConciliacion(int conciliacionBancariaId)
        {


            int moduloMCAId = UsuarioModulosMCABL.GetModuloUsuario(User.Identity.Name);

            var toModulo = repoModulosMCA.GetByID(moduloMCAId);

            var toConciliacion = repoConciliacion.Get(filter: x => x.conciliacionBancariaId == conciliacionBancariaId).Select(x => new { x.cuentaId, x.fechaTransaccion }).FirstOrDefault();
        
            string fecha = toConciliacion.fechaTransaccion.ToString("dd/M/yyyy");
            int cuentaId = toConciliacion.cuentaId;
            var tocuenta = repoCuentas.Get(filter: x => x.cuentaId == cuentaId && x.Cuentabanco == true).Select(x => new { x.descripcion, x.ModulosMCA.nombreModuloMCA, x.ModulosMCA.numContrato, x.ModulosMCA.Municipios.municipio, x.ModulosMCA.Departamentos.departamento, x.ModulosMCA.Proyectos.nombreProyecto, x.ModulosMCA.Proyectos.Ucrs.organismo }).FirstOrDefault();
            var toSaldolibro = ConciliacionBancariasBL.GetOperacionC(conciliacionBancariaId).Select(x => new { x.resta }).FirstOrDefault();
        
            var tousuarioContador = repoUsuarioModulosMCA.Get(filter: x => x.moduloMCAId == moduloMCAId && x.estadoId == EstadosBL.KEY_ACTIVO && x.tipoUsuarioId == 2).Select(x => new { nombres = x.Usuarios.nombres + " " + x.Usuarios.apellidos }).FirstOrDefault();
            var tousuarioTesorero = repoUsuarioModulosMCA.Get(filter: x => x.moduloMCAId == moduloMCAId && x.estadoId == EstadosBL.KEY_ACTIVO && x.tipoUsuarioId == 3).Select(x => new { nombres = x.Usuarios.nombres + " " + x.Usuarios.apellidos }).FirstOrDefault();
            var tousuarioPresidente = repoUsuarioModulosMCA.Get(filter: x => x.moduloMCAId == moduloMCAId && x.estadoId == EstadosBL.KEY_ACTIVO && x.tipoUsuarioId == 1).Select(x => new { nombres = x.Usuarios.nombres + " " + x.Usuarios.apellidos }).FirstOrDefault();
            var rptConciliacionBancaria = new rptConciliacionBancaria();
            rptConciliacionBancaria.lblucr.Text = "MODULO COMUNITARIO DE ADOQUINADO MTI- " + tocuenta.organismo;
            rptConciliacionBancaria.lblproyecto.Text = tocuenta.nombreProyecto;
            rptConciliacionBancaria.lblTitulo.Text = tocuenta.nombreModuloMCA;
            rptConciliacionBancaria.lbldepto.Text = tocuenta.departamento;
            rptConciliacionBancaria.lblmunicipio.Text = tocuenta.municipio;
            rptConciliacionBancaria.lblfechas.Text = "CONCILIACIÓN BANCARIA AL " + fecha;
            rptConciliacionBancaria.lbmodulo.Text = tocuenta.numContrato;
            rptConciliacionBancaria.lblCuenta.Text = "CUENTA CORRIENTE N°" + tocuenta.descripcion;
            rptConciliacionBancaria.lblUsuario.Text = User.Identity.Name;
            rptConciliacionBancaria.lblContador.Text = tousuarioContador.nombres;
            rptConciliacionBancaria.lblTesorero.Text = tousuarioTesorero.nombres;
            rptConciliacionBancaria.lblPresidente.Text = tousuarioPresidente.nombres;
            if (toSaldolibro.resta == 0)
            {
                rptConciliacionBancaria.lbloperacion.Text = "(Más) PARTIDA DE CONCILIACION";
            }
            else
            {
                rptConciliacionBancaria.lbloperacion.Text = "(Menos) PARTIDA DE CONCILIACION";
            }
            rptConciliacionBancaria.Parameters["conciliacionBancariaId"].Value = conciliacionBancariaId;
            Session["reporte"] = rptConciliacionBancaria;

            return RedirectToAction("DownloadReport", "Reportes", new { Area = "", tipoExp = 0 });
        }

        #endregion

    }
}