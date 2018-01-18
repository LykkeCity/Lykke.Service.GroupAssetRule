using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.ClientAssetRule.Settings.ServiceSettings.Rabbit
{
    public class RegulationQueue
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }

        public string Exchange { get; set; }
    }
}
