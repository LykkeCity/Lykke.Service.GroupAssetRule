
using System.Collections.Generic;
using System.Linq;
using Lykke.Service.ClientAssetRule.Client.Models;

namespace Lykke.Service.ClientAssetRule.Client
{
    internal static class AutorestClientMapper
    {
        internal static RuleModel ToModel(this AutorestClient.Models.AssetGroupRuleModel model)
        {
            return new RuleModel
            {
                Id = model.Id,
                Name = model.Name,
                RegulationId = model.RegulationId,
                AllowedAssetGroups = model.AllowedAssetGroups?.ToList() ?? new List<string>(),
                DeclinedAssetGroups = model.DeclinedAssetGroups?.ToList() ?? new List<string>()
            };
        }

        internal static AssetConditionLayerRuleModel ToModel(this AutorestClient.Models.AssetConditionLayerRuleModel model)
        {
            return new AssetConditionLayerRuleModel
            {
                Name = model.Name,
                RegulationId = model.RegulationId,
                Layers = model.Layers?.ToList() ?? new List<string>()
            };
        }
    }
}
