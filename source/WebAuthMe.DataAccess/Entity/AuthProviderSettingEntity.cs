using Microsoft.WindowsAzure.Storage.Table;

namespace WebAuthMe.DataAccess.Entity
{
    public class AuthProviderSettingEntity : TableEntity
    {
        public AuthProviderSettingEntity()
        {
        }

        public string ConfigurationRaw { get; set; }

        public AuthProviderSettingEntity(string uniqueApplicatioName, string authenticationProvider)
        {
            this.PartitionKey = uniqueApplicatioName;
            this.RowKey = authenticationProvider;
        }
    }
}