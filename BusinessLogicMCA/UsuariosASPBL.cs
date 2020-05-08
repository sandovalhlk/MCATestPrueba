using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using DataLayerMCA;

namespace SistContabilidadMCA
{
    public class UsuariosASPBL : BaseRepository<AspNetUsers>
    {
        #region Modelos de Procedimientos Almacenados
        public class RUserRol
        {
            public string UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int keyEmpleadoCatalogo { get; set; }
            public string RoleId { get; set; }
        }
        public class RUsuario
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Rol { get; set; }
            public bool LockoutEnabled { get; set; }
            public bool EmailConfirmed { get; set; }
            public int? usuarioId { get; set; }
            public int? ucrId { get; set; }
            
            public string Id { get; set; }

        }
        #endregion

        #region Funciones 
        public static bool ExisteUser(string UserName, out string mensaje)
        {
            mensaje = string.Empty;

            string NameUser = UserName.Trim().ToUpper();

            var existeUsuario = new UsuariosASPBL().Get(filter: x => x.UserName.Trim().ToUpper() == NameUser).ToList();

            if (existeUsuario.Count > 0)
            {
                mensaje = "El nombre de usuario digitado, ya fue registrado.";
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool ExisteEmail(string Email, out string mensaje)
        {
            mensaje = string.Empty;

            string correo = Email.Trim().ToUpper();

            var existeCorreo = new UsuariosASPBL().Get(filter: x => x.Email.Trim().ToUpper() == correo).ToList();

            if (existeCorreo.Count > 0)
            {
                mensaje = "El Email digitado, ya fue registrado.";
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool addUserRol(string UserId, string FirstName, string LastName,  string RolId, int userMCAId, int ucrId)
        {
            var repo = new BaseRepository<RUserRol>();
            repo.AddParameter("@UserId", UserId);
            repo.AddParameter("@FirstName", FirstName);
            repo.AddParameter("@LastName", LastName);
            repo.AddParameter("@RoleId", RolId);
            repo.AddParameter("@userMCAId", userMCAId);
            repo.AddParameter("@ucrId", ucrId);
            repo.GetFromDatabaseWithQuery("AddUserRol").ToList<RUserRol>();
            return true;
        }
        public static RUsuario GetUsuario(string UserName)
        {
            var repo = new BaseRepository<RUsuario>();
            repo.AddParameter("@UserName", UserName);
            return repo.GetFromDatabaseWithQuery("getUsuario").Single();
        }

        
        #endregion
    }
}
