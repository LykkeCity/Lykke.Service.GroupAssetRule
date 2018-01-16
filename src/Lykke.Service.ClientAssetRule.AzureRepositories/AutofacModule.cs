using Autofac;
using AzureStorage;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.ClientAssetRule.AzureRepositories.AssetConditionLayer;
using Lykke.Service.ClientAssetRule.AzureRepositories.AssetGroup;
using Lykke.Service.ClientAssetRule.Core.Repositories;
using Lykke.SettingsReader;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ClientAssetRule.AzureRepositories
{
    public class AutofacModule : Module
    {
        private readonly IReloadingManager<string> _connectionString;
        private readonly ILog _log;

        public AutofacModule(IReloadingManager<string> connectionString, ILog log)
        {
            _connectionString = connectionString;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            const string tableName = "ClientAssetRule";

            builder.RegisterInstance<IAssetGroupRuleRepository>(
                new AssetGroupRuleRepository(CreateTable<AssetGroupRuleEntity>(tableName)));

            builder.RegisterInstance<IAssetConditionLayerRuleRepository>(
                new AssetConditionLayerRuleRepository(CreateTable<AssetConditionLayerRuleEntity>(tableName)));
        }

        private INoSQLTableStorage<T> CreateTable<T>(string name)
            where T : TableEntity, new()
        {
            return AzureTableStorage<T>.Create(_connectionString, name, _log);
        }
    }
}
