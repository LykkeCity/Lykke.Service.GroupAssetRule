using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.ClientAssetRule.Models
{
    public class RuleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string RegulationId { get; set; }
        public List<string> AllowedAssetGroups { get; set; }
        public List<string> DeclinedAssetGroups { get; set; }
    }
}
