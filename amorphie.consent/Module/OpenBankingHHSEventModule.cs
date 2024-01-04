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
using amorphie.consent.core.DTO.OpenBanking.Event;
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


    protected async Task<IResult> EventSubsrciptionPost([FromBody] OlayAbonelikIstegiDto olayAbonelikIstegi,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            //Check if post data is valid to process.
            var checkValidationResult =
                await IsDataValidToEventSubsrciptionPost(olayAbonelikIstegi,context, configuration, yosInfoService,
                    httpContext);
            if (!checkValidationResult.Result)
            {//Data not valid
                return Results.BadRequest(checkValidationResult.Message);
            }
            
            //Generate entity object
            var eventSubscriptionEntity = new OBEventSubscription()
            {
                IsActive = true,
                YOSCode = olayAbonelikIstegi.katilimciBlg.yosKod,
                HHSCode = olayAbonelikIstegi.katilimciBlg.hhsKod,
                CreatedAt = DateTime.UtcNow,
                SubsriptionTypes = ConvertSubscriptionTypes(olayAbonelikIstegi.abonelikTipleri),
                SubscriptionNumber = Guid.NewGuid()
            };
            //Generate response object
            OlayAbonelikDto olayAbonelik = new OlayAbonelikDto()
            {
                olayAbonelikNo = eventSubscriptionEntity.SubscriptionNumber.ToString(),
                abonelikTipleri = olayAbonelikIstegi.abonelikTipleri,
                katilimciBlg = olayAbonelikIstegi.katilimciBlg,
                olusturmaZamani = DateTime.UtcNow,
                guncellemeZamani = DateTime.UtcNow
            };
          
            context.OBEventSubscriptions.Add(eventSubscriptionEntity);
            await context.SaveChangesAsync();
            return Results.Ok(olayAbonelik);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    private string[][] ConvertSubscriptionTypes(List<AbonelikTipleriDto> abonelikTipleri)
    {
        var subscriptionTypes = abonelikTipleri.Select(i  =>new string[] { i.olayTipi, i.kaynakTipi }).ToArray();
        return subscriptionTypes;
    }


    /// <summary>
    /// Checks if data is valid for account consent post process
    /// </summary>
    /// <param name="olayAbonelikIstegi">To be checked data</param>
    /// <param name="configuration">Config object</param>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <param name="httpContext">Context object to get header parameters</param>
    /// <exception cref="NotImplementedException"></exception>
    private async Task<ApiResult> IsDataValidToEventSubsrciptionPost(OlayAbonelikIstegiDto olayAbonelikIstegi,
        ConsentDbContext context,
        IConfiguration configuration,
        IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        ApiResult result = new();

        //Check message required basic properties
        if (olayAbonelikIstegi.katilimciBlg is null
            || olayAbonelikIstegi.abonelikTipleri?.Any() is null or false
           )
        {
            result.Result = false;
            result.Message =
                "katilimciBlg, abonelikTipleri should be in event subscription request message";
            return result;
        }

        //Check KatılımcıBilgisi
        if (string.IsNullOrEmpty(olayAbonelikIstegi.katilimciBlg.hhsKod) //Required fields
            || string.IsNullOrEmpty(olayAbonelikIstegi.katilimciBlg.yosKod)
            || configuration["HHSCode"] != olayAbonelikIstegi.katilimciBlg.hhsKod)
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. HHSKod YOSKod required";
            return result;
        }

        //Check header fields
        result = await IsHeaderDataValid(httpContext, configuration, yosInfoService, olayAbonelikIstegi.katilimciBlg);
        if (!result.Result)
        {
            //validation error in header fields
            return result;
        }

        //Check aboneliktipleri data validation
        var sourceTypes = ConstantHelper.GetKaynakTipList();
        var eventTypes = ConstantHelper.GetOlayTipList();
        if (olayAbonelikIstegi.abonelikTipleri.Any(a => !eventTypes.Contains(a.olayTipi))
            || olayAbonelikIstegi.abonelikTipleri.Any(a => !sourceTypes.Contains(a.kaynakTipi)))
        {
            result.Result = false;
            result.Message =
                "TR.OHVPS.Resource.InvalidFormat. TR.OHVPS.DataCode.OlayTip and/or TR.OHVPS.DataCode.KaynakTip wrong.";
            return result;
        }
        //TODO:Özlem check this
        //Olay Abonelik kaydı oluşturmak isteyen YÖS'ün ODS API tanımı HHS tarafından kontrol edilmelidir. YÖS'ün tanımı olmaması halinde "HTTP 400-TR.OHVPS.Business.InvalidContent" hatası verilmelidir.

        //1 YÖS'ün 1 HHS'de 1 adet abonelik kaydı olabilir.
        if (await context.OBEventSubscriptions.AnyAsync(s =>
                s.YOSCode == olayAbonelikIstegi.katilimciBlg.yosKod && s.IsActive))
        {
            result.Result = false;
            result.Message =
                "HTTP 400 -TR.OHVPS.Business.InvalidContent -Kaynak Çakışması";
            return result;
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
    /// <param name="katilimciBlg">Katilimci data object</param>
    /// <returns>Validation result</returns>
    private async Task<ApiResult> IsHeaderDataValid(HttpContext context,
        IConfiguration configuration,
        IYosInfoService yosInfoService,
        KatilimciBilgisiDto katilimciBlg)
    {
        ApiResult result = new();
        var header = ModuleHelper.GetHeader(context);//Get header object
        
        if (!await ModuleHelper.IsHeaderValidForEvents(header, configuration, yosInfoService))
        {//Header is not valid
            result.Result = false;
            result.Message = "There is a problem in header required values. Some key(s) can be missing or wrong.";
            return result;
        }

        //Check header data and message data
        if (header.XASPSPCode != katilimciBlg.hhsKod)
        {
            //HHSCode must match with header x-aspsp-code
            result.Result = false;
            result.Message = "TR.OHVPS.Connection.InvalidASPSP. HHSKod must match with header x-aspsp-code";
            return result;
        }

        if (header.XTPPCode != katilimciBlg.yosKod)
        {
            //YOSCode must match with header x-tpp-code
            result.Result = false;
            result.Message = "TR.OHVPS.Connection.InvalidTPP. YosKod must match with header x-tpp-code";
            return result;
        }

        return result;
    }
}