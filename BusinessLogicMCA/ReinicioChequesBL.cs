using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayerMCA;
using Repository;

namespace BusinessLogicMCA
{
    public class ReinicioChequesBL : BaseRepository<ReinicioCheques>
    {
        #region Metodos
        public static void AddReinicio(int numero, string justificante, int comprobanteId, int moduloMCAId)
        {
            var repoReinicio = new ReinicioChequesBL();
            ReinicioCheques reinicioCheque = new ReinicioCheques();

            reinicioCheque.fecha = DateTime.Now;
            reinicioCheque.justificacion = justificante;
            reinicioCheque.numero = numero;
            reinicioCheque.comprobanteId = comprobanteId;
            reinicioCheque.usuario = System.Web.HttpContext.Current.User.Identity.Name;
            reinicioCheque.moduloMCAId = moduloMCAId;

            repoReinicio.Add(reinicioCheque);

        }

        #endregion

        #region Areas

        #endregion

    }
}
