using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Client_App.Startup))]
namespace Client_App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
