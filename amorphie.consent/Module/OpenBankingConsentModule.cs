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
using amorphie.core.Base;
using amorphie.consent.core.DTO.OpenBanking;

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
        routeGroupBuilder.MapGet("/search", SearchMethod);

        routeGroupBuilder.MapPost("/hhs/accountInformationConsent", AccountInformationConsentPost);
        routeGroupBuilder.MapPost("/hhs/paymentInformationConsent", PaymentInformationConsentPost);
        routeGroupBuilder.MapGet("/hhs/hhsAccount/{consentId}", GetHhsConsentById);
        routeGroupBuilder.MapGet("hhs/hhsPayment/{consentId}", GetPaymentConsentById);

        routeGroupBuilder.MapPost("/yos/accountInformationConsent", AccountInformationConsentPost);
        routeGroupBuilder.MapPost("/yos/paymentInformationConsent", PaymentInformationConsentPost);
        routeGroupBuilder.MapPost("/yos/token", HhsToken);
        routeGroupBuilder.MapGet("/yos/userId/{userId}", GetAllHhsConsentWithTokensByUserId);//Tokenlerın sadece sonuncusu gelecek
        //TODO:MehmetAkbaba
        routeGroupBuilder.MapGet("/hhsGetLatestToken/userId/{userId}", GetHhsConsentWithLatestTokensByUserId);//Bu metod silinecek
      
    }
    //hhs bizim bankamızı açacaklar. UI web ekranlarımız.
    //yos burgan uygulaması.

    public async Task<IResult> GetHhsConsentById(
       Guid consentId,
       [FromServices] ConsentDbContext context,
       [FromServices] IMapper mapper)
    {
        try
        {
            var consentWithTokens = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == consentId);
            var serializedData = JsonSerializer.Deserialize<HesapBilgisiRizaIstegiDto>(consentWithTokens.AdditionalData);
            serializedData!.Id = consentWithTokens.Id;
            serializedData.UserId = consentWithTokens.UserId;
            // var hhsConsentDTO = mapper.Map<HesapBilgisiRizaIstegiResponse>(serializedData);

            return Results.Ok(serializedData);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }
    public async Task<IResult> GetPaymentConsentById(
   Guid consentId,
   [FromServices] ConsentDbContext context,
   [FromServices] IMapper mapper)
    {
        try
        {
            var consentWithTokens = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == consentId);
            var serializedData = JsonSerializer.Deserialize<OdemeEmriRizaIstegiDto>(consentWithTokens.AdditionalData);
            serializedData!.Id = consentWithTokens.Id;
            serializedData.UserId = consentWithTokens.UserId;
            // var hhsConsentDTO = mapper.Map<HesapBilgisiRizaIstegiResponse>(serializedData);

            return Results.Ok(serializedData);
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
                    yenilemeBelirteciGecerlilikSuresi = refreshTokens.ExpireTime,

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
                        yenilemeBelirteciGecerlilikSuresi = refresh.ExpireTime,
                        CreatedAt = access.CreatedAt,
                        ModifiedAt = access.ModifiedAt
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


    protected async Task<IResult> AccountInformationConsentPost([FromBody] HesapBilgisiRizaIstegiDto dto,
       [FromServices] ConsentDbContext context,
       [FromServices] IMapper mapper)
    {
        var returnData = new Consent();
        try
        {
            var existingConsent = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == dto.Id);
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
                    dto.hspBlg

                });
                existingConsent.Description = dto.Description;
                existingConsent.ModifiedAt = DateTime.UtcNow;
                existingConsent.State = dto.rzBlg?.rizaDrm;
                existingConsent.ConsentType = "Account Information Consent";


                context.Consents.Update(existingConsent);
            }
            else
            {
                var consentData = mapper.Map<Consent>(dto);
                consentData.AdditionalData = JsonSerializer.Serialize(new
                {
                    dto.rzBlg,
                    dto.kmlk,
                    dto.katilimciBlg,
                    dto.gkd,
                    dto.hspBlg
                });

                consentData.State = dto.rzBlg?.rizaDrm;
                consentData.ConsentType = "Account Information Consent";

                context.Consents.Add(consentData);
                returnData = consentData;
            }
            await context.SaveChangesAsync();
            return Results.Ok(returnData);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    protected async Task<IResult> PaymentInformationConsentPost([FromBody] OdemeEmriRizaIstegiDto dto,
      [FromServices] ConsentDbContext context,
      [FromServices] IMapper mapper)
    {
        var resultData = new Consent();
        try
        {

            var existingConsent = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == dto.Id);

            if (existingConsent != null)
            {
                existingConsent.AdditionalData = JsonSerializer.Serialize(new
                {
                    dto.rzBlg,
                    dto.katilimciBlg,
                    dto.gkd,
                    dto.odmBsltm,
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


                consent.State = "Yetki Bekleniyor";
                dto.gkd.yetTmmZmn = DateTime.UtcNow.AddMinutes(5);
                consent.ConsentType = "Payment Information Consent";
                consent.xGroupId = "1234567890";
                context.Consents.Add(consent);
                var riza = new RizaBilgileriDto
                {
                    rizaNo = consent.Id.ToString(),
                    rizaDrm = consent.State,
                    olusZmn = DateTime.UtcNow,
                    gnclZmn = DateTime.UtcNow,
                };
                consent.AdditionalData = JsonSerializer.Serialize(new
                {
                    riza,
                    dto.katilimciBlg,
                    dto.gkd,
                    dto.odmBsltm,
                });
                resultData = consent;
            }

            await context.SaveChangesAsync();
            return Results.Ok(resultData);
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
