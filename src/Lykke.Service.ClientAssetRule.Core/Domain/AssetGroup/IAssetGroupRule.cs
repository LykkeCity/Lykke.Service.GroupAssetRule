using System.Collections.Generic;

namespace Lykke.Service.ClientAssetRule.Core.Domain.AssetGroup
{
    public interface IAssetGroupRule
    {
        string Id { get; }

        string Name { get; }

        string RegulationId { get; }

        List<string> AllowedAssetGroups { get; }

        List<string> DeclinedAssetGroups { get; }
    }
}
