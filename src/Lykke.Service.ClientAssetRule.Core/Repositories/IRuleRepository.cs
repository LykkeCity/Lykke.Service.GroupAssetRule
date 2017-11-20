using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientAssetRule.Core.Domain;

namespace Lykke.Service.ClientAssetRule.Core.Repositories
{
    public interface IRuleRepository
    {
        Task<IEnumerable<IRule>> GetAllAsync();
        Task<IRule> GetByIdAsync(string id);
        Task<IEnumerable<IRule>> GetByRegulationIdAsync(string regulationId);
        Task InsertAsync(IRule rule);
        Task UpdateAsync(IRule rule);
        Task DeleteAsync(string id);
    }
}
