using Autofac;
using Common;
using Lykke.Service.ClientAssetRule.Core.Settings.ServiceSettings;
using Lykke.Service.ClientAssetRule.Rabbit.Subscribers;

namespace Lykke.Service.ClientAssetRule.Modules
{
    public class RabbitModule : Module
    {
        private readonly RabbitMqSettings _settings;

        public RabbitModule(RabbitMqSettings settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RegulationSubscriber>()
                .AsSelf()
                .As<IStartable>()
                .As<IStopable>()
                .AutoActivate()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.RegulationQueue));
        }
    }
}
