using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ClientAssetRule.AzureRepositories
{
    public class RuleEntity : TableEntity
    {
        public string Name { get; set; }

        public string RegulationId { get; set; }

        public string AllowedAssetGroups { get; set; }

        public string DeclinedAssetGroups { get; set; }
    }
}
