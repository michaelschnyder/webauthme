using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebAuthMe.DataAccess.Entity
{
    public class AuthProviderSettingEntity : TableEntity
    {
        public AuthProviderSettingEntity()
        {
        }

        public string ConfigurationRaw { get; set; }

        public Dictionary<string, string> Configuration
        {
            get
            {
                var dict = new Dictionary<string, string>();

                var splitted = this.ConfigurationRaw.Split('&');
                foreach (var pair in splitted)
                {
                    var otherSplit = pair.Split('=');

                    dict.Add(otherSplit[0], otherSplit[1]);
                }

                return dict;

            }
        }

        public AuthProviderSettingEntity(string uniqueApplicatioName, string authenticationProvider)
        {
            this.PartitionKey = uniqueApplicatioName;
            this.RowKey = authenticationProvider;
        }
    }
}