using System;

namespace Lykke.Service.ClientAssetRule.Core.Domain
{
    public class ClientRegulation : IClientRegulation
    {
        public ClientRegulation()
        {
        }

        public ClientRegulation(string regulationId, bool kyc, bool active)
        {
            RegulationId = regulationId ?? throw new ArgumentNullException(nameof(regulationId));
            Kyc = kyc;
            Active = active;
        }

        public string RegulationId { get; set; }

        public bool Kyc { get; set; }

        public bool Active { get; set; }
    }
}
