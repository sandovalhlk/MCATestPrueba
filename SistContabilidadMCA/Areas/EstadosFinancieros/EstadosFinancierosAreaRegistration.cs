using System.Web.Mvc;

namespace SistContabilidadMCA.Areas.EstadosFinancieros
{
    public class EstadosFinancierosAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EstadosFinancieros";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "EstadosFinancieros_default",
                "EstadosFinancieros/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}