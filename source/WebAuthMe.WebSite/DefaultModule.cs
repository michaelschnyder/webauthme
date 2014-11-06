using System.Linq;
using Nancy;
using WebAuthMe.Core;
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
                    var providers = this.data.GetAuthProviderSettingsForApplication(app.PartitionKey);
                }

                return "Select AuthProvider for " + uniqueAppName;

            };
        }
    }
}
