using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PropertyForSale.Startup))]
namespace PropertyForSale
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
