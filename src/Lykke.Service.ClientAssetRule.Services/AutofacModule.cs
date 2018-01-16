using Autofac;
using Lykke.Service.ClientAssetRule.Core.Services;

namespace Lykke.Service.ClientAssetRule.Services
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            builder.RegisterType<AssetGroupRuleService>()
                .As<IAssetGroupRuleService>();

            builder.RegisterType<AssetConditionLayerRuleService>()
                .As<IAssetConditionLayerRuleService>();
        }
    }
}
