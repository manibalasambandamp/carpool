using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarPool.Startup))]
namespace CarPool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
