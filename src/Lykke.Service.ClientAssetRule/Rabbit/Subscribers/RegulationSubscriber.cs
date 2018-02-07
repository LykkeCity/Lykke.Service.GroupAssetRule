using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Common;
using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using System.Threading.Tasks;
using Lykke.Service.ClientAssetRule.Core.Services;
using Lykke.Service.ClientAssetRule.Settings.ServiceSettings.Rabbit;
using Lykke.Service.ClientAssetRule.Rabbit.Messages;

namespace Lykke.Service.ClientAssetRule.Rabbit.Subscribers
{
    public class RegulationSubscriber : IStartable, IStopable
    {
        private readonly IAssetConditionLayerRuleService _assetConditionLayerRuleService;
        private readonly IAssetGroupRuleService _ruleService;
        private readonly RegulationQueue _settings;
        private readonly ILog _log;
        private RabbitMqSubscriber<RegulationMessage> _subscriber;

        public RegulationSubscriber(
            IAssetConditionLayerRuleService assetConditionLayerRuleService,
            IAssetGroupRuleService ruleService,
            RegulationQueue settings,
            ILog log)
        {
            _assetConditionLayerRuleService = assetConditionLayerRuleService;
            _log = log;
            _ruleService = ruleService;
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

        private async Task ProcessMessageAsync(RegulationMessage message)
        {
            try
            {
                List<string> regulations = message.Regulations
                    .Select(o => o.RegulationId)
                    .ToList();

                await _assetConditionLayerRuleService.SetAsync(message.ClientId, regulations);

                await _log.WriteInfoAsync(nameof(RegulationSubscriber), nameof(ProcessMessageAsync),
                    message.ToJson(), "Client asset conditions layers updated");

                await _ruleService.SetAsync(message.ClientId, regulations);

                await _log.WriteInfoAsync(nameof(RegulationSubscriber), nameof(ProcessMessageAsync),
                    message.ToJson(), "Client asset groups updated");
            }
            catch (Exception exception)
            {
                await _log.WriteErrorAsync(nameof(RegulationSubscriber), nameof(ProcessMessageAsync),
                    message.ToJson(), exception);
                throw;
            }
        }
    }
}
