using System.Collections.Generic;

namespace Lykke.Service.ClientAssetRule.Core.Domain.AssetConditionLayer
{
    public class AssetConditionLayerRule : IAssetConditionLayerRule
    {
        public AssetConditionLayerRule()
        {
        }

        public AssetConditionLayerRule(string name, string regulationId, List<string> layers)
        {
            Name = name;
            RegulationId = regulationId;
            Layers = layers;
        }

        public string Name { get; set; }
        public string RegulationId { get; set; }
        public List<string> Layers { get; set; }
    }
}
