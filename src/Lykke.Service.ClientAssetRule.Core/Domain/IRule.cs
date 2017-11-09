using System.Collections.Generic;

namespace Lykke.Service.ClientAssetRule.Core.Domain
{
    public interface IRule
    {
        string Id { get; }

        string Name { get; }

        string RegulationId { get; }

        List<string> AllowedAssetGroups { get; }

        List<string> DeclinedAssetGroups { get; }
    }
}
