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
using amorphie.consent.core.DTO.OpenBanking.YOS;
using amorphie.consent.core.Enum;

namespace amorphie.consent.Module;

public class OpenBankingYOSConsentModule : BaseBBTRoute<OpenBankingConsentDto, Consent, ConsentDbContext>
{
    public OpenBankingYOSConsentModule(WebApplication app)
        : base(app)
    {
    }

    public override string[]? PropertyCheckList => new string[] { "ConsentType", "State" };

    public override string? UrlFragment => "OpenBankingConsentYOS";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapPost("/hesap-bilgisi-rizasi", AccountInformationConsentPost);
        routeGroupBuilder.MapPost("/odeme-emri-rizasi", PaymentInformationConsentPost);
        routeGroupBuilder.MapPost("/token", HhsToken);
        routeGroupBuilder.MapGet("/GetUserConsents/userId/{userId}/consentType/{consentType}",
            GetConsentsWithTokensByUserId); //Tokenlerın sadece sonuncusu gelecek
    }

    //yos burgan uygulamasi.

    #region YOS

    public async Task<IResult> GetConsentsWithTokensByUserId(
        Guid userId,
        string consentType,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] ITranslationService translationService,
        [FromServices] ILanguageService languageService,
        HttpContext httpContext)
    {
        string selectedLanguage = await languageService.GetLanguageAsync(httpContext);

        try
        {
            var consentEntities = await context.Consents.AsNoTracking()
                .Include(c => c.Token)
                .Where(c => c.UserId == userId
                            && c.ConsentType == consentType).ToListAsync();
            
            var responseConsents = new List<YOSConsentDto>();

            foreach (var consentEntity in consentEntities)
            {
                var accessTokens = consentEntity.Token
                    .Where(token => token.TokenType == "Access Token" && !string.IsNullOrEmpty(token.TokenValue))
                    .OrderByDescending(token => token.CreatedAt)
                    .FirstOrDefault();

                var refreshTokens = consentEntity.Token
                    .Where(token => token.TokenType == "Refresh Token" && !string.IsNullOrEmpty(token.TokenValue))
                    .OrderByDescending(token => token.CreatedAt)
                    .FirstOrDefault();

                responseConsents.Add( new YOSConsentDto
                {
                    Id = consentEntity.Id,
                    AdditionalData = consentEntity.AdditionalData,
                    Description = consentEntity.Description,
                    xGroupId = consentEntity.xGroupId,
                    Token = accessTokens != null && refreshTokens != null
                        ? 
                            new()
                            {
                                Id = accessTokens.ConsentId,
                                erisimBelirteci = accessTokens.TokenValue,
                                gecerlilikSuresi = accessTokens.ExpireTime,
                                yenilemeBelirteci = refreshTokens.TokenValue,
                                yenilemeBelirteciGecerlilikSuresi = refreshTokens.ExpireTime,
                                CreatedAt = accessTokens.CreatedAt,
                                ModifiedAt = accessTokens.ModifiedAt
                            }
                        : null
                });
            }

            return Results.Ok(responseConsents);
        }
        catch (Exception ex)
        {
            var errorMessage = await translationService.GetTranslatedMessageAsync(selectedLanguage, "Errors.Problem");
            return Results.Problem(errorMessage);
        }
    }

    protected async Task<IResult> AccountInformationConsentPost([FromBody] HesapBilgisiRizaIstegiDto rizaIstegi,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper)
    {
        var returnData = new Consent();
        try
        {
            //Get consent in db
            var existingConsent = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == rizaIstegi.Id
                                          && c.ConsentType == OpenBankingConstants.ConsentType.OpenBankingYOSAccount);
            
            if (existingConsent != null)
            {//Update consent
                existingConsent.AdditionalData = JsonSerializer.Serialize(new
                {
                    rizaIstegi.rzBlg,
                    rizaIstegi.kmlk,
                    rizaIstegi.katilimciBlg,
                    rizaIstegi.gkd,
                    rizaIstegi.hspBlg
                });
                existingConsent.Description = rizaIstegi.Description;
                existingConsent.ModifiedAt = DateTime.UtcNow;
                existingConsent.State = rizaIstegi.rzBlg?.rizaDrm;
                existingConsent.ConsentType = OpenBankingConstants.ConsentType.OpenBankingYOSAccount;
                existingConsent.xGroupId = rizaIstegi.xGroupId;
                context.Consents.Update(existingConsent);
            }
            else
            {//Insert consent
                var consentData = mapper.Map<Consent>(rizaIstegi);
                consentData.AdditionalData = JsonSerializer.Serialize(new
                {
                    rizaIstegi.rzBlg,
                    rizaIstegi.kmlk,
                    rizaIstegi.katilimciBlg,
                    rizaIstegi.gkd,
                    rizaIstegi.hspBlg
                });

                consentData.State = rizaIstegi.rzBlg?.rizaDrm;
                consentData.ConsentType = OpenBankingConstants.ConsentType.OpenBankingYOSAccount;
                consentData.xGroupId = rizaIstegi.xGroupId;
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

    protected async Task<IResult> PaymentInformationConsentPost([FromBody] OdemeEmriRizaIstegiDto rizaIstegi,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper)
    {
        var resultData = new Consent();
        try
        {
            if (rizaIstegi == null)
            {
                return Results.BadRequest();
            }

            var existingConsent = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == rizaIstegi.Id
                                                && c.ConsentType == OpenBankingConstants.ConsentType.OpenBankingYOSPayment);

            if (existingConsent != null)
            {//Update consent
                existingConsent.AdditionalData = JsonSerializer.Serialize(new
                {
                    rizaIstegi.rzBlg,
                    rizaIstegi.katilimciBlg,
                    rizaIstegi.gkd,
                    rizaIstegi.odmBsltm,
                });
                existingConsent.xGroupId = rizaIstegi.xGroupId ?? Guid.NewGuid().ToString();
                existingConsent.Description = rizaIstegi.Description;
                existingConsent.ModifiedAt = DateTime.UtcNow;
                existingConsent.State = rizaIstegi.rzBlg?.rizaDrm;
                existingConsent.ConsentType = OpenBankingConstants.ConsentType.OpenBankingYOSPayment;

                context.Consents.Update(existingConsent);
                resultData = existingConsent;
            }
            else
            {//Insert consent
                var consent = mapper.Map<Consent>(rizaIstegi);
                consent.State = rizaIstegi.rzBlg?.rizaDrm;
                consent.ConsentType = OpenBankingConstants.ConsentType.OpenBankingYOSPayment;
                consent.xGroupId = rizaIstegi.xGroupId;
                consent.Description = rizaIstegi.Description;
                consent.AdditionalData = JsonSerializer.Serialize(new
                {
                    rizaIstegi.rzBlg,
                    rizaIstegi.katilimciBlg,
                    rizaIstegi.gkd,
                    rizaIstegi.odmBsltm,
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


    private (OpenBankingTokenDto erisimToken, OpenBankingTokenDto yenilemeToken) MapTokens(List<Token> tokens,
        IMapper mapper)
    {
        var erisimToken = mapper.Map<OpenBankingTokenDto>(tokens.FirstOrDefault(t => t.TokenType == "Access Token"));
        var yenilemeToken = mapper.Map<OpenBankingTokenDto>(tokens.FirstOrDefault(t => t.TokenType == "Refresh Token"));

        return (erisimToken, yenilemeToken);
    }
}