using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyWarmPetWeb.Startup))]
namespace MyWarmPetWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
