using System.Collections.Generic;

namespace Lykke.Service.ClientAssetRule.Core.Domain
{
    public class AssetGroups : IAssetGroups
    {
        public List<string> Allowed { get; set; }

        public List<string> Declined { get; set; }
    }
}
