using System.Collections.Generic;
using System.Linq;
using Nancy;
using WebAuthMe.Core;
using WebAuthMe.Core.AuthProvider;
using WebAuthMe.DataAccess;
using WebAuthMe.DataAccess.Entity;

namespace WebAuthMe.WebSite
{
    public class CallbackModule : NancyModule
    {
        private AzureDataAccessLayer data;

        public CallbackModule()
        {
            var connectionString = WebAuthConfiguration.Current.ConnectionString;
            this.data = new AzureDataAccessLayer(connectionString);

            Get["/callback/{name}"] = x =>
            {
                var authProviderName = x.Name;

                var uniqueAppName = this.GetAppName();

                IYammerAuthProvider provider = null;
                AuthProviderSettingEntity providerSettings = null;

                UserIdentity userIdentity = null;

                if (authProviderName == "yammer")
                {
                    providerSettings = this.data.GetAuthProviderSettingsForApplication(uniqueAppName)
                            .FirstOrDefault(p => p.RowKey == authProviderName);

                    provider = new YammerAuthProvider(providerSettings.Configuration);


                }

                if (provider != null)
                {

                    var dict = new Dictionary<string, string>();
                    var splitted2 = this.Request.Url.Query.Split('&');

                    foreach (var pair in splitted2)
                    {
                        var otherSplit = pair.Split('=');

                        dict.Add(otherSplit[0], otherSplit[1]);
                    }

                    userIdentity = provider.HandleCallback(dict);

                }

                if (userIdentity != null)
                {
                    var factory = new TokenFactory();

                    var appConfig = this.data.GetApplication(this.GetAppName());

                    var tokenString = factory.CreateToken(userIdentity, appConfig.SymmetricSecurityKey, appConfig.Audience);

                    return tokenString;
                }

                return "Cannot handle " + authProviderName;

            };
        }

        private string GetAppName()
        {
            var splitted = this.Request.Url.HostName.Split('.');

            var uniqueAppName = splitted.First();

            return uniqueAppName;
        }
    }
}
