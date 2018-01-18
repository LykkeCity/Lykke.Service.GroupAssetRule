using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Service.Assets.Client;
using Lykke.Service.ClientAssetRule.Core.Domain.AssetGroup;
using Lykke.Service.ClientAssetRule.Core.Repositories;
using Lykke.Service.ClientAssetRule.Core.Services;
using Lykke.Service.ClientAssetRule.Core.Utils;

namespace Lykke.Service.ClientAssetRule.Services
{
    public class AssetGroupRuleService : IAssetGroupRuleService
    {
        private readonly IAssetGroupRuleRepository _ruleRepository;
        private readonly IAssetsService _assetsService;
        private readonly ILog _log;

        public AssetGroupRuleService(
            IAssetGroupRuleRepository ruleRepository,
            IAssetsService assetsService,
            ILog log)
        {
            _ruleRepository = ruleRepository;
            _assetsService = assetsService;
            _log = log;
        }

        public Task<IEnumerable<IAssetGroupRule>> GetAllAsync()
        {
            return _ruleRepository.GetAllAsync();
        }

        public async Task SetAsync(string clientId, IReadOnlyList<string> regulations)
        {
            var allowed = new HashSet<string>();
            var declined = new HashSet<string>();

            foreach (string regulation in regulations)
            {
                IEnumerable<IAssetGroupRule> rules = await _ruleRepository.GetByRegulationIdAsync(regulation);

                foreach (IAssetGroupRule rule in rules)
                {
                    allowed.UnionWith(rule.AllowedAssetGroups);
                    declined.UnionWith(rule.DeclinedAssetGroups);
                }
            }

            allowed.ExceptWith(declined);
            
            try
            {
                foreach (string groupName in declined)
                {
                    await _assetsService.AssetGroupRemoveClientAsync(clientId, groupName);

                    await _log.WriteInfoAsync(nameof(AssetConditionLayerRuleService), nameof(SetAsync),
                        clientId
                            .ToContext(nameof(clientId))
                            .ToContext(nameof(groupName), groupName)
                            .ToJson(),
                        "Asset group removed from client.");
                }

                foreach (string groupName in allowed)
                {
                    await _assetsService.AssetGroupAddOrReplaceClientAsync(clientId, groupName);

                    await _log.WriteInfoAsync(nameof(AssetConditionLayerRuleService), nameof(SetAsync),
                        clientId
                            .ToContext(nameof(clientId))
                            .ToContext(nameof(groupName), groupName)
                            .ToJson(),
                        "Asset group added to client.");
                }
            }
            catch (Exception exception)
            {
                await _log.WriteErrorAsync(nameof(AssetGroupRuleService), nameof(SetAsync),
                    clientId
                        .ToContext(nameof(clientId))
                        .ToJson(), exception);

                throw;
            }
        }

        public  Task<IAssetGroupRule> GetByIdAsync(string id)
        {
            return _ruleRepository.GetByIdAsync(id);
        }

        public async Task InsertAsync(IAssetGroupRule rule)
        {
            IEnumerable<IAssetGroupRule> rules = await _ruleRepository.GetByRegulationIdAsync(rule.RegulationId);

            if(rules.Any())
                throw new InvalidOperationException("Rule for specified regulation already exist");

            await _ruleRepository.InsertAsync(rule);
        }

        public Task UpdateAsync(IAssetGroupRule rule)
        {
            return _ruleRepository.UpdateAsync(rule);
        }

        public Task DeleteAsync(string id)
        {
            return _ruleRepository.DeleteAsync(id);
        }
    }
}
