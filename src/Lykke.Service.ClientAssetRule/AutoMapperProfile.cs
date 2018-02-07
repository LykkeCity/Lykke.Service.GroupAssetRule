using AutoMapper;
using Lykke.Service.ClientAssetRule.Core.Domain.AssetGroup;
using Lykke.Service.ClientAssetRule.Core.Domain.AssetConditionLayer;
using Lykke.Service.ClientAssetRule.Models.AssetGroupRule;
using Lykke.Service.ClientAssetRule.Models.AssetConditionLayerRule;

namespace Lykke.Service.ClientAssetRule
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AssetGroupRuleModel, AssetGroupRule>(MemberList.Source);
            CreateMap<NewAssetGroupRuleModel, AssetGroupRule>(MemberList.Destination)
                .ForMember(e => e.Id, option => option.Ignore());
            CreateMap<IAssetGroupRule, AssetGroupRuleModel>(MemberList.Source);

            CreateMap<IAssetConditionLayerRule, AssetConditionLayerRuleModel>(MemberList.Source);
            CreateMap<AssetConditionLayerRuleModel, AssetConditionLayerRule>(MemberList.Destination);
        }
    }
}
