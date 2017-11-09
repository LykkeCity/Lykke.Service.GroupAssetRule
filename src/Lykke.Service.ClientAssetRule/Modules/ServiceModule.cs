using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.ClientAssetRule.AzureRepositories;
using Lykke.Service.ClientAssetRule.Core.Repositories;
using Lykke.Service.ClientAssetRule.Core.Services;
using Lykke.Service.ClientAssetRule.Core.Settings.ServiceSettings;
using Lykke.Service.ClientAssetRule.Services;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.ClientAssetRule.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<ClientAssetRuleSettings> _settings;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public ServiceModule(IReloadingManager<ClientAssetRuleSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            // TODO: Do not register entire settings in container, pass necessary settings to services which requires them
            // ex:
            //  builder.RegisterType<QuotesPublisher>()
            //      .As<IQuotesPublisher>()
            //      .WithParameter(TypedParameter.From(_settings.CurrentValue.QuotesPublication))

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

            RegisterRepositories(builder);
            RegisterServices(builder);

            builder.Populate(_services);
        }

        private void RegisterRepositories(ContainerBuilder builder)
        {
            const string tableName = "ClientAssetRule";

            builder.Register(c => new RuleRepository(
                    AzureTableStorage<RuleEntity>.Create(_settings.ConnectionString(x => x.Db.DataConnectionString),
                        tableName, _log)))
                .As<IRuleRepository>();
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<RuleService>()
                .As<IRuleService>();
        }
    }
}
