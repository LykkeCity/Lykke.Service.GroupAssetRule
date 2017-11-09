using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientAssetRule.Core.Domain;

namespace Lykke.Service.ClientAssetRule.Core.Services
{
    public interface IRuleService
    {
        Task<IEnumerable<IRule>> GetAllAsync();
        Task<IRule> GetByIdAsync(string id);
        Task InsertAsync(IRule rule);
        Task UpdateAsync(IRule rule);
        Task DeleteAsync(string id);
    }
}
