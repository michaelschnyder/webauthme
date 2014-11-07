using System;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Mustache;
using Nancy;
using Newtonsoft.Json;
using WebAuthMe.Core;
using WebAuthMe.Core.AuthProvider;
using WebAuthMe.DataAccess;
using WebAuthMe.Services;

namespace WebAuthMe.WebSite
{
    public class DefaultModule : NancyModule
    {
        private readonly AuthProviderService authProviderService;

        public DefaultModule()
        {
            this.authProviderService = new AuthProviderService(WebAuthConfiguration.Current);

            Get["/greet/{name}"] = x => string.Concat("Hello ", x.name);

            Get["/"] = x =>
            {
                var uniqueAppName = this.GetUniqueAppName();

                var appAuthInfo = this.authProviderService.GetAppAuthInfo(uniqueAppName);

                if (appAuthInfo == null)
                {
                    return "Sorry! No configuration for " + uniqueAppName;
                }

                try
                {
                    var template = this.authProviderService.GetAuthScreenTemplate(uniqueAppName);

                    var compiler = new FormatCompiler();

                    Generator generator = compiler.Compile(template);
                    var result = generator.Render(appAuthInfo);

                    return result;

                }
                catch (Exception e)
                {
                    var result = "<h1>" + appAuthInfo.AppName + "</h1>";
                    appAuthInfo.AuthProviders.ForEach(i => result += "<br><a href=\"" + i.LoginUrl + "\">" + i.Name + "</a>");
                    return result;
                }
            };
        }

        private string GetUniqueAppName()
        {
            var hostName = this.Request.Url.HostName;

            var splitted = hostName.Split('.');

            var uniqueAppName = splitted.First();
            return uniqueAppName;
        }
    }
}
