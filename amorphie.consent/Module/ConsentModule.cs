using amorphie.core.Module.minimal_api;
using Microsoft.AspNetCore.Mvc;
using amorphie.consent.core.Search;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using amorphie.core.Swagger;
using Microsoft.OpenApi.Models;
using amorphie.consent.data;
using amorphie.consent.core.Model;

namespace amorphie.consent.Module;

public class ConsentModule : BaseBBTRoute<ConsentDTO, Consent, ConsentDbContext>
{
    public ConsentModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "ConsentType", "State" };

    public override string? UrlFragment => "consent";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);

        routeGroupBuilder.MapGet("/search", SearchMethod);

        routeGroupBuilder.MapGet("/custom-method", CustomMethod);
    }


    [AddSwaggerParameter("Test Required", ParameterLocation.Header, true)]
    protected async ValueTask<IResult> CustomMethod()
    {
        return Results.Ok();
    }

    protected async ValueTask<IResult> SearchMethod(
     [FromServices] ConsentDbContext context,
     [FromServices] IMapper mapper,
     [AsParameters] ConsentSearch consentSearch,
     HttpContext httpContext,
     CancellationToken token
 )
    {
        int skipRecords = (consentSearch.Page - 1) * consentSearch.PageSize;

        IList<Consent> resultList = await context
            .Set<Consent>()
            .AsNoTracking()
            .Where(x => string.IsNullOrEmpty(consentSearch.Keyword) || x.AdditionalData.ToLower().Contains(consentSearch.Keyword.ToLower()))
            .OrderBy(x => x.CreatedAt)
            .Skip(skipRecords)
            .Take(consentSearch.PageSize)
            .ToListAsync(token);

        return (resultList != null && resultList.Count > 0)
            ? Results.Ok(mapper.Map<IList<ConsentDTO>>(resultList))
            : Results.NoContent();
    }
}
