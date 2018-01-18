// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.ClientAssetRule.Client.AutorestClient.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class AssetConditionLayerRuleModel
    {
        /// <summary>
        /// Initializes a new instance of the AssetConditionLayerRuleModel
        /// class.
        /// </summary>
        public AssetConditionLayerRuleModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the AssetConditionLayerRuleModel
        /// class.
        /// </summary>
        public AssetConditionLayerRuleModel(string name, string regulationId, IList<string> layers)
        {
            Name = name;
            RegulationId = regulationId;
            Layers = layers;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "RegulationId")]
        public string RegulationId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Layers")]
        public IList<string> Layers { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Name == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Name");
            }
            if (RegulationId == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "RegulationId");
            }
            if (Layers == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Layers");
            }
        }
    }
}