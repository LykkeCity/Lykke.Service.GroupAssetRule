namespace Lykke.Service.ClientAssetRule.Rabbit.Messages
{
    public class Regulation
    {
        public string RegulationId { get; set; }

        public bool Kyc { get; set; }

        public bool Active { get; set; }
    }
}
