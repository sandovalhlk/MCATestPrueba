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
 public  class ModulosMCABL : BaseRepository<ModulosMCA>
    {


        #region Funciones 
       

        public static bool ModulosCuenta(int moduloMCAId)
        {
            var ModulosCuenta = new CuentasBL().Get(filter: x => x.moduloMCAId == moduloMCAId && x.estadoId == EstadosBL.KEY_ACTIVO).ToList();
            if (ModulosCuenta.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

           
          
        public static bool ExisteModulo(ModulosMCA toModulosMCA)
        {
            
            long tempKeyModel = toModulosMCA.moduloMCAId;
            string nombreModulo = toModulosMCA.nombreModuloMCA.Trim().ToUpper();
            string ruc = toModulosMCA.ruc.Trim().ToUpper();
            string contrato = toModulosMCA.numContrato.ToUpper();
            var existeModulo = new ModulosMCABL().Get(filter: x => x.nombreModuloMCA.Trim().ToUpper() == nombreModulo &&  x.ruc == ruc && x.numContrato == contrato  && x.proyectoId != tempKeyModel ).ToList();

            if (existeModulo.Count > 0)
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

        public static List<CListaModulos> GetListaModulos()
        {
            var repo = new BaseRepository<CListaModulos>();
            return repo.GetFromDatabaseWithQuery("GetListaModulos").ToList<CListaModulos>();
        }


        public static List<CListaModulosConciliacion> GetListadoMCA(int ucrId)
        {
            var repo = new BaseRepository<CListaModulosConciliacion>();
            repo.AddParameter("@ucrId", ucrId);
            return repo.GetFromDatabaseWithQuery("getListadoMCA").ToList<CListaModulosConciliacion>();
        }

        public static List<CListFecha> GetFechas(DateTime fecha)
        {
            var repo = new BaseRepository<CListFecha>();
            repo.AddParameter("@fecha", fecha);       
            return repo.GetFromDatabaseWithQuery("getFechas").ToList<CListFecha>();
        }

        public static List<CListModulosMCA> GetModulosMCA()
        {
            var repo = new BaseRepository<CListModulosMCA>();
            return repo.GetFromDatabaseWithQuery("GetListModulosMCA").ToList<CListModulosMCA>();
        }


        #endregion

        #region Procedimientos Almacenados

        public class CListaModulos : ModulosMCA
        {
            public string NombreProyecto { get; set; }
            public string nombre { get; set; }

        }

        public class CListaModulosConciliacion : ModulosMCA
        {
            public string nombreProyecto { get; set; }
            public string convenio { get; set; }
            public string departamento { get; set; }
            public string municipio { get; set; }
            public int ucrId { get; set; }

        }


        public class CListModulosMCA
        {
            public int moduloMCAId { get; set; }
            public string nombreModuloMCA { get; set; }
            public string numContrato { get; set; }
            public string departamento { get; set; }
            public string municipio { get; set; }
            public string nombreProyecto { get; set; }


        }


        public class CListFecha
        {
            public string fechaInicial { get; set; }
            public string fechaFinal { get; set; }
            public string mesMov { get; set; }
            public string dia { get; set; }
            public string mes { get; set; }
            public string ano { get; set; }         



        }


        #endregion




    }
}
