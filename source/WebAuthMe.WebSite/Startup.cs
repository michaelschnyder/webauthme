using Microsoft.Owin;
using Nancy;
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

            StaticConfiguration.DisableErrorTraces = false;
        }
    }
}
