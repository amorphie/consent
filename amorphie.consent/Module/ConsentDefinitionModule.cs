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
        [AsParameters] ConsentDefinitionSearch consentSearch,
        HttpContext httpContext,
        CancellationToken token
    )
    {
        IList<ConsentDefinition> resultList = await context
            .Set<ConsentDefinition>()
            .AsNoTracking()
            .Where(
                x =>
                    x.Name.Contains(consentSearch.Keyword!)
                    || x.RoleAssignment.Contains(consentSearch.Keyword!)
            )
            .Skip(consentSearch.Page)
            .Take(consentSearch.PageSize)
            .ToListAsync(token);

        return (resultList != null && resultList.Count > 0)
            ? Results.Ok(mapper.Map<IList<ConsentDefinitionDTO>>(resultList))
            : Results.NoContent();
    }
}
