using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayerMCA;
using Repository;


namespace BusinessLogicMCA
{
    public class CierreCuentasBL : BaseRepository<CierreCuentas>
    {
        #region Metodos
        public static int CierreSaldso(int padre)
        {


            return CierreSaldso(padre);
        }

        public static void CierreCuentaOficial(int cierreId)
        {
            using (var contabilidadMCA = new ContabilidadMCA())
            {
                using (var transaction = contabilidadMCA.Database.BeginTransaction())
                {
                    try
                    {
                        //CierreCuentasOficial cierreCuentasOficiales = new CierreCuentasOficial();
                        var cierresCuentas = new CierreCuentasBL().Get(filter: x => x.cierreId == cierreId);

                      //  var repoCierreCuentasOficial = new CierreCuentasOficialBL();

                        foreach (var cco in cierresCuentas)
                        {

                            var cierreCuentasOficiales = new CierreCuentasOficial();
                            cierreCuentasOficiales.cierreId = cco.cierreId;
                            cierreCuentasOficiales.credito = cco.credito;
                            cierreCuentasOficiales.cuentaId = cco.cuentaId;
                            cierreCuentasOficiales.debito = cco.debito;
                            cierreCuentasOficiales.fecha = cco.fecha;
                            cierreCuentasOficiales.fechaTransaccion = cco.fechaTransaccion;
                            cierreCuentasOficiales.saldoFinalCredito = cco.saldoFinalCredito;
                            cierreCuentasOficiales.saldoFinalDebito = cco.saldoFinalDebito;
                            cierreCuentasOficiales.saldoInicialCredito = cco.saldoInicialCredito;
                            cierreCuentasOficiales.saldoInicialDebito = cco.saldoInicialDebito;

                            //repoCierreCuentasOficial.Add(cierreCuentasOficiales);
                            contabilidadMCA.CierreCuentasOficial.Add(cierreCuentasOficiales);
                            contabilidadMCA.SaveChanges();

                        }

                        transaction.Commit();

                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction Roll backed due to some exception, REPLICA DE CIERREOFICIAL");
                        //   return false;
                    }
                }

            }
        }
        #endregion
    }
}
