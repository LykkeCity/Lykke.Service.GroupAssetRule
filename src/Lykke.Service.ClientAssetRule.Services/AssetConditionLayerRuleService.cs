using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Service.Assets.Client;
using Lykke.Service.ClientAssetRule.Core.Domain.AssetConditionLayer;
using Lykke.Service.ClientAssetRule.Core.Repositories;
using Lykke.Service.ClientAssetRule.Core.Services;
using Lykke.Service.ClientAssetRule.Core.Utils;

namespace Lykke.Service.ClientAssetRule.Services
{
    public class AssetConditionLayerRuleService : IAssetConditionLayerRuleService
    {
        private readonly IAssetConditionLayerRuleRepository _assetConditionLayerRuleRepository;
        private readonly IAssetsService _assetsService;
        private readonly ILog _log;

        public AssetConditionLayerRuleService(
            IAssetConditionLayerRuleRepository assetConditionLayerRuleRepository,
            IAssetsService assetsService,
            ILog log)
        {
            _assetConditionLayerRuleRepository = assetConditionLayerRuleRepository;
            _assetsService = assetsService;
            _log = log;
        }

        public async Task<IReadOnlyList<IAssetConditionLayerRule>> GetAsync()
        {
            return await _assetConditionLayerRuleRepository.GetAsync();
        }

        public async Task<IAssetConditionLayerRule> GetAsync(string regulationId)
        {
            return await _assetConditionLayerRuleRepository.GetAsync(regulationId);
        }

        public async Task SetAsync(string clientId, IReadOnlyList<string> regulations)
        {
            IReadOnlyList<IAssetConditionLayerRule> rules = await _assetConditionLayerRuleRepository.GetAsync();

            HashSet<string> allowed = rules
                .Where(o => regulations.Contains(o.RegulationId))
                .SelectMany(o => o.Layers)
                .ToHashSet();

            HashSet<string> declined = rules
                .SelectMany(o => o.Layers)
                .ToHashSet();
            
            declined.ExceptWith(allowed);

            try
            {
                foreach (string assetConditionLayerId in declined)
                {
                    await _assetsService.AssetConditionRemoveLayerFromClientAsync(clientId, assetConditionLayerId);

                    await _log.WriteInfoAsync(nameof(AssetConditionLayerRuleService), nameof(SetAsync),
                        clientId
                            .ToContext(nameof(clientId))
                            .ToContext(nameof(assetConditionLayerId), assetConditionLayerId)
                            .ToJson(),
                        "Asset condition layer removed from client.");
                }

                foreach (string assetConditionLayerId in allowed)
                {
                    await _assetsService.AssetConditionAddLayerToClientAsync(clientId, assetConditionLayerId);

                    await _log.WriteInfoAsync(nameof(AssetConditionLayerRuleService), nameof(SetAsync),
                        clientId
                            .ToContext(nameof(clientId))
                            .ToContext(nameof(assetConditionLayerId), assetConditionLayerId)
                            .ToJson(),
                        "Asset condition layer added to client.");
                }
            }
            catch (Exception exception)
            {
                await _log.WriteErrorAsync(nameof(AssetConditionLayerRuleService), nameof(SetAsync),
                    clientId
                        .ToContext(nameof(clientId))
                        .ToJson(), exception);

                throw;
            }
        }

        public async Task AddAsync(IAssetConditionLayerRule assetConditionLayerRule)
        {
            await _assetConditionLayerRuleRepository.InsertAsync(assetConditionLayerRule);
        }

        public async Task UpdateAsync(IAssetConditionLayerRule assetConditionLayerRule)
        {
            await _assetConditionLayerRuleRepository.UpdateAsync(assetConditionLayerRule);
        }

        public async Task DeleteAsync(string regulationId)
        {
            await _assetConditionLayerRuleRepository.DeleteAsync(regulationId);
        }
    }
}
