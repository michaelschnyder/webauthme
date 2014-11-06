namespace WebAuthMe.Core.AuthProvider
{
    public class YammerAuthProvider : IYammerAuthProvider
    {
        public string Id 
        {
            get { return "Yammer"; }
        }

        public string GetLoginUrl(params object[] settings)
        {
            return string.Format("https://www.yammer.com/dialog/oauth?client_id={0}", settings);
        }

        public UserIdentity HandleCallback(string callBackInfo)
        {
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
