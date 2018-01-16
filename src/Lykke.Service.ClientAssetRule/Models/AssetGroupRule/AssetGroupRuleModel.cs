using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.ClientAssetRule.Models.AssetGroupRule
{
    public class AssetGroupRuleModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string RegulationId { get; set; }

        [Required]
        public List<string> AllowedAssetGroups { get; set; }

        [Required]
        public List<string> DeclinedAssetGroups { get; set; }
    }
}
