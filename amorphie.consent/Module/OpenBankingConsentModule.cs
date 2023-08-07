using amorphie.core.Module.minimal_api;
using Microsoft.AspNetCore.Mvc;
using amorphie.consent.core.Search;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using amorphie.core.Swagger;
using Microsoft.OpenApi.Models;
using amorphie.consent.data;
using amorphie.consent.core.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace amorphie.consent.Module;

public class OpenBankingConsentModule : BaseBBTRoute<OpenBankingConsentDTO, Consent, ConsentDbContext>
{
    public OpenBankingConsentModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "ConsentType", "State" };

    public override string? UrlFragment => "OpenBankingConsent";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapPost("/saveConsentData", CustomPost);
        routeGroupBuilder.MapGet("/get", GetConsentWithPermissionsAndToken);
        routeGroupBuilder.MapGet("/search", SearchMethod);
        routeGroupBuilder.MapGet("/getByUserId/{userId}", GetConsentWithPermissionsAndTokenByUserId);
        routeGroupBuilder.MapPost("/hhs", HhsPost);
    }



    protected async Task<IResult> CustomPost([FromBody] OpenBankingConsentDTO dto,
    [FromServices] ConsentDbContext context,
    [FromServices] IMapper mapper)
    {
        try
        {
            var consent = mapper.Map<Consent>(dto);
            context.Consents.Add(consent);
            await context.SaveChangesAsync();
            return Results.Ok(consent);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    protected async Task<IResult> GetConsentWithPermissionsAndToken(Guid consentId,
       [FromServices] ConsentDbContext context,
       [FromServices] IMapper mapper)
    {
        try
        {

            var consent = await context.Consents
                .Include(c => c.ConsentPermission)
                .Include(c => c.Token)
                .FirstOrDefaultAsync(c => c.Id == consentId);

            if (consent == null)
            {
                return Results.NotFound("Consent not found.");
            }

            var consentDTO = mapper.Map<OpenBankingConsentDTO>(consent);
            return Results.Ok(consentDTO);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    protected async Task<IResult> GetConsentWithPermissionsAndTokenByUserId(Guid userId,
    [FromServices] ConsentDbContext context,
    [FromServices] IMapper mapper)
    {
        try
        {
            var consents = await context.Consents
                .Where(c => c.UserId == userId)
                .Include(c => c.ConsentPermission)
                .Include(c => c.Token)
                .ToListAsync();

            if (consents == null || consents.Count == 0)
            {
                return Results.NotFound("Consents not found for the specified user.");
            }
            
            //Can there be more than one consent for a userId? Therefore, would it be more sensible to return the data in a list format?
            var consentDTOs = mapper.Map<IList<OpenBankingConsentDTO>>(consents);
            return Results.Ok(consentDTOs);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

  protected async Task<IResult> HhsPost([FromBody] HesapBilgisiRizaIstegiResponse dto,
    [FromServices] ConsentDbContext context,
    [FromServices] IMapper mapper)
{
    try
    {
        var consent = mapper.Map<Consent>(dto);

        var additionalData = new
        {
            dto.RzBlg,
            dto.Kmlk,
            dto.KatilimciBlg,
            dto.Gkd,
            dto.HspBlg,
            dto.AyrintiBlg
        };

        consent.AdditionalData = JsonSerializer.Serialize(additionalData);

        if (dto.RzBlg?.RizaDrm != null)
        {
            consent.State = dto.RzBlg?.RizaDrm!;
            consent.ConsentType = "AccountInfoConsent";
        }

        context.Consents.Add(consent);
        await context.SaveChangesAsync();
        return Results.Ok(consent);
    }
    catch (Exception ex)
    {
        return Results.Problem($"An error occurred: {ex.Message}");
    }
}
    protected async ValueTask<IResult> SearchMethod(
      [FromServices] ConsentDbContext context,
      [FromServices] IMapper mapper,
      [AsParameters] ConsentSearch consentSearch,
      CancellationToken token
  )
    {
        int skipRecords = (consentSearch.Page - 1) * consentSearch.PageSize;

        IQueryable<Consent> query = context.Consents
            .Include(c => c.Token)
            .Include(c => c.ConsentPermission)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(consentSearch.Keyword))
        {
            string keyword = consentSearch.Keyword.ToLower();
            query = query.AsNoTracking().Where(x => EF.Functions.ToTsVector("english", string.Join(" ", x.State, x.ConsentType, x.AdditionalData))
             .Matches(EF.Functions.PlainToTsQuery("english", consentSearch.Keyword)));
        }

        IList<Consent> resultList = await query.OrderBy(x => x.CreatedAt)
            .Skip(skipRecords)
            .Take(consentSearch.PageSize)
            .ToListAsync(token);

        return (resultList != null && resultList.Count > 0)
            ? Results.Ok(mapper.Map<IList<OpenBankingConsentDTO>>(resultList))
            : Results.NoContent();
    }
}
