using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.ClientAssetRule.Models.AssetConditionLayerRule
{
    public class AssetConditionLayerRuleModel
    {
        public AssetConditionLayerRuleModel()
        {
            Layers = new List<string>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string RegulationId { get; set; }

        [Required]
        public List<string> Layers { get; set; }
    }
}
