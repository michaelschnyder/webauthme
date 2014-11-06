using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using WebAuthMe.DataAccess.Entity;

namespace WebAuthMe.DataAccess
{
    public interface IAzureDataAccessLayer
    {
        SecuredApplicationEntity GetApplication(string uniqueName);
        List<AuthenticationProviderSettingEntity> GetAuthProviderSettingsForApplication(string uniqueName);
    }

    public class AzureDataAccessLayer : IAzureDataAccessLayer
    {
        private string connectionString;
        private CloudStorageAccount account;
        private CloudTableClient tableClient;
        
        public string SecuredApplicationTableName = "securedapps";
        public string AuthProvidersTableName = "authproviders";
        private CloudTable securedApps;
        private CloudTable authProviders;

        public AzureDataAccessLayer(string connectionString)
        {
            this.connectionString = connectionString;

            this.account = CloudStorageAccount.Parse(this.connectionString);
            this.tableClient = this.account.CreateCloudTableClient();

            this.securedApps = tableClient.GetTableReference(SecuredApplicationTableName);
            this.authProviders = tableClient.GetTableReference(AuthProvidersTableName);

            this.securedApps.CreateIfNotExists();
            this.authProviders.CreateIfNotExists();
        }

        public SecuredApplicationEntity GetApplication(string uniqueName)
        {
            TableQuery<SecuredApplicationEntity> rangeQuery = new TableQuery<SecuredApplicationEntity>().Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, uniqueName));

            var result = this.securedApps.ExecuteQuery(rangeQuery);

            return result.First();
        }

        public List<AuthenticationProviderSettingEntity> GetAuthProviderSettingsForApplication(string uniqueName)
        {
            TableQuery<AuthenticationProviderSettingEntity> rangeQuery = new TableQuery<AuthenticationProviderSettingEntity>().Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, uniqueName));

            var result = this.authProviders.ExecuteQuery(rangeQuery);

            return result.ToList();
        }
    
    }
}
