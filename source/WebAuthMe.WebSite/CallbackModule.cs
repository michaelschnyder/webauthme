using Nancy;
using WebAuthMe.Core;
using WebAuthMe.Core.AuthProvider;

namespace WebAuthMe.WebSite
{
    public class CallbackModule : NancyModule
    {
        public CallbackModule()
        {

            Get["/callback/{name}"] = x =>
            {
                var authProviderName = x.Name.ToString();

                IYammerAuthProvider provider = null;
                UserIdentity userIdentity = null;

                if (authProviderName == "yammer")
                {
                    provider = new YammerAuthProvider();

                }

                if (provider != null)
                {
                    userIdentity = provider.HandleCallback(this.Request.Path);

                }

                if (userIdentity != null)
                {
                    var factory = new TokenFactory();

                    var tokenString = factory.CreateToken(userIdentity);

                    return tokenString;
                }

                return "Cannot handle " + authProviderName;

            };
        }
    }
}
