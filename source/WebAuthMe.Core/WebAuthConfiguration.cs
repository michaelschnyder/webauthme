using System.Net;

namespace WebAuthMe.Core
{
    public class WebAuthConfiguration
    {
        public IPEndPoint IpEndPoint { get; set; }

        public string ConnectionString { get; set; }

        public static WebAuthConfiguration Current;

    }
}