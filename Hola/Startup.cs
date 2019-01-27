using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hola.Startup))]
namespace Hola
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
