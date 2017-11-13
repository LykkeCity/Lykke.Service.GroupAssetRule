using System;
using System.Linq;
using Autofac;
using Common;
using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using System.Threading.Tasks;
using Lykke.Service.ClientAssetRule.Core.Domain;
using Lykke.Service.ClientAssetRule.Core.Services;
using Lykke.Service.ClientAssetRule.Core.Settings.ServiceSettings;
using Lykke.Service.ClientAssetRule.Rabbit.Messages;

namespace Lykke.Service.ClientAssetRule.Rabbit.Subscribers
{
    public class RegulationSubscriber : IStartable, IStopable
    {
        private readonly ILog _log;
        private readonly IClientAssetService _clientAssetService;
        private readonly RegulationQueue _settings;
        private RabbitMqSubscriber<RegulationMessage> _subscriber;

        public RegulationSubscriber(ILog log, IClientAssetService clientAssetService, RegulationQueue settings)
        {
            _log = log;
            _clientAssetService = clientAssetService;
            _settings = settings;
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .CreateForSubscriber(_settings.ConnectionString, _settings.Exchange, "clientassetrule")
                .MakeDurable();

            settings.DeadLetterExchangeName = null;

            _subscriber = new RabbitMqSubscriber<RegulationMessage>(settings,
                    new ResilientErrorHandlingStrategy(_log, settings,
                        TimeSpan.FromSeconds(10),
                        next: new DeadQueueErrorHandlingStrategy(_log, settings)))
                .SetMessageDeserializer(new JsonMessageDeserializer<RegulationMessage>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .SetLogger(_log)
                .Start();
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }

        public void Stop()
        {
            _subscriber.Stop();
        }

        private Task ProcessMessageAsync(RegulationMessage message)
        {
            return _clientAssetService.UpdateAsync(message.ClientId, message.Regulations.Select(o =>
                new ClientRegulation
                {
                    RegulationId = o.RegulationId,
                    Active = o.Active,
                    Kyc = o.Kyc
                }));
        }
    }
}
