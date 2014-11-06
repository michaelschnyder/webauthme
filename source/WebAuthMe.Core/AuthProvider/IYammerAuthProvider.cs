namespace WebAuthMe.Core.AuthProvider
{
    public interface IYammerAuthProvider
    {
        string GetLoginUrl(params object[] settings);
        UserIdentity HandleCallback(string callBackInfo);
    }
}