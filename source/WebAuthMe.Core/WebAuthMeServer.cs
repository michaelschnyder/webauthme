using System;
using System.Net;
using Microsoft.Owin.Hosting;
using WebAuthMe.WebApi;

namespace WebAuthMe.Core
{
    public class WebAuthMeServer
    {
        private readonly IPEndPoint endpoint;
        private string baseAddress;
        private IDisposable webApp;

        public WebAuthMeServer(IPEndPoint endpoint)
        {
            this.endpoint = endpoint;
        }

        public IPEndPoint Endpoint
        {
            get { return this.endpoint; }
        }

        public void Start()
        {

            this.baseAddress = "http://" + this.Endpoint.Address + ":" + this.Endpoint.Port + "/api";

            // Start OWIN host 
            this.webApp = WebApp.Start<Startup>(baseAddress);


        }

        public void Stop()
        {
            
        }
    }
}
