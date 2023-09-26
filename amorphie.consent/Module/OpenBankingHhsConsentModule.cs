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

public class OpenBankingHHSConsentModule : BaseBBTRoute<OpenBankingConsentDTO, Consent, ConsentDbContext>
{
    public OpenBankingHHSConsentModule(WebApplication app)
        : base(app) { }

    public override string[]? PropertyCheckList => new string[] { "ConsentType", "State" };

    public override string? UrlFragment => "OpenBankingConsentHHS";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapGet("/search", SearchMethod);
        routeGroupBuilder.MapPost("/UpdatePaymentConsentStatus/{consentId}/{status}", UpdatePaymentConsentStatus);
        routeGroupBuilder.MapPost("/UpdatePaymentConsentForAuthorization", UpdatePaymentConsentForAuthorization);
        routeGroupBuilder.MapPost("/hesap-bilgisi-rizasi", AccountInformationConsentPost);
        routeGroupBuilder.MapPost("/odeme-emri-rizasi", PaymentInformationConsentPost);
        routeGroupBuilder.MapGet("/hesap-bilgisi-rizasi/{consentId}", GetAccountConsentById);
        routeGroupBuilder.MapGet("/odeme-emri-rizasi/{consentId}", GetPaymentConsentById);
        //TODO:Ozlem /odeme-emri/{odemeEmriNo} bu metod eklenecek
    }
    //hhs bizim bankamizi acacaklar. UI web ekranlarimiz


    #region HHS

    /// <summary>
    /// Get consent additional data by Id casting to HesapBilgisiRizaIstegiDto type of object
    /// </summary>
    /// <param name="consentId"></param>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <returns>HesapBilgisiRizaIstegiDto type of object</returns>
    public async Task<IResult> GetAccountConsentById(
     Guid consentId,
     [FromServices] ConsentDbContext context,
     [FromServices] IMapper mapper)
    {
        try
        {
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == consentId);
            var serializedData = JsonSerializer.Deserialize<HesapBilgisiRizaIstegiDto>(entity.AdditionalData);
            serializedData!.Id = entity.Id;
            serializedData.UserId = entity.UserId;
            // var hhsConsentDTO = mapper.Map<HesapBilgisiRizaIstegiResponse>(serializedData);

            return Results.Ok(serializedData);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Get consent additional data by Id casting to OdemeEmriRizaIstegiDto type of object
    /// </summary>
    /// <param name="consentId"></param>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <returns>OdemeEmriRizaIstegiDto type of object</returns>
    public async Task<IResult> GetPaymentConsentById(Guid consentId,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper)
    {
        try
        {
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == consentId);
            var serializedData = JsonSerializer.Deserialize<OdemeEmriRizaIstegiDto>(entity.AdditionalData);
            serializedData!.Id = entity.Id;
            serializedData.UserId = entity.UserId;

            return Results.Ok(serializedData);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    protected async Task<IResult> UpdatePaymentConsentStatus(Guid id,
        string state,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper)
    {
        var resultData = new Consent();
        try
        {

            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == id);
            if (entity == null)
            {
                return Results.BadRequest();
            }

            var additionalData = JsonSerializer.Deserialize<OdemeEmriRizaIstegiDto>(entity.AdditionalData);
            additionalData.rzBlg.rizaDrm = state;
            entity.AdditionalData = JsonSerializer.Serialize(additionalData);
            entity.ModifiedAt = DateTime.UtcNow;
            entity.State = state;

            context.Consents.Update(entity);
            await context.SaveChangesAsync();
            return Results.Ok(resultData);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    protected async Task<IResult> UpdatePaymentConsentForAuthorization([FromBody] UpdatePCForAuthorizationDto savePCStatusSenderAccount,
      [FromServices] ConsentDbContext context,
      [FromServices] IMapper mapper)
    {
        var resultData = new Consent();
        try
        {

            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == savePCStatusSenderAccount.Id);
            if (entity == null)
            {
                return Results.BadRequest();
            }

            var additionalData = JsonSerializer.Deserialize<OdemeEmriRizaIstegiDto>(entity.AdditionalData);
            //Check if sender account is already selected
            bool isSenderAccountSet = string.IsNullOrEmpty(additionalData.odmBsltm.gon.hspNo) || string.IsNullOrEmpty(additionalData.odmBsltm.gon.hspRef);
            if (!isSenderAccountSet
                && savePCStatusSenderAccount.SenderAccount == null)
            {
                return Results.BadRequest();
            }
            additionalData.rzBlg.rizaDrm = savePCStatusSenderAccount.State;
            if (!isSenderAccountSet)
            {
                additionalData.odmBsltm.gon = savePCStatusSenderAccount.SenderAccount;
            }

            entity.AdditionalData = JsonSerializer.Serialize(additionalData);
            entity.ModifiedAt = DateTime.UtcNow;
            entity.State = savePCStatusSenderAccount.State;

            context.Consents.Update(entity);
            await context.SaveChangesAsync();
            return Results.Ok(resultData);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
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


    #endregion


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
