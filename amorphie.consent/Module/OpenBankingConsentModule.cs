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
        routeGroupBuilder.MapPost("/hhs/accountInformationConsent", AccountInformationConsentPost);
        routeGroupBuilder.MapPost("/hhs/paymentInformationConsent", PaymentInformationConsentPost);
        routeGroupBuilder.MapPost("/hhs/token", HhsToken);
        routeGroupBuilder.MapGet("/hhs/userId/{userId}", GetAllHhsConsentWithTokensByUserId);
        routeGroupBuilder.MapGet("/hhsGetLatestToken/userId/{userId}", GetHhsConsentWithLatestTokensByUserId);
        routeGroupBuilder.MapGet("/hhs/{consentId}", GetHhsConsentWithTokensById);
        routeGroupBuilder.MapGet("/hhs/consentType/{consentType}", GetHhsConsentWithTokensByType);
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

    public async Task<IResult> GetHhsConsentWithTokensById(
       Guid consentId,
       [FromServices] ConsentDbContext context,
       [FromServices] IMapper mapper)
    {
        try
        {
            var consentWithTokens = await context.Consents
                .Include(c => c.Token)
                .FirstOrDefaultAsync(c => c.Id == consentId);

            if (consentWithTokens == null)
            {
                return Results.NotFound("Consent not found.");
            }

            var accessTokens = consentWithTokens.Token
                .Where(token => token.TokenType == "Access Token" && !string.IsNullOrEmpty(token.TokenValue))
                .ToList();

            var refreshTokens = consentWithTokens.Token
                .Where(token => token.TokenType == "Refresh Token" && !string.IsNullOrEmpty(token.TokenValue))
                .ToList();

            // if (accessTokens.Count == 0 || refreshTokens.Count == 0)
            // {
            //     return Results.Problem("Both access and refresh tokens are required.");
            // }

            var hhsConsentDTO = new HhsConsentDto
            {
                Id = consentWithTokens.Id,
                AdditionalData = consentWithTokens.AdditionalData,
                description = consentWithTokens.Description,
                xGroupId = consentWithTokens.xGroupId,
                Token = accessTokens.Zip(refreshTokens, (access, refresh) => new TokenModel
                {
                    Id = access.ConsentId,
                    erisimBelirteci = access.TokenValue,
                    gecerlilikSuresi = access.ExpireTime,
                    yenilemeBelirteci = refresh.TokenValue,
                    yenilemeBelirteciGecerlilikSuresi = refresh.ExpireTime
                }).ToList()
            };

            return Results.Ok(hhsConsentDTO);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }
    public async Task<IResult> GetHhsConsentWithLatestTokensByUserId(
       Guid userId,
       string? consentType,
       [FromServices] ConsentDbContext context,
       [FromServices] IMapper mapper)
    {
        try
        {
            IQueryable<Consent> query = context.Consents
                .Include(c => c.Token)
                .Where(c => c.UserId == userId);

            if (!string.IsNullOrEmpty(consentType))
            {
                query = query.Where(c => c.ConsentType == consentType);
            }

            var consentWithTokens = await query
                .OrderByDescending(c => c.CreatedAt) // Get the most recent consent
                .FirstOrDefaultAsync();

            if (consentWithTokens == null)
            {
                return Results.NotFound("Consent not found.");
            }

            var accessTokens = consentWithTokens.Token
                .Where(token => token.TokenType == "Access Token" && !string.IsNullOrEmpty(token.TokenValue))
                .LastOrDefault(); // Get the most recent Access Token

            var refreshTokens = consentWithTokens.Token
                .Where(token => token.TokenType == "Refresh Token" && !string.IsNullOrEmpty(token.TokenValue))
                .LastOrDefault(); // Get the most recent Refresh Token

            // if (accessTokens == null || refreshTokens == null)
            // {
            //     return Results.Problem("Both access and refresh tokens are required.");
            // }

            var hhsConsentDTO = new HhsConsentDto
            {
                Id = consentWithTokens.Id,
                AdditionalData = consentWithTokens.AdditionalData,
                description = consentWithTokens.Description,
                xGroupId = consentWithTokens.xGroupId,
                Token = new List<TokenModel>
            {
                new TokenModel
                {
                    Id = accessTokens.ConsentId,
                    erisimBelirteci = accessTokens.TokenValue,
                    gecerlilikSuresi = accessTokens.ExpireTime,
                    yenilemeBelirteci = refreshTokens.TokenValue,
                    yenilemeBelirteciGecerlilikSuresi = refreshTokens.ExpireTime
                }
            }
            };

            return Results.Ok(hhsConsentDTO);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    public async Task<IResult> GetAllHhsConsentWithTokensByUserId(
        Guid userId,
        string? consentType,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] ITranslationService translationService,
        [FromServices] ILanguageService languageService,
        HttpContext httpContext)
    {
        string selectedLanguage = await languageService.GetLanguageAsync(httpContext);

        try
        {

            IQueryable<Consent> query = context.Consents
                .Include(c => c.Token)
                .Where(c => c.UserId == userId);

            if (!string.IsNullOrEmpty(consentType))
            {
                query = query.Where(c => c.ConsentType == consentType);
            }

            var consentsWithTokens = await query.ToListAsync();

            if (consentsWithTokens == null || consentsWithTokens.Count == 0)
            {
                var notFoundMessage = await translationService.GetTranslatedMessageAsync(selectedLanguage, "Errors.ConsentsNotFound");
                return Results.NotFound(notFoundMessage);
            }

            var hhsConsentDTOs = new List<HhsConsentDto>();

            foreach (var consentWithTokens in consentsWithTokens)
            {
                var accessTokens = consentWithTokens.Token
                    .Where(token => token.TokenType == "Access Token" && !string.IsNullOrEmpty(token.TokenValue))
                    .ToList();

                var refreshTokens = consentWithTokens.Token
                    .Where(token => token.TokenType == "Refresh Token" && !string.IsNullOrEmpty(token.TokenValue))
                    .ToList();

                var hhsConsentDTO = new HhsConsentDto
                {
                    Id = consentWithTokens.Id,
                    AdditionalData = consentWithTokens.AdditionalData,
                    description = consentWithTokens.Description,
                    xGroupId = consentWithTokens.xGroupId,
                    Token = accessTokens.Zip(refreshTokens, (access, refresh) => new TokenModel
                    {
                        Id = access.ConsentId,
                        erisimBelirteci = access.TokenValue,
                        gecerlilikSuresi = access.ExpireTime,
                        yenilemeBelirteci = refresh.TokenValue,
                        yenilemeBelirteciGecerlilikSuresi = refresh.ExpireTime
                    }).ToList()
                };

                hhsConsentDTOs.Add(hhsConsentDTO);
            }

            return Results.Ok(hhsConsentDTOs);
        }
        catch (Exception ex)
        {
            var errorMessage = await translationService.GetTranslatedMessageAsync(selectedLanguage, "Errors.Problem");
            return Results.Problem(errorMessage);
        }
    }

    public async Task<IResult> GetHhsConsentWithTokensByType(
        string consentType,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper)
    {
        try
        {
            var consentWithTokens = await context.Consents
                .Include(c => c.Token)
                .FirstOrDefaultAsync(c => c.ConsentType == consentType);

            if (consentWithTokens == null)
            {
                return Results.NotFound("Consent not found.");
            }

            var accessTokens = consentWithTokens.Token
                .Where(token => token.TokenType == "Access Token" && !string.IsNullOrEmpty(token.TokenValue))
                .ToList();

            var refreshTokens = consentWithTokens.Token
                .Where(token => token.TokenType == "Refresh Token" && !string.IsNullOrEmpty(token.TokenValue))
                .ToList();

            // if (accessTokens.Count == 0 || refreshTokens.Count == 0)
            // {
            //     return Results.Problem("Both access and refresh tokens are required.");
            // }

            var hhsConsentDTO = new HhsConsentDto
            {
                Id = consentWithTokens.Id,
                AdditionalData = consentWithTokens.AdditionalData,
                description = consentWithTokens.Description,
                xGroupId = consentWithTokens.xGroupId,
                Token = accessTokens.Zip(refreshTokens, (access, refresh) => new TokenModel
                {
                    Id = access.ConsentId,
                    erisimBelirteci = access.TokenValue,
                    gecerlilikSuresi = access.ExpireTime,
                    yenilemeBelirteci = refresh.TokenValue,
                    yenilemeBelirteciGecerlilikSuresi = refresh.ExpireTime
                }).ToList()
            };

            return Results.Ok(hhsConsentDTO);
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


    protected async Task<IResult> AccountInformationConsentPost([FromBody] HesapBilgisiRizaIstegiResponse dto,
       [FromServices] ConsentDbContext context,
       [FromServices] IMapper mapper)
    {
        try
        {
            var existingConsent=await context.Consents
                .FirstOrDefaultAsync(c => c.Id == dto.id);
            // var existingConsent = await context.Consents
            //     .FirstOrDefaultAsync(c => c.Id == dto.Id &&
            //                                c.AdditionalData.Contains($"\"RizaNo\":\"{dto.rzBlg.rizaNo}\""));

            if (existingConsent != null)
            {
                existingConsent.AdditionalData = JsonSerializer.Serialize(new
                {
                    dto.rzBlg,
                    dto.kmlk,
                    dto.katilimciBlg,
                    dto.gkd,
                    dto.hspBlg,
                    dto.ayrBlg,
                });
                existingConsent.Description = dto.Description;
                existingConsent.ModifiedAt = DateTime.UtcNow;
                existingConsent.State = dto.rzBlg?.rizaDrm;
                existingConsent.ConsentType = "Account Information Consent";


                context.Consents.Update(existingConsent);
            }
            else
            {
                var consent = mapper.Map<Consent>(dto);
                consent.AdditionalData = JsonSerializer.Serialize(new
                {
                    dto.rzBlg,
                    dto.kmlk,
                    dto.katilimciBlg,
                    dto.gkd,
                    dto.hspBlg,
                    dto.ayrBlg,
                });

                consent.State = dto.rzBlg?.rizaDrm;
                consent.ConsentType = "Account Information Consent";

                context.Consents.Add(consent);
            }

            await context.SaveChangesAsync();
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }
    protected async Task<IResult> PaymentInformationConsentPost([FromBody] HesapBilgisiRizaIstegiResponse dto,
      [FromServices] ConsentDbContext context,
      [FromServices] IMapper mapper)
    {
        try
        {
            var existingConsent = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == dto.id);

            if (existingConsent != null)
            {
                existingConsent.AdditionalData = JsonSerializer.Serialize(new
                {
                    dto.rzBlg,
                    dto.kmlk,
                    dto.katilimciBlg,
                    dto.gkd,
                    dto.hspBlg,
                    dto.ayrBlg,
                });
                existingConsent.Description = dto.Description;
                existingConsent.ModifiedAt = DateTime.UtcNow;
                existingConsent.State = dto.rzBlg?.rizaDrm;
                existingConsent.ConsentType = "Payment Information Consent";

                context.Consents.Update(existingConsent);
            }
            else
            {
                var consent = mapper.Map<Consent>(dto);
                consent.AdditionalData = JsonSerializer.Serialize(new
                {
                    dto.rzBlg,
                    dto.kmlk,
                    dto.katilimciBlg,
                    dto.gkd,
                    dto.hspBlg,
                    dto.ayrBlg,
                });

                consent.State = dto.rzBlg?.rizaDrm;
                consent.ConsentType = "Payment Information Consent";

                context.Consents.Add(consent);
            }

            await context.SaveChangesAsync();
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }
    protected async Task<IResult> HhsToken([FromBody] TokenModel tokenModel,
    [FromServices] ConsentDbContext context,
    [FromServices] IMapper mapper)
    {
        try
        {
            var (erisimToken, yenilemeToken) = mapper.Map<(Token, Token)>(tokenModel);

            context.Tokens.Add(erisimToken);
            context.Tokens.Add(yenilemeToken);
            await context.SaveChangesAsync();
            var tokenList = new[] { erisimToken, yenilemeToken }
                .Select(mapper.Map<Token>)
                .ToList();

            return Results.Ok(tokenList);
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
    private (TokenModel erisimToken, TokenModel yenilemeToken) MapTokens(List<Token> tokens, IMapper mapper)
    {
        var erisimToken = mapper.Map<TokenModel>(tokens.FirstOrDefault(t => t.TokenType == "Access Token"));
        var yenilemeToken = mapper.Map<TokenModel>(tokens.FirstOrDefault(t => t.TokenType == "Refresh Token"));

        return (erisimToken, yenilemeToken);
    }

}
