using amorphie.consent.core.DTO;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.core.Module.minimal_api;

public class YosInfoModule : BaseBBTRoute<YosInfoDto, YosInfo, ConsentDbContext>
{
    public YosInfoModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "marka", "unvan" };

    public override string? UrlFragment => "yosInfo";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
    }
}