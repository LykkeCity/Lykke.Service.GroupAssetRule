using System;
using Autofac;
using Common;
using Common.Log;
using Lykke.Service.Assets.Client;
using Lykke.Service.ClientAssetRule.Rabbit.Subscribers;
using Lykke.Service.ClientAssetRule.Settings;

namespace Lykke.Service.ClientAssetRule
{
    public class AutofacModule : Module
    {
        private readonly AppSettings _settings;
        private readonly ILog _log;

        public AutofacModule(AppSettings settings, ILog log)
        {
            _settings = settings;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterInstance<IAssetsService>(new AssetsService(new Uri(_settings.AssetsServiceClient.ServiceUrl)));

            builder.RegisterType<RegulationSubscriber>()
                .AsSelf()
                .As<IStartable>()
                .As<IStopable>()
                .AutoActivate()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.ClientAssetRuleService.RabbitMq.RegulationQueue));
        }
    }
}
