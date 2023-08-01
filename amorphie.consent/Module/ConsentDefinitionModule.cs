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
        routeGroupBuilder.MapGet("/clientId/{clientId}", GetConsentDefinitionByClientId);
    }

    #region  ConsentDefinitionModule GetConsentDefinitionByName Method
    // This method retrieves a consent definition from the database based on the provided consent definition name.
    // It performs a case-insensitive search in the database to find the consent definition with the given name.
    // If the consent definition is found, it maps it to a ConsentDefinitionDTO and returns it as a successful result.
    // If the consent definition is not found, it returns a not found result with an appropriate message.
    protected async ValueTask<IResult> GetConsentDefinitionByName(
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        string consentDefinitionName
    )
    {
        // Retrieve the consent definition from the database based on the given consent definition name.
        var consentDefinition = await context.Set<ConsentDefinition>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == consentDefinitionName.ToLower());

        if (consentDefinition != null)
        {
            var resultDTO = mapper.Map<ConsentDefinitionDTO>(consentDefinition);
            return Results.Ok(resultDTO);
        }
        else
        {
            return Results.NotFound($"ConsentDefinition '{consentDefinitionName}' is not found.");
        }
    }
    #endregion

    #region ConsentDefinitionModule GetConsentDefinitionByClientIdName Method
    // This method retrieves a consent definition from the database based on the provided client ID and consent definition name.
    // It performs a case-insensitive search in the database to find the consent definition with the given client ID and name.
    // If the consent definition is found, it maps it to a ConsentDefinitionDTO and returns it as a successful result.
    // If the consent definition is not found, it returns a not found result with an appropriate message.
    protected async ValueTask<IResult> GetConsentDefinitionByClientIdName(
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        string clientId,
        string consentDefinitionName //Maybe optional ??
    )
    {
        // Retrieve the consent definition from the database based on the given client ID and consent definition name.
        var consentDefinition = await context.Set<ConsentDefinition>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ClientId.Contains(clientId) && x.Name.ToLower() == consentDefinitionName.ToLower());

        if (consentDefinition != null)
        {
            var resultDTO = mapper.Map<ConsentDefinitionDTO>(consentDefinition);
            return Results.Ok(resultDTO);
        }
        else
        {
            return Results.NotFound($"Client ID '{clientId}' or ConsentDefinitionName '{consentDefinitionName}' is not found");
        }
    }
    #endregion

    #region ConsentDefinitionModule GetConsentDefinitionByClientId Method

    // This method retrieves a consent definition from the database based on the provided client ID.
    // It performs a case-insensitive search in the database to find the consent definition with the given client ID.
    // If the consent definition is found, it maps it to a ConsentDefinitionDTO and returns it as a successful result.
    // If the consent definition is not found, it returns a not found result with an appropriate message.

    protected async ValueTask<IResult> GetConsentDefinitionByClientId(
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        string clientId
    )
    {
        // Retrieve the consent definition from the database based on the given client ID.
        // The AsNoTracking method disables the tracking of objects returned by the query, and the FirstOrDefaultAsync method
        // returns the first element of the sequence that satisfies the given condition.

        var consentDefinition = await context.Set<ConsentDefinition>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ClientId.Contains(clientId));

        // If the consent definition is found, map it to a ConsentDefinitionDTO and return it as a successful result.
        if (consentDefinition != null)
        {
            var resultDTO = mapper.Map<ConsentDefinitionDTO>(consentDefinition);
            return Results.Ok(resultDTO);
        }
        else
        {
            // If the consent definition is not found, return a not found result with an appropriate message.
            return Results.NotFound($"Consent definition with Client ID '{clientId}' is not found");
        }
    }

    #endregion

    #region ConsentDefinitionModule SearchMethod
    // This method retrieves a list of consent definitions from the database based on the provided search criteria.
    // It performs filtering and pagination in the database using the search criteria.
    // If there are results, it returns them as a successful result with a list of ConsentDefinitionDTO objects.
    // If there are no results, it returns a no content result.
   protected async ValueTask<IResult> SearchMethod(
    [FromServices] ConsentDbContext context,
    [FromServices] IMapper mapper,
    [AsParameters] ConsentDefinitionSearch consentDefinitionSearch,
    CancellationToken token
)
{
    int skipRecords = (consentDefinitionSearch.Page - 1) * consentDefinitionSearch.PageSize;

    IQueryable<ConsentDefinition> query = context.ConsentDefinitions.AsNoTracking();

    if (!string.IsNullOrEmpty(consentDefinitionSearch.Keyword))
    {
        string keyword = consentDefinitionSearch.Keyword.ToLower();
       query = query.AsNoTracking().Where(x => EF.Functions.ToTsVector("english", string.Join(" ", x.Name, x.RoleAssignment))
           .Matches(EF.Functions.PlainToTsQuery("english", consentDefinitionSearch.Keyword)));
    }

    IList<ConsentDefinition> resultList = await query
        .OrderBy(x => x.CreatedAt)
        .Skip(skipRecords)
        .Take(consentDefinitionSearch.PageSize)
        .ToListAsync(token);

    IList<ConsentDefinitionDTO> resultDTOList = mapper.Map<IList<ConsentDefinitionDTO>>(resultList);

    return (resultDTOList != null && resultDTOList.Count > 0)
        ? Results.Ok(resultDTOList)
        : Results.NoContent();
}
    #endregion
}
