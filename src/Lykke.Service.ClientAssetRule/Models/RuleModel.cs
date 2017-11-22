using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.ClientAssetRule.Models
{
    public class RuleModel
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
