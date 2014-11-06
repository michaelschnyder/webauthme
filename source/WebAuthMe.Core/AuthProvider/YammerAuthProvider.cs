using System.Collections.Generic;

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

            return new UserIdentity()
            {
                FirstName = "John",
                LastName = "Doe",
                MailAddress = "john.doe@yammer.com"
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
