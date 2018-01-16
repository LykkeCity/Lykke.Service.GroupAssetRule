using System.Collections.Generic;

namespace Lykke.Service.ClientAssetRule.Core.Domain.AssetConditionLayer
{
    public interface IAssetConditionLayerRule
    {
        string Name { get; }

        string RegulationId { get; }

        List<string> Layers { get; }
    }
}
