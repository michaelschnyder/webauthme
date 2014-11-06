using Microsoft.Owin.Hosting;
using System;
using System.Net;
using WebAuthMe.Core;
using WebAuthMe.WebApi;

namespace WebAuthMe.Server
{
    public class WebAuthMeServer
    {
        private readonly WebAuthConfiguration configuration;

        private string baseAddress;
        private IDisposable webSiteApp;
        private IDisposable webSiteApi;

        public WebAuthMeServer(WebAuthConfiguration configuration)
        {
            this.configuration = configuration;
            WebAuthConfiguration.Current = configuration;
        }

        public IPEndPoint Endpoint
        {
            get { return this.configuration.IpEndPoint; }
        }

        public void Start()
        {

            this.baseAddress = "http://" + this.Endpoint.Address + ":" + this.Endpoint.Port;

            // Start OWIN host  for WebApi
            this.webSiteApi = WebApp.Start<Startup>(this.baseAddress + "/api");

            // Start OWIN host for WebSite
            this.webSiteApp = WebApp.Start<WebSite.Startup>(this.baseAddress + "/");

            
        }

        public void Stop()
        {
            
        }
    }
}
