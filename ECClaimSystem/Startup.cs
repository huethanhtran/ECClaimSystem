using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ECClaimSystem.Startup))]
namespace ECClaimSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
