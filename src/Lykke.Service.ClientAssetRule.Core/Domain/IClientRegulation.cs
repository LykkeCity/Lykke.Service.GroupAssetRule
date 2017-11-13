namespace Lykke.Service.ClientAssetRule.Core.Domain
{
    public interface IClientRegulation
    {
        bool Active { get; set; }
        bool Kyc { get; set; }
        string RegulationId { get; set; }
    }
}
