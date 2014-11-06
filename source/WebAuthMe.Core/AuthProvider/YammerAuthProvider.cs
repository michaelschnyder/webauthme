using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace WebAuthMe.Core.AuthProvider
{
    public class YammerAuthProvider : IYammerAuthProvider
    {
        private readonly Dictionary<string, string> configuration;

        public string Id 
        {
            get { return "Yammer"; }
        }

        public YammerAuthProvider(Dictionary<string, string> configuration)
        {
            this.configuration = configuration;
        }

        public string GetLoginUrl()
        {
            return string.Format("https://www.yammer.com/dialog/oauth?client_id={0}", this.configuration["ClientId"]);
        }

        public UserIdentity HandleCallback(Dictionary<string, string> info)
        {
            const string accessUrlPattern = "https://www.yammer.com/oauth2/access_token.json?client_id={0}&client_secret={1}&code={2}";

            var url = string.Format(accessUrlPattern, this.configuration["ClientId"], this.configuration["ClientSecret"], info["code"]);

            var response = new HttpClient().GetStringAsync(url).Result;

            var yammerInfo = JsonConvert.DeserializeObject<RootObject>(response);

            return new UserIdentity()
            {
                FirstName = yammerInfo.user.first_name,
                LastName = yammerInfo.user.last_name,
                MailAddress = yammerInfo.user.contact.email_addresses.First().address
            };
        }
    }

    public class UserIdentity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string MailAddress { get; set; }
    }
}
