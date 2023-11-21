using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.DTO.OpenBanking.YOS;
using amorphie.consent.core.Model;
using AutoMapper;
using Newtonsoft.Json;

namespace amorphie.consent.Mapper
{
    public sealed class ResourceMapper : Profile
    {
        public ResourceMapper()
        {
            CreateMap<YosInfo, YosInfoDto>().ReverseMap();
            CreateMap<Consent, ConsentDto>().ReverseMap();
            CreateMap<Consent, HesapBilgisiRizaIstegiDto>().ReverseMap();
            CreateMap<Consent, OdemeEmriRizaIstegiDto>().ReverseMap();
            CreateMap<Consent, OpenBankingConsentDto>()
                .ReverseMap();
            CreateMap<Consent, HHSAccountConsentDto>().ForMember(dest => dest.AdditionalData,
                opt => opt.MapFrom(src => JsonConvert.DeserializeObject<HesapBilgisiRizasiHHSDto>(src.AdditionalData)));
            CreateMap<Consent, HHSPaymentConsentDto>().ForMember(dest => dest.AdditionalData,
                opt => opt.MapFrom(src => JsonConvert.DeserializeObject<OdemeEmriRizasiHHSDto>(src.AdditionalData)));
            CreateMap<Token, TokenDto>().ReverseMap();
            CreateMap<Consent, YOSConsentDto>().ReverseMap();
            // CreateMap<Token, TokenModel>().ReverseMap();
            CreateMap<Consent, YOSConsentDto>().ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token)).ReverseMap();
            CreateMap<OpenBankingTokenDto, (Token erisimToken, Token yenilemeToken)>()
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

            CreateMap<OpenBankingTokenDto, (Token erisimToken, Token yenilemeToken)>()
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
            CreateMap<Token, OpenBankingTokenDto>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.erisimBelirteci, opt => opt.MapFrom(src => src.TokenValue))
             .ForMember(dest => dest.gecerlilikSuresi, opt => opt.MapFrom(src => src.ExpireTime))
             .ForMember(dest => dest.yenilemeBelirteci, opt => opt.MapFrom(src => src.TokenValue))
             .ForMember(dest => dest.yenilemeBelirteciGecerlilikSuresi, opt => opt.MapFrom(src => src.ExpireTime));

            CreateMap<HesapBilgisiRizaIstegiHHSDto, HesapBilgisiRizasiHHSDto>();
            CreateMap<GkdRequestDto, GkdDto>();
            CreateMap<IzinBilgisiRequestDto, IzinBilgisiDto>();
            CreateMap<HesapBilgisiRequestDto, HesapBilgisiDto>();

            CreateMap<OdemeEmriRizaIstegiHHSDto, OdemeEmriRizasiHHSDto>();
            CreateMap<GkdRequestDto, GkdDto>();
            CreateMap<OdemeBaslatmaRequestDto, OdemeBaslatmaDto>();
            CreateMap<AliciHesapRequestDto, AliciHesapDto>();
            CreateMap<OdemeAyrintilariRequestDto, OdemeAyrintilariDto>();

            CreateMap<OBAccountReference, OBAccountReferenceDto>();
        }
    }
}
