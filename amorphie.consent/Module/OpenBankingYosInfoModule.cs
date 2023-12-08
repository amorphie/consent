using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.core.Module.minimal_api;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace amorphie.consent.Module;

public class OpenBankingYosInfoModule : BaseBBTRoute<OBYosInfoDto, OBYosInfo, ConsentDbContext>
{
    public OpenBankingYosInfoModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "marka", "unvan" };

    public override string? UrlFragment => "OpenBankingYosInfo";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapGet("/code/{code}", GetYosWithYosCode);
    }
    
    public async Task<IResult> GetYosWithYosCode(
     [FromServices] ConsentDbContext context,
     IMapper mapper,
     string code)
    {
        var yosInfo = await context.OBYosInfos
            .AsNoTracking()
            .FirstOrDefaultAsync(y => y.kod == code);

        if (yosInfo != null)
        {
            var data = mapper.Map<OBYosInfoDto>(yosInfo);
            return Results.Ok(data);
        }
        return Results.NotFound("YosInfo not found");
    }
}