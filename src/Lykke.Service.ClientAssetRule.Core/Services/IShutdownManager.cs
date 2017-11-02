using System.Threading.Tasks;

namespace Lykke.Service.ClientAssetRule.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}