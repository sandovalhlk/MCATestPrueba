using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayerMCA;
using Repository;


namespace BusinessLogicMCA
{
    public class ConveniosBL : BaseRepository<Convenios>
    {

        #region Funciones 
        public static bool ExisteConvenios(Convenios toConvenios, out string mensaje)
        {
            mensaje = string.Empty;

            long tempKeyModel = toConvenios.convenioId;

            string nombreConvenios = toConvenios.convenio.Trim().ToUpper();

            var existeConvenio = new ConveniosBL().Get(filter: x => x.convenio.Trim().ToUpper() == nombreConvenios && x.convenioId != tempKeyModel && x.estadoId == EstadosBL.KEY_ACTIVO).ToList();

            if (existeConvenio.Count > 0)
            {
                mensaje = "El nombre del Convenio digitado, ya fue registrado.";
                return true;
            }
            else
            {
                return false;
            }
        }


        public static bool ConvenioProyecto(int convenioId)
        {
            var ConvenioProyectos = new ProyectosBL().Get(filter: x => x.convenioId == convenioId && x.estadoId == EstadosBL.KEY_ACTIVO).ToList();
            if (ConvenioProyectos.Count > 0)
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
