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

public class ConsentDefinitionModule : BaseBBTRoute<ConsentDefinitionDTO, ConsentDefinition, ConsentDbContext>
{
    public ConsentDefinitionModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "Name", "RoleAssignment" };

    public override string? UrlFragment => "consentDefinition";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);

        routeGroupBuilder.MapGet("/search", SearchMethod);
        routeGroupBuilder.MapGet("/consentName/{consentName}", GetConsentDefinitionByName);

        routeGroupBuilder.MapGet("/clientId/{clientId}/consentName/{consentName}", GetConsentDefinitionByClientIdName);
    }
    protected async ValueTask<IResult> GetConsentDefinitionByName()
    {
        // TODO : Get consent definition by consentName
        return Results.Ok();

    }
    protected async ValueTask<IResult> GetConsentDefinitionByClientIdName()
    {
        // TODO : Get consent definition by consentName
        return Results.Ok();

    }
    
    protected async ValueTask<IResult> SearchMethod(
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [AsParameters] ConsentDefinitionSearch consentDefinitionSearch,
        HttpContext httpContext,
        CancellationToken token
    )
    {
        int skipRecords = (consentDefinitionSearch.Page - 1) * consentDefinitionSearch.PageSize;

        IList<ConsentDefinition> resultList = await context
            .Set<ConsentDefinition>()
            .AsNoTracking()
            .Where(x => string.IsNullOrEmpty(consentDefinitionSearch.Keyword) || x.Name.ToLower().Contains(consentDefinitionSearch.Keyword.ToLower()))
            .OrderBy(x => x.CreatedAt)
            .Skip(skipRecords)
            .Take(consentDefinitionSearch.PageSize)
            .ToListAsync(token);

        IList<ConsentDefinitionDTO> resultDTOList = mapper.Map<IList<ConsentDefinitionDTO>>(resultList);

        return (resultDTOList != null && resultDTOList.Count > 0)
            ? Results.Ok(resultDTOList)
            : Results.NoContent();
    }
}
