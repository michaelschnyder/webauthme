using Microsoft.Owin;
using Owin;
using WebAuthMe.WebSite;

[assembly: OwinStartup(typeof(Startup))]

namespace WebAuthMe.WebSite
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }
}
