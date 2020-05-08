using System.Web.Mvc;

namespace SistContabilidadMCA.Areas.AdministracionMCA
{
    public class AdministracionMCAAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AdministracionMCA";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AdministracionMCA_default",
                "AdministracionMCA/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}