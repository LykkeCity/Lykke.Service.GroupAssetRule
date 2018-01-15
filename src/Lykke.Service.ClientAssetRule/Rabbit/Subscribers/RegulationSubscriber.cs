using System;
using System.Linq;
using Autofac;
using Common;
using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client;
using Lykke.Service.ClientAssetRule.Core.Domain;
using Lykke.Service.ClientAssetRule.Core.Services;
using Lykke.Service.ClientAssetRule.Settings.ServiceSettings.Rabbit;
using Lykke.Service.ClientAssetRule.Rabbit.Messages;

namespace Lykke.Service.ClientAssetRule.Rabbit.Subscribers
{
    public class RegulationSubscriber : IStartable, IStopable
    {
        private readonly ILog _log;
        private readonly IRuleService _ruleService;
        private readonly IAssetsService _assetsService;
        private readonly RegulationQueue _settings;
        private RabbitMqSubscriber<RegulationMessage> _subscriber;

        public RegulationSubscriber(ILog log, IRuleService ruleService, IAssetsService assetsService, RegulationQueue settings)
        {
            _log = log;
            _ruleService = ruleService;
            _assetsService = assetsService;
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
                var clientRegulations = message.Regulations
                    .Select(o => new ClientRegulation
                    {
                        RegulationId = o.RegulationId,
                        Active = o.Active,
                        Kyc = o.Kyc
                    });

                IAssetGroups assetGroups = await _ruleService.GetAssetGroupsAsync(clientRegulations);

                foreach (string assetGroup in assetGroups.Declined)
                    await _assetsService.AssetGroupRemoveClientAsync(message.ClientId, assetGroup);

                foreach (string assetGroup in assetGroups.Allowed)
                    await _assetsService.AssetGroupAddOrReplaceClientAsync(message.ClientId, assetGroup);

                await _log.WriteInfoAsync(nameof(RegulationSubscriber), nameof(ProcessMessageAsync),
                    $"Client asset groups updated. {nameof(message)}: {message.ToJson()}. {nameof(assetGroups)}: {assetGroups.ToJson()}.");
            }
            catch (Exception exception)
            {
                await _log.WriteWarningAsync(nameof(RegulationSubscriber), nameof(ProcessMessageAsync),
                    $"{exception.Message}. {nameof(message)}: {message.ToJson()}.");
                throw;
            }
        }
    }
}
