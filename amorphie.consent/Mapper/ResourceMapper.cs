using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.consent.core.Model;
using AutoMapper;

namespace amorphie.consent.Mapper
{
    public sealed class ResourceMapper : Profile
    {
        public ResourceMapper()
        {
            CreateMap<Consent, ConsentDTO>().ReverseMap();
            CreateMap<Consent, OpenBankingConsentDTO>()
                .ForMember(dest => dest.ConsentPermission, opt => opt.MapFrom(src => src.ConsentPermission)) // Handle ConsentPermission mapping
                .ReverseMap();
            CreateMap<ConsentPermission, ConsentPermissionDto>().ReverseMap();
            CreateMap<ConsentDefinition, ConsentDefinitionDTO>().ReverseMap();
            CreateMap<Token, TokenDto>().ReverseMap();
        }
    }
}
