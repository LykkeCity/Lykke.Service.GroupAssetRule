using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.ClientAssetRule.Client.Models
{
    /// <summary>
    /// Represents an asset group rule.
    /// </summary>
    public class RuleModel
    {
        /// <summary>
        /// Gets or sets a rule id.
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Gets or sets a rule name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets a regulation id.
        /// </summary>
        public string RegulationId { get; set; }
        
        /// <summary>
        /// Gets or sets a collection of allowed asset groups.
        /// </summary>
        public List<string> AllowedAssetGroups { get; set; }
        
        /// <summary>
        /// Gets or sets a collection of declined asset groups.
        /// </summary>
        public List<string> DeclinedAssetGroups { get; set; }
    }
}
