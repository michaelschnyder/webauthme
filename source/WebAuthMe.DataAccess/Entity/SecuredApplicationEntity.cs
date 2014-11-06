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
}
