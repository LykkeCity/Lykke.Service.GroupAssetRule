using Lykke.Service.ClientAssetRule.Settings.Clients;
using Lykke.Service.ClientAssetRule.Settings.ServiceSettings;
using Lykke.Service.ClientAssetRule.Settings.SlackNotifications;

namespace Lykke.Service.ClientAssetRule.Settings
{
    public class AppSettings
    {
        public ClientAssetRuleSettings ClientAssetRuleService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
        public AssetsServiceClientSettings AssetsServiceClient { get; set; }
    }
}
