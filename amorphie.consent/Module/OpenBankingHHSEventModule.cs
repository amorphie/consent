using amorphie.core.Module.minimal_api;
using Microsoft.AspNetCore.Mvc;
using amorphie.consent.core.Search;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using amorphie.core.Swagger;
using Microsoft.OpenApi.Models;
using amorphie.consent.data;
using amorphie.consent.core.Model;
using System.Text.Json;
using System.Text.Json.Serialization;
using amorphie.consent.core.DTO;
using amorphie.core.Base;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.Helper;
using amorphie.consent.Service;
using amorphie.consent.Service.Interface;
using Microsoft.AspNetCore.Http.HttpResults;

namespace amorphie.consent.Module;

public class OpenBankingHHSEventModule : BaseBBTRoute<OpenBankingConsentDto, Consent, ConsentDbContext>
{
    public OpenBankingHHSEventModule(WebApplication app)
        : base(app)
    {
    }

    public override string[]? PropertyCheckList => new string[] { "ConsentType", "State" };

    public override string? UrlFragment => "OpenBankingEvent";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapPost("/olay-abonelik", EventSubsrciptionPost);
    }
    
    
    protected async Task<IResult> EventSubsrciptionPost([FromBody] HesapBilgisiRizaIstegiHHSDto rizaIstegi,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            //Check if post data is valid to process.
            var checkValidationResult = await IsDataValidToEventSubsrciptionPost(rizaIstegi, configuration, yosInfoService, httpContext);
            if (!checkValidationResult.Result)
            {//Data not valid
                return Results.BadRequest(checkValidationResult.Message);
            }
          

            var consentEntity = new Consent();
            context.Consents.Add(consentEntity);
            //Generate response object
            HesapBilgisiRizasiHHSDto hesapBilgisiRizasi = mapper.Map<HesapBilgisiRizasiHHSDto>(rizaIstegi);
            //Set consent data
            hesapBilgisiRizasi.rzBlg = new RizaBilgileriDto()
            {
                rizaNo = consentEntity.Id.ToString(),
                olusZmn = DateTime.UtcNow,
                gnclZmn = DateTime.UtcNow,
                rizaDrm = OpenBankingConstants.RizaDurumu.YetkiBekleniyor
            };
            //Set gkd data
            hesapBilgisiRizasi.gkd.hhsYonAdr = string.Format(configuration["HHSForwardingAddress"] ?? string.Empty, consentEntity.Id.ToString());
            hesapBilgisiRizasi.gkd.yetTmmZmn = DateTime.UtcNow.AddMinutes(5);
            consentEntity.AdditionalData = JsonSerializer.Serialize(hesapBilgisiRizasi);
            consentEntity.State = OpenBankingConstants.RizaDurumu.YetkiBekleniyor;
            consentEntity.StateModifiedAt = DateTime.UtcNow;
            consentEntity.ConsentType = ConsentConstants.ConsentType.OpenBankingAccount;
            consentEntity.Variant = hesapBilgisiRizasi.katilimciBlg.yosKod;
            consentEntity.ObConsentIdentityInfos = new List<OBConsentIdentityInfo>
            {
                new()
                {//Get consent identity data to identity entity 
                    IdentityData = hesapBilgisiRizasi.kmlk.kmlkVrs,
                    IdentityType = hesapBilgisiRizasi.kmlk.kmlkTur,
                    InstitutionIdentityData = hesapBilgisiRizasi.kmlk.krmKmlkVrs,
                    InstitutionIdentityType = hesapBilgisiRizasi.kmlk.krmKmlkTur,
                    UserType = hesapBilgisiRizasi.kmlk.ohkTur
                }
            };
            context.Consents.Add(consentEntity);
            await context.SaveChangesAsync();
            return Results.Ok(hesapBilgisiRizasi);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    
    /// <summary>
    /// Checks if data is valid for account consent post process
    /// </summary>
    /// <param name="rizaIstegi">To be checked data</param>
    /// <param name="configuration">Config object</param>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <param name="httpContext">Context object to get header parameters</param>
    /// <exception cref="NotImplementedException"></exception>
    private async Task<ApiResult> IsDataValidToEventSubsrciptionPost(HesapBilgisiRizaIstegiHHSDto rizaIstegi,
    IConfiguration configuration,
    IYosInfoService yosInfoService,
    HttpContext httpContext)
    {
        ApiResult result = new(); 
        var header = ModuleHelper.GetHeader(httpContext);
        // //Check header fields
        // result = await IsHeaderDataValid(httpContext, configuration, yosInfoService, header);
        // if (!result.Result)
        // {//validation error in header fields
        //     return result;
        // }
        //Check message required basic properties
        if (rizaIstegi.katilimciBlg is null
            || rizaIstegi.gkd == null
            || rizaIstegi.kmlk == null
            || rizaIstegi.hspBlg == null
            || rizaIstegi.hspBlg.iznBlg == null)
        {
            result.Result = false;
            result.Message =
                "katilimciBlg, gkd,odmBsltm, kmlk, hspBlg, spBlg.iznBlg should be in consent request message";
            return result;
        }

        //Check KatılımcıBilgisi
        if (string.IsNullOrEmpty(rizaIstegi.katilimciBlg.hhsKod)//Required fields
            || string.IsNullOrEmpty(rizaIstegi.katilimciBlg.yosKod)
            || configuration["HHSCode"] != rizaIstegi.katilimciBlg.hhsKod)
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. HHSKod YOSKod required";
            return result;
        }

        if (header.XASPSPCode != rizaIstegi.katilimciBlg.hhsKod)
        {//HHSCode must match with header x-aspsp-code
            result.Result = false;
            result.Message = "TR.OHVPS.Connection.InvalidASPSP. HHSKod must match with header x-aspsp-code";
            return result;
        }
        if (header.XTPPCode != rizaIstegi.katilimciBlg.yosKod)
        {//YOSCode must match with header x-tpp-code
            result.Result = false;
            result.Message = "TR.OHVPS.Connection.InvalidTPP. YosKod must match with header x-tpp-code";
            return result;
        }



        //Check Kimlik
        if (string.IsNullOrEmpty(rizaIstegi.kmlk.kmlkTur)//Check required fields
            || string.IsNullOrEmpty(rizaIstegi.kmlk.kmlkVrs)
            || (string.IsNullOrEmpty(rizaIstegi.kmlk.krmKmlkTur) != string.IsNullOrEmpty(rizaIstegi.kmlk.krmKmlkVrs))
            || string.IsNullOrEmpty(rizaIstegi.kmlk.ohkTur)
            || !ConstantHelper.GetKimlikTurList().Contains(rizaIstegi.kmlk.kmlkTur)
            || !ConstantHelper.GetOHKTurList().Contains(rizaIstegi.kmlk.ohkTur)
            || (!string.IsNullOrEmpty(rizaIstegi.kmlk.krmKmlkTur) && !ConstantHelper.GetKurumKimlikTurList().Contains(rizaIstegi.kmlk.krmKmlkTur)))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. Kmlk data is not valid";
            return result;
        }

        //Check field constraints
        if ((rizaIstegi.kmlk.kmlkTur == OpenBankingConstants.KimlikTur.TCKN && rizaIstegi.kmlk.kmlkVrs.Trim().Length != 11)
            || (rizaIstegi.kmlk.kmlkTur == OpenBankingConstants.KimlikTur.MNO && rizaIstegi.kmlk.kmlkVrs.Trim().Length > 30)
             || (rizaIstegi.kmlk.kmlkTur == OpenBankingConstants.KimlikTur.YKN && rizaIstegi.kmlk.kmlkVrs.Trim().Length != 11)
            || (rizaIstegi.kmlk.kmlkTur == OpenBankingConstants.KimlikTur.PNO && (rizaIstegi.kmlk.kmlkVrs.Trim().Length < 7 || rizaIstegi.kmlk.kmlkVrs.Length > 9))
            || (rizaIstegi.kmlk.krmKmlkTur == OpenBankingConstants.KurumKimlikTur.TCKN && rizaIstegi.kmlk.kmlkVrs.Trim().Length != 11)
            || (rizaIstegi.kmlk.krmKmlkTur == OpenBankingConstants.KurumKimlikTur.MNO && rizaIstegi.kmlk.kmlkVrs.Trim().Length > 30)
            || (rizaIstegi.kmlk.krmKmlkTur == OpenBankingConstants.KurumKimlikTur.VKN && rizaIstegi.kmlk.kmlkVrs.Trim().Length != 10))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. Kmlk data validation failed.";
            return result;
        }


        //Check HesapBilgisi
        //Check izinbilgisi properties
        if ((rizaIstegi.hspBlg.iznBlg.iznTur?.Any() ?? false) == false
            || rizaIstegi.hspBlg.iznBlg.iznTur.Any(i => !ConstantHelper.GetIzinTurList().Contains(i))
            || rizaIstegi.hspBlg.iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.TemelHesapBilgisi) == false
            || (rizaIstegi.hspBlg.iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.AyrintiliIslemBilgisi) && !rizaIstegi.hspBlg.iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.TemelIslemBilgisi))
            || (rizaIstegi.hspBlg.iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.AnlikBakiyeBildirimi) && !rizaIstegi.hspBlg.iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.BakiyeBilgisi)))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. IznBld iznTur check failed. IznTur required and should contain TemelHesapBilgisi permission.";
            return result;
        }

        if (rizaIstegi.hspBlg.iznBlg.erisimIzniSonTrh == System.DateTime.MinValue
          || rizaIstegi.hspBlg.iznBlg.erisimIzniSonTrh > System.DateTime.UtcNow.AddMonths(6)
          || rizaIstegi.hspBlg.iznBlg.erisimIzniSonTrh < System.DateTime.UtcNow.AddDays(1))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. IznBld erisimIzniSonTrh data check failed. It should be between tomorrow and 6 months later ";
            return result;
        }

        //Check işlem sorgulama başlangıç zamanı
        if (rizaIstegi.hspBlg.iznBlg.hesapIslemBslZmn.HasValue)
        {
            //Temel işlem bilgisi ve/veya ayrıntılı işlem bilgisi seçilmiş olması gerekir
            if (rizaIstegi.hspBlg.iznBlg?.iznTur?.Any(p => p == OpenBankingConstants.IzinTur.TemelIslemBilgisi
                                                        || p == OpenBankingConstants.IzinTur.AyrintiliIslemBilgisi) == false)
            {
                result.Result = false;
                result.Message = "TR.OHVPS.Resource.InvalidFormat. IznTur temelislem or ayrintiliIslem should be selected.";
                return result;
            }
            if (rizaIstegi.hspBlg.iznBlg?.hesapIslemBslZmn.Value < DateTime.UtcNow.AddMonths(-12))//Data constraints
            {
                result.Result = false;
                result.Message = "TR.OHVPS.Resource.InvalidFormat. hesapIslemBslZmn not valid. Maximum 12 months before.";
                return result;
            }
        }
        if (rizaIstegi.hspBlg.iznBlg.hesapIslemBtsZmn.HasValue)//Check işlem sorgulama bitiş zamanı
        {
            //Temel işlem bilgisi ve/veya ayrıntılı işlem bilgisi seçilmiş olması gerekir
            if (rizaIstegi.hspBlg.iznBlg?.iznTur?.Any(p => p == OpenBankingConstants.IzinTur.TemelIslemBilgisi
                                                        || p == OpenBankingConstants.IzinTur.AyrintiliIslemBilgisi) == false)
            {
                result.Result = false;
                result.Message = "TR.OHVPS.Resource.InvalidFormat IznTur temelislem or ayrintiliIslem should be selected.";
                return result;
            }
            if (rizaIstegi.hspBlg.iznBlg?.hesapIslemBtsZmn.Value > DateTime.UtcNow.AddMonths(12))//Data constraints
            {
                result.Result = false;
                result.Message = "TR.OHVPS.Resource.InvalidFormat. hesapIslemBtsZmn not valid. Maximum 12 months later.";
                return result;
            }
        }
        return result;
    }

    /// <summary>
    ///  Checks if header is varlid.
    /// Checks required fields.
    /// Checks 
    /// </summary>
    /// <param name="context">Context</param>
    /// <param name="configuration">Configuration instance</param>
    /// <param name="yosInfoService">Yos service instance</param>
    /// <param name="header">Header object</param>
    /// <returns>Validation result</returns>
    private async Task<ApiResult> IsHeaderDataValid(HttpContext context,
        IConfiguration configuration,
        IYosInfoService yosInfoService,
        RequestHeaderDto header = null)
    {
        ApiResult result = new();
        if (header is null)
        {
            header = ModuleHelper.GetHeader(context);
        }
        if (!await ModuleHelper.IsHeaderValid(header, configuration, yosInfoService))
        {
            result.Result = false;
            result.Message = "There is a problem in header required values. Some key(s) can be missing or wrong.";
            return result;
        }

        return result;
    }


}
