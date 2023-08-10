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
            CreateMap<Consent,HesapBilgisiRizaIstegiResponse>().ReverseMap();
            CreateMap<Consent, OpenBankingConsentDTO>()
                .ForMember(dest => dest.ConsentPermission, opt => opt.MapFrom(src => src.ConsentPermission)) // Handle ConsentPermission mapping
                .ReverseMap();
            CreateMap<ConsentPermission, ConsentPermissionDto>().ReverseMap();
            CreateMap<ConsentDefinition, ConsentDefinitionDTO>().ReverseMap();
            CreateMap<Token, TokenDto>().ReverseMap();
            CreateMap<ConsentDataDto, Consent>().ReverseMap();
            CreateMap<Consent,HhsConsentDto>().ReverseMap();
            // CreateMap<Token, TokenModel>().ReverseMap();
            CreateMap<Consent, HhsConsentDto>().ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token)).ReverseMap();
              CreateMap<TokenModel, (Token erisimToken, Token yenilemeToken)>()
            .ConstructUsing((src, ctx) =>
            {
                var erisimToken = new Token
                {
                    TokenValue = src.erisimBelirteci,
                    TokenType = "Access Token",
                    ExpireTime = src.gecerlilikSuresi,
                    ConsentId = src.ConsentId
                };

                var yenilemeToken = new Token
                {
                    TokenValue = src.yenilemeBelirteci,
                    TokenType = "Refresh Token",
                    ExpireTime = src.yenilemeBelirteciGecerlilikSuresi,
                    ConsentId = src.ConsentId
                };

                return (erisimToken, yenilemeToken);
            }).ReverseMap();

            CreateMap<TokenModel, (Token erisimToken, Token yenilemeToken)>()
    .ConstructUsing((src, ctx) =>
    {
        Token? erisimToken = null;
        Token? yenilemeToken = null;

        if (src.erisimBelirteci != null)
        {
            erisimToken = new Token
            {
                TokenValue = src.erisimBelirteci,
                TokenType = "Access Token",
                ExpireTime = src.gecerlilikSuresi,
                ConsentId = src.ConsentId
            };
        }

        if (src.yenilemeBelirteci != null)
        {
            yenilemeToken = new Token
            {
                TokenValue = src.yenilemeBelirteci,
                TokenType = "Refresh Token",
                ExpireTime = src.yenilemeBelirteciGecerlilikSuresi,
                ConsentId = src.ConsentId
            };
        }

        return (erisimToken, yenilemeToken);
    }).ReverseMap();
       CreateMap<Token, TokenModel>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.erisimBelirteci, opt => opt.MapFrom(src => src.TokenValue))
        .ForMember(dest => dest.gecerlilikSuresi, opt => opt.MapFrom(src => src.ExpireTime))
        .ForMember(dest => dest.yenilemeBelirteci, opt => opt.MapFrom(src => src.TokenValue))
        .ForMember(dest => dest.yenilemeBelirteciGecerlilikSuresi, opt => opt.MapFrom(src => src.ExpireTime));
        }
    }
}
