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
 public  class ProyectosBL : BaseRepository<Proyectos>
    {
        #region Modelos de los procedimientos Almacenados
        public class CListaProyectos : Proyectos
        {

            public string convenio { get; set; }
            public string ucr { get; set; }


        }
        #endregion

        #region Procedimientos Almacenados

        public static List<CListaProyectos> GetListaProyectos()
        {
            var repo = new BaseRepository<CListaProyectos>();
            return repo.GetFromDatabaseWithQuery("GetListaProyectos").ToList<CListaProyectos>();
        }
        #endregion

        #region Funciones 
        public static bool ExisteProyecto(Proyectos toProyecto, out string mensaje)
        {
            mensaje = string.Empty;
            long tempKeyModel = toProyecto.proyectoId;
            string nombreProyectos = toProyecto.nombreProyecto.Trim().ToUpper();
            var existeProyecto = new ProyectosBL().Get(filter: x => x.nombreProyecto.Trim().ToUpper() == nombreProyectos && x.proyectoId != tempKeyModel && x.estadoId != EstadosBL.KEY_ANULADO).ToList();

            if (existeProyecto.Count > 0)
            {
                mensaje = "El Proyecto digitado, ya fue registrado.";
                return true;
            }
            else
            {
                return false;
            }

        }


        public static bool Proyectomodulo(int proyectoId)
        {
            var proyectomodulos = new ModulosMCABL().Get(filter: x => x.proyectoId == proyectoId).ToList();
            if (proyectomodulos.Count > 0)
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
