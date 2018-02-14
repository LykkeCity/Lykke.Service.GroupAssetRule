using Lykke.Service.ClientAssetRule.Settings.ServiceSettings.Db;
using Lykke.Service.ClientAssetRule.Settings.ServiceSettings.Rabbit;

namespace Lykke.Service.ClientAssetRule.Settings.ServiceSettings
{
    public class ClientAssetRuleSettings
    {
        public DbSettings Db { get; set; }

        public RabbitMqSettings RabbitMq { get; set; }
    }
}
