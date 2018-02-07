using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientAssetRule.Core.Domain.AssetGroup;

namespace Lykke.Service.ClientAssetRule.Core.Services
{
    public interface IAssetGroupRuleService
    {
        Task<IEnumerable<IAssetGroupRule>> GetAllAsync();
        Task SetAsync(string clientId, IReadOnlyList<string> regulations);
        Task<IAssetGroupRule> GetByIdAsync(string id);
        Task InsertAsync(IAssetGroupRule rule);
        Task UpdateAsync(IAssetGroupRule rule);
        Task DeleteAsync(string id);
    }
}
