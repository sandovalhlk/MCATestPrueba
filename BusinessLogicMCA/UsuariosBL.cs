using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayerMCA;
using Repository;

namespace BusinessLogicMCA
{
 public  class UsuariosBL : BaseRepository<Usuarios>
    {
        #region Modelos de Procedimientos Almacenados

        public class RListUsuario
        {
            public int usuarioId { get; set; }
            public string nombres { get; set; }
            public string apellidos { get; set; }
            public string cedula { get; set; }
            public string userName { get; set; }
            public string estado { get; set; }
            public string bloqueado { get; set; }
        }

        #endregion

        #region Funciones 

        public static Usuarios ValidarUsuarioMCALogin(int usuarioId)
        {

            var toUservalidar = new UsuariosBL().Get(filter: x => x.usuarioId == usuarioId && x.AspNetUsers.Id != null).FirstOrDefault();


            return toUservalidar;
        }


        public static List<RListUsuario> GetListaUsuarios(int estadoId)
        {
            var repo = new BaseRepository<RListUsuario>();
            repo.AddParameter("@estadoId", estadoId);
            return repo.GetFromDatabaseWithQuery("getUsuarioList").ToList<RListUsuario>();
        }


        #endregion
    }
}
