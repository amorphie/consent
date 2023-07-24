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
        routeGroupBuilder.MapPost("/saveConsentData", SaveConsentDataToDatabase);
        routeGroupBuilder.MapGet("/search", SearchMethod);
        routeGroupBuilder.MapGet("/user/{userId}/consentType/{consentType}", GetUserConsent);
        routeGroupBuilder.MapGet("/user/{userId}", GetUserConsentByUserId);
    }


    #region ConsentModule GetUserConsent Method
    //This method retrieves the consent information of a user from the database
    // using the user's identity (userId) and the consent type (consentType).
    // The consentType parameter may be optional??. The method performs filtering in the database
    // to find the relevant consent information based on the provided parameters.
    // If the user's consent is found, it returns a successful result with the consent information,
    // otherwise, it returns a not found result indicating that the user's consent could not be found.
    protected async Task<IResult> GetUserConsent(
     [FromServices] ConsentDbContext context,
     Guid userId,
     string? consentType //Required parameter maybe optional ?
 )
    {
        // TODO: In this method, you can retrieve the relevant user's consent from the database
        // using the user identity (userId) and the consent type (consentType).
        // For example, you can perform filtering in the database using the userId and consentType parameters.

        var userConsents = await context.Set<Consent>()
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.ConsentType == consentType)
            .ToListAsync();

        if (userConsents.Count > 0)
        {
            // User Consent Found
            return Results.Ok(userConsents);
        }
        else
        {
            // User Consent Not Found
            return Results.NotFound("Kullanıcının rızası bulunamadı.");
        }
    }
    #endregion


    #region GetUserConsentByUserId Method
    // This method retrieves the consent of a specific user from the database. It is used to query the relevant user's consent
    // using the user identity (userId). For example, filtering can be performed in the database based on the userId parameter.
    protected async Task<IResult> GetUserConsentByUserId(
        [FromServices] ConsentDbContext context,
        Guid userId
    )
    {
        // By querying the "Consent" table in the database, we retrieve the consents that match the UserId field with the userId value.
        // The AsNoTracking method disables the tracking of objects returned by the query. As a result, any changes made to the
        // data in the database will not be tracked and won't be reflected in real-time.
        // Using the Where method, we select consents that have the UserId field matching the userId value, and use the ToListAsync method
        // to asynchronously send the query to the database.

        var userConsents = await context.Set<Consent>()
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync();

        if (userConsents.Count > 0)
        {
            return Results.Ok(userConsents);
        }
        else
        {
            return Results.NotFound("Kullanıcının rızası bulunamadı.");
        }
    }
    #endregion

    #region ConsentModule SaveConsentDataToDatabase Method
    //This method saves consent data to the database. It receives a ConsentDataDto object
    // containing the necessary consent information to be stored in the database. The method creates
    // new instances of ConsentDefinition, Consent, and ConsentPermission based on the provided data,
    // and saves them to the database using the context. It also uses a transaction to ensure data integrity.
    // If the data is successfully saved, it returns a successful result with the ConsentDataDto containing
    // the saved data. If any error occurs during the process, it rolls back the transaction and returns
    // a bad request result with an error message.
    protected async Task<IResult> SaveConsentDataToDatabase(ConsentDataDto consentData,
    [FromServices] ConsentDbContext context,
    [FromServices] IMapper mapper)
    {
        using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var consentDefinition = new ConsentDefinition
            {

                Name = consentData.ConsentDefinitionName,
                RoleAssignment = consentData.RoleAssignment,
                Scope = consentData.Scope,
                ClientId = consentData.ClientId
            };
            var consent = new Consent
            {
                ConsentDefinitionId = consentDefinition.Id,
                ConsentType = consentData.ConsentType,
                State = consentData.State,
                AdditionalData = consentData.AdditionalData,
                UserId = Guid.NewGuid()
            };

            var consentPermission = new ConsentPermission
            {
                ConsentId = consent.Id,
                Permission = consentData.Permission
            };

            context.Add<ConsentDefinition>(consentDefinition);
            context.Add<Consent>(consent);
            context.Add<ConsentPermission>(consentPermission);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();


            var resultDto = new ConsentDataDto
            {
                ConsentType = consent.ConsentType,
                State = consent.State,
                AdditionalData = consent.AdditionalData,
                ConsentDefinitionName = consentDefinition.Name,
                RoleAssignment = consentDefinition.RoleAssignment,
                Scope = consentDefinition.Scope,
                ClientId = consentDefinition.ClientId,
                Permission = consentPermission.Permission
            };

            return Results.Ok(resultDto);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return Results.BadRequest($"Veri kaydedilirken bir hata oluştu.${e.StackTrace}");
        }
    }
    #endregion


    #region ConsentModule Search Method
    // This method retrieves a list of consent definitions from the database based on the provided
    // search criteria in the ConsentDefinitionSearch object. It performs filtering and pagination in the database
    // using the search criteria and returns the results as a list of ConsentDefinitionDTO objects.
    // If there are results, it returns a successful result with the list of consent definitions,
    // otherwise, it returns a no content result.
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

    #endregion
}
