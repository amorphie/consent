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
using amorphie.consent.core.DTO.OpenBanking.HHS;

namespace amorphie.consent.Module;

public class OpenBankingYOSConsentModule : BaseBBTRoute<OpenBankingConsentDTO, Consent, ConsentDbContext>
{
    public OpenBankingYOSConsentModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "ConsentType", "State" };

    public override string? UrlFragment => "OpenBankingConsentYOS";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapPost("/hesap-bilgisi-rizasi", AccountInformationConsentPost);
        routeGroupBuilder.MapPost("/odeme-emri-rizasi", PaymentInformationConsentPost);
        routeGroupBuilder.MapPost("/token", HhsToken);
        routeGroupBuilder.MapGet("/userId/{userId}", GetAllHhsConsentWithTokensByUserId);//Tokenlerın sadece sonuncusu gelecek

    }

    //yos burgan uygulamasi.

    #region YOS

    public async Task<IResult> GetAllHhsConsentWithTokensByUserId(
   Guid userId,
   [FromServices] ConsentDbContext context,
   [FromServices] IMapper mapper,
   [FromServices] ITranslationService translationService,
   [FromServices] ILanguageService languageService,
   HttpContext httpContext)
    {
        string selectedLanguage = await languageService.GetLanguageAsync(httpContext);

        try
        {
            var consentsWithTokens = await context.Consents
                .Include(c => c.Token)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            var hhsConsentDTOs = new List<HhsConsentDto>();

            foreach (var consentWithTokens in consentsWithTokens)
            {
                var accessTokens = consentWithTokens.Token
                    .Where(token => token.TokenType == "Access Token" && !string.IsNullOrEmpty(token.TokenValue))
                    .OrderByDescending(token => token.CreatedAt)
                    .FirstOrDefault();

                var refreshTokens = consentWithTokens.Token
                    .Where(token => token.TokenType == "Refresh Token" && !string.IsNullOrEmpty(token.TokenValue))
                    .OrderByDescending(token => token.CreatedAt)
                    .FirstOrDefault();

                var hhsConsentDTO = new HhsConsentDto
                {
                    Id = consentWithTokens.Id,
                    AdditionalData = consentWithTokens.AdditionalData,
                    description = consentWithTokens.Description,
                    xGroupId = consentWithTokens.xGroupId,
                    Token = accessTokens != null && refreshTokens != null ? new List<OpenBankingTokenDto>
                {
                    new OpenBankingTokenDto
                    {
                        Id = accessTokens.ConsentId,
                        erisimBelirteci = accessTokens.TokenValue,
                        gecerlilikSuresi = accessTokens.ExpireTime,
                        yenilemeBelirteci = refreshTokens.TokenValue,
                        yenilemeBelirteciGecerlilikSuresi = refreshTokens.ExpireTime,
                        CreatedAt = accessTokens.CreatedAt,
                        ModifiedAt = accessTokens.ModifiedAt
                    }
                } : null
                };

                hhsConsentDTOs.Add(hhsConsentDTO);
            }

            var consentsWithoutTokens = consentsWithTokens
                .Where(c => hhsConsentDTOs.All(dto => dto.Id != c.Id))
                .Select(consent => new HhsConsentDto
                {
                    Id = consent.Id,
                    AdditionalData = consent.AdditionalData,
                    description = consent.Description,
                    xGroupId = consent.xGroupId,
                    Token = null // Tokenlar yok
                });

            hhsConsentDTOs.AddRange(consentsWithoutTokens);

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
                existingConsent.ConsentType = "H";
                existingConsent.xGroupId = dto.xGroupId;
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
                consentData.ConsentType = "H";
                consentData.xGroupId = dto.xGroupId;
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
            if (dto == null)
            {
                return Results.BadRequest();
            }
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
                existingConsent.xGroupId = dto.xGroupId ?? Guid.NewGuid().ToString();
                existingConsent.Description = dto.Description;
                existingConsent.ModifiedAt = DateTime.UtcNow;
                existingConsent.State = dto.rzBlg?.rizaDrm;
                existingConsent.ConsentType = "O";

                context.Consents.Update(existingConsent);
            }
            else
            {
                var consent = mapper.Map<Consent>(dto);
                consent.State = dto.rzBlg?.rizaDrm;
                consent.ConsentType = "O";
                consent.xGroupId = dto.xGroupId;
                consent.Description = dto.Description;
                consent.AdditionalData = JsonSerializer.Serialize(new
                {
                    dto.rzBlg,
                    dto.katilimciBlg,
                    dto.gkd,
                    dto.odmBsltm,
                });
                context.Consents.Add(consent);
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

    protected async Task<IResult> HhsToken([FromBody] OpenBankingTokenDto tokenModel,
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

    #endregion


    private (OpenBankingTokenDto erisimToken, OpenBankingTokenDto yenilemeToken) MapTokens(List<Token> tokens, IMapper mapper)
    {
        var erisimToken = mapper.Map<OpenBankingTokenDto>(tokens.FirstOrDefault(t => t.TokenType == "Access Token"));
        var yenilemeToken = mapper.Map<OpenBankingTokenDto>(tokens.FirstOrDefault(t => t.TokenType == "Refresh Token"));

        return (erisimToken, yenilemeToken);
    }

}
