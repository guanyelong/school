using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SP.HIS.Startup))]
namespace SP.HIS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
