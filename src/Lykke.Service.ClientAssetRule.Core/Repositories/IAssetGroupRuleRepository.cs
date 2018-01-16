using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientAssetRule.Core.Domain.AssetGroup;

namespace Lykke.Service.ClientAssetRule.Core.Repositories
{
    public interface IAssetGroupRuleRepository
    {
        Task<IEnumerable<IAssetGroupRule>> GetAllAsync();
        Task<IAssetGroupRule> GetByIdAsync(string id);
        Task<IEnumerable<IAssetGroupRule>> GetByRegulationIdAsync(string regulationId);
        Task InsertAsync(IAssetGroupRule rule);
        Task UpdateAsync(IAssetGroupRule rule);
        Task DeleteAsync(string id);
    }
}
