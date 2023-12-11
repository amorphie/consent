using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.consent.data.Migrations;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;
using Microsoft.EntityFrameworkCore;

namespace amorphie.consent.Service;

public class YosInfoService : IYosInfoService
{
    private readonly ConsentDbContext _context;

    public YosInfoService(ConsentDbContext context)
    {
        _context = context;
    }


    public async Task<ApiResult> IsYosInApplication(string yosCode)
    {
        ApiResult result = new();
        try
        {
            var anyYos = await _context.OBYosInfos
                .AsNoTracking()
                .AnyAsync(y => y.kod == yosCode);

            result.Data = anyYos;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }


}