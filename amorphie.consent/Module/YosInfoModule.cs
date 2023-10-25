using amorphie.consent.core.DTO;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.core.Module.minimal_api;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace amorphie.consent.Module;
public class YosInfoModule : BaseBBTRoute<YosInfoDto, YosInfo, ConsentDbContext>
{
    public YosInfoModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "marka", "unvan" };

    public override string? UrlFragment => "yosInfo";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapGet("/yosId/{yosId}", GetYosWithYosId);
    }




    protected async Task<IResult> GetYosWithYosId(
     [FromServices] ConsentDbContext context,
     IMapper mapper,
     string yosId
 )
    {
        var yosInfo = await context.Set<YosInfo>()
            .AsNoTracking()
            .Where(x => x.kod == yosId)
            .FirstOrDefaultAsync();

        if (yosInfo != null)
        {
            var data = mapper.Map<YosInfoDto>(yosInfo);
            return Results.Ok(data);
        }
        else
        {
            return Results.NotFound("YosInfo not found");
        }
    }
}