using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayerMCA;
using Repository;
using BusinessLogicMCA;
using System.Data.Entity;

namespace BusinessLogicMCA
{
    public class ComprobanteCuentasBL : BaseRepository<ComprobanteCuentas>
    {
        #region
        /// <summary>
        /// CIERRE DE CUENTAS EN BASE A UN CIERRE CREACION DEL DETALLE DEL CIERRO O CALCULO DE SALDOS
        /// </summary>
        /// <param name="fechaSaldos"></param>
        /// <param name="moduloMCAId"></param>
        /// <param name="cierreId"></param>
        /// <returns></returns>
        public static bool CerrarCuentas(DateTime fechaSaldos, int moduloMCAId, int cierreId) //solicitar la fecha para evaluar el mes en cuestion
        {
            
            var toCuentas = new CuentasBL().Get(filter: x => x.moduloMCAId == moduloMCAId).ToList();  //OBTENER LISTADO DE CUENTAS MCA            
            int nivel = toCuentas.Max(x => x.nivel); //BUSCAR EL MAXIMO NIVEL DE LAS CUENTAS            
            var toPadres = toCuentas.Where(x => x.jerarquia != 0 && x.nivel == nivel); //FILTRA A LAS CUENTAS CON EL NIVEL MAS BAJO 
            var cierre = new CierresBL().GetByID(cierreId);

            while (toPadres.Count() > 0) //RECORRE MIENTRAS TENGA REGISTROS
            {

                foreach (Cuentas c2 in toPadres)
                {
                    double[] listSaldosP = new double[6];
                    var toHijas = new List<CierreCuentas>();   //VARIABLE QUE ALMACENA LAS CUENTAS HIJAS DE UNA CUENTA
                    var saldosC = new SaldosCuenta();

                    if (c2.hijos.Count > 0)
                    {
                        //CARGA TODOAS LAS CUENTAS HIJAS DE UNA CUENTA EN UN CIERRE DETERMINADO
                        toHijas = new CierreCuentasBL().Get(filter: x => x.cierreId == cierreId && x.Cuentas.padre.cuentaId == c2.cuentaId).ToList();

                        saldosC.saldoInicialCredito = toHijas.Sum(x => x.saldoInicialCredito);
                        saldosC.saldoInicialDebito = toHijas.Sum(x => x.saldoInicialDebito);
                        saldosC.saldoFinalCredito = toHijas.Sum(x => x.saldoFinalCredito);
                        saldosC.saldoFinalDebito = toHijas.Sum(x => x.saldoFinalDebito);
                        saldosC.credito = toHijas.Sum(x => x.credito);
                        saldosC.debito = toHijas.Sum(x => x.debito);

                    }
                    else  //SI NO TIENE CUENTAS HIJAS SE TOMARA LOS SALDOS DIRECTAMENTE DE LOS MOVIMIENTOS DE LA CUENTA ESTO PASA CON CUENTAS DETALLES O EL UTIMO NIVEL
                    {
                        /************ EN TEORIA ESTA VARIABLE NO ES NECESARIA*****************************/
                        //toHijas = new CierreCuentasBL().Get(filter: x => x.cierreId == cierreId && x.cuentaId == c2.cuentaId).ToList();
                        /*****************************************/
                        saldosC = saldosDetalle(fechaSaldos, c2, cierre.tipoCierreId); //CALCULO DE SALDOS DE LOS COMPROBANTES

                    }
                    //CREO QUE AQUI ES DONDE TENGO QUE SABER SI EL CIERRE ES OFICIAL O NO!!!!*************************
                    CerrarCuentaV2(fechaSaldos, c2.cuentaId, cierreId, saldosC, c2.ModulosMCA.moduloMCAId);

                }

                nivel = nivel - 1; //DESPUES DE RECORRE LAS CUENTAS EN BASE A UN NIVEL. DECREMENTO EL NIVEL PARA EVALUAR EL SIGUIENTE                       
                toPadres = toCuentas.Where(x => x.nivel == nivel); //ACTUALIZO EL OBJETO Y LO CARGO CON LOS DATOS DE CUENTA DEL SIGUIENTE NIVEL

            }

            return true;
        }

        /*********************************************/
        //public static bool CerrarCuentasPeriodo(DateTime fechaSaldos, int moduloMCAId, int cierreId) //solicitar la fecha para evaluar el mes en cuestion
        //{
        //    //OBTENER LISTADO DE CUENTAS MCA
        //    var toCuentas = new CuentasBL().Get(filter: x => x.moduloMCAId == moduloMCAId && x.nivel == 5).Where(x => x.codigo.Split('-')[0] == "4" || x.codigo.Split('-')[0] == "5").ToList();

        //    //CIERRE MENSUAL PONER EN CERO LAS CUENTAS DE INGRESO Y COSTO

        //    var repoComprobante = new ComprobantesBL();
        //    var repoComprobanteCuenta = new ComprobanteCuentasBL();

        //    //Comprobantes toComprobanteResult = new Comprobantes(); // CrarComprobanteCierre(moduloMCAId);
        //    //Comprobantes toComprobanteCosto = new Comprobantes(); // CrarComprobanteCierre(moduloMCAId);

        //   // var toComprobanteResult = CrearComprobante(moduloMCAId,fechaSaldos);

        //    //TENGO QUE SUMAR TODOS LOS COMPROBANTES DE LAS CUENTAS COSTOS Y GASTOS
        //    double ingresoD = 0, ingresoH = 0, gastoD = 0, gastoH = 0;

        //    var comprobanteIngreso = repoComprobanteCuenta.Get(filter: x => x.Cuentas.moduloMCAId == moduloMCAId && x.Cuentas.nivel == 5 && x.Cuentas.codigo.Split('-')[0] == "4"); //4
        //    var comprobanteGasto = repoComprobanteCuenta.Get(filter: x => x.Cuentas.moduloMCAId == moduloMCAId && x.Cuentas.nivel == 5 && x.Cuentas.codigo.Split('-')[0] == "5"); //5
        //    //ingreso= ingresox.
        //    ingresoD = comprobanteIngreso.Sum(x => x.debito);
        //    ingresoH = comprobanteIngreso.Sum(x => x.credito);

        //    gastoD = comprobanteGasto.Sum(x => x.debito);
        //    gastoH = comprobanteGasto.Sum(x => x.credito);



        //    /***************************************************************************/
        //    foreach (Cuentas c in toCuentas)
        //    {
        //        //COMPROBANTE DE CUENTA INGRESO O COSTO
        //        var operacionDC = c.ComprobanteCuentas.Where(x => x.fechaTransaccion.Year == fechaSaldos.Year).Sum(x => x.debito) - c.ComprobanteCuentas.Where(x => x.fechaTransaccion.Year == fechaSaldos.Year).Sum(x => x.credito);

        //        ComprobanteCuentas toComprobanteCuenta = new ComprobanteCuentas();
        //        ComprobanteCuentas toComprobanteCuentaRE = new ComprobanteCuentas();

        //        if (operacionDC != 0)
        //        {

        //            toComprobanteCuenta.cuentaId = c.cuentaId;
        //            toComprobanteCuenta.comprobanteId = toComprobanteResult.comprobanteId;
        //            if (operacionDC > 0)
        //            {
        //                toComprobanteCuenta.credito = operacionDC;
        //                toComprobanteCuenta.debito = 0;
        //                //Cuenta Resultado del Ejercicio
        //                toComprobanteCuentaRE.credito = 0;
        //                toComprobanteCuentaRE.debito = operacionDC;
        //            }
        //            else
        //            {
        //                toComprobanteCuenta.credito = 0;
        //                toComprobanteCuenta.debito = operacionDC;
        //                //Cuenta Resultado del Ejercicio
        //                toComprobanteCuentaRE.credito = operacionDC;
        //                toComprobanteCuentaRE.debito = 0;
        //            }

        //            toComprobanteCuenta.estadoId = 3;
        //            toComprobanteCuenta.fecha = DateTime.Now;
        //            toComprobanteCuenta.fechaTransaccion = fechaSaldos;  //--------------------********************************
        //            repoComprobanteCuenta.Add(toComprobanteCuenta);

        //            toComprobanteCuentaRE.comprobanteId = toComprobanteResult.comprobanteId;
        //            toComprobanteCuentaRE.cuentaId = new CuentasBL().Get(x => x.moduloMCAId == moduloMCAId && x.nivel == 5 && x.codigo == "3-1-1-01-01").Select(x => x.cuentaId).FirstOrDefault(); //  56; //CUENTA RESULTADO DEL EJERCICIO

        //            toComprobanteCuentaRE.estadoId = 3;
        //            toComprobanteCuentaRE.fecha = DateTime.Now;
        //            toComprobanteCuentaRE.fechaTransaccion = fechaSaldos;  //--------------------********************************
        //            repoComprobanteCuenta.Add(toComprobanteCuentaRE);


        //        }
        //    }

        //    return true;
        //}

        //public static bool CerrarContabilidad(DateTime fechaSaldos, int moduloMCAId, int cierreId) //solicitar la fecha para evaluar el mes en cuestion
        //{
        //    //OBTENER LISTADO DE CUENTAS MCA
        //    var toCuentas = new CuentasBL().Get(filter: x => x.moduloMCAId == moduloMCAId && x.nivel == 5).Where(x => x.codigo.Split('-')[0] == "4" || x.codigo.Split('-')[0] == "5").ToList();

        //    //CIERRE MENSUAL PONER EN CERO LAS CUENTAS DE INGRESO Y COSTO

        //    var repoComprobante = new ComprobantesBL();
        //    var repoComprobanteCuenta = new ComprobanteCuentasBL();

        //    Comprobantes toComprobanteResult = new Comprobantes(); // CrarComprobanteCierre(moduloMCAId);


        //  //  toComprobanteResult = CrearComprobante(moduloMCAId,fechaSaldos);//*******borrar esto!!!!!!!!!!!


        //    foreach (Cuentas c in toCuentas)
        //    {
        //        //COMPROBANTE DE CUENTA INGRESO O COSTO
        //        var operacionDC = c.ComprobanteCuentas.Where(x => x.fechaTransaccion.Year == fechaSaldos.Year).Sum(x => x.debito) - c.ComprobanteCuentas.Where(x => x.fechaTransaccion.Year == fechaSaldos.Year).Sum(x => x.credito);
        //        ComprobanteCuentas toComprobanteCuentaRE = new ComprobanteCuentas();
        //        ComprobanteCuentas toComprobanteCuenta = new ComprobanteCuentas();
        //        if (operacionDC != 0)
        //        {

        //            toComprobanteCuenta.cuentaId = c.cuentaId;
        //            toComprobanteCuenta.comprobanteId = toComprobanteResult.comprobanteId;
        //            if (operacionDC > 0)
        //            {
        //                toComprobanteCuenta.credito = operacionDC;
        //                toComprobanteCuenta.debito = 0;
        //                //Cuenta Resultado del Ejercicio
        //                toComprobanteCuentaRE.credito = 0;
        //                toComprobanteCuentaRE.debito = operacionDC;
        //            }
        //            else
        //            {
        //                toComprobanteCuenta.credito = 0;
        //                toComprobanteCuenta.debito = operacionDC;
        //                //Cuenta Resultado del Ejercicio
        //                toComprobanteCuentaRE.credito = operacionDC;
        //                toComprobanteCuentaRE.debito = 0;
        //            }

        //            toComprobanteCuenta.estadoId = 3;
        //            toComprobanteCuenta.fecha = DateTime.Now;
        //            toComprobanteCuenta.fechaTransaccion = fechaSaldos;  //--------------------********************************
        //            repoComprobanteCuenta.Add(toComprobanteCuenta);

        //            toComprobanteCuentaRE.comprobanteId = toComprobanteResult.comprobanteId;
        //            toComprobanteCuentaRE.cuentaId = new CuentasBL().Get(x => x.moduloMCAId == moduloMCAId && x.nivel == 5 && x.codigo == "3-1-1-01-01").Select(x => x.cuentaId).FirstOrDefault(); //  56; //CUENTA RESULTADO DEL EJERCICIO

        //            toComprobanteCuentaRE.estadoId = 3;
        //            toComprobanteCuentaRE.fecha = DateTime.Now;
        //            toComprobanteCuentaRE.fechaTransaccion = fechaSaldos;  //--------------------********************************
        //            repoComprobanteCuenta.Add(toComprobanteCuentaRE);

        //        }
        //    }

        //    return true;
        //}

        /// <summary>
        /// Funcion que creara el comprobante del resultado del ejecicion entre la cuenta ingreso y gasto que se insertara en patrimonio
        /// </summary>
        /// <param name="moduloMCAId"></param>
        /// <returns> objeto Comprobante </returns>
        public static bool CrearComprobante(int moduloMCAId, DateTime fechaTrans, int tipoComprobante)
        {

            using (var contabilidadMCA = new ContabilidadMCA())
            {
                using (var transaction = contabilidadMCA.Database.BeginTransaction())
                {
                    try
                    {                       
                        /*************************************************************
                        *ANUALAR EL COMPROBANTE ANTERIOR SI EXISTE BUSQUEDA EN BASE AL AÑO //TENGO QUE DIFERENCIAR AL COMPROBANTE DEL ANIO DEL TOTAL
                        **************************************************************/
                        Comprobantes toComprobante = new Comprobantes(); //OBJETO COMPROBANTE   
                        var comprobanteAnterior = contabilidadMCA.Comprobantes.SingleOrDefault(x => x.fechaComprobante.Year == fechaTrans.Year && x.estadoId != EstadosBL.KEY_ANULADO && x.tipoComprobanteId == tipoComprobante);

                        if (comprobanteAnterior != null)
                        {
                            comprobanteAnterior.estadoId = EstadosBL.KEY_ANULADO;
                            contabilidadMCA.Comprobantes.Attach(comprobanteAnterior);
                            contabilidadMCA.Entry(comprobanteAnterior).State = EntityState.Modified;
                            contabilidadMCA.SaveChanges();

                        }

                        string numeroComprobante = ""; //SE CREA UN COMPROBANTE PARA REALIZAR LOS MOVIMIENTOS
                        toComprobante.concepto = tipoComprobante == 3 ? "Resultado del Ejercicio" + fechaTrans.ToString() : "Cierre de la Contabilidad " + fechaTrans.ToString();
                        toComprobante.estadoId = EstadosBL.KEY_EJECUTADO;
                        toComprobante.fecha = DateTime.Now;
                        toComprobante.fechaComprobante = fechaTrans;
                        //sacar el maximo comprobante 
                        //GENERAR NUMERO EN BASE AL ULTIMO COMPROBANTE
                       
                        numeroComprobante = "1";
                        toComprobante.numero = numeroComprobante;
                        toComprobante.tipoComprobanteId = tipoComprobante;
                        contabilidadMCA.Comprobantes.Add(toComprobante);
                        contabilidadMCA.SaveChanges();

                        //AQUI SE BUSCARAN LAS CUENTAS Q NO ESTEN EN CERO Y SE AGREGARAN AL COMPROBANTECUENTA
                        //BUSCAR LAS CUENTAS DETALLES DE NIVEL 5 DE INGRESO Y GASTO Y SELECCIONAR LAS QUE SU SUMATORIA DE SALDO FINAL D-H SEA DIFERENTE DE CERO
                        //BUSCAR EL ULTIMO CIERRE Y OBTENER LA LISTA DE CIERRE CUENTAS 
                        Cierres ultimoCierre = new Cierres();
                        int ultimoMes = fechaTrans.Month == 1 ? 12 : fechaTrans.Month;
                        if (tipoComprobante == 3)
                            ultimoCierre = new CierresBL().Get(x => x.moduloMCAId == moduloMCAId && x.mes == 12 && x.estadoId == EstadosBL.KEY_ASENTADO && x.anio == fechaTrans.Year).LastOrDefault();
                        else
                            ultimoCierre = new CierresBL().Get(x => x.moduloMCAId == moduloMCAId && x.mes == (ultimoMes - 1) && x.estadoId == EstadosBL.KEY_ASENTADO && x.anio == fechaTrans.Year).LastOrDefault();


                        //tengo q hacer una referencia directa a las cuentas 4 y 5 
                        var cierresCuentas = new CierreCuentasBL().Get(x => x.cierreId == ultimoCierre.cierreId && x.Cuentas.nivel == 5).Where(s => s.Cuentas.codigo.Split('-')[0] == "4" || s.Cuentas.codigo.Split('-')[0] == "5");
                        double saldoIngreso = 0, saldoGasto = 0, saldoPatrimonio = 0;

                        saldoIngreso = cierresCuentas.Where(x => x.Cuentas.codigo.Split('-')[0] == "4").Sum(s => s.saldoFinalCredito - s.saldoFinalDebito);
                        saldoGasto = cierresCuentas.Where(x => x.Cuentas.codigo.Split('-')[0] == "5").Sum(s => s.saldoFinalDebito - s.saldoFinalCredito);
                        saldoPatrimonio = saldoIngreso - saldoGasto; //SALDO CALCULADO

                        var comprobanteCuentaP = new ComprobanteCuentas(); //SE CREA UN MOVIMIENTO PARA PATRIMONIO
                        comprobanteCuentaP.comprobanteId = toComprobante.comprobanteId;

                        //AQUI HAY QUE PREGUNTAR QUE PASA CUANDO EL RESULTADO DEL EJERCICIO ES CERO O CUANDO INGRESO MENOS GASTO DA CERO A QUE SE LE PASA EL GASTO A DEBE O AL HABER
                        if (saldoPatrimonio >= 0)
                        {
                            comprobanteCuentaP.credito = saldoPatrimonio;
                            comprobanteCuentaP.debito = 0;
                        }
                        else
                        {
                            comprobanteCuentaP.debito = saldoPatrimonio*(-1);
                            comprobanteCuentaP.credito = 0;
                        }

                        comprobanteCuentaP.cuentaId = new CuentasBL().Get(x => x.nivel == 5 && x.codigo == "3-1-1-01-01" && x.moduloMCAId == moduloMCAId).FirstOrDefault().cuentaId;

                        comprobanteCuentaP.estadoId = 3;
                        comprobanteCuentaP.fecha = DateTime.Now;
                        comprobanteCuentaP.fechaTransaccion = tipoComprobante == 3 ? ultimoCierre.fechaTransaccion : fechaTrans;
                        contabilidadMCA.ComprobanteCuentas.Add(comprobanteCuentaP);
                        contabilidadMCA.SaveChanges();

                        foreach (CierreCuentas cc in cierresCuentas) /// HASTA AQUI LLEGO EL ANALISIS
                        {
                            var comprobanteCuenta = new ComprobanteCuentas();
                            comprobanteCuenta.comprobanteId = toComprobante.comprobanteId;
                            //VALIDAR QUE LA OPERACION DEL DEBE CON EL HABER NO SEA CERO PARA QUE NO SE REGISTRE

                            if (Math.Abs(cc.saldoFinalCredito) + Math.Abs(cc.saldoFinalDebito) != 0)
                            {
                                //Validar el SIGNO  ------------- que pasa si el saldo es cero.....
                                if (cc.Cuentas.codigo.Split('-')[0] == "4") //INGRESOS
                                {
                                    if ((cc.saldoFinalCredito - cc.saldoFinalDebito) > 0)
                                    {
                                        comprobanteCuenta.debito = cc.saldoFinalCredito - cc.saldoFinalDebito;
                                        comprobanteCuenta.credito = 0;
                                    }
                                    else if ((cc.saldoFinalCredito - cc.saldoFinalDebito) < 0)
                                    {
                                        comprobanteCuenta.credito = cc.saldoFinalCredito - cc.saldoFinalDebito;
                                        comprobanteCuenta.debito = 0;
                                    }
                                }
                                else if (cc.Cuentas.codigo.Split('-')[0] == "5") //GASTOS
                                {
                                    if ((cc.saldoFinalDebito - cc.saldoFinalCredito) > 0)
                                    {
                                        comprobanteCuenta.debito = 0;
                                        comprobanteCuenta.credito = cc.saldoFinalDebito - cc.saldoFinalCredito;
                                    }
                                    else if ((cc.saldoFinalDebito - cc.saldoFinalCredito) < 0)
                                    {
                                        comprobanteCuenta.credito = 0;
                                        comprobanteCuenta.debito = cc.saldoFinalDebito - cc.saldoFinalCredito;
                                    }
                                }
                                comprobanteCuenta.cuentaId = cc.cuentaId;
                                comprobanteCuenta.estadoId = 3;
                                comprobanteCuenta.fecha = DateTime.Now;
                                comprobanteCuenta.fechaTransaccion = tipoComprobante == 3 ? ultimoCierre.fechaTransaccion : fechaTrans;
                                contabilidadMCA.ComprobanteCuentas.Add(comprobanteCuenta);
                                contabilidadMCA.SaveChanges();
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction Roll backed due to some exception");
                        return false;
                    }
                }
            }
        }
        /*********************************************/
        public class SaldosCuenta
        {
            public double saldoInicialCredito { get; set; }
            public double saldoInicialDebito { get; set; }
            public double saldoFinalCredito { get; set; }
            public double saldoFinalDebito { get; set; }
            public double credito { get; set; }
            public double debito { get; set; }

        }

        /// <summary>
        /// CALUCLO DE SALDOS DE CUENTAS EN BASE A SUS COMPRONATES
        /// </summary>
        /// <param name="fechaSaldos"></param>
        /// <param name="c"></param>
        /// <returns>SALDOS DE LAS CUENTAS EN BASE A COMPROBANTES</returns>
        public static SaldosCuenta saldosDetalle(DateTime fechaSaldos, Cuentas c, int tipoCierreId)
        {
            //int xc = 0;
            var saldosD = new SaldosCuenta();
            int mesInicio = 0, mesFinal = 0, anio = 0;   //parametros de cierre con respecto a fecha

            DateTime fechainicio = DateTime.Parse("01/" + fechaSaldos.Month.ToString() + "/" + fechaSaldos.Year.ToString()); //CONTRUCCION DE FECHA INICIAL

            if (tipoCierreId == 2)//
                fechainicio = DateTime.Parse("31/" + fechaSaldos.Month.ToString() + "/" + fechaSaldos.Year.ToString()); //CONTRUCCION DE FECHA INICIAL

            if (fechaSaldos.Month == 1) //VALIDAR SI ES UN INICIO DE AÑO
            {
                //anio = (fechaSaldos.Year - 1);
                anio = fechaSaldos.Year;
                mesInicio = 12;
                mesFinal = 1;
            }
            else //ES CUALQUIER OTRO MES DEL AÑO
            {
                anio = fechaSaldos.Year;
                mesInicio = (fechaSaldos.Month - 1);
                mesFinal = fechaSaldos.Month;
            }


            //if (c.cuentaId == 806) /// PROBABLEMENTE SE QUITE XQ NO HACE FALTA.
            //     xc = 1;

            //CALCULO DE SALDOS INICIALES DE LA CUENTA
            //saldosD.saldoInicialCredito = c.ComprobanteCuentas.Where(x => (x.fechaTransaccion < fechainicio) && x.estadoId == EstadosBL.KEY_EJECUTADO).Sum(x => x.credito);
            saldosD.saldoInicialCredito = c.ComprobanteCuentas.Where(x => (x.fechaTransaccion < fechainicio) && x.Comprobantes.estadoId == EstadosBL.KEY_EJECUTADO && x.estadoId != EstadosBL.KEY_ANULADO).Sum(x => x.credito);
            //saldosD.saldoInicialDebito = c.ComprobanteCuentas.Where(x => (x.fechaTransaccion < fechainicio) && x.estadoId == EstadosBL.KEY_EJECUTADO).Sum(x => x.debito);
            saldosD.saldoInicialDebito = c.ComprobanteCuentas.Where(x => (x.fechaTransaccion < fechainicio) && x.Comprobantes.estadoId == EstadosBL.KEY_EJECUTADO && x.estadoId != EstadosBL.KEY_ANULADO).Sum(x => x.debito);
            //SALDOS ACTUALES
            //saldosD.credito = c.ComprobanteCuentas.Where(x => x.fechaTransaccion.Month == mesFinal && x.fechaTransaccion.Year == anio && x.estadoId == EstadosBL.KEY_EJECUTADO).Sum(x => x.credito);
            if (tipoCierreId == 2)
            {
                saldosD.credito = c.ComprobanteCuentas.Where(x => x.fechaTransaccion == fechainicio && x.Comprobantes.estadoId == EstadosBL.KEY_EJECUTADO && x.estadoId != EstadosBL.KEY_ANULADO).Sum(x => x.credito);
                //saldosD.debito = c.ComprobanteCuentas.Where(x => x.fechaTransaccion.Month == mesFinal && x.fechaTransaccion.Year == anio && x.estadoId == EstadosBL.KEY_EJECUTADO).Sum(x => x.debito);
                saldosD.debito = c.ComprobanteCuentas.Where(x => x.fechaTransaccion == fechainicio && x.Comprobantes.estadoId == EstadosBL.KEY_EJECUTADO && x.estadoId != EstadosBL.KEY_ANULADO).Sum(x => x.debito);
            }
            else
            {
                saldosD.credito = c.ComprobanteCuentas.Where(x => x.fechaTransaccion.Month == mesFinal && x.fechaTransaccion.Year == anio && x.Comprobantes.estadoId == EstadosBL.KEY_EJECUTADO && x.estadoId != EstadosBL.KEY_ANULADO).Sum(x => x.credito);
                //saldosD.debito = c.ComprobanteCuentas.Where(x => x.fechaTransaccion.Month == mesFinal && x.fechaTransaccion.Year == anio && x.estadoId == EstadosBL.KEY_EJECUTADO).Sum(x => x.debito);
                saldosD.debito = c.ComprobanteCuentas.Where(x => x.fechaTransaccion.Month == mesFinal && x.fechaTransaccion.Year == anio && x.Comprobantes.estadoId == EstadosBL.KEY_EJECUTADO && x.estadoId != EstadosBL.KEY_ANULADO).Sum(x => x.debito);
            }

            //Calculo de Saldos finales de la Cuenta
            saldosD.saldoFinalCredito = saldosD.saldoInicialCredito + saldosD.credito;
            saldosD.saldoFinalDebito = saldosD.saldoInicialDebito + saldosD.debito;

            return saldosD;
        }

        /// <summary>
        /// Crea los saldos del cierre de una cuenta en especifico
        /// </summary>
        /// <param name="fechaSaldos"></param>
        /// <param name="cuentaId"></param>
        /// <param name="cierreId"></param>
        /// <param name="saldosC"></param>
        /// <param name="moduloMCAId"></param>
        /// <param name="cierreOficial"></param>
        /// <returns></returns>
        public static bool CerrarCuentaV2(DateTime fechaSaldos, int cuentaId, int cierreId, SaldosCuenta saldosC, int moduloMCAId)
        {
            CierreCuentas toCierreCuenta = new CierreCuentas(); //Declaracion del objeto de cierre de cuentas

            toCierreCuenta.cuentaId = cuentaId;
            toCierreCuenta.cierreId = cierreId;
            //toCierreCuenta.moduloMCAId =moduloMCAId; ***

            toCierreCuenta.saldoInicialCredito = saldosC.saldoInicialCredito;
            toCierreCuenta.saldoInicialDebito = saldosC.saldoInicialDebito;
            toCierreCuenta.saldoFinalCredito = saldosC.saldoFinalCredito;
            toCierreCuenta.saldoFinalDebito = saldosC.saldoFinalDebito;
            toCierreCuenta.credito = saldosC.credito;
            toCierreCuenta.debito = saldosC.debito;

            toCierreCuenta.fecha = DateTime.Now;
            toCierreCuenta.fechaTransaccion = fechaSaldos;
            CierreCuentasBL repoCierreCuentas = new CierreCuentasBL();
            repoCierreCuentas.Add(toCierreCuenta);

            return true;
        }
        #endregion


    }


}
