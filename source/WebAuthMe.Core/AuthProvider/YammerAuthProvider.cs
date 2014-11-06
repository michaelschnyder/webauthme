using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using Newtonsoft.Json;

namespace WebAuthMe.Core.AuthProvider
{
    public class YammerAuthProvider : IYammerAuthProvider
    {
        private readonly Dictionary<string, string> configuration;
        private string accountPictureSizeConfigName = "AccountPictureSize";

        public string Id 
        {
            get { return "Yammer"; }
        }

        public YammerAuthProvider(Dictionary<string, string> configuration)
        {
            this.configuration = configuration;

            if (!this.configuration.ContainsKey(this.accountPictureSizeConfigName))
            {
                this.configuration.Add(this.accountPictureSizeConfigName, "100");
            }
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

            var magUrl100x100 = yammerInfo.user.mugshot_url_template;

            var accountPictureSize = this.configuration[accountPictureSizeConfigName];

            magUrl100x100 = magUrl100x100.Replace("{width}", accountPictureSize);
            magUrl100x100 = magUrl100x100.Replace("{height}", accountPictureSize);

            return new UserIdentity()
            {
                FirstName = yammerInfo.user.first_name,
                LastName = yammerInfo.user.last_name,
                MailAddress = yammerInfo.user.contact.email_addresses.First().address,
                AccountPictureUrl = magUrl100x100
            };
        }
    }

    public class UserIdentity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string MailAddress { get; set; }
        public string AccountPictureUrl { get; set; }
    }
}
