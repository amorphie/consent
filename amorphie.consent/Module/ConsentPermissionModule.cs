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

public class ConsentPermissionModule : BaseBBTRoute<ConsentPermissionDto, ConsentPermission, ConsentDbContext>
{
    public ConsentPermissionModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "Permission" };

    public override string? UrlFragment => "consentPermission";

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
     [AsParameters] ConsentPermissionSearch consentPermissionSearch,
     HttpContext httpContext,
     CancellationToken token
 )
    {
        int skipRecords = (consentPermissionSearch.Page - 1) * consentPermissionSearch.PageSize;

        IList<ConsentPermission> resultList = await context
            .Set<ConsentPermission>()
            .AsNoTracking()
            .Where(x => string.IsNullOrEmpty(consentPermissionSearch.Keyword) || x.Permission.ToLower().Contains(consentPermissionSearch.Keyword.ToLower()))
            .OrderBy(x => x.CreatedAt)
            .Skip(skipRecords)
            .Take(consentPermissionSearch.PageSize)
            .ToListAsync(token);

        return (resultList != null && resultList.Count > 0)
            ? Results.Ok(mapper.Map<IList<ConsentPermissionDto>>(resultList))
            : Results.NoContent();
    }
}
