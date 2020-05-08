using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayerMCA;
using Repository;

namespace BusinessLogicMCA
{
    public class BalanzaComprobacionBL
    {

        #region Modelos de los procedimientos Almacenados
        public static List<CListTotalesBalanza> GetTotalesBalanza(int moduloMCAId, DateTime fecha)
        {
            var repo = new BaseRepository<CListTotalesBalanza>();
            repo.AddParameter("@moduloMCAId", moduloMCAId);
            repo.AddParameter("@fecha", fecha);
            return repo.GetFromDatabaseWithQuery("rptTotalBalanzaComprobacion").ToList<CListTotalesBalanza>();
        }      

       
        #endregion

        #region Procedimientos Almacenados

        public class CListTotalesBalanza
        {
          public double? TotalsaldoInicialDebito { get; set; }
          public double? TotalsaldoInicialCredito { get; set; }
          public double? Totaldebito { get; set; }
          public double? Totalcredito { get; set; }
          public double? TotalsaldoFinalDebito { get; set; }
          public double? TotalsaldoFinalCredito { get; set; }

        }


 




        #endregion

    }
}





