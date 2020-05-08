using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayerMCA;
using Repository;

namespace BusinessLogicMCA
{
 public  class EstadosBL : BaseRepository<Estados>
    {
        #region Definicion de Variables

        public const int KEY_ACTIVO = 1;
        public const int KEY_ANULADO = 2;
        public const int KEY_EJECUTADO = 3;
        public const int KEY_ASENTADO = 4;
        public const int KEY_ANULADO_DB = 5;

        #endregion
    }
}
