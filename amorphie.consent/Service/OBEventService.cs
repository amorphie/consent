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
        string sourceType)
    {
        try
        {
            
            //TODO:Özlem bu servisin başarılı olmaması durumu için ne yapılmalı düşün
            
            //TODO:Özlem Aynı kaynak numarası ile aynı olay-kaynak tipinde, 1 YÖS’e ait, 1 adet iletilemeyen olay kaydı olabilir. Bunu incele
            //Generates OBEvent and OBEventItem entities in db.
            ApiResult insertResult =
                await CreateOBEventEntityObject(consentId, katilimciBilgisi, eventType, sourceType, _context, _mapper);
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
                var olayIstegi = _mapper.Map<OlayIstegiDto>(eventEntity);
                var bkmServiceResponse = await _bkmService.SendEventToYos(olayIstegi);
                if (bkmServiceResponse.Result)//Success from service
                {
                    eventEntity.ResponseCode = (int)(bkmServiceResponse.Data ?? 200);
                    eventEntity.ModifiedAt = DateTime.UtcNow;
                }
                else
                {
                    eventEntity.ResponseCode = (int)(bkmServiceResponse.Data ?? 400);
                    eventEntity.ModifiedAt = DateTime.UtcNow;
                    eventEntity.LastTryTime = DateTime.UtcNow;
                    eventEntity.TryCount = (eventEntity.TryCount ?? 0) + 1;
                }
                _context.OBEvents.Update(eventEntity);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            //TODO:Ozlem log this case
        }
    }
     


      private async Task<ApiResult> CreateOBEventEntityObject(string consentId,
        KatilimciBilgisiDto katilimciBilgisi,
        string eventType,
        string sourceType,
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

        //Create event entity
        OBEvent eventEntity = new OBEvent()
        {
            HHSCode = katilimciBilgisi.hhsKod,
            YOSCode = katilimciBilgisi.yosKod,
            IsUndeliverable = false,
            TryCount = relation.RetryCount,
            LastTryTime = null,
            ModuleName = OpenBankingConstants.ModuleName.HHS,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            OBEventItems = new List<OBEventItem>()
        };
        context.OBEvents.Add(eventEntity); //Add to get Id
        //TODO:Özlem if balance update code
        var itemEntity = new OBEventItem()
        {
            OBEventId = eventEntity.Id,
            EventType = eventType,
            SourceType = sourceType,
            SourceNumber = consentId.ToString(),
            EventDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
        };
        context.OBEventItems.Add(itemEntity); //Add to get id
        itemEntity.EventNumber = itemEntity.Id.ToString();
        //Generate yos post message
        var postMessage = mapper.Map<OlayIstegiDto>(eventEntity);
        eventEntity.AdditionalData = JsonSerializer.Serialize(postMessage);
        await context.SaveChangesAsync();
        result.Data = eventEntity;
        return result;
    }
    
}