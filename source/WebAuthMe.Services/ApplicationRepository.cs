using System.Collections.Generic;

namespace WebAuthenticateMe.Services
{
    public class ApplicationRepository
    {
        public SecuredApplication GetApplication(string id)
        {
            return new SecuredApplication()
            {
                AuthenticationProviders = new AuthenticationProvider[]{ new YammerAuthenticationProvider(), }
            };
        }
    }

    public class SecuredApplication
    {
        public IList<AuthenticationProvider> AuthenticationProviders { get; set; }
    }

    public abstract class AuthenticationProvider
    {

    }

    class YammerAuthenticationProvider : AuthenticationProvider
    {

    }
}
