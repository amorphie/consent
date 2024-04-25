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
            //Filter consent according to parameters
            var consents = await context.Consents.AsNoTracking()
                .Where(c => c.ClientCode == clientCode
                            && c.UserTCKN == userTCKN
                            && c.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi)
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


}