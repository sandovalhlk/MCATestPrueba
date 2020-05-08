using System.Web.Mvc;

namespace SistContabilidadMCA.Areas.ContabilidadMCA
{
    public class ContabilidadMCAAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ContabilidadMCA";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ContabilidadMCA_default",
                "ContabilidadMCA/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}