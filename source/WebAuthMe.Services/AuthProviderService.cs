using System.Collections.Generic;
using System.Linq;
using WebAuthMe.Core;
using WebAuthMe.Core.AuthProvider;
using WebAuthMe.DataAccess;

namespace WebAuthMe.Services
{
    public class AuthProviderService
    {
        private readonly WebAuthConfiguration configuration;
        private AzureDataAccessLayer data;

        public AuthProviderService(WebAuthConfiguration configuration)
        {
            this.configuration = configuration;
            var connectionString = this.configuration.ConnectionString;
            this.data = new AzureDataAccessLayer(connectionString);
        }

        public AppAuthInfoDto GetAppAuthInfo(string appIdentifier)
        {
            var app = this.data.GetApplication(appIdentifier);

            if (app == null)
            {
                return null;
            }
            var appAuthInfo = new AppAuthInfoDto()
            {
                AppIdentifier = appIdentifier,
                AppName = appIdentifier,
                AuthProviders = new List<AuthProviderDto>()
            };

            var providers = this.data.GetAuthProviderSettingsForApplication(app.PartitionKey);

            foreach (var provider in providers)
            {
                var loginUrl = string.Empty;

                if (provider.RowKey == "yammer")
                {
                    var concreteProvider = new YammerAuthProvider(provider.Configuration);

                    loginUrl = concreteProvider.GetLoginUrl();
                }

                appAuthInfo.AuthProviders.Add(new AuthProviderDto()
                {
                    Name = provider.RowKey,
                    LoginUrl = loginUrl
                });

            }

            return appAuthInfo;
        }

        public string GetAuthScreenTemplate(string appIdentifier)
        {
            var app = this.data.GetApplication(appIdentifier);

            return app.AuthScreenTemplate;
        }
    }

    public class AppAuthInfoDto
    {
        public string AppIdentifier { get; set; }

        public string AppName { get; set; }

        public List<AuthProviderDto> AuthProviders { get; set; } 

    }

    public class AuthProviderDto
    {
        public string Name { get; set; }

        public string LoginUrl { get; set; }
    }
}
