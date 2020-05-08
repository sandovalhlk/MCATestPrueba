using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayerMCA;
using Repository;

namespace BusinessLogicMCA
{
    public class AcuerdoSupMCABL : BaseRepository<AcuerdoSupMCA>
    {
        #region Funciones 
        public static bool ExisteAcuerdo(AcuerdoSupMCA toAcuerdo, out string mensaje)
        {
            mensaje = string.Empty;
            long tempKeyModel = toAcuerdo.acuerdoSupMCAId;
            int numeroAcuerdo = toAcuerdo.numero;
            int moduloId = toAcuerdo.moduloMACId;
            var existeAcuerdo = new AcuerdoSupMCABL().Get(filter: x => x.numero == numeroAcuerdo && x.acuerdoSupMCAId != tempKeyModel && x.moduloMACId == moduloId).ToList();

            if (existeAcuerdo.Count > 0)
            {
                mensaje = "El Acuerdo digitado, ya fue registrado.";
                return true;
            }
            else
            {
                return false;
            }

        }


        //Válida si hay un acuerdo posterior al seleccionado este no se puede editar
        public static bool ValidarAcuerdo(int acuerdoSupMCAId, DateTime fechaAcuerdo, int moduloMCAId)
        {
            DateTime ultimoAcuerdo = new AcuerdoSupMCABL().Get(x => x.moduloMACId == moduloMCAId && x.estadoId == EstadosBL.KEY_ACTIVO).Select(x => x.fechaInicial).LastOrDefault();
            var acuerdos = new AcuerdoSupMCABL().Get(filter: x => x.moduloMACId == moduloMCAId && ultimoAcuerdo > fechaAcuerdo && x.estadoId == EstadosBL.KEY_ACTIVO).ToList();
            if (acuerdos.Count > 0)
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
