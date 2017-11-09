
using System.Linq;
using Lykke.Service.ClientAssetRule.Client.Models;

namespace Lykke.Service.ClientAssetRule.Client
{
    internal static class AutorestClientMapper
    {
        internal static RuleModel ToModel(this AutorestClient.Models.RuleModel model)
        {
            return new RuleModel
            {
                Id = model.Id,
                Name = model.Name,
                RegulationId = model.RegulationId,
                AllowedAssetGroups = model.AllowedAssetGroups.ToList(),
                DeclinedAssetGroups = model.AllowedAssetGroups.ToList()
            };
        }
    }
}
