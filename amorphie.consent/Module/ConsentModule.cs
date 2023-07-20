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

    }
    protected async Task<IResult> SaveConsentDataToDatabase(ConsentDataDto consentData, [FromServices] ConsentDbContext context, [FromServices] IMapper mapper)
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

            context.Set<ConsentDefinition>().Add(consentDefinition);
            context.Set<Consent>().Add(consent);
            context.Set<ConsentPermission>().Add(consentPermission);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();


            var resultDTO = new ConsentDataDto
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

            return Results.Ok(resultDTO);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return Results.BadRequest($"Veri kaydedilirken bir hata oluştu.${e.StackTrace}");
        }
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
