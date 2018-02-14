using System;
using System.Collections.Generic;

namespace Lykke.Service.ClientAssetRule.Core.Domain.AssetGroup
{
    public class AssetGroupRule : IAssetGroupRule
    {
        public AssetGroupRule()
        {
        }

        public AssetGroupRule(string id, string name, string regulationId, List<string> allowedAssetGroups, List<string> declinedAssetGroups)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            RegulationId = regulationId ?? throw new ArgumentNullException(nameof(regulationId));
            AllowedAssetGroups = allowedAssetGroups ?? throw new ArgumentNullException(nameof(allowedAssetGroups));
            DeclinedAssetGroups = declinedAssetGroups ?? throw new ArgumentNullException(nameof(declinedAssetGroups));
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string RegulationId { get; set; }
        public List<string> AllowedAssetGroups { get; set; }
        public List<string> DeclinedAssetGroups { get; set; }
    }
}
