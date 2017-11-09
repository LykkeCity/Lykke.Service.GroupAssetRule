using AutoMapper;
using Lykke.Service.ClientAssetRule.Core.Domain;
using Lykke.Service.ClientAssetRule.Models;

namespace Lykke.Service.ClientAssetRule.AutoMapper
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<RuleModel, Rule>();
            CreateMap<NewRuleModel, Rule>();
            CreateMap<IRule, RuleModel>();
        }

        public override string ProfileName => "Default profile";
    }
}
