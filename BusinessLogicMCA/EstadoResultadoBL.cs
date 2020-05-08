using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayerMCA;
using Repository;

namespace BusinessLogicMCA
{
    public class EstadoResultadoBL
    {

        #region Modelos de los procedimientos Almacenados
        public static List<CListTotalesEstadoResultado> GetEstadoResultado(int moduloMCAId, DateTime fecha1, DateTime fecha2)
        {
            var repo = new BaseRepository<CListTotalesEstadoResultado>();
            repo.AddParameter("@moduloMCAId", @moduloMCAId);
            repo.AddParameter("@fechaInicial", fecha1);
            repo.AddParameter("@fechaFinal", fecha2);
            return repo.GetFromDatabaseWithQuery("rptTotalEstadoResultado").ToList<CListTotalesEstadoResultado>();
        }
        #endregion

        #region Procedimientos Almacenados

        public class CListTotalesEstadoResultado
        {
            public double? totalSaldoAnterior { get; set; }
            public double? totalMovimientoMes { get; set; }
            public double? totalSaldoAcumulados { get; set; }

        }
        #endregion
    }
}



