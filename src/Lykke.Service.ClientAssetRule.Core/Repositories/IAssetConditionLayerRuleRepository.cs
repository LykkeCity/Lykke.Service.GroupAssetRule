using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientAssetRule.Core.Domain.AssetConditionLayer;

namespace Lykke.Service.ClientAssetRule.Core.Repositories
{
    public interface IAssetConditionLayerRuleRepository
    {
        Task<IReadOnlyList<IAssetConditionLayerRule>> GetAsync();
        Task<IAssetConditionLayerRule> GetAsync(string regulationId);
        Task InsertAsync(IAssetConditionLayerRule assetConditionLayerRule);
        Task UpdateAsync(IAssetConditionLayerRule assetConditionLayerRule);
        Task DeleteAsync(string regulationId);
    }
}
