using System;
using Autofac;

namespace Lykke.Service.ClientAssetRule.Client
{
    public static class AutofacExtension
    {
        public static void RegisterClientAssetRuleClient(this ContainerBuilder builder, ClientAssetRuleServiceClientSettings settings)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (string.IsNullOrWhiteSpace(settings.ServiceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(settings.ServiceUrl));

            builder.RegisterInstance(new ClientAssetRuleClient(settings.ServiceUrl)).As<IClientAssetRuleClient>().SingleInstance();
        }
    }
}
