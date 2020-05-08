using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayerMCA;
using Repository;
using BusinessLogicMCA;

namespace BusinessLogicMCA
{
    public class ConciliacionBancariasBL: BaseRepository<ConciliacionBancarias>
    {

        #region Funciones 

        public static bool Existeconciliacion( int cuentaId, DateTime fechaTransaccion, out string mensaje)
        {
            mensaje = string.Empty;

            
            var existeConciliacion = new ConciliacionBancariasBL().Get(filter: x => x.fechaTransaccion.Month == fechaTransaccion.Month && x.fechaTransaccion.Year == fechaTransaccion.Year && x.cuentaId==cuentaId && x.estadoId!=2).FirstOrDefault();

            if (existeConciliacion!=null)
            {
                mensaje = "El periodo digitado de la Transaccion, ya fue registrada.";
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CierreCuenta(int cuentaId)
        {
            var cierreCuenta = new CierreCuentasBL().Get(filter: x => x.cuentaId == cuentaId).ToList();
            if (cierreCuenta.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ExisteCuenta(int moduloMCAId)
        {

            var existeCuenta = new CuentasBL().Get(filter: x => x.moduloMCAId == moduloMCAId && x.Cuentabanco == true).ToList();

            if (existeCuenta.Count == 0)
            {

                return false;
            }
            else
            {
                return true;
            }

        }

        public static bool ValidarConciliacion(int conciliacionBancariaId)
        {
            var conciliacion = new ConciliacionBancariasBL().Get(filter: x => x.conciliacionBancariaId == conciliacionBancariaId && x.estadoId == EstadosBL.KEY_EJECUTADO).ToList();          
            if (conciliacion.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Modelos de los procedimientos Almacenados
        public class CListadoConciliacion
        {
            
            public string nombreProyecto { get; set; }
            public string nombreModuloMCA { get; set; }
            public DateTime fechaTransaccion { get; set; }
            public double saldoEstadoCuentaBanco { get; set; }
            public double partidaConsiliacion { get; set; }
            public double saldoenLibro { get; set; }
            public int cuentaId { get; set; }
            public int conciliacionBancariaId { get; set; }
            
        }

        public class CListadoCuentaProyecto
        {
            public string nombreProyecto { get; set; }
            public string nombreModuloMCA { get; set; }
            public string codigo { get; set; }
            public string descripcion { get; set; }
            public int cuentaId { get; set; }

        }

        public class CConciliacionOperacion        {
            
            public int resta { get; set; }

        }


        #endregion

        #region Procedimientos Almacenados

        public static List<CListadoConciliacion> GetListadoConciliacion(int cuentaId)
        {
            var repo = new BaseRepository<CListadoConciliacion>();
            repo.AddParameter("@cuentaId", cuentaId);           
            return repo.GetFromDatabaseWithQuery("GetListaConciliacion").ToList<CListadoConciliacion>();
        }

        public static List<CListadoCuentaProyecto> GetListaCuentasProyecto(int cuentaId)
        {
            var repo = new BaseRepository<CListadoCuentaProyecto>();
            repo.AddParameter("@cuentaId", cuentaId);
            return repo.GetFromDatabaseWithQuery("GetListaCuentasProyecto").ToList<CListadoCuentaProyecto>();
        }

        public static List<CListadoCuentaProyecto> GetListaCuentasProyectoUcr(int? ucrId)
        {
            var repo = new BaseRepository<CListadoCuentaProyecto>();
            repo.AddParameter("@cuentaId", ucrId);
            return repo.GetFromDatabaseWithQuery("GetListaCuentasProyectoUcr").ToList<CListadoCuentaProyecto>();
        }


        public static List<CConciliacionOperacion>GetOperacionC(int conciliacionBancariaId)
        {
            var repo = new BaseRepository<CConciliacionOperacion>();
            repo.AddParameter("@conciliacionBancariaId", conciliacionBancariaId);      
            return repo.GetFromDatabaseWithQuery("GetOperacionC").ToList<CConciliacionOperacion>();
        }

        #endregion


    }
}
