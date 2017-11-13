using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientAssetRule.Core.Domain;

namespace Lykke.Service.ClientAssetRule.Core.Services
{
    public interface IClientAssetService
    {
        Task UpdateAsync(string clientId, IEnumerable<IClientRegulation> clientRegulations);
    }
}
