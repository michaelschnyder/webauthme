using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using WebAuthMe.Core;
using WebAuthMe.Server;

namespace WebAuthMe.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private WebAuthMeServer webAuthMeServer;

        public override void Run()
        {
            Trace.TraceInformation("WebAuthMe.Worker is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("WebAuthMe.Worker has been started");

            var roleInstanceEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["GlobalEndpoint"];
            var connectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");

            this.webAuthMeServer = new WebAuthMeServer(new WebAuthConfiguration()
            {
                IpEndPoint = roleInstanceEndpoint.IPEndpoint,
                ConnectionString = connectionString
            });

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WebAuthMe.Worker is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WebAuthMe.Worker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {

            this.webAuthMeServer.Start();

            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}
