using System.Collections.Generic;

namespace Lykke.Service.ClientAssetRule.Rabbit.Messages
{
    public class RegulationMessage
    {
        public RegulationMessage()
        {
            Regulations = new List<Regulation>();
        }

        public string ClientId { get; set; }

        public List<Regulation> Regulations { get; set; }
    }
}
