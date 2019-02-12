using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ats.Startup))]
namespace Ats
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
