using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.Authorization;
using amorphie.consent.core.DTO.Contract;
using amorphie.consent.core.DTO.Contract.ContractInstance;
using amorphie.consent.core.DTO.Contract.DocumentInstance;
using amorphie.consent.core.DTO.Contract.TemplateRender;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.consent.Helper;
using amorphie.consent.Service.Interface;
using amorphie.core.Module.minimal_api;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace amorphie.consent.Module;

public class AuthorizationModule : BaseBBTRoute<ConsentDto, Consent, ConsentDbContext>
{
    public AuthorizationModule(WebApplication app)
        : base(app)
    {
    }
    public override string[]? PropertyCheckList => null;
    public override string? UrlFragment => "Authorization";
    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapGet("/CheckAuthorization/clientCode={clientCode}&userId={userId}&roleId={roleId}&scopeId={scopeId}&consentType={consentType}", CheckAuthorization);
        routeGroupBuilder.MapGet("/CheckOBAuthorization/rizaNo={rizaNo}&userTCKN={userTCKN}", CheckOBAuthorization);
        routeGroupBuilder.MapPost("/CheckAuthorizationForLogin/clientCode={clientCode}&roleId={roleId}&userTCKN={userTCKN}", CheckAuthorizationForLogin);
        routeGroupBuilder.MapPost("/AuthorizeForLogin", AuthorizateForLogin);
    }

    /// <summary>
    /// Check if there is any valid consent with given parameters
    /// </summary>
    /// <param name="clientCode">ClientCode</param>
    /// <param name="userId">User Id</param>
    /// <param name="roleId">Role Id</param>
    /// <param name="scopeId">Scope Id</param>
    /// <param name="consentType">Consent Type</param>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="httpContext"></param>
    /// <returns>If there is any valid consent with given parameters</returns>
    public async Task<IResult> CheckAuthorization(
        string clientCode,
        Guid userId,
        Guid roleId,
        Guid scopeId,
        string consentType,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        HttpContext httpContext)
    {
        try
        {
            var today = DateTime.UtcNow;
            var authAccountConsentStatusList = ConstantHelper.GetAuthorizedConsentStatusListForAccount(); //Get authorized status list for account
            var authPaymentConsentStatusList = ConstantHelper.GetAuthorizedConsentStatusListForPayment(); //Get authorized status list for payment
                                                                                                          //Filter consent according to parameters
            var consents = await context.Consents.AsNoTracking().Where(c =>
                    c.ClientCode == clientCode
                    && c.UserId == userId
                    && c.RoleId == roleId
                    && c.ScopeId == scopeId
                    && c.ConsentType == consentType
                    && ((c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                           && authAccountConsentStatusList.Contains(c.State)
                           && c.OBAccountConsentDetails.Any(r => r.LastValidAccessDate >= today))
                        || (c.ConsentType == ConsentConstants.ConsentType.OpenBankingPayment
                            && authPaymentConsentStatusList.Contains(c.State))))
                .ToListAsync();
            if (consents?.Any() ?? false)
            {
                return Results.Ok();
            }
            else
            {//Not authorized
                return Results.Forbid();
            }

        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Checks if any active Open Banking authorization of rizaNo is match with given userTCKN
    /// </summary>
    /// <param name="rizaNo">Open Banking riza no</param>
    /// <param name="userTCKN">User identity number</param>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="httpContext"></param>
    /// <returns>If any open banking consent userTCKN and rizaNo matches.</returns>
    public async Task<IResult> CheckOBAuthorization(
        Guid rizaNo,
        long userTCKN,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        HttpContext httpContext)
    {
        try
        {
            var today = DateTime.UtcNow;
            var authAccountConsentStatusList = ConstantHelper.GetAuthorizedConsentStatusListForAccount(); //Get authorized status list for account
            var authPaymentConsentStatusList = ConstantHelper.GetAuthorizedConsentStatusListForPayment(); //Get authorized status list for payment
            //Filter consent according to parameters
            var consents = await context.Consents.AsNoTracking().Where(c =>
                    c.Id == rizaNo
                    && c.UserTCKN == userTCKN
                    && ((c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                         && authAccountConsentStatusList.Contains(c.State)
                         && c.OBAccountConsentDetails.Any(r => r.LastValidAccessDate >= today))
                        || (c.ConsentType == ConsentConstants.ConsentType.OpenBankingPayment
                            && authPaymentConsentStatusList.Contains(c.State))))
                .ToListAsync();
            if (consents?.Any() ?? false)
            {
                return Results.Ok();
            }
            else
            {//Not authorized
                return Results.Forbid();
            }

        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Check any yetkikullanildi state consent in system for IBLogin
    /// </summary>
    /// <param name="clientCode">Client Code</param>
    /// <param name="roleId">Role Id</param>
    /// <param name="userTCKN">Users Identity Number</param>
    /// <param name="scopeTCKN">Scope Identity Number. Same with usertckn in most cases</param>
    /// <param name="context">Context instance object</param>
    /// <param name="contractService">Contract service object</param>
    /// <param name="mapper">Mapper object</param>
    /// <param name="configuration">Configuration instance</param>
    /// <param name="httpContext">HttpContext object</param>
    /// <returns>Is any yetkikuıllanildi state consent in system</returns>
    public async Task<IResult> CheckAuthorizationForLogin(
      string clientCode,
      Guid roleId,
      long userTCKN,
      long scopeTCKN,
      [FromServices] ConsentDbContext context,
      [FromServices] IContractService contractService,
      [FromServices] IMapper mapper,
      [FromServices] IConfiguration configuration,
      HttpContext httpContext)
    {
        try
        {
            var response = new ContractResponseDto();
            //Filter consent according to parameters
            var consents = await context.Consents.AsNoTracking().Where(c =>
                    c.ClientCode == clientCode
                    && c.RoleId == roleId
                    && c.ScopeTCKN == scopeTCKN
                    && c.UserTCKN == userTCKN
                    && c.ConsentType == ConsentConstants.ConsentType.IBLogin)
                .ToListAsync();

            if (consents?.Any(c => c.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi) ?? false)
            {//Authorized user
                return Results.Ok(response);
            }
            //unauthroized
            return Results.Unauthorized();

        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }



    public async Task<IResult> AuthorizateForLogin([FromBody] SaveConsentForLoginDto saveConsent,
      [FromServices] ConsentDbContext context,
      [FromServices] IContractService contractService,
      [FromServices] IMapper mapper,
      [FromServices] IConfiguration configuration,
      HttpContext httpContext)
    {
        try
        {
            //Filter consent according to parameters
            var consents = await context.Consents.AsNoTracking().Where(c =>
                    c.ClientCode == saveConsent.ClientCode
                    && c.RoleId == saveConsent.RoleId
                    && c.ScopeTCKN == saveConsent.ScopeTCKN
                    && c.UserTCKN == saveConsent.UserTCKN
                    && c.ConsentType == ConsentConstants.ConsentType.IBLogin)
                .ToListAsync();

            if (consents?.Any(c => c.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi) ?? false)
            {//Authorized user
                return Results.Ok();
            }

            //If any yetkibekleniyor state consent, update 
            var consent = consents.FirstOrDefault(c => c.State == OpenBankingConstants.RizaDurumu.YetkiBekleniyor);
            if (consent != null)//Update consent
            {
                consent.State = OpenBankingConstants.RizaDurumu.YetkiKullanildi;
                consent.ModifiedAt = DateTime.UtcNow;
                consent.StateModifiedAt = DateTime.UtcNow;
                context.Consents.Update(consent);
            }
            else//If there is no yetkibekleniyor state consent in db, insert consent
            {
                consent = new Consent();
                consent.ScopeTCKN = saveConsent.ScopeTCKN;
                consent.UserTCKN = saveConsent.UserTCKN;
                consent.ConsentType = ConsentConstants.ConsentType.IBLogin;
                consent.RoleId = saveConsent.RoleId;
                consent.ClientCode = saveConsent.ClientCode;
                consent.State = OpenBankingConstants.RizaDurumu.YetkiKullanildi;
                consent.AdditionalData = string.Empty;
                consent.CreatedAt = DateTime.UtcNow;
                consent.StateModifiedAt = DateTime.UtcNow;
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



}