using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SistContabilidadMCA.Models;
using DataLayerMCA;
using BusinessLogicMCA;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SistContabilidadMCA.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Instancias
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private UsuariosASPBL repoUsuario = new UsuariosASPBL();
        #endregion

        #region Login y LogOff        
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (model.UserName != null && model.Password != null)
            {
                var result = await SignInManager.PasswordSignInAsync(model.UserName.Trim(), model.Password.Trim(), model.RememberMe, shouldLockout: true);
                switch (result)
                {
                    case SignInStatus.Success:
                        {
                            var usuario = UsuariosASPBL.GetUsuario(model.UserName.Trim());
                            if (usuario.LockoutEnabled == true)
                            {

                                Session["usuario"] = usuario.FirstName + ' ' + usuario.LastName;
                                Session["Rol"] = usuario.Rol;
                                Session["usuarioMCAId"] = usuario.usuarioId;
                                if (usuario.EmailConfirmed == true)
                                    return RedirectToAction("Index", "Home", new { Area = "" });
                                else
                                    return RedirectToAction("ChangePassword", "Account");
                            }
                            else
                            {
                                ViewBag.error = "Usuario bloqueado o deshabilitado";
                                return View(model);
                            }


                        }

                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                        ViewBag.error = "Usuario o contraseña invalida";
                        return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Cambio de Contrasena

        [HttpGet]
        public ActionResult ChangePassword(int? id)
        {
            ViewBag.rol = Global.GetRoleUser(User);
            ViewBag.sms = id.HasValue ? "Contraseña cambiada exitosamente" : null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordAccountModelsView model)
        {

            model.UserName = User.Identity.Name;
            var user = await UserManager.FindByNameAsync(model.UserName);
            /***Valida la contraseña anterior**/
            bool existe = CheckPaswordUser(user, model.OldPassword.Trim());
            if (!existe)
            {
                ViewBag.error = "Su vieja contraseña no coincide con la registrada";
                return View();
            }
            /***Valida si la nueva contraseña es identica a la anterior ***/
            bool passwordRepetida = CheckPaswordUser(user, model.NewPassword.Trim());
            if (passwordRepetida)
            {
                ViewBag.error = "Su vieja contraseña no puede ser identica a la nueva, favor intente otra contraseña";
                return View();
            }
            /***Proceso de cambio de contraseña***/
            UserManager<ApplicationUser> _userManager_ = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            _userManager_.RemovePassword(user.Id);
            _userManager_.AddPassword(user.Id, model.NewPassword);
            var toUsuario = repoUsuario.GetByID(user.Id);
            toUsuario.EmailConfirmed = true;
            repoUsuario.Update(toUsuario);
            ViewBag.exito = "Contraseña cambiada exitosamente";
            return View();
        }
        #endregion

        #region Recuperación de Contraseña

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RecoveryPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RecoveryPassword(RecoveryPasswordViewModel recovery)
        {
            if (recovery.EmailUser != null)
            {
                /**** Validar si el email escrito esta registrado en un usuario****/
                var toUsuario = repoUsuario.Get(x => x.Email == recovery.EmailUser).SingleOrDefault();
                if (toUsuario == null)
                {
                    ViewBag.errorEmail = "El correo electrónico no se encuentra en los registro de usuarios.";
                    return View();
                }

                else
                {
                    /***Actualizacion del campo EmailConfirmed,  para que solicite el cambio de contraseña cuando inicie seseion***/
                    toUsuario.EmailConfirmed = false;
                    repoUsuario.Update(toUsuario);
                    /***Generacion de Contraseña***/
                    string NewPassword = IdentityManagerBL.GeneratePassword();
                    UserManager<ApplicationUser> _userManager_ = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    _userManager_.RemovePassword(toUsuario.Id);
                    _userManager_.AddPassword(toUsuario.Id, NewPassword);
                    /***Enviar al correo los datos correspondientes***/
                    bool datosEmailConfigurados = EmailBL.EmailDatosConfigurado();
                    if (datosEmailConfigurados)
                    {
                        try
                        {
                            string body = EmailBL.GetHtmlEmail(opcion: 1, httpContext: Request, FirstName: toUsuario.FirstName, LastName: toUsuario.LastName, UserName: toUsuario.UserName, contrasena: NewPassword);
                            EmailBL.SendEmail(Subject: "Solicitud de Contraseña", ToEmail: toUsuario.Email, body: body);

                        }
                        catch (Exception ex)
                        {
                            ///**** En caso de algun error, notificar en la aplicacion ****/
                            ViewBag.errorEmail = ex.Message;
                            return View();
                        }
                    }
                    else
                    {
                        /**** En caso de algun error, notificar en la aplicacion ****/
                        ViewBag.errorEmail = "Ocurrio un error al enviar el correo electrónico. No tiene configurado los datos del correo electronico, favor configurar on comunicarse con el administrador del sistema";
                        return View();
                    }
                }
                ViewBag.exitoso = "La solicitud de contraseña fue generada exitosamente, favor revisar su correo electrónico " + recovery.EmailUser.Trim();
                return View();
            }
            else
                return View();
        }


        #endregion


        #region Funciones
        /***Funcion para validar contraseña***/
        public static bool CheckPaswordUser(ApplicationUser user, string password)
        {
            UserManager<ApplicationUser> _userManager_ = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            bool retorno = _userManager_.CheckPassword(user, password);
            return retorno;
        }
        #endregion

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }


}