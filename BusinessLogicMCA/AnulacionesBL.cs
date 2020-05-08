using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayerMCA;
using Repository;

namespace BusinessLogicMCA
{
 public  class AnulacionesBL : BaseRepository<Anulaciones>
    {
        #region Metodos
        public static void Anular(string razon, long entidadId, string entidadNombre, string usuario)
        {

            var toAnulacion = new Anulaciones();
            toAnulacion.razon = razon;
            toAnulacion.entidadId = entidadId;
            toAnulacion.entidadNombre = entidadNombre;
            toAnulacion.usuario = usuario;
            toAnulacion.fecha = DateTime.Now;

            var repoAnulaciones = new AnulacionesBL();
            repoAnulaciones.Add(toAnulacion);

        }
        #endregion
    }
}