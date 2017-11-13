using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client;
using Lykke.Service.ClientAssetRule.Core.Domain;
using Lykke.Service.ClientAssetRule.Core.Repositories;
using Lykke.Service.ClientAssetRule.Core.Services;

namespace Lykke.Service.ClientAssetRule.Services
{
    public class ClientAssetService : IClientAssetService
    {
        private readonly IRuleRepository _ruleRepository;
        private readonly IAssetsService _assetsService;

        public ClientAssetService(IRuleRepository ruleRepository, IAssetsService assetsService)
        {
            _ruleRepository = ruleRepository;
            _assetsService = assetsService;
        }

        public async Task UpdateAsync(string clientId, IEnumerable<IClientRegulation> clientRegulations)
        {
            var allowed = new List<string>();
            var declined = new List<string>();

            foreach (IClientRegulation clientRegulation in clientRegulations)
            {
                IEnumerable<IRule> rules = await _ruleRepository.GetByRegulationIdAsync(clientRegulation.RegulationId);

                foreach (IRule rule in rules)
                {
                    allowed.AddRange(rule.AllowedAssetGroups);
                    declined.AddRange(rule.DeclinedAssetGroups);
                }
            }

            declined = declined.Distinct()
                .ToList();

            allowed = allowed.Distinct()
                .Except(declined)
                .ToList();


            foreach (string assetGroup in declined)
                await _assetsService.AssetGroupRemoveClientAsync(clientId, assetGroup);

            foreach (string assetGroup in allowed)
                await _assetsService.AssetGroupAddOrReplaceClientAsync(clientId, assetGroup);
        }
    }
}
