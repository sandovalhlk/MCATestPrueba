using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicMCA;
using DataLayerMCA;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using SistContabilidadMCA.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Text;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SistContabilidadMCA.Areas.Seguridad.Controllers
{
    [Authorize]
    public class AdministracionUsuariosController : Controller
    {
        #region Instancias
        // private RolesBL repoRoles = new RolesBL();
        private UsuariosBL repoUsuarios = new UsuariosBL();
        private ApplicationUserManager _userManager;
        private AspNetRolesBL repoAspNetRoles = new AspNetRolesBL();
        private EstadosBL repoEstados = new EstadosBL();
        private UsuariosASPBL repoUsuariosASP = new UsuariosASPBL();

        #endregion

        #region HTTP       

        [HttpGet]
        public ActionResult RegistroUsuarios()
        {
            //  ViewBag.Empleados = new SelectList(ObjetosVacaciones.GetListaEmpleados().Select(x => new { x.idEmpleado, nombre = x.nombre + " " + x.apellido, x.cargo, x.organigramaId }).OrderBy(x => x.nombre).ToList(), "idEmpleado", "nombre");
            //ViewBag.SelectUserMCA = new SelectList(new UsuariosBL().GetAll(), "usuarioId", "nombres", "apellidos");
            ViewBag.SelectUserMCA = new SelectList(repoUsuarios.GetAll().Select(x => new { x.usuarioId, nombre = x.nombres + " " + x.apellidos }).OrderBy(x => x.nombre).ToList(), "usuarioId", "nombre");
            ViewBag.SelectRolMCA = new SelectList(new AspNetRolesBL().GetAll(), "Id", "Name");
            ViewBag.SelectUcr = new SelectList(new UcrsBL().GetAll() ,"ucrId","ucr");
            return View();
        }

        public ActionResult AdministracionUser()
        {
            ViewBag.SelectEstado = new SelectList(repoEstados.Get(filter: x => x.estadoId == EstadosBL.KEY_ACTIVO || x.estadoId == EstadosBL.KEY_ANULADO).ToList(), "estadoId", "estado");
            ViewBag.SelectRol = new SelectList(repoAspNetRoles.GetAll().ToList(), "Id", "Name");
            return View();
        }

        #endregion

        #region Vistas Parciales
        public ActionResult ListUsuarios(int estadoId)
        {
            //var toUsuarios = repoUsuarios.Get(filter: x => x.AspNetUsers.estadoId == estadoId).ToList();
            var toUsuarios = UsuariosBL.GetListaUsuarios(estadoId).ToList();
            return PartialView("_ListUsuarios", toUsuarios);
        }

        #endregion

        #region Funciones AJAX

        [HttpPost]
        public async Task<ActionResult> SaveUser(RegisterViewModel toUser, string RolId, int usuarioMCAId, int ucrId)
        {
            string mensaje = "";
            /*** Verificar si el nombre de login y email ya existe, en caso afirmativo no se permite el ingreso de los datos ***/
            bool loginExiste = UsuariosASPBL.ExisteUser(toUser.UserName, out mensaje);
            bool emailExiste = UsuariosASPBL.ExisteEmail(toUser.Email, out mensaje);
            //verificar que el empleado no este asociado a un login 
            var toUserMCAValidar = UsuariosBL.ValidarUsuarioMCALogin(usuarioMCAId);

            if (loginExiste == false && emailExiste == false && toUserMCAValidar == null)
            {
                toUser.UserName = toUser.UserName.ToUpper().Trim().Replace("Ñ", "N");
                /***Generacion de Contraseña***/
                RegisterPasswordViewModel newPasswordModel = new RegisterPasswordViewModel();
                newPasswordModel.NewPassword = IdentityManagerBL.GeneratePassword();
                toUser.Password = newPasswordModel.NewPassword;
                /***Obtencion de datos Usuario: Nombres y Apellidos***/
                //var toEmpleado = ObjetosVacaciones.GetEmpleado(toUser.keyEmpleadoCatalogo);


                var toUserMCA = repoUsuarios.GetByID(usuarioMCAId);

                toUser.FirstName = toUserMCA.nombres; // toEmpleado.nombre.ToUpper().Trim().Replace("Ñ", "N");
                toUser.LastName = toUserMCA.apellidos; //toEmpleado.apellido.ToUpper().Trim().Replace("Ñ", "N");
                /***Registro de Uusario***/
                var user = new ApplicationUser { UserName = toUser.UserName.Trim(), Email = toUser.Email.Trim() };
                var result = await UserManager.CreateAsync(user, toUser.Password.ToString());
                if (result.Succeeded)
                {
                    /***Registro de User-ROL y datos de usuarios***/
                    UsuariosASPBL.addUserRol(user.Id, toUser.FirstName, toUser.LastName,  RolId, toUserMCA.usuarioId,ucrId);

                    /***Enviar contrase;a por email***/
                    bool datosEmailConfigurados = EmailBL.EmailDatosConfigurado();
                    if (datosEmailConfigurados)
                    {
                        /***Enviar al correo los datos correspondientes ***/
                        try
                        {
                            string body = EmailBL.GetHtmlEmail(opcion: 0, httpContext: Request, FirstName: toUser.FirstName, LastName: toUser.LastName, UserName: toUser.UserName, contrasena: newPasswordModel.NewPassword);
                            EmailBL.SendEmailPasswordWelcome(Subject: "Creacion de Inicio de Sesión. Usted debe cambiar su contraseña en el proximo incio de sesión.", Body: body,
                            NameWelcome: string.Format("{0} {1}", toUser.FirstName, toUser.LastName), ToEmail: toUser.Email, userApp: toUser.UserName, NewPassword: newPasswordModel.NewPassword);
                        }
                        catch (Exception ex)
                        {
                            /**** En caso de algun error, notificar en la aplicacion ****/
                            mensaje = ex.Message;
                        }
                    }
                }
                else
                    mensaje = result.Errors.FirstOrDefault();
            }

            else
            {
                if (loginExiste == true)
                {
                    mensaje = "El usuario " + toUser.UserName + " ya fue registrado";
                }

                if (emailExiste == true)
                {
                    mensaje = "El Email " + toUser.Email + " ya fue registrado";
                }

                if (loginExiste == true && emailExiste == true)
                {
                    mensaje = "El usuario " + toUser.UserName + " y Email " + toUser.Email + " ya fueron registrados";
                }
            }
            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditUser(int usuarioId)
        {
            var toUsuario = repoUsuarios.Get(filter: x => x.usuarioId == usuarioId).Select(x => new { x.nombres, login = x.AspNetUsers.UserName, estadoId = x.AspNetUsers.estadoId, rolId = x.AspNetUsers.AspNetRoles.Select(s => s.Id).FirstOrDefault(), bloqueado= x.AspNetUsers.LockoutEnabled }).FirstOrDefault();

            return Json(toUsuario, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveUsuario(int usuarioId, int estadoId, string rolId)
        {
            string msj = "";
            //Obtener el nuevo Rol a Actualizar
            try
            {

                var toUsuario = repoUsuarios.GetByID(usuarioId);


                using (var context = new DataLayerMCA.ContabilidadMCA())
                {

                    AspNetUsers userA = context.AspNetUsers.Find(toUsuario.aspNetUserId);
                    userA.estadoId = estadoId;

                    if (estadoId==2)
                        userA.LockoutEnabled = false;
                    else if (estadoId == 1)
                        userA.LockoutEnabled = true;


                    context.Entry(userA).State = EntityState.Modified;

                    AspNetUsers toUserAsp = context.AspNetUsers.Include(a => a.AspNetRoles).ToList().Find(ca => ca.Id == userA.Id);
                    toUserAsp.AspNetRoles.Clear();

                    AspNetRoles rolu = context.AspNetRoles.Find(rolId);
                    toUserAsp.AspNetRoles.Add(rolu);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msj = ex.Message;
            }


            return Json(msj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SavePersonalDirectiva(string nombres, string apellidos, string cedula)
        {
            string mensaje = "";
            var toUsuarios = new Usuarios();
            toUsuarios.nombres = nombres;
            toUsuarios.apellidos = apellidos;
            toUsuarios.cedula = cedula;
            repoUsuarios.Add(toUsuarios);
            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetComboPersonalDirectiva()
        {
            var toUsuarios = repoUsuarios.GetAll().Select(x => new {
                x.usuarioId,
                nombres = x.nombres + " " + x.apellidos
            }).ToList();
            return Json(toUsuarios, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Desbloquear(string login, bool bloqueado)
        {
            string msj = "";
            var keyUser = UsuariosASPBL.GetUsuario(login);
            

            try
            {

            if (bloqueado == true && (keyUser!=null))
            {
                    UserManager<ApplicationUser> _userManager_ = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    _userManager_.SetLockoutEnabled(keyUser.Id, false);
                    msj = "Usuario bloqueado exitosamente.";
                }
            else if (bloqueado == false && (keyUser != null))
                {

                    UserManager<ApplicationUser> _userManager_ = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    _userManager_.SetLockoutEnabled(keyUser.Id, true);
                    msj = "Usuario habilitado exitosamente";

                }

            }
            catch (Exception e)
            {
                msj = "Ocurrio un error " + e.Message;
                //throw;
            }

            return Json(msj,JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Funciones Especiales
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion       
    }
}