using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Filters;
using FluentValidation.WebApi;
using Microsoft.Owin;
using Owin;
using WebAuthMe.WebApi;
using WebAuthMe.WebApi.Infrastructure;

[assembly: OwinStartup(typeof(Startup))]

namespace WebAuthMe.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //
            // Web API configuration and services
            var config = new HttpConfiguration();

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Start clean by replacing with filter provider for global configuration.
            // For these globally added filters we need not do any ordering as filters are 
            // executed in the order they are added to the filter collection
            config.Services.Replace(typeof(IFilterProvider), new ConfigurationFilterProvider());

            // Custom action filter provider which does ordering
            config.Services.Add(typeof(IFilterProvider), new OrderedFilterProvider());

            // configure FluentValidation model validator provider
            FluentValidationModelValidatorProvider.Configure(GlobalConfiguration.Configuration);

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            app.UseWebApi(config); 
        }
    }
}
