using System.Collections.Generic;

namespace Lykke.Service.ClientAssetRule.Client.Models
{
    /// <summary>
    /// Represents an asset condition layer rule.
    /// </summary>
    public class AssetConditionLayerRuleModel
    {
        /// <summary>
        /// Gets or sets a rule name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a regulation id.
        /// </summary>
        public string RegulationId { get; set; }

        /// <summary>
        /// Gets or sets a collection of asset condition layers.
        /// </summary>
        public List<string> Layers { get; set; }
    }
}
