using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Lykke.Service.ClientAssetRule.AzureRepositories.AssetConditionLayer;
using Lykke.Service.ClientAssetRule.Core.Domain.AssetConditionLayer;

namespace Lykke.Service.ClientAssetRule.AzureRepositories
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AssetConditionLayerRuleEntity, AssetConditionLayerRule>(MemberList.Destination)
                .ForMember(dest => dest.Layers, opt => opt.MapFrom(src => Split(src.Layers)))
                .ConstructUsing(o => new AssetConditionLayerRule());

            CreateMap<IAssetConditionLayerRule, AssetConditionLayerRuleEntity>(MemberList.Source)
                .ForMember(dest => dest.Layers, opt => opt.MapFrom(src => Join(src.Layers)))
                .ConstructUsing(o => new AssetConditionLayerRuleEntity());
        }

        private static string Join(List<string> values)
            => string.Join("|", values ?? new List<string>());

        private static List<string> Split(string value)
            => (value ?? "").Split("|", StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}
