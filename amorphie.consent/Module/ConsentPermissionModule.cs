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

    }

    #region ConsentPermissionModule SearchMethod
    // This method retrieves a list of consent permissions from the database based on the provided search criteria.
    // It performs filtering and pagination in the database using the search criteria.
    // If there are results, it returns them as a successful result with a list of ConsentPermissionDto objects.
    // If there are no results, it returns a no content result.
    protected async ValueTask<IResult> SearchMethod(
    [FromServices] ConsentDbContext context,
    [FromServices] IMapper mapper,
    [AsParameters] ConsentPermissionSearch consentPermissionSearch,
    CancellationToken token
)
{
    int skipRecords = (consentPermissionSearch.Page - 1) * consentPermissionSearch.PageSize;

    IQueryable<ConsentPermission> query = context.ConsentPermissions.AsNoTracking();

    if (!string.IsNullOrEmpty(consentPermissionSearch.Keyword))
    {
        string keyword = consentPermissionSearch.Keyword.ToLower();
          query = query.AsNoTracking().Where(x => EF.Functions.ToTsVector("english", string.Join(" ", x.Permission))
           .Matches(EF.Functions.PlainToTsQuery("english", consentPermissionSearch.Keyword)));
    }

    IList<ConsentPermission> resultList = await query.OrderBy(x => x.CreatedAt)
        .Skip(skipRecords)
        .Take(consentPermissionSearch.PageSize)
        .ToListAsync(token);

    return (resultList != null && resultList.Count > 0)
        ? Results.Ok(mapper.Map<IList<ConsentPermissionDto>>(resultList))
        : Results.NoContent();
}
    #endregion
}
