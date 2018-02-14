using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.ClientAssetRule.Core.Domain.AssetConditionLayer;
using Lykke.Service.ClientAssetRule.Core.Repositories;

namespace Lykke.Service.ClientAssetRule.AzureRepositories.AssetConditionLayer
{
    public class AssetConditionLayerRuleRepository : IAssetConditionLayerRuleRepository
    {
        private readonly INoSQLTableStorage<AssetConditionLayerRuleEntity> _storage;

        public AssetConditionLayerRuleRepository(INoSQLTableStorage<AssetConditionLayerRuleEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IReadOnlyList<IAssetConditionLayerRule>> GetAsync()
        {
            IEnumerable<AssetConditionLayerRuleEntity> entities
                = await _storage.GetDataAsync(GetPartitionKey());

            return Mapper.Map<List<AssetConditionLayerRule>>(entities)
                .Cast<IAssetConditionLayerRule>()
                .ToList();
        }

        public async Task<IAssetConditionLayerRule> GetAsync(string regulationId)
        {
            AssetConditionLayerRuleEntity entity =
                await _storage.GetDataAsync(GetPartitionKey(), GetRowKey(regulationId));

            return Mapper.Map<AssetConditionLayerRule>(entity);
        }

        public async Task InsertAsync(IAssetConditionLayerRule assetConditionLayerRule)
        {
            var entity = new AssetConditionLayerRuleEntity(GetPartitionKey(), GetRowKey(assetConditionLayerRule.RegulationId));

            Mapper.Map(assetConditionLayerRule, entity);

            await _storage.InsertAsync(entity);
        }

        public async Task UpdateAsync(IAssetConditionLayerRule assetConditionLayerRule)
        {
            await _storage.MergeAsync(GetPartitionKey(), GetRowKey(assetConditionLayerRule.RegulationId), entity =>
            {
                Mapper.Map(assetConditionLayerRule, entity);
                return entity;
            });
        }

        public async Task DeleteAsync(string regulationId)
        {
            await _storage.DeleteAsync(GetPartitionKey(), GetRowKey(regulationId));
        }

        private static string GetPartitionKey()
            => "AssetConditionLayerRule";

        private static string GetRowKey(string regulationId)
            => regulationId.ToLower().Trim();
    }
}
