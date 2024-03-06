using System.Net;
using System.Text.Json;
using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.Event;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.consent.data.Migrations;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace amorphie.consent.Service;

public class OBEventService : IOBEventService
{
    private readonly ConsentDbContext _context;
    private readonly IMapper _mapper;
    private readonly IBKMService _bkmService;

    public OBEventService(ConsentDbContext context,
        IMapper mapper,
        IBKMService bkmService)
    {
        _context = context;
        _mapper = mapper;
        _bkmService = bkmService;
    }


    public async Task DoEventProcess(
        string consentId,
        KatilimciBilgisiDto katilimciBilgisi,
        string eventType,
        string sourceType,
        string sourceNumber)
    {
        try
        {
            //TODO:Özlem bu servisin başarılı olmaması durumu için ne yapılmalı düşün
            
            //Generates OBEvent and OBEventItem entities in db.
            ApiResult insertResult =
                await CreateOBEventEntityObject(consentId, katilimciBilgisi, eventType, sourceType, sourceNumber, _context, _mapper);
            if (!insertResult.Result || insertResult.Data == null)
            {
                //TODO:Ozlem log this case
                //Event could not be created in database
                return;
            }

            //Check if it is instant Post message to YOS
            var isImmediateNotification = await _context.OBEventTypeSourceTypeRelations.AsNoTracking().Where(r =>
                r.EventType == eventType
                && r.SourceType == sourceType).Select(r => r.IsImmediateNotification).FirstOrDefaultAsync();
            if (isImmediateNotification)
            {
                //Send message to yos
                var eventEntity = (OBEvent)insertResult.Data;
                await SendEventToYos(eventEntity);
            }
        }
        catch (Exception)
        {
            //TODO:Ozlem log this case
        }
    }

    public async Task<ApiResult> SendEventToYos(OBEvent eventEntity)
    {
        ApiResult result = new();
        var eventResult = new EventApiResultDto();
        result.Data = eventResult;
        try
        {
            //Get event retry information
            var eventRetryInformation = await _context.OBEventTypeSourceTypeRelations
                .AsNoTracking()
                .Where(r =>
                    r.EventType == eventEntity.EventType
                    && r.SourceType == eventEntity.SourceType
                    && r.EventNotificationReporter == OpenBankingConstants.EventNotificationReporter.HHS)
                .FirstOrDefaultAsync();
            if (eventRetryInformation == null)
            {
                result.Result = false;
                result.Message =  "Invalid event type source type relation";
                eventResult.ContinueTry = true;
                eventResult.StatusCode =  (int)HttpStatusCode.BadRequest;
                return result;
            }

            //Retry policy uygulanmamalıdır. İlk istek gönderilemediği durumda İletilemeyen Olaylara eklenmelidir.
            if (eventRetryInformation.RetryCount == null)
            {
                if (eventEntity.TryCount != 0)
                {
                    //Mark as undeliverable
                    eventEntity.ModifiedAt = DateTime.UtcNow;
                    eventEntity.DeliveryStatus = OpenBankingConstants.EventDeliveryStatus.Undeliverable;
                    _context.OBEvents.Update(eventEntity);
                    await _context.SaveChangesAsync();
                    eventResult.ContinueTry = false;//sending process completed
                    return result;
                }
            }

            //Check if event will retry
            int entityTryCount = eventEntity.TryCount ?? 0;
            int maxRetryCount = eventRetryInformation.RetryCount ?? 1;
            if (entityTryCount < maxRetryCount)
            {
                //Send message to yos
                var olayIstegi = _mapper.Map<OlayIstegiDto>(eventEntity);
                var bkmServiceResponse = await _bkmService.SendEventToYos(olayIstegi);
                if (bkmServiceResponse.Result) //Success from service
                {
                    eventEntity.DeliveryStatus = OpenBankingConstants.EventDeliveryStatus.CompletedSuccessfully;
                    eventEntity.ResponseCode = (int)(bkmServiceResponse.Data ?? 200);
                    eventEntity.ModifiedAt = DateTime.UtcNow;
                    eventEntity.LastTryTime = DateTime.UtcNow;
                    eventEntity.TryCount = entityTryCount + 1;
                    eventResult.ContinueTry = false;//sending process completed
                }
                else
                {
                    eventEntity.ResponseCode = (int)(bkmServiceResponse.Data ?? 400);
                    eventEntity.ModifiedAt = DateTime.UtcNow;
                    eventEntity.LastTryTime = DateTime.UtcNow;
                    eventEntity.TryCount = entityTryCount + 1;
                    if (eventEntity.TryCount >= maxRetryCount)
                    {
                        //Mark as undeliverable
                        eventEntity.DeliveryStatus = OpenBankingConstants.EventDeliveryStatus.Undeliverable;
                        eventResult.ContinueTry = false;//sending process completed
                    }
                }
                _context.OBEvents.Update(eventEntity);
                await _context.SaveChangesAsync();
            }
            else
            {//Try count limit exceed. Do not send, set as undeliverable.
                if (eventEntity.DeliveryStatus ==  OpenBankingConstants.EventDeliveryStatus.Processing)
                {
                    //Mark as undeliverable
                    eventEntity.DeliveryStatus = OpenBankingConstants.EventDeliveryStatus.Undeliverable;
                    eventEntity.ModifiedAt = DateTime.UtcNow;
                    _context.OBEvents.Update(eventEntity);
                    await _context.SaveChangesAsync();
                    eventResult.ContinueTry = false;//sending process completed
                }
            }

            eventResult.StatusCode =  (int)HttpStatusCode.OK;
            return result;
        }
        catch (Exception ex)
        {
            //TODO:Ozlem log this case
            eventResult.StatusCode =  (int)HttpStatusCode.InternalServerError;
            result.Result = false;
            result.Message = ex.Message;
            return result;
        }
    }

    /// <summary>
    /// Creates and insert event entity according to given data
    /// </summary>
    /// <param name="consentId"></param>
    /// <param name="katilimciBilgisi"></param>
    /// <param name="eventType"></param>
    /// <param name="sourceType"></param>
    /// <param name="sourceNumber"></param>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <returns>Event Entity create response</returns>
    private async Task<ApiResult> CreateOBEventEntityObject(string consentId,
        KatilimciBilgisiDto katilimciBilgisi,
        string eventType,
        string sourceType,
        string sourceNumber,
        ConsentDbContext context,
        IMapper mapper)
    {
        ApiResult result = new();
        //Get eventtype source type relation from database
        var relation = await context.OBEventTypeSourceTypeRelations.FirstOrDefaultAsync(r => r.EventType == eventType
            && r.SourceType == sourceType);
        if (relation == null)
        {
            result.Result = false;
            result.Message =
                "EventType SourceType relation not found in system.";
            return result;
        }
        
        //TODO:Özlem Aynı kaynak numarası ile aynı olay-kaynak tipinde, 1 YÖS’e ait, 1 adet iletilemeyen olay kaydı olabilir. Bunu incele
        var anyEventInDb = await context.OBEvents.AnyAsync(e => e.EventType == eventType 
                                                  && e.SourceType == e.SourceType 
                                                  && e.SourceNumber == sourceNumber 
                                                  && e.YOSCode == katilimciBilgisi.yosKod
                                                  && e.ModuleName == OpenBankingConstants.ModuleName.HHS
                                                  && e.DeliveryStatus != OpenBankingConstants.EventDeliveryStatus.CompletedSuccessfully);
        if (anyEventInDb)
        {
            result.Result = false;
            result.Message = "Aynı kaynak numarası ile aynı olay-kaynak tipinde, 1 YÖS’e ait, 1 adet iletilemeyen olay kaydı olabilir.";
            return result;
        }
        //Create event entity
        OBEvent eventEntity = new OBEvent()
        {
            HHSCode = katilimciBilgisi.hhsKod,
            YOSCode = katilimciBilgisi.yosKod,
            DeliveryStatus = OpenBankingConstants.EventDeliveryStatus.Processing,
            TryCount = 0,
            LastTryTime = null,
            ModuleName = OpenBankingConstants.ModuleName.HHS,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
        context.OBEvents.Add(eventEntity); //Add to get Id
        eventEntity.EventNumber = eventEntity.Id.ToString(); 
        eventEntity.EventType = eventType;
        eventEntity.SourceType = sourceType;
        eventEntity.SourceNumber = sourceNumber;
        eventEntity.EventDate = DateTime.UtcNow;

        //Generate yos post message
        var postMessage = mapper.Map<OlayIstegiDto>(eventEntity);
        eventEntity.AdditionalData = JsonSerializer.Serialize(postMessage);
        await context.SaveChangesAsync();
        result.Data = eventEntity;
        return result;
    }
}