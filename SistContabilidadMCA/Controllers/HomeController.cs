using DevExpress.Web.Mvc;
using SistContabilidadMCA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SistContabilidadMCA.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.Name != null || User.Identity.Name.Length > 0)
            {
              
                ViewBag.rol=Global.GetRoleUser(User);
              
            }
            else
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}