using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.Authorization;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.Event;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.DTO.OpenBanking.YOS;
using amorphie.consent.core.DTO.Tag;
using amorphie.consent.core.Model;
using AutoMapper;
using Newtonsoft.Json;

namespace amorphie.consent.Mapper;

public sealed class ResourceMapper : Profile
{
    public ResourceMapper()
    {
        CreateMap<Consent, ConsentDto>().ReverseMap();
        CreateMap<Consent, HesapBilgisiRizaIstegiDto>().ReverseMap();
        CreateMap<Consent, OdemeEmriRizaIstegiDto>().ReverseMap();
        CreateMap<Consent, OpenBankingConsentDto>()
            .ReverseMap();
        CreateMap<Consent, HHSAccountConsentDto>()
            .ForMember(dest => dest.AdditionalData,
                opt => opt.MapFrom(src => JsonConvert.DeserializeObject<HesapBilgisiRizasiHHSDto>(src.AdditionalData)))
            .ForMember(dest => dest.UserTCKN, opt => opt.MapFrom(src => src.UserTCKN))
            .ForMember(dest => dest.CustomerNumber, opt => opt.MapFrom<CustomerNumberResolver>())
            .ForMember(dest => dest.InstitutionCustomerNumber, opt => opt.MapFrom<InstitutionCustomerNumberResolver>());
        CreateMap<Consent, HHSPaymentConsentDto>()
            .ForMember(dest => dest.AdditionalData,
                opt => opt.MapFrom(src =>
                    JsonConvert.DeserializeObject<OdemeEmriRizasiWithMsrfTtrHHSDto>(src.AdditionalData)))
            .ForMember(dest => dest.UserTCKN, opt => opt.MapFrom(src => src.UserTCKN))
            .ForMember(dest => dest.CustomerNumber, opt => opt.MapFrom<CustomerNumberResolverPayment>())
            .ForMember(dest => dest.InstitutionCustomerNumber,
                opt => opt.MapFrom<InstitutionCustomerNumberResolverPayment>());
        CreateMap<OBAccountConsentDetail, OBAccountConsentDetailDto>();
        CreateMap<Token, TokenDto>().ReverseMap();
        CreateMap<Consent, YOSConsentDto>().ReverseMap();
        // CreateMap<Token, TokenModel>().ReverseMap();
        CreateMap<Consent, YOSConsentDto>().ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Tokens))
            .ReverseMap();
        CreateMap<SaveConsentForLoginDto, SaveConsentDto>()
            .ForMember(dest => dest.ConsentType, opt => opt.Ignore());
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
        CreateMap<OdemeEmriRizasiWithMsrfTtrHHSDto, OdemeEmriRizasiHHSDto>();
        CreateMap<OdemeBaslatmaWithMsrfTtrDto, OdemeBaslatmaDto>();
        CreateMap<GkdRequestDto, GkdDto>();
        CreateMap<OdemeBaslatmaRequestDto, OdemeBaslatmaDto>();
        CreateMap<AliciHesapRequestDto, AliciHesapDto>();
        CreateMap<OdemeAyrintilariRequestDto, OdemeAyrintilariDto>();
        CreateMap<AbonelikTipleriDto, OBEventSubscriptionType>()
            .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.olayTipi))
            .ForMember(dest => dest.SourceType, opt => opt.MapFrom(src => src.kaynakTipi))
            .ReverseMap();
        CreateMap<ListIslemBilgileriDto, IslemBilgileriDto>();
        CreateMap<OBEventSubscription, OlayAbonelikDto>()
            .ForPath(dest => dest.katilimciBlg.hhsKod, opt => opt.MapFrom(src => src.HHSCode))
            .ForPath(dest => dest.katilimciBlg.yosKod, opt => opt.MapFrom(src => src.YOSCode))
            .ForMember(dest => dest.olayAbonelikNo, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.olusturmaZamani, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.guncellemeZamani, opt => opt.MapFrom(src => src.ModifiedAt))
            .ForMember(dest => dest.abonelikTipleri, opt => opt.MapFrom(src => src.OBEventSubscriptionTypes));
        CreateMap<OBEvent, OlaylarDto>()
            .ForMember(dest => dest.kaynakTipi, opt => opt.MapFrom(src => src.SourceType))
            .ForMember(dest => dest.kaynakNo, opt => opt.MapFrom(src => src.SourceNumber))
            .ForMember(dest => dest.olayTipi, opt => opt.MapFrom(src => src.EventType))
            .ForMember(dest => dest.olayNo, opt => opt.MapFrom(src => src.EventNumber))
            .ForMember(dest => dest.olayZamani, opt => opt.MapFrom(src => src.EventDate));

        CreateMap<OBEvent, OlayIstegiDto>()
            .ForPath(dest => dest.katilimciBlg.hhsKod, opt => opt.MapFrom(src => src.HHSCode))
            .ForPath(dest => dest.katilimciBlg.yosKod, opt => opt.MapFrom(src => src.YOSCode))
            .ForMember(dest => dest.olaylar, opt => opt.MapFrom<OlaylarResolver>());

        CreateMap<OBYosInfo, OBYosInfoDto>().ReverseMap();
        CreateMap<OBYosInfoDto, OBYosInfo>()
            .ForMember(dest => dest.Adresler, opt => opt.MapFrom(src => src.adresler))
            .ForMember(dest => dest.LogoBilgileri, opt => opt.MapFrom(src => src.logoBilgileri)).ReverseMap();

        CreateMap<OBHhsInfo, OBHhsInfoDto>()
            .ForMember(dest => dest.apiBilgileri,
                opt => opt.ConvertUsing(new JsonToListTypeConverter<HhsApiBilgiDto>(), src => src.ApiBilgileri))
            .ForMember(dest => dest.logoBilgileri,
                opt => opt.ConvertUsing(new JsonToListTypeConverter<LogoBilgisiDto>(), src => src.LogoBilgileri))
            .ReverseMap();
        CreateMap<OBYosInfo, OBYosInfoDto>()
            .ForMember(dest => dest.adresler,
                opt => opt.ConvertUsing(new JsonToListTypeConverter<AdresDto>(), src => src.Adresler))
            .ForMember(dest => dest.logoBilgileri,
                opt => opt.ConvertUsing(new JsonToListTypeConverter<LogoBilgisiDto>(), src => src.LogoBilgileri))
            .ForMember(dest => dest.apiBilgileri,
                opt => opt.ConvertUsing(new JsonToListTypeConverter<YosApiBilgiDto>(), src => src.ApiBilgileri))
            .ReverseMap();
        CreateMap<ApiResult, List<OBHhsInfoDto>>()
            .ConvertUsing(src => src.Data as List<OBHhsInfoDto>);
        CreateMap<ApiResult, List<OBYosInfoDto>>()
            .ConvertUsing(src => src.Data as List<OBYosInfoDto>);
        CreateMap<ApiResult, OBYosInfoDto>()
            .ConvertUsing(src => src.Data as OBYosInfoDto);
        CreateMap<ApiResult, OBHhsInfoDto>()
            .ConvertUsing(src => src.Data as OBHhsInfoDto);
        CreateMap<ApiResult, PhoneNumberDto>()
            .ConvertUsing(src => src.Data as PhoneNumberDto);
    }
}

public class JsonToListTypeConverter<T> : IValueConverter<string, List<T>>
{
    public List<T> Convert(string source, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source))
        {
            return new List<T>();
        }

        return System.Text.Json.JsonSerializer.Deserialize<List<T>>(source) ?? new List<T>();
    }
}