using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WGHotel.Startup))]
namespace WGHotel
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
