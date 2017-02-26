using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Liquid.Security.UI.Startup))]
namespace Liquid.Security.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
