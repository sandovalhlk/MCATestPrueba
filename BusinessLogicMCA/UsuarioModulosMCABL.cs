using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayerMCA;
using Repository;
using SistContabilidadMCA;

namespace BusinessLogicMCA
{
 public  class UsuarioModulosMCABL : BaseRepository<UsuarioModulosMCA>
    {
        public static List<ListaFirmantes> GetListaFirmantes(int moduloMCAId)
        {
            var repo = new BaseRepository<ListaFirmantes>();
            repo.AddParameter("@moduloMCAId", moduloMCAId);
            //repo.AddParameter("@solicitudId", solicitudId);
            List<ListaFirmantes> listaDatos = repo.GetFromDatabaseWithQuery("dbo.GetListaFirmantes").ToList();
            return listaDatos;
        }

        #region Clases Internas


        /// <summary>
        ///  Lista de los veiculos, area correspondiente (Formulario de Solicitudes)
        /// </summary>
        public class ListaFirmantes
        {
            //dbo.Usuarios.telefonos, dbo.Usuarios.direccion, 
            //dbo.TipoUsuarios.tipoUsuario
            public int usuarioId { get; set; }
            public string nombres { get; set; }
            public string apellidos { get; set; }
            public string cedula { get; set; }
            public int moduloMCAId { get; set; }
            public int usuarioModuloMCAId { get; set; }
            public string tipoUsuario { get; set; }


        }

        #endregion

        #region Metodos 
        public static bool existeFirmante(int moduloMCAId, int tipoUsuarioId, out string mensaje)
        {
            mensaje = string.Empty;

            var existeFirmante = new UsuarioModulosMCABL().Get(filter: x => x.tipoUsuarioId == tipoUsuarioId && x.moduloMCAId == moduloMCAId).ToList();
            if (existeFirmante.Count > 0)
            {
                mensaje = "El Tipo de Firmante, ya fue registrado.";
                return true;
            }
            else
            {
                return false;
            }

        }


        public static int ModuloUsuario(int usuarioId)
        {
            int moduloMCAId = 0;
         var  UsermoduloMCA = new UsuarioModulosMCABL().Get(filter: x => x.usuarioId == usuarioId && x.estadoId == EstadosBL.KEY_ACTIVO).FirstOrDefault();
          

            if (UsermoduloMCA != null)
                moduloMCAId = UsermoduloMCA.moduloMCAId;


            return moduloMCAId;
        }

        public static int GetModuloUsuario(string usuario)
        {

            var user = UsuariosASPBL.GetUsuario(usuario);

            int moduloMCAId = 0;
            var UsermoduloMCA = new UsuarioModulosMCABL().Get(filter: x => x.usuarioId == user.usuarioId && x.estadoId == EstadosBL.KEY_ACTIVO).FirstOrDefault();


            if (UsermoduloMCA != null)
                moduloMCAId = UsermoduloMCA.moduloMCAId;


            return moduloMCAId;
        }


        public static object GetModuloUsuarioMCA(string usuario)
        {
            var userMCA = UsuariosASPBL.GetUsuario(usuario);
        
            return userMCA;
        }

        public static IEnumerable<UsuarioModulosMCA> GetUsuariosModulos(string usuario)
        {
            //var userMCA = UsuariosASPBL. GetUsuario(usuario);

            var userModulos = new UsuarioModulosMCABL().Get(x => x.Usuarios.AspNetUsers.UserName == usuario);

            return userModulos;
        }
        #endregion

    }


}
