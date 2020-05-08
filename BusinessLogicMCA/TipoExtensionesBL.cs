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
    public class TipoExtensionesBL : BaseRepository<TipoExtensiones>
    {
        #region Funciones 
        public static bool ExisteTipoExtensiones(TipoExtensiones toTipoExtensiones, out string mensaje)
        {
            mensaje = string.Empty;
     
            long tempKeyModel = toTipoExtensiones.tipoExtensionId;

            string tipoExtension = toTipoExtensiones.tipoExtencion.Trim().ToUpper();

            var existeTipoExtension = new TipoExtensionesBL().Get(filter: x => x.tipoExtencion.Trim().ToUpper() == tipoExtension && x.tipoExtensionId != tempKeyModel && x.estadoId == EstadosBL.KEY_ACTIVO).ToList();

            if (existeTipoExtension.Count > 0)
            {
                mensaje = "El tipo de Extensión digitado, ya fue registrado.";
                return true;
            }
            else
            {
                return false;
            }
        }



        public static bool tipoExtAcuerdo(int tipoExtensionId)
        {
            var tipoEAcuerdo = new AcuerdoSupMCABL().Get(filter: x => x.tipoExtencionId == tipoExtensionId && x.estadoId == EstadosBL.KEY_ACTIVO).ToList();
            if (tipoEAcuerdo.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion




    }
}
