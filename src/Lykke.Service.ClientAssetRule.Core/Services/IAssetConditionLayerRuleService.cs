using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientAssetRule.Core.Domain.AssetConditionLayer;

namespace Lykke.Service.ClientAssetRule.Core.Services
{
    public interface IAssetConditionLayerRuleService
    {
        Task<IEnumerable<IAssetConditionLayerRule>> GetAsync();
        Task<IAssetConditionLayerRule> GetAsync(string regulationId);
        Task SetAsync(string clientId, IEnumerable<string> regulations);
        Task AddAsync(IAssetConditionLayerRule assetConditionLayerRule);
        Task UpdateAsync(IAssetConditionLayerRule assetConditionLayerRule);
        Task DeleteAsync(string regulationId);
    }
}
