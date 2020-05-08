using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayerMCA;
using Repository;

namespace BusinessLogicMCA
{
 public  class CuentasBL : BaseRepository<Cuentas>
    {
        #region Metodos
        public static string CrearCatalogo(int moduloMCAId)
        {

            try
            {
                var repo = new BaseRepository<RListaCuentas>();
                repo.AddParameter("@moduloId", moduloMCAId);
                repo.GetFromDatabaseWithQuery("CrearCatalogoCuentas").ToList<RListaCuentas>();
                return "El catalgo se agrego satisfactoriamente";

            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

      

        public static bool ResultadoEjercicio(int moduloMCAId, int cierreId)
        {
            /*********************************/
            //Calcular los saldos de las cuentas 4 y 5
            double saldoIngreso, saldoGasto, resultEjercicio;

            // var ingreso = new CierreCuentasBL().Get(filter: x => x.cierreId == cierreId && x.moduloMCAId == moduloMCAId && x.Cuentas.codigo.Substring(0, 1) == "4" && x.Cuentas.nivel == 5); // .FirstOrDefault();
            var ingreso = new CierreCuentasBL().Get(filter: x => x.cierreId == cierreId && x.Cuentas.codigo.Substring(0, 1) == "4" && x.Cuentas.nivel == 5); // .FirstOrDefault();
            //var gasto = new CierreCuentasBL().Get(filter: x => x.cierreId == cierreId && x.moduloMCAId == moduloMCAId && x.Cuentas.codigo.Substring(0, 1) == "5" && x.Cuentas.nivel == 5); //.FirstOrDefault();
            var gasto = new CierreCuentasBL().Get(filter: x => x.cierreId == cierreId && x.Cuentas.codigo.Substring(0, 1) == "5" && x.Cuentas.nivel == 5); //.FirstOrDefault();
            saldoIngreso = ingreso.Sum(x => x.credito) - ingreso.Sum(x => x.debito); // debito - ingreso.credito;
            saldoGasto = gasto.Sum(x => x.debito) - gasto.Sum(x => x.credito);
            resultEjercicio = saldoIngreso - saldoGasto;

            var repoCierreCuenta = new CierreCuentasBL();
            var toCuenta = new CuentasBL().Get(filter: x => x.codigo == "3-1-1-1-01" && x.moduloMCAId == moduloMCAId).FirstOrDefault();

            //var toCierreCuenta = repoCierreCuenta.Get(x => x.cuentaId == toCuenta.cuentaId && x.cierreId == cierreId && x.moduloMCAId == moduloMCAId).FirstOrDefault();
            var toCierreCuenta = repoCierreCuenta.Get(x => x.cuentaId == toCuenta.cuentaId && x.cierreId == cierreId).FirstOrDefault();


            toCierreCuenta.credito = resultEjercicio;
            toCierreCuenta.debito = 0;
            toCierreCuenta.saldoInicialCredito = toCuenta.CierreCuentas.Sum(s => s.credito); //saldosC.saldoInicialCredito;
            toCierreCuenta.saldoInicialDebito = toCuenta.CierreCuentas.Sum(x => x.debito);//saldosC.saldoInicialDebito;
            toCierreCuenta.saldoFinalCredito = toCierreCuenta.saldoInicialCredito + toCierreCuenta.credito;  // saldosC.saldoFinalCredito;
            toCierreCuenta.saldoFinalDebito = toCierreCuenta.saldoInicialDebito + toCierreCuenta.debito;//saldosC.saldoFinalDebito;

            repoCierreCuenta.Update(toCierreCuenta);
            return true;

        }

        public static bool NulidadCuenta(int cuentaId)
        {
            var toCuenta = new CuentasBL().GetByID(cuentaId);
            if (toCuenta.hijos.Count > 0 || toCuenta.ComprobanteCuentas.Count > 0)
                return false;
            else
                return true;

        }
        #endregion
    }

    #region Clases Internas
    public class RListaCuentas : Cuentas
    {

    }
    #endregion
}

