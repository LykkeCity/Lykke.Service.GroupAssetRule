using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ClientAssetRule.AzureRepositories.AssetGroup
{
    public class AssetGroupRuleEntity : TableEntity
    {
        public string Name { get; set; }

        public string RegulationId { get; set; }

        public string AllowedAssetGroups { get; set; }

        public string DeclinedAssetGroups { get; set; }
    }
}
