using System.Linq;
using System.Text;
using Nancy;
using WebAuthMe.Core;
using WebAuthMe.Core.AuthProvider;
using WebAuthMe.DataAccess;

namespace WebAuthMe.WebSite
{
    public class DefaultModule : NancyModule
    {
        private IAzureDataAccessLayer data;

        public DefaultModule()
        {
            var connectionString = WebAuthConfiguration.Current.ConnectionString;
            this.data = new AzureDataAccessLayer(connectionString);
            
            Get["/greet/{name}"] = x => string.Concat("Hello ", x.name);

            Get["/"] = x =>
            {

                var hostName = this.Request.Url.HostName;

                var splitted = hostName.Split('.');

                var uniqueAppName = splitted.First();

                var app = this.data.GetApplication(uniqueAppName);

                if (app != null)
                {
                    var response = new StringBuilder();

                    var providers = this.data.GetAuthProviderSettingsForApplication(app.PartitionKey);

                    // Create login methods

                    response.AppendLine("<h1>Select Auth for :" + uniqueAppName + "</h1>");

                    response.AppendLine("<ol>");

                    foreach (var provider in providers)
                    {
                        var allConfig = provider.ConfigurationRaw.Split(',');

                        var values = allConfig.Select(i => i.Split('=').Last()).ToArray();
                        var loginUrl = string.Empty;

                        if (provider.RowKey == "yammer")
                        {
                            var concreteProvider = new YammerAuthProvider(provider.Configuration);

                            loginUrl = concreteProvider.GetLoginUrl();
                        }

                        response.AppendLine(string.Format("<li><a href=\"{0}\">Take {1}</a></li>",loginUrl, provider.RowKey));

                    }

                    response.AppendLine("</ol>");
                    return response.ToString();
                }

                return "Sorry No AuthProvider for " + uniqueAppName;

            };
        }
    }
}
