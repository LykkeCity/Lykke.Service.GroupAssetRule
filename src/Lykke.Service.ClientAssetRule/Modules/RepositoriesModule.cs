using Autofac;
using AzureStorage;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.ClientAssetRule.AzureRepositories;
using Lykke.Service.ClientAssetRule.Core.Repositories;
using Lykke.SettingsReader;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ClientAssetRule.Modules
{
    public class RepositoriesModule : Module
    {
        private const string TableName = "ClientAssetRule";

        private readonly IReloadingManager<string> _connectionString;
        private readonly ILog _log;

        public RepositoriesModule(IReloadingManager<string> connectionString, ILog log)
        {
            _connectionString = connectionString;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new RuleRepository(CreateTable<RuleEntity>()))
                .As<IRuleRepository>();
        }

        private INoSQLTableStorage<T> CreateTable<T>() where T : TableEntity, new()
        {
            return AzureTableStorage<T>.Create(_connectionString, TableName, _log);
        }
    }
}
