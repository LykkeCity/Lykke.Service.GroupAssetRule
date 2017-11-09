using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.ClientAssetRule.Core.Domain;
using Lykke.Service.ClientAssetRule.Core.Repositories;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ClientAssetRule.AzureRepositories
{
    public class RuleRepository : IRuleRepository
    {
        private readonly INoSQLTableStorage<RuleEntity> _storage;

        public RuleRepository(INoSQLTableStorage<RuleEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IEnumerable<IRule>> GetAllAsync()
        {
            IEnumerable<RuleEntity> entities = await _storage.GetDataAsync(GetPartitionKey());

            return entities.Select(FromEntity);
        }

        public async Task<IRule> GetByIdAsync(string id)
        {
            RuleEntity entity = await _storage.GetDataAsync(GetPartitionKey(), id);

            return entity != null ? FromEntity(entity) : null;
        }

        public async Task<IEnumerable<IRule>> GetByRegulationIdAsync(string regulationId)
        {
            string partitionKeyFilter = TableQuery
                .GenerateFilterCondition(nameof(RuleEntity.PartitionKey), QueryComparisons.Equal, GetPartitionKey());

            string regulationIdFilter = TableQuery
                .GenerateFilterCondition(nameof(RuleEntity.RegulationId), QueryComparisons.Equal, regulationId);

            string filter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, regulationIdFilter);

            var query = new TableQuery<RuleEntity>().Where(filter);

            IEnumerable<RuleEntity> entities = await _storage.WhereAsync(query);

            return entities.Select(FromEntity);
        }

        public Task InsertAsync(IRule rule)
        {
            return _storage.InsertAsync(CreateEntity(rule));
        }

        public Task UpdateAsync(IRule rule)
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
        
        private static RuleEntity CreateEntity(IRule rule)
            => new RuleEntity
            {
                PartitionKey = GetPartitionKey(),
                RowKey = GetRowKey(),
                Name = rule.Name,
                RegulationId = rule.RegulationId,
                AllowedAssetGroups = Join(rule.AllowedAssetGroups),
                DeclinedAssetGroups = Join(rule.DeclinedAssetGroups)
            };

        private static IRule FromEntity(RuleEntity entity)
            => new Rule
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
