using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebAuthMe.Core;

namespace WebAuthenticateMe.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var ipEndPoint = new IPEndPoint(IPAddress.Loopback, 9001);

            var core = new WebAuthMeServer(ipEndPoint);
            core.Start();

            Console.Write("Service started on " + ipEndPoint + ". Press a key to end");
            Console.ReadKey();

            core.Stop();
        }
    }
}
