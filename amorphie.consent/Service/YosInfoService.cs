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

namespace amorphie.consent.Service;

public class YosInfoService : IYosInfoService
{
    private readonly ConsentDbContext _context;
    private readonly IMapper _mapper;

    public YosInfoService(ConsentDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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
    
    public async Task<ApiResult> CheckIfYosHasDesiredRole(string yosCode,
        List<AbonelikTipleriDto> abonelikTipleri, 
        List<OBEventTypeSourceTypeRelation> eventTypeSourceTypeRelations)
    {
        ApiResult result = new();
        try
        {
            //Get yos role of desired abonelik tipleri
           var selectedYosRoles = eventTypeSourceTypeRelations.Where(r =>
                abonelikTipleri.Any(a => a.olayTipi == r.EventType && a.kaynakTipi == r.SourceType))
               .Select(r => r.YOSRole)
               .Distinct();
           
           //Set yosRole list
           List<string> toBeCheckedRoles = new List<string>();
           if (selectedYosRoles.Contains(OpenBankingConstants.EventTypeSourceTypeRelationYosRole.HBH))
           {
               toBeCheckedRoles.Add(OpenBankingConstants.BKMServiceRole.HBHS);
           }
           if (selectedYosRoles.Contains(OpenBankingConstants.EventTypeSourceTypeRelationYosRole.OBH))
           {
               toBeCheckedRoles.Add(OpenBankingConstants.BKMServiceRole.OBHS);
           }
           
           //Check yos in database if has specified roles
            var isAnyYosWithRole = await _context.OBYosInfos
                .AsNoTracking()
                .AnyAsync(y => y.Roller.Any(r => toBeCheckedRoles.Contains(r) ) 
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
            {//Check if yos apibilgileri object contains desired api
                doesYosProvidesApi =   _mapper.Map<OBYosInfoDto>(yos).apiBilgileri.Any(a => a.api == apiName);
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


    
}