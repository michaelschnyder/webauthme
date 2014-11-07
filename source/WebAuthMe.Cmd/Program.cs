using System;
using System.Net;
using WebAuthMe.Core;
using WebAuthMe.Server;

namespace WebAuthMe.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var ipEndPoint = new IPEndPoint(IPAddress.Loopback, 81);

            var core = new WebAuthMeServer(new WebAuthConfiguration()
            {
                IpEndPoint = ipEndPoint,
                ConnectionString = "UseDevelopmentStorage=true"
            });

            core.Start();

            Console.Write("Service started on " + ipEndPoint + ". Press a key to end");
            Console.ReadKey();

            core.Stop();
        }
    }
}
