using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.ClientAssetRule.Core.Domain.AssetGroup;
using Lykke.Service.ClientAssetRule.Core.Repositories;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ClientAssetRule.AzureRepositories.AssetGroup
{
    public class AssetGroupRuleRepository : IAssetGroupRuleRepository
    {
        private readonly INoSQLTableStorage<AssetGroupRuleEntity> _storage;

        public AssetGroupRuleRepository(INoSQLTableStorage<AssetGroupRuleEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IEnumerable<IAssetGroupRule>> GetAllAsync()
        {
            IEnumerable<AssetGroupRuleEntity> entities = await _storage.GetDataAsync(GetPartitionKey());

            return entities.Select(FromEntity);
        }

        public async Task<IAssetGroupRule> GetByIdAsync(string id)
        {
            AssetGroupRuleEntity entity = await _storage.GetDataAsync(GetPartitionKey(), id);

            return entity != null ? FromEntity(entity) : null;
        }

        public async Task<IEnumerable<IAssetGroupRule>> GetByRegulationIdAsync(string regulationId)
        {
            string partitionKeyFilter = TableQuery
                .GenerateFilterCondition(nameof(AssetGroupRuleEntity.PartitionKey), QueryComparisons.Equal, GetPartitionKey());

            string regulationIdFilter = TableQuery
                .GenerateFilterCondition(nameof(AssetGroupRuleEntity.RegulationId), QueryComparisons.Equal, regulationId);

            string filter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, regulationIdFilter);

            var query = new TableQuery<AssetGroupRuleEntity>().Where(filter);

            IEnumerable<AssetGroupRuleEntity> entities = await _storage.WhereAsync(query);

            return entities.Select(FromEntity);
        }

        public Task InsertAsync(IAssetGroupRule rule)
        {
            return _storage.InsertAsync(CreateEntity(rule));
        }

        public Task UpdateAsync(IAssetGroupRule rule)
        {
            return _storage.MergeAsync(GetPartitionKey(), rule.Id, entity =>
            {
                entity.Name = rule.Name;
                entity.RegulationId = rule.RegulationId;
                entity.AllowedAssetGroups = Join(rule.AllowedAssetGroups);
                entity.DeclinedAssetGroups = Join(rule.DeclinedAssetGroups);
                return entity;
            });
        }

        public Task DeleteAsync(string id)
        {
            return _storage.DeleteAsync(GetPartitionKey(), id);
        }

        private static string GetPartitionKey()
            => "ClientAssetRule";

        private static string GetRowKey()
            => Guid.NewGuid().ToString("D");
        
        private static AssetGroupRuleEntity CreateEntity(IAssetGroupRule rule)
            => new AssetGroupRuleEntity
            {
                PartitionKey = GetPartitionKey(),
                RowKey = GetRowKey(),
                Name = rule.Name,
                RegulationId = rule.RegulationId,
                AllowedAssetGroups = Join(rule.AllowedAssetGroups),
                DeclinedAssetGroups = Join(rule.DeclinedAssetGroups)
            };

        private static IAssetGroupRule FromEntity(AssetGroupRuleEntity entity)
            => new AssetGroupRule
            {
                Id = entity.RowKey,
                Name = entity.Name,
                RegulationId = entity.RegulationId,
                AllowedAssetGroups = Split(entity.AllowedAssetGroups),
                DeclinedAssetGroups = Split(entity.DeclinedAssetGroups)
            };

        private static string Join(List<string> values)
            => string.Join("|", values ?? new List<string>());

        private static List<string> Split(string value)
            => (value ?? "").Split("|", StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}
