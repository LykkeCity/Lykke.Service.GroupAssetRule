using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ClientAssetRule.AzureRepositories.AssetConditionLayer
{
    public class AssetConditionLayerRuleEntity : TableEntity
    {
        public AssetConditionLayerRuleEntity()
        {
        }

        public AssetConditionLayerRuleEntity(string partitionKey, string rowKey)
            :base(partitionKey, rowKey)
        {
        }

        public string Name { get; set; }
        public string RegulationId { get; set; }
        public string Layers { get; set; }
    }
}
