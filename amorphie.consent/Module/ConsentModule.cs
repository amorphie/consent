using amorphie.core.Module.minimal_api;
using Microsoft.AspNetCore.Mvc;
using amorphie.consent.core.Search;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using amorphie.core.Swagger;
using Microsoft.OpenApi.Models;
using amorphie.consent.data;
using amorphie.consent.core.Model;
using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.Consent;
using amorphie.consent.core.Enum;

namespace amorphie.consent.Module;

public class ConsentModule : BaseBBTRoute<ConsentDto, Consent, ConsentDbContext>
{
    public ConsentModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "ConsentType", "State" };

    public override string? UrlFragment => "consent";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapGet("/search", SearchMethod);
        routeGroupBuilder.MapGet(
            "/GetUserConsents/clientCode={clientCode}&userTCKN={userTCKN}",
            GetUserConsents);
        routeGroupBuilder.MapDelete("/CancelLoginConsents", CancelLoginConsents);
        routeGroupBuilder.MapDelete("/CancelLoginConsent", CancelLoginConsent);
    }


    /// <summary>
    /// Get user active consents- YetkiKullanıldı state by checking clientcode and usertckn
    /// </summary>
    /// <returns>User's active consents list</returns>
    public async Task<IResult> GetUserConsents(
        string clientCode,
        long userTCKN,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper)
    {
        try
        {
            var today = DateTime.UtcNow;
            //Filter consent according to parameters
            var consents = await context.Consents.AsNoTracking()
                .Where(c => c.ClientCode == clientCode
                            && c.UserTCKN == userTCKN
                            && c.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi
                            && (c.LastValidAccessDate == null 
                                || (c.LastValidAccessDate != null && c.LastValidAccessDate > today)))
                .ToListAsync();

            if (consents?.Any() ?? false)
            {
                return Results.Ok(mapper.Map<List<ConsentDto>>(consents));
            }
            //No consent in the system
            return Results.NotFound();
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Cancels IBLogin types of yetkikullanildi state of consents.
    /// Consent state is turned to yetkiIptal state
    /// </summary>
    /// <returns></returns>
    public async Task<IResult> CancelLoginConsents([FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        HttpContext httpContext)
    {
        try
        {
            //Filter login consents
            var consents = await context.Consents.Where(c =>
                    c.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi
                    && c.ConsentType == ConsentConstants.ConsentType.IBLogin)
                .ToListAsync();

            //Update consents
            consents = consents?.Select(c =>
            {
                c.ModifiedAt = DateTime.UtcNow;
                c.State = OpenBankingConstants.RizaDurumu.YetkiIptal;
                c.StateModifiedAt = DateTime.UtcNow;
                c.StateCancelDetailCode = OpenBankingConstants.RizaIptalDetayKodu.IBLogin_ServisIstegiIleIptal;
                return c;
            }).ToList();

            if (consents != null && consents.Any())
            {
                context.Consents.UpdateRange(consents);
                await context.SaveChangesAsync();
            }
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    
    public async Task<IResult> CancelLoginConsent([FromBody] CancelLoginConsentDto cancelData,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        HttpContext httpContext)
    {
        try
        {
            //Check data validity
            ApiResult isDataValidResult = IsDataValidToCancelLoginConsent(cancelData);
            if (!isDataValidResult.Result) //Error in data validation
            {
                return Results.BadRequest(isDataValidResult.Message);
            }
            //Filter login consents
            var consents = await context.Consents.Where(c =>
                    c.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi
                    && c.ConsentType == ConsentConstants.ConsentType.IBLogin
                    && c.ClientCode == cancelData.ClientCode
                    && c.UserTCKN == cancelData.UserTCKN
                    && c.ScopeTCKN == cancelData.ScopeTCKN)
                .ToListAsync();

            //Update consents
            consents = consents?.Select(c =>
            {
                c.ModifiedAt = DateTime.UtcNow;
                c.State = OpenBankingConstants.RizaDurumu.YetkiIptal;
                c.StateModifiedAt = DateTime.UtcNow;
                c.StateCancelDetailCode = OpenBankingConstants.RizaIptalDetayKodu.IBLogin_ServisIstegiIleIptal;
                return c;
            }).ToList();

            if (consents != null && consents.Any())
            {
                context.Consents.UpdateRange(consents);
                await context.SaveChangesAsync();
            }
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    public async ValueTask<IResult> SearchMethod(
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [AsParameters] ConsentSearch consentSearch,
        CancellationToken token
    )
    {
        int skipRecords = (consentSearch.Page - 1) * consentSearch.PageSize;

        IQueryable<Consent> query = context.Consents.AsNoTracking();

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
            ? Results.Ok(mapper.Map<IList<ConsentDto>>(resultList))
            : Results.NoContent();
    }
    
   
    /// <summary>
    /// Checks cancelloginconsent data
    /// </summary>
    /// <param name="cancelData">To be checked data</param>
    /// <returns>Data validation result</returns>
    private ApiResult IsDataValidToCancelLoginConsent(CancelLoginConsentDto cancelData)
    {
        ApiResult result = new();

        //check clientcode
        if (string.IsNullOrEmpty(cancelData.ClientCode))
        {
            result.Result = false;
            result.Message = "Client code parameter is required.";
            return result;
        }
        //check tckn
        if (cancelData.UserTCKN <= 0 || cancelData.ScopeTCKN <= 0)
        {
            result.Result = false;
            result.Message = "usertckn, scope tckn not valid. Can not be zero or negative.";
            return result;
        }

        return result;
    }


}