using Microsoft.WindowsAzure.Storage.Table;

namespace WebAuthMe.DataAccess.Entity
{
    public class SecuredApplicationEntity : TableEntity
    {
        public SecuredApplicationEntity()
        {
        }

        public SecuredApplicationEntity(string uniqueApplicatioName)
        {
            this.PartitionKey = uniqueApplicatioName;
        }
    }

    public class AuthenticationProviderSettingEntity : TableEntity
    {
        public AuthenticationProviderSettingEntity()
        {
        }

        public AuthenticationProviderSettingEntity(string uniqueApplicatioName, string authenticationProvider)
        {
            this.PartitionKey = uniqueApplicatioName;
            this.RowKey = authenticationProvider;
        }
    }

}
