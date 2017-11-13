namespace Lykke.Service.ClientAssetRule.Core.Settings.ServiceSettings
{
    public class ClientAssetRuleSettings
    {
        public DbSettings Db { get; set; }

        public RabbitMqSettings RabbitMq { get; set; }
    }
}
