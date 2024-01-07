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

public class OpenBankingHHSEventModule : BaseBBTRoute<OlayAbonelikDto, OBEventSubscription, ConsentDbContext>
{
    public OpenBankingHHSEventModule(WebApplication app)
        : base(app)
    {
    }

    public override string[]? PropertyCheckList => new string[] { "HHSCode", "YOSCode" };

    public override string? UrlFragment => "OpenBankingHHSEvent";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapGet("/olay-abonelik", GetEventSubscription);
        routeGroupBuilder.MapPost("/olay-abonelik", EventSubsrciptionPost);
        routeGroupBuilder.MapDelete("/olay-abonelik/{olayAbonelikNo}", DeleteEventSubsrciption);
    }

    /// <summary>
    /// Get YOS's active event subscription record
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="configuration"></param>
    /// <param name="yosInfoService"></param>
    /// <param name="httpContext"></param>
    /// <returns>YOS's active event subscription record</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    public async Task<IResult> GetEventSubscription([FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            var header = ModuleHelper.GetHeader(httpContext);//Get header object
            //Check header fields
            ApiResult headerValidation = await IsHeaderDataValid(httpContext, configuration, yosInfoService, header: header);
            if (!headerValidation.Result)
            {//Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }

            //Get entity from db
            var entity = await context.OBEventSubscriptions
                .Include(s => s.OBEventSubscriptionTypes)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.YOSCode == header.XTPPCode
                                          && s.HHSCode == header.XASPSPCode
                                          && s.ModuleName == OpenBankingConstants.ModuleName.HHS
                                          && s.IsActive);
            var activeSubscription = mapper.Map<OlayAbonelikDto>(entity);
            return Results.Ok(activeSubscription);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    /// <summary>
    /// Do Event Subcscription process of yos.
    /// </summary>
    /// <param name="olayAbonelikIstegi">Request object</param>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="configuration"></param>
    /// <param name="yosInfoService"></param>
    /// <param name="httpContext"></param>
    /// <returns>EventSubscription response</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
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
                await IsDataValidToEventSubsrciptionPost(olayAbonelikIstegi, context, configuration, yosInfoService,
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
                ModuleName = OpenBankingConstants.ModuleName.HHS,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                OBEventSubscriptionTypes = mapper.Map<IList<OBEventSubscriptionType>>(olayAbonelikIstegi.abonelikTipleri)
            };
            context.OBEventSubscriptions.Add(eventSubscriptionEntity);
            //Generate response object
            OlayAbonelikDto olayAbonelik = new OlayAbonelikDto()
            {
                olayAbonelikNo = eventSubscriptionEntity.Id.ToString(),
                abonelikTipleri = olayAbonelikIstegi.abonelikTipleri,
                katilimciBlg = olayAbonelikIstegi.katilimciBlg,
                olusturmaZamani = DateTime.UtcNow,
                guncellemeZamani = DateTime.UtcNow
            };

            await context.SaveChangesAsync();
            return Results.Ok(olayAbonelik);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    protected async Task<IResult> DeleteEventSubsrciption(Guid id,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] ITokenService tokenService,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            var header = ModuleHelper.GetHeader(httpContext);//Get header object
            //Check header fields
            ApiResult headerValidation = await IsHeaderDataValid(httpContext, configuration, yosInfoService, header: header);
            if (!headerValidation.Result)
            {//Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }

            //Get entity from db
            var entity = await context.OBEventSubscriptions
                .FirstOrDefaultAsync(s => s.Id == id
                                          && s.YOSCode == header.XTPPCode
                                          && s.HHSCode == header.XASPSPCode
                                          && s.ModuleName == OpenBankingConstants.ModuleName.HHS
                                          && s.IsActive);
            ApiResult dataValidationResult = IsDataValidToDeleteEventSubsrciption(entity);//Check data validation
            if (!dataValidationResult.Result)
            {//Data not valid
                return Results.BadRequest(dataValidationResult.Message);
            }

            //Update entity
            entity.ModifiedAt = DateTime.UtcNow;
            entity.IsActive = false;
            context.OBEventSubscriptions.Update(entity);
            await context.SaveChangesAsync();
            
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    


    /// <summary>
    /// Checks if data is valid for EventSubsrciption consent post process
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
        result = await IsHeaderDataValid(httpContext, configuration, yosInfoService, katilimciBlg: olayAbonelikIstegi.katilimciBlg);
        if (!result.Result)
        {
            //validation error in header fields
            return result;
        }

        //Check aboneliktipleri data validation
        var eventTypeSourceTypeRelations = await context.OBEventTypeSourceTypeRelations
             .AsNoTracking()
             .Where(r => r.EventNotificationReporter == OpenBankingConstants.EventNotificationReporter.HHS)
             .ToListAsync();


        //Event Type check. Descpriton from document:
        //"Olay Tipleri ve Kaynak Tipleri İlişkisi" tablosunda "Olay Bildirim Yapan" kolonu "HHS" olan olay tipleri ile veri girişine izin verilir. 
        if (olayAbonelikIstegi.abonelikTipleri.Any(a => !eventTypeSourceTypeRelations.Any(r => r.EventType == a.olayTipi && r.SourceType == a.kaynakTipi)))
        {
            result.Result = false;
            result.Message =
                "TR.OHVPS.Resource.InvalidFormat. TR.OHVPS.DataCode.OlayTip and/or TR.OHVPS.DataCode.KaynakTip wrong.";
            return result;
        }

        //TODO:Özlem Mehmet yös tablosunu bitirince burayı güncelle
        //Source Type check.  Descpriton from document:
        //HHS, YÖS API üzerinden YÖS'ün rollerini alarak uygun kaynak tiplerine kayıt olmasını sağlar.



        //TODO:Özlem Mehmet yös tablosunu bitirince burayı güncelle
        //Descpriton from document: Olay Abonelik kaydı oluşturmak isteyen YÖS'ün ODS API tanımı HHS tarafından kontrol edilmelidir. 
        //YÖS'ün tanımı olmaması halinde "HTTP 400-TR.OHVPS.Business.InvalidContent" hatası verilmelidir.

        //Descpriton from document: 1 YÖS'ün 1 HHS'de 1 adet abonelik kaydı olabilir.
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
    ///  Checks if data is valid to delete EventSubsrciption
    /// </summary>
    /// <param name="entity">To be checked entity</param>
    /// <returns>Is data valid to delete EventSubsrciption</returns>
    private ApiResult IsDataValidToDeleteEventSubsrciption(OBEventSubscription entity)
    {
        ApiResult result = new();
        //TODO:Özlem If there is any undeliverable object, or not finished consent state. Will I delete the subscription?
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
    /// <param name="katilimciBlg">Katilimci data object default value with null</param>
    /// <returns>Validation result</returns>
    private async Task<ApiResult> IsHeaderDataValid(HttpContext context,
        IConfiguration configuration,
        IYosInfoService yosInfoService,
        RequestHeaderDto? header = null,
        KatilimciBilgisiDto? katilimciBlg = null)
    {
        ApiResult result = new();
        if (header == null)//Get header
        {
            header = ModuleHelper.GetHeader(context);//Get header object
        }

        if (!await ModuleHelper.IsHeaderValidForEvents(header, configuration, yosInfoService))
        {//Header is not valid
            result.Result = false;
            result.Message = "There is a problem in header required values. Some key(s) can be missing or wrong.";
            return result;
        }

        //If there is katilimciBlg object, validate data in it with header
        if (katilimciBlg != null)
        {
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
        }
        return result;
    }
}