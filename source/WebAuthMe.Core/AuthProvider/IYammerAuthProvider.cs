using System.Collections.Generic;

namespace WebAuthMe.Core.AuthProvider
{
    public interface IYammerAuthProvider
    {
        
        string GetLoginUrl();
        UserIdentity HandleCallback(Dictionary<string, string> info);
    }
}