using Lykke.Service.ClientAssetRule.Core.Settings.ServiceSettings;
using Lykke.Service.ClientAssetRule.Core.Settings.SlackNotifications;

namespace Lykke.Service.ClientAssetRule.Core.Settings
{
    public class AppSettings
    {
        public ClientAssetRuleSettings ClientAssetRuleService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
