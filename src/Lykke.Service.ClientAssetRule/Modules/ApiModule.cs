using System;
using Autofac;
using Lykke.Service.Assets.Client;
using Lykke.Service.ClientAssetRule.Settings.Clients;

namespace Lykke.Service.ClientAssetRule.Modules
{
    public class ApiModule : Module
    {
        private readonly AssetsServiceClientSettings _settings;

        public ApiModule(AssetsServiceClientSettings settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance<IAssetsService>(new AssetsService(new Uri(_settings.ServiceUrl)));
        }
    }
}
