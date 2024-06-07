using amorphie.core.Module.minimal_api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using amorphie.core.Swagger;
using Microsoft.OpenApi.Models;
using amorphie.consent.data;
using amorphie.consent.core.Model;
using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.Event;
using amorphie.consent.core.Enum;
using amorphie.consent.Helper;
using amorphie.consent.Service.Interface;
using System.Net;

namespace amorphie.consent.Module;

public class OpenBankingHHSEventModule : BaseBBTRoute<OlayAbonelikDto, OBEventSubscription, ConsentDbContext>
{
    public OpenBankingHHSEventModule(WebApplication app)
        : base(app)
    {
    }

    public override string[] PropertyCheckList => new[] { "HHSCode", "YOSCode" };

    public override string UrlFragment => "OpenBankingHHSEvent";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapGet("/olay-abonelik", GetEventSubscription);
        routeGroupBuilder.MapGet("/olay-abonelik/{olayAbonelikNo}/iletilemeyen-olaylar",
            GetEventSubscriptionUnDeliveredEvents);
        routeGroupBuilder.MapPost("/olay-abonelik", EventSubsrciptionPost);
        routeGroupBuilder.MapPut("/olay-abonelik/{olayAbonelikNo}", UpdateEventSubsrciption);
        routeGroupBuilder.MapDelete("/olay-abonelik/{olayAbonelikNo}", DeleteEventSubsrciption);
        routeGroupBuilder.MapPost("/olay-dinleme/{eventType}/{sourceType}/{eventId}", DoEventSchedulerProcess);
        routeGroupBuilder.MapPost("/sistem-olay-dinleme", SystemEventPost);
    }

    #region EventSubscription

    /// <summary>
    /// Get YOS's active event subscription record
    /// </summary>
    /// <returns>YOS's active event subscription record</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    public async Task<IResult> GetEventSubscription([FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        [FromServices] IOBErrorCodeDetailService errorCodeDetailService,
        HttpContext httpContext)
    {
        try
        {
            List<OBErrorCodeDetail> errorCodeDetails =  await errorCodeDetailService.GetErrorCodeDetailsAsync();
            var header = OBModuleHelper.GetHeader(httpContext); //Get header object
            //Check header fields
            ApiResult headerValidation =
                await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, header,
                    errorCodeDetails: errorCodeDetails,  isEventHeader:true);
            if (!headerValidation.Result)
            {
                //Missing header fields
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, headerValidation.Data);
                return Results.Content(headerValidation.Data.ToJsonString(),"application/json", statusCode: HttpStatusCode.BadRequest.GetHashCode());
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
            OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, activeSubscription);
            return Results.Content(activeSubscription.ToJsonString(),"application/json" ,statusCode: HttpStatusCode.OK.GetHashCode());
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets UnDelivered Events of Given Event Subscription
    /// Max 100 records are listed
    /// It gives maximum 1 day left records
    /// </summary>
    /// <param name="olayAbonelikNo">Event subscription number</param>
    /// <param name="syfNo">Page number</param>
    /// <param name="olyZmnBslTrh">Event query start date time. Optional parameter. If not set. 1 day past is set.</param>
    /// <param name="olyZmnBtsTrh">Event query end date time. Optional parameter. If not set. Query time - now - is set.</param>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="configuration"></param>
    /// <param name="yosInfoService"></param>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    public async Task<IResult> GetEventSubscriptionUnDeliveredEvents(string olayAbonelikNo,
        int? syfNo,
        DateTime? olyZmnBslTrh,
        DateTime? olyZmnBtsTrh,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        [FromServices] IOBErrorCodeDetailService errorCodeDetailService,
        HttpContext httpContext)
    {
        try
        {
            var header = OBModuleHelper.GetHeader(httpContext); //Get header object
            List<OBErrorCodeDetail> errorCodeDetails =  await errorCodeDetailService.GetErrorCodeDetailsAsync();
            //Check header fields
            ApiResult headerValidation =
                await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, header,
                    errorCodeDetails: errorCodeDetails,  isEventHeader:true);
            if (!headerValidation.Result)
            {
                //Missing header fields
                return Results.BadRequest(headerValidation.Data);
            }

            if (string.IsNullOrEmpty(olayAbonelikNo))
            {
                return Results.BadRequest("olayAbonelikNo is not valid.");
            }

            //Get eventsubscription from db
            var subscription = await context.OBEventSubscriptions
                .Include(s => s.OBEventSubscriptionTypes)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.YOSCode == header.XTPPCode
                                          && s.HHSCode == header.XASPSPCode
                                          && s.ModuleName == OpenBankingConstants.ModuleName.HHS
                                          && s.IsActive
                                          && s.Id.ToString() == olayAbonelikNo);
            if (subscription == null) //There is no subscription in system
            {
                return Results.BadRequest("There is no event subscription in the system with providing olayAbonelikNo");
            }

            DateTime queryTime = DateTime.UtcNow;
            DateTime minStartTime = queryTime.AddDays(-1); //Min 1 day left events will be given
            //Start date
            DateTime eventQueryStartDate =
                olyZmnBslTrh.HasValue && olyZmnBslTrh >= minStartTime && olyZmnBslTrh < queryTime
                    ? olyZmnBslTrh.Value
                    : minStartTime;
            //End Date
            DateTime eventQueryEndDate =
                olyZmnBtsTrh.HasValue && olyZmnBtsTrh <= queryTime && olyZmnBtsTrh > minStartTime
                    ? olyZmnBtsTrh.Value
                    : queryTime;
            int skipCount = syfNo.HasValue && syfNo.Value > 1 ? ((syfNo.Value - 1) * 100) : 0;
            var eventItems = (await context.OBEvents.Where(e =>
                        e.DeliveryStatus == OpenBankingConstants.RecordDeliveryStatus.Undeliverable
                        && e.HHSCode == subscription.HHSCode
                        && e.YOSCode == subscription.YOSCode
                        && e.ModuleName == OpenBankingConstants.ModuleName.HHS
                        && e.EventDate >= eventQueryStartDate
                        && e.EventDate <= eventQueryEndDate)
                    .ToListAsync())
                .Where(e =>
                    subscription.OBEventSubscriptionTypes.Any(st =>
                        st.EventType == e.EventType && st.SourceType == e.SourceType)
                )
                .OrderBy(e => e.EventDate)
                .Skip(skipCount)
                .Take(100)
                .ToList();

            OlayIstegiDto responseObject = new OlayIstegiDto()
            {
                katilimciBlg = new KatilimciBilgisiDto()
                {
                    hhsKod = subscription.HHSCode,
                    yosKod = subscription.YOSCode
                },
                olaylar = mapper.Map<List<OlaylarDto>>(eventItems)
            };
            return Results.Ok(responseObject);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    /// <summary>
    /// Do Event Subcscription process of yos.
    /// </summary>
    /// <returns>EventSubscription response</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    protected async Task<IResult> EventSubsrciptionPost([FromBody] OlayAbonelikIstegiDto olayAbonelikIstegi,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        [FromServices] IOBErrorCodeDetailService errorCodeDetailService,
        HttpContext httpContext)
    {
        try
        {
            var header = OBModuleHelper.GetHeader(httpContext); //Get header object
            List<OBErrorCodeDetail> errorCodeDetails =  await errorCodeDetailService.GetErrorCodeDetailsAsync();
            //Check if post data is valid to process.
            var checkValidationResult =
                await IsDataValidToEventSubsrciptionPost(olayAbonelikIstegi,header, errorCodeDetails:errorCodeDetails, context, configuration, yosInfoService,
                    httpContext);
            if (!checkValidationResult.Result)
            {
                //Data not valid
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, checkValidationResult.Data);
                return Results.Content(checkValidationResult.Data.ToJsonString(),"application/json", statusCode: HttpStatusCode.BadRequest.GetHashCode());
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
                XRequestId = header.XRequestID,
                OBEventSubscriptionTypes =
                    mapper.Map<IList<OBEventSubscriptionType>>(olayAbonelikIstegi.abonelikTipleri)
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
            OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, olayAbonelik);
            httpContext.Response.ContentType = "application/json";
            return Results.Content(olayAbonelik.ToJsonString(), "application/json", statusCode: HttpStatusCode.Created.GetHashCode());
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates current event subcsription by eventSubscriptionNumber
    /// </summary>
    /// <returns>Updated Event Subscription Object</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    protected async Task<IResult> UpdateEventSubsrciption([FromBody] OlayAbonelikDto olayAbonelik,
        string olayAbonelikNo,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        [FromServices] IOBErrorCodeDetailService errorCodeDetailService,
        HttpContext httpContext)
    {
        try
        {
            var header = OBModuleHelper.GetHeader(httpContext); //Get header object
            List<OBErrorCodeDetail> errorCodeDetails =  await errorCodeDetailService.GetErrorCodeDetailsAsync();
            //Check if post data is valid to process.
            var checkValidationResult =
                await IsDataValidToUpdateEventSubsrciption(olayAbonelik, olayAbonelikNo, errorCodeDetails:errorCodeDetails, header, context, configuration,
                    yosInfoService,
                    httpContext);
            if (!checkValidationResult.Result)
            {
                //Data not valid
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, checkValidationResult.Data);
                return Results.Content(checkValidationResult.Data.ToJsonString(),"application/json", statusCode: HttpStatusCode.BadRequest.GetHashCode());
            }
            
            //Get entity from db
            var entity = await context.OBEventSubscriptions
                .Include(s => s.OBEventSubscriptionTypes)
                .FirstOrDefaultAsync(s => s.Id.ToString() == olayAbonelikNo
                                          && s.YOSCode == header.XTPPCode
                                          && s.HHSCode == header.XASPSPCode
                                          && s.ModuleName == OpenBankingConstants.ModuleName.HHS
                                          && s.IsActive);
            
            if (entity is null) //No event subscription in system to update
            {
                var response = OBErrorResponseHelper.GetNotFoundError(httpContext, errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.NotFoundAbonelikNo); 
                
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, response);
                return Results.Content(response.ToJsonString(), "application/json",
                    statusCode: HttpStatusCode.NotFound.GetHashCode());
            }

            //Delete OBEventSubscriptionTypes
            context.OBEventSubscriptionTypes.RemoveRange(entity.OBEventSubscriptionTypes);
            //Insert new 
            var obEventSubscriptionTypes = mapper.Map<IList<OBEventSubscriptionType>>(olayAbonelik.abonelikTipleri);
            obEventSubscriptionTypes = obEventSubscriptionTypes.Select(st =>
            {
                st.OBEventSubscriptionId = entity.Id;
                return st;
            }).ToList();
            context.OBEventSubscriptionTypes.AddRange(obEventSubscriptionTypes);
            //update entity object
            entity.ModifiedAt = DateTime.UtcNow;
            context.OBEventSubscriptions.Update(entity);
            await context.SaveChangesAsync();
            var responseObject = mapper.Map<OlayAbonelikDto>(entity);
            httpContext.Response.ContentType = "application/json";
            return Results.Content(responseObject.ToJsonString(), "application/json", statusCode: 200);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    /// <summary>
    /// Deletes given event subscription
    /// </summary>
    /// <param name="id">To be deleted event subscription record id</param>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="configuration"></param>
    /// <param name="yosInfoService"></param>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    protected async Task<IResult> DeleteEventSubsrciption(Guid id,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        [FromServices] IOBErrorCodeDetailService errorCodeDetailService,
        HttpContext httpContext)
    {
        try
        {
            List<OBErrorCodeDetail> errorCodeDetails =  await errorCodeDetailService.GetErrorCodeDetailsAsync();
            var header = OBModuleHelper.GetHeader(httpContext); //Get header object
            //Check header fields
            ApiResult headerValidation =
                await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, header,
                    errorCodeDetails: errorCodeDetails, isEventHeader:true);
            if (!headerValidation.Result)
            {
                //Missing header fields
                return Results.BadRequest(headerValidation.Data);
            }

            //Get entity from db
            var entity = await context.OBEventSubscriptions
                .FirstOrDefaultAsync(s => s.Id == id
                                          && s.YOSCode == header.XTPPCode
                                          && s.HHSCode == header.XASPSPCode
                                          && s.ModuleName == OpenBankingConstants.ModuleName.HHS
                                          && s.IsActive);
            if (entity == null)
            {
                return Results.NotFound();
            }

            ApiResult dataValidationResult = IsDataValidToDeleteEventSubsrciption(entity); //Check data validation
            if (!dataValidationResult.Result)
            {
                //Data not valid
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

    #endregion


    /// <summary>
    /// BKM calls this method post sistem-olay-dinleme process.
    /// Creates system event record.
    /// </summary>
    /// <returns>Does successfully get the system event message</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    protected async Task<IResult> SystemEventPost([FromBody] OlayIstegiDto olayIstegi,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        [FromServices] IOBEventService eventService,
        [FromServices] ILogger<OpenBankingHHSEventModule> logger,
        [FromServices] IOBErrorCodeDetailService errorCodeDetailService,
        HttpContext httpContext)
    {
        try
        {
             List<OBErrorCodeDetail> errorCodeDetails =  await errorCodeDetailService.GetErrorCodeDetailsAsync();
            var header = OBModuleHelper.GetHeader(httpContext); //Get header object
            //Check if post data is valid to process.
            var checkValidationResult =
                await IsDataValidToSystemEventPost(olayIstegi, header,errorCodeDetails, context, configuration, yosInfoService,
                    httpContext);
            if (!checkValidationResult.Result)
            {
                //Data not valid
                return Results.BadRequest(checkValidationResult.Data);
            }

            //Check if any system record in database with same data
           
            var systemEventEntity = await context.OBSystemEvents.FirstOrDefaultAsync(se =>
                se.YOSCode == olayIstegi.katilimciBlg.yosKod
                && se.ModuleName == OpenBankingConstants.ModuleName.HHS
                && se.EventType == olayIstegi.olaylar[0].olayTipi
                && se.SourceType == olayIstegi.olaylar[0].kaynakTipi
                && se.SourceNumber == olayIstegi.olaylar[0].kaynakNo
                && se.XRequestId == header.XRequestID);
            if (systemEventEntity != null) //Event already in system
            {
                if (!systemEventEntity.IsCompleted) //Already processed
                {
                    //Do system event process
                    await eventService.DoHhsSystemEventProcess(systemEventEntity.Id);
                }

                return Results.Accepted();
            }

            //Generate entity object
            systemEventEntity = new OBSystemEvent()
            {
                YOSCode = olayIstegi.katilimciBlg.yosKod,
                HHSCode = olayIstegi.katilimciBlg.hhsKod,
                ModuleName = OpenBankingConstants.ModuleName.HHS,
                XRequestId = header.XRequestID,
                IsCompleted = false,
                EventDate = DateTime.UtcNow,
                EventType = olayIstegi.olaylar[0].olayTipi,
                EventNumber = olayIstegi.olaylar[0].olayNo,
                SourceType = olayIstegi.olaylar[0].kaynakTipi,
                SourceNumber = olayIstegi.olaylar[0].kaynakNo,
                TryCount = 0,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };
            //save entity
            context.OBSystemEvents.Add(systemEventEntity);
            await context.SaveChangesAsync();

            //Do system event process
            await eventService.DoHhsSystemEventProcess(systemEventEntity.Id);
            return Results.Accepted();
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    protected async Task<EventApiResultDto> DoEventSchedulerProcess(
        string eventType,
        string sourceType,
        Guid eventId,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IOBEventService eventService,
        [FromServices] ILogger<OpenBankingHHSEventModule> logger,
        HttpContext httpContext)
    {
        var eventResult = new EventApiResultDto();
        try
        {
            //Get event from database
            var eventEntity = await context.OBEvents.FirstOrDefaultAsync(e => e.Id == eventId
                                                                              && e.EventType == eventType
                                                                              && e.SourceType == sourceType
                                                                              && e.ModuleName ==
                                                                              OpenBankingConstants.ModuleName.HHS
                                                                              && e.DeliveryStatus ==
                                                                              OpenBankingConstants.RecordDeliveryStatus
                                                                                  .Processing);
            if (eventEntity == null)
            {
                //No desired event in system
                eventResult.ContinueTry = false;
                eventResult.StatusCode = (int)HttpStatusCode.NoContent;
                return eventResult;
            }

            //Process event, if ok, send event to yos
            var sendToYosResult = await eventService.SendEventToYos(eventEntity);
            if (sendToYosResult.Data != null)
            {
                return (EventApiResultDto)sendToYosResult.Data;
            }

            return eventResult;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing event scheduler");
            eventResult.StatusCode = (int)HttpStatusCode.InternalServerError;
            return eventResult;
        }
    }


    protected async Task<bool> DoHhsSystemEventSchedulerProcess(
        Guid systemEventId,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IOBEventService eventService,
        [FromServices] ILogger<OpenBankingHHSEventModule> logger)
    {
        var continueTry = true;
        try
        {
            return await eventService.DoHhsSystemEventProcess(systemEventId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing system event.");
            return continueTry;
        }
    }


    /// <summary>
    /// Checks if data is valid for EventSubsrciption consent post process
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private async Task<ApiResult> IsDataValidToEventSubsrciptionPost(OlayAbonelikIstegiDto olayAbonelikIstegi,
        RequestHeaderDto header,
        List<OBErrorCodeDetail> errorCodeDetails,
        ConsentDbContext context,
        IConfiguration configuration,
        IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        ApiResult result = new();
        //Check header fields
        result = await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, header, isXJwsSignatureRequired: true,
            katilimciBlg: olayAbonelikIstegi.katilimciBlg, errorCodeDetails: errorCodeDetails, isEventHeader:true);
        if (!result.Result)
        {
            //validation error in header fields
            return result;
        }
        string objectName = OBErrorCodeConstants.ObjectNames.OlayAbonelikIstegi;
        
        //Check message required basic properties/objects
        if (!OBConsentValidationHelper.PrepareAndCheckInvalidFormatProperties_OAObject(olayAbonelikIstegi, httpContext, errorCodeDetails, out var errorResponse,objectName))
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }
        
        
        //Check KatılımcıBilgisi
        result = OBConsentValidationHelper.IsKatilimciBlgDataValid(httpContext, configuration,
            katilimciBlg: olayAbonelikIstegi.katilimciBlg, errorCodeDetails: errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            //validation error in katiliciBilgisi data fields
            return result;
        }
        

        //Check aboneliktipleri data validation
        var eventTypeSourceTypeRelations = await context.OBEventTypeSourceTypeRelations
            .AsNoTracking()
            .Where(r => r.EventNotificationReporter == OpenBankingConstants.EventNotificationReporter.HHS)
            .ToListAsync();

        //Check AbonelikTipleriObject
        result = OBConsentValidationHelper.IsAbonelikTipleriObjectDataValid(httpContext, configuration,
            olayAbonelikIstegi.abonelikTipleri, eventTypeSourceTypeRelations, errorCodeDetails: errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            //validation error in abonelikTipleri data fields
            return result;
        }
      

        //Source Type check.  Descpriton from document:
        //HHS, YÖS API üzerinden YÖS'ün rollerini alarak uygun kaynak tiplerine kayıt olmasını sağlar.
        result = await OBConsentValidationHelper.CheckIfYosHasDesiredRole(httpContext, yosInfoService,olayAbonelikIstegi.katilimciBlg.yosKod,
            olayAbonelikIstegi.abonelikTipleri, eventTypeSourceTypeRelations, errorCodeDetails:errorCodeDetails, objectName:objectName);
        if (!result.Result)
        {
            return result;
        }

        //Descpriton from document: Olay Abonelik kaydı oluşturmak isteyen YÖS'ün ODS API tanımı HHS tarafından kontrol edilmelidir. 
        //YÖS'ün tanımı olmaması halinde "HTTP 400-TR.OHVPS.Business.InvalidContent" hatası verilmelidir.
        result = await  OBConsentValidationHelper.CheckIfYosProvidesDesiredApi(httpContext, yosInfoService,olayAbonelikIstegi.katilimciBlg.yosKod,
            OpenBankingConstants.YosApi.OlayDinleme,errorCodeDetails,objectName:objectName);
        if (!result.Result)
        {
            return result;
        }

        //Descpriton from document: 1 YÖS'ün 1 HHS'de 1 adet abonelik kaydı olabilir. HTTP 400 -TR.OHVPS.Business.InvalidContent -Kaynak Çakışması"
        if (await context.OBEventSubscriptions.AnyAsync(s =>
                s.YOSCode == olayAbonelikIstegi.katilimciBlg.yosKod && s.IsActive))
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(httpContext, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidContentThereIsAlreadyEventSubscriotion);
            return result;
        }

        return result;
    }

    /// <summary>
    /// Checks if data is valid for EventSubsrciption consent post process
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private async Task<ApiResult> IsDataValidToUpdateEventSubsrciption(OlayAbonelikDto olayAbonelik,
        string olayAbonelikNoParameter,
        List<OBErrorCodeDetail> errorCodeDetails,
        RequestHeaderDto header,
        ConsentDbContext context,
        IConfiguration configuration,
        IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        ApiResult result = new();
        //Check header fields
        result = await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, header, isXJwsSignatureRequired: true,
            katilimciBlg: olayAbonelik.katilimciBlg, errorCodeDetails: errorCodeDetails, isEventHeader:true);
        if (!result.Result)
        {
            //validation error in header fields
            return result;
        }
        //Get entity from db
        var entity = await context.OBEventSubscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id.ToString() == olayAbonelikNoParameter
                                      && s.ModuleName == OpenBankingConstants.ModuleName.HHS
                                      && s.IsActive);
        if (entity is null) //No event subscription in system to update
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetNotFoundError(httpContext, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.NotFoundAbonelikNo);
            return result;
        }
        
        string objectName = OBErrorCodeConstants.ObjectNames.OlayAbonelikPut;
        
        //Check message required basic properties/objects
        if (!OBConsentValidationHelper.PrepareAndCheckInvalidFormatProperties_OAObject(olayAbonelik, httpContext, errorCodeDetails, out var errorResponse,objectName))
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }
        //Check Olayabonelikno
        result = OBConsentValidationHelper.CheckOlayAbonelikNo(olayAbonelikNoParameter, olayAbonelik.olayAbonelikNo, httpContext, errorCodeDetails: errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            //validation error in katiliciBilgisi data fields
            return result;
        }
        //Check KatılımcıBilgisi
        result = OBConsentValidationHelper.IsKatilimciBlgDataValid(httpContext, configuration,
            katilimciBlg: olayAbonelik.katilimciBlg, errorCodeDetails: errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            //validation error in katiliciBilgisi data fields
            return result;
        }

        //Check aboneliktipleri data validation
        var eventTypeSourceTypeRelations = await context.OBEventTypeSourceTypeRelations
            .AsNoTracking()
            .Where(r => r.EventNotificationReporter == OpenBankingConstants.EventNotificationReporter.HHS)
            .ToListAsync();
        

        //Check AbonelikTipleriObject
        result = OBConsentValidationHelper.IsAbonelikTipleriObjectDataValid(httpContext, configuration,
            olayAbonelik.abonelikTipleri, eventTypeSourceTypeRelations, errorCodeDetails: errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            //validation error in abonelikTipleri data fields
            return result;
        }
      

        //Source Type check.  Descpriton from document:
        //HHS, YÖS API üzerinden YÖS'ün rollerini alarak uygun kaynak tiplerine kayıt olmasını sağlar.
        result = await OBConsentValidationHelper.CheckIfYosHasDesiredRole(httpContext, yosInfoService,olayAbonelik.katilimciBlg.yosKod,
            olayAbonelik.abonelikTipleri, eventTypeSourceTypeRelations, errorCodeDetails:errorCodeDetails, objectName:objectName);
        if (!result.Result)
        {
            return result;
        }

        //Descpriton from document: Olay Abonelik kaydı oluşturmak isteyen YÖS'ün ODS API tanımı HHS tarafından kontrol edilmelidir. 
        //YÖS'ün tanımı olmaması halinde "HTTP 400-TR.OHVPS.Business.InvalidContent" hatası verilmelidir.
        result = await  OBConsentValidationHelper.CheckIfYosProvidesDesiredApi(httpContext, yosInfoService,olayAbonelik.katilimciBlg.yosKod,
            OpenBankingConstants.YosApi.OlayDinleme,errorCodeDetails,objectName:objectName);
        if (!result.Result)
        {
            return result;
        }

        //Descpriton from document: 1 YÖS'ün 1 HHS'de 1 adet abonelik kaydı olabilir. HTTP 400 -TR.OHVPS.Business.InvalidContent -Kaynak Çakışması"
        if (await context.OBEventSubscriptions.AnyAsync(s =>
                s.YOSCode == olayAbonelik.katilimciBlg.yosKod && s.IsActive))
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(httpContext, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidContentThereIsAlreadyEventSubscriotion);
            return result;
        }

        return result;
    }


    /// <summary>
    ///  Checks if data is valid to delete EventSubsrciption
    /// </summary>
    /// <param name="entity">To be checked entity</param>
    /// <returns>Is data valid to delete EventSubsrciption</returns>
    private ApiResult IsDataValidToDeleteEventSubsrciption(OBEventSubscription? entity)
    {
        ApiResult result = new();
        if (entity is null) //No event subscription in system to update
        {
            result.Result = false;
            result.Message = "No event subscription in the system";
            return result;
        }
        //TODO:Özlem If there is any undeliverable object, or not finished consent state. Will I delete the subscription?

        return result;
    }

    private async Task<ApiResult> IsDataValidToSystemEventPost(OlayIstegiDto olayIstegi,
        RequestHeaderDto header,
        List<OBErrorCodeDetail> errorCodeDetails,
        ConsentDbContext context,
        IConfiguration configuration,
        IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        ApiResult result = new();
        //Check header fields
        result = await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, header,
            katilimciBlg: olayIstegi.katilimciBlg, isXJwsSignatureRequired: true, errorCodeDetails: errorCodeDetails,  isEventHeader:true);
        if (!result.Result)
        {
            //validation error in header fields
            return result;
        }

        //Check message required basic properties
        if (olayIstegi?.katilimciBlg is null
            || olayIstegi.olaylar?.Any() is null or false
            || olayIstegi.olaylar.Count != 1
           )
        {
            result.Result = false;
            result.Message =
                "katilimciBlg, olaylar should be in event request message and there should only be 1 event.";
            return result;
        }

        //Check KatılımcıBilgisi
        if (string.IsNullOrEmpty(olayIstegi.katilimciBlg.hhsKod) //Required fields
            || string.IsNullOrEmpty(olayIstegi.katilimciBlg.yosKod)
            || configuration["HHSCode"] != olayIstegi.katilimciBlg.hhsKod)
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. HHSKod YOSKod required";
            return result;
        }
        

        //Check aboneliktipleri data validation
        var eventTypeSourceTypeRelations = await context.OBEventTypeSourceTypeRelations
            .AsNoTracking()
            .Where(r => r.EventNotificationReporter == OpenBankingConstants.EventNotificationReporter.BKM
                        && r.SourceType == olayIstegi.olaylar[0].kaynakTipi
                        && r.EventType == olayIstegi.olaylar[0].olayTipi)
            .ToListAsync();


        //Event Type source type check.
        if (!(eventTypeSourceTypeRelations?.Any() ?? false))
        {
            //no relation in db
            result.Result = false;
            result.Message =
                "TR.OHVPS.Resource.InvalidFormat. TR.OHVPS.DataCode.OlayTip and/or TR.OHVPS.DataCode.KaynakTip wrong.";
            return result;
        }

        return result;
    }
}