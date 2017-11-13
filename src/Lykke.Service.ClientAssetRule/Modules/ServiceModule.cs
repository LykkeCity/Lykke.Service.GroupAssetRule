using Autofac;
using Common.Log;
using Lykke.Service.ClientAssetRule.Core.Services;
using Lykke.Service.ClientAssetRule.Services;

namespace Lykke.Service.ClientAssetRule.Modules
{
    public class ServiceModule : Module
    {
        private readonly ILog _log;
        
        public ServiceModule(ILog log)
        {
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            builder.RegisterType<RuleService>()
                .As<IRuleService>();

            builder.RegisterType<ClientAssetService>()
                .As<IClientAssetService>();
        }
    }
}
