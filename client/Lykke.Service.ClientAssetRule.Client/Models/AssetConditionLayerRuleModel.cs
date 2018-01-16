using System.Collections.Generic;

namespace Lykke.Service.ClientAssetRule.Client.Models
{
    public class AssetConditionLayerRuleModel
    {
        public string Name { get; set; }

        public string RegulationId { get; set; }

        public List<string> Layers { get; set; }
    }
}
