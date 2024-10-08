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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace amorphie.consent.Service;

public class YosInfoService : IYosInfoService
{
    private readonly ConsentDbContext _context;
    private readonly IMapper _mapper;
    private readonly IBKMService _bkmService;
    private readonly ILogger<OBEventService> _logger;

    public YosInfoService(ConsentDbContext context,
        IMapper mapper,
        IBKMService bkmService,
        ILogger<OBEventService> logger)
    {
        _context = context;
        _mapper = mapper;
        _bkmService = bkmService;
        _logger = logger;
    }


    public async Task<ApiResult> IsYosInApplication(string yosCode)
    {
        ApiResult result = new();
        try
        {
            var anyYos = await _context.OBYosInfos
                .AsNoTracking()
                .AnyAsync(y => y.Kod == yosCode);

            result.Data = anyYos;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }

    public async Task<ApiResult> GetYosByCode(string yosCode)
    {
        ApiResult result = new();
        try
        {
            var entity = await _context.OBYosInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(y => y.Kod == yosCode);
            result.Data = entity;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }
    
    public async Task<ApiResult> GetYosPublicKey(string yosCode)
    {
        ApiResult result = new();
        try
        {
            var entity = await _context.OBYosInfos
                .AsNoTracking()
                .Where(y => y.Kod == yosCode)
                .Select(y => y.AcikAnahtar)
                .FirstOrDefaultAsync();
            result.Data = entity;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }

    public async Task<ApiResult> CheckIfYosHasDesiredRole(string yosCode,
        string eventTypeSourceTypeRelationYosRole)
    {
        ApiResult result = new();
        try
        {
            //Set yosRole 
            string requiredYosRole = String.Empty;
            if (eventTypeSourceTypeRelationYosRole == OpenBankingConstants.EventTypeSourceTypeRelationYosRole.HBH)
            {
                requiredYosRole = OpenBankingConstants.BKMServiceRole.HBHS;
            }
            if (eventTypeSourceTypeRelationYosRole == OpenBankingConstants.EventTypeSourceTypeRelationYosRole.OBH)
            {
                requiredYosRole = OpenBankingConstants.BKMServiceRole.OBHS;
            }

            //Check yos in database if has specified roles
            var isAnyYosWithRole = await _context.OBYosInfos
                .AsNoTracking()
                .AnyAsync(y => y.Roller.Any(r => r == requiredYosRole)
                               && y.Kod == yosCode);
            result.Data = isAnyYosWithRole;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }


    public async Task<ApiResult> CheckIfYosProvidesDesiredApi(string yosCode,
        string apiName)
    {
        ApiResult result = new();
        try
        {
            bool doesYosProvidesApi = false;
            //Check yos by yosCode in database having specified api
            var yos = await _context.OBYosInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(y => y.Kod == yosCode);
            if (yos != null)
            {
                //Check if yos apibilgileri object contains desired api
                doesYosProvidesApi = _mapper.Map<OBYosInfoDto>(yos).apiBilgileri.Any(a => a.api == apiName);
            }

            result.Data = doesYosProvidesApi;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }


    public async Task<ApiResult> IsYosSubscsribed(string yosKod, string eventType, string sourceType)
    {
        ApiResult result = new();
        try
        {
            result.Data = true;
            bool isSubscriped = await _context.OBEventSubscriptions.AsNoTracking().AnyAsync(s =>
                s.ModuleName == OpenBankingConstants.ModuleName.HHS
                && s.YOSCode == yosKod
                && s.IsActive
                && s.OBEventSubscriptionTypes.Any(t =>
                    t.SourceType == sourceType
                    && t.EventType == eventType));
            if (isSubscriped == false)
            {
                //Yos does not have subscription
                result.Data = false;
                return result;
            }
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }
    
    public async Task<ApiResult> IsYosAddressCorrect(string yosCode, string authType, string address)
    {
        ApiResult result = new();
        try
        {
            bool isAddressCorrect = false;
           //Get yos
            var yos = await _context.OBYosInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(y => y.Kod == yosCode);
            if (yos != null)
            {
                //Check if yos address contains desired address
                isAddressCorrect = _mapper.Map<OBYosInfoDto>(yos)?.adresler.Any(a => a.yetYntm == authType
                    && a.adresDetaylari.Any(d => address.StartsWith(d.tmlAdr))) ?? false;
            }
            result.Data = isAddressCorrect;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }

    public async Task<ApiResult> SaveYos(string yosCode)
    {
        ApiResult result = new();
        try
        {
            //Get yos from bkm
            var getYosResult = await _bkmService.GetYos(yosCode);
            if (!getYosResult.Result)//error in service
            {
                return getYosResult;
            }

            if (getYosResult.Data == null)//Yos data is null
            {
                result.Result = false;
                result.Message = "Yos info is empty";
                return result;
            }

            var yosInfoEntity = await _context.OBYosInfos
                .FirstOrDefaultAsync(y => y.Kod == yosCode);

            OBYosInfoDto yosInfoDto = (OBYosInfoDto)getYosResult.Data;
            if (yosInfoEntity != null)//Update current yos
            {
                yosInfoDto.Id = yosInfoEntity.Id;
                _mapper.Map(yosInfoDto, yosInfoEntity);

                yosInfoEntity.LogoBilgileri = JsonConvert.SerializeObject(yosInfoDto.logoBilgileri);
                yosInfoEntity.ApiBilgileri = JsonConvert.SerializeObject(yosInfoDto.apiBilgileri);
                yosInfoEntity.Adresler = JsonConvert.SerializeObject(yosInfoDto.adresler);
                yosInfoEntity.ModifiedAt = DateTime.UtcNow;

                _context.OBYosInfos.Update(yosInfoEntity);
            }
            else
            {//Insert yos
                yosInfoEntity = _mapper.Map<OBYosInfo>(yosInfoDto);
                yosInfoEntity.LogoBilgileri = JsonConvert.SerializeObject(yosInfoDto.logoBilgileri);
                yosInfoEntity.ApiBilgileri = JsonConvert.SerializeObject(yosInfoDto.apiBilgileri);
                yosInfoEntity.CreatedAt = DateTime.UtcNow;
                yosInfoEntity.ModifiedAt = DateTime.UtcNow;
                _context.OBYosInfos.Add(yosInfoEntity);
            }
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }
}