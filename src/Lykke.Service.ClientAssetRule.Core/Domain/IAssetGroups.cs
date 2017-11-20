using System.Collections.Generic;

namespace Lykke.Service.ClientAssetRule.Core.Domain
{
    public interface IAssetGroups
    {
        List<string> Allowed { get; set; }
        List<string> Declined { get; set; }
    }
}