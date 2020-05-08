using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SistContabilidadMCA.Startup))]
namespace SistContabilidadMCA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
