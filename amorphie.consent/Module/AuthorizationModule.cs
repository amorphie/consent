using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.Authorization;
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
        routeGroupBuilder.MapGet("/CheckOBAuthorization/rizaNo={rizaNo}&userTCKN={userTCKN}", CheckOBAuthorization);
        routeGroupBuilder.MapGet(
            "/CheckAuthorizationForLogin/clientCode={clientCode}&roleId={roleId}&userTCKN={userTCKN}&scope={scope}",
            CheckAuthorizationForLogin);
        routeGroupBuilder.MapGet(
            "/CheckConsent/clientCode={clientCode}&userTCKN={userTCKN}&scope={scope}",
            CheckConsent);
        routeGroupBuilder.MapPost("/AuthorizeForLogin", AuthorizeForLogin);
        routeGroupBuilder.MapPost("/Authorize", Authorize);
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
            var authAccountConsentStatusList =
                ConstantHelper.GetAuthorizedConsentStatusListForAccount(); //Get authorized status list for account
            var authPaymentConsentStatusList =
                ConstantHelper.GetAuthorizedConsentStatusListForPayment(); //Get authorized status list for payment
            //Filter consent according to parameters
            var consents = await context.Consents.AsNoTracking().Where(c =>
                    c.Id == rizaNo
                    && c.UserTCKN == userTCKN
                    && ((c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                         && authAccountConsentStatusList.Contains(c.State)
                         && c.LastValidAccessDate > today)
                        || (c.ConsentType == ConsentConstants.ConsentType.OpenBankingPayment
                            && authPaymentConsentStatusList.Contains(c.State))))
                .ToListAsync();
            if (consents?.Any() ?? false)
            {
                return Results.Ok();
            }
            else
            {
                //Not authorized
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
    /// <param name="scope">Scope Identity Number. Same with usertckn in most cases</param>
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
        string scope,
        [FromServices] ConsentDbContext context,
        [FromServices] IConfiguration configuration,
        HttpContext httpContext)
    {
        try
        {
            var today = DateTime.UtcNow;
            //Filter consent according to parameters
            var consents = await context.Consents.AsNoTracking().Where(c =>
                    c.ClientCode == clientCode
                    && c.RoleId == roleId
                    && c.Scope == scope
                    && c.UserTCKN == userTCKN
                    && c.ConsentType == ConsentConstants.ConsentType.IBLogin
                    && c.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi
                    && (c.LastValidAccessDate == null
                        || (c.LastValidAccessDate != null && c.LastValidAccessDate > today)))
                .ToListAsync();

            if (consents?.Any() ?? false)
            {
                //Authorized user
                return Results.Ok();
            }

            //unauthroized
            return Results.Unauthorized();
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Check any yetkikullanildi state of valid consent in system
    /// Checks clientCode,userTckn,scope
    /// </summary>
    /// <returns>Is any yetkikuıllanildi state of valid consent in system</returns>
    public async Task<IResult> CheckConsent(
        string clientCode,
        long userTCKN,
        string scope,
        [FromServices] ConsentDbContext context,
        [FromServices] IConfiguration configuration,
        HttpContext httpContext)
    {
        try
        {
            var today = DateTime.UtcNow;
            //Filter consent according to parameters
            var consents = await context.Consents.AsNoTracking().Where(c =>
                    c.ClientCode == clientCode
                    && c.Scope == scope
                    && c.UserTCKN == userTCKN
                    && c.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi
                    && (c.LastValidAccessDate == null
                        || (c.LastValidAccessDate != null && c.LastValidAccessDate > today)))
                .ToListAsync();

            if (consents?.Any() ?? false)
            {
                //Authorized user
                return Results.Ok();
            }

            //unauthroized
            return Results.Unauthorized();
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    /// <summary>
    ///  If there isn't any consent in the system,
    /// Creates yetkikullanildi state iblogin type of consent with given variables
    /// If there is already consent in system, cancels that consent with cancel detail code:01 YeniRizaTalebiIleIptal and recreate new one.
    /// </summary>
    public async Task<IResult> AuthorizeForLogin([FromBody] SaveConsentForLoginDto saveConsentForLogin,
        [FromServices] ConsentDbContext context,
        [FromServices] IConfiguration configuration,
        [FromServices] IMapper mapper,
        HttpContext httpContext)
    {
        var saveConsentDto = mapper.Map<SaveConsentDto>(saveConsentForLogin);
        saveConsentDto.ConsentType = ConsentConstants.ConsentType.IBLogin;
        return await Authorize(saveConsentDto, context, configuration, httpContext);
    }
    
    /// <summary>
    ///  If there isn't any consent in the system,
    /// Creates yetkikullanildi state of given type of consent with given variables
    /// If there is already consent in system, cancels that consent with cancel detail code:01 YeniRizaTalebiIleIptal and recreate new one.
    /// </summary>
    public async Task<IResult> Authorize([FromBody] SaveConsentDto saveConsent,
        [FromServices] ConsentDbContext context,
        [FromServices] IConfiguration configuration,
        HttpContext httpContext)
    {
        try
        {
            var checkDataResult = IsDataValidToAuthorize(saveConsent);
            if (!checkDataResult.Result)
            {
                return Results.BadRequest(checkDataResult.Message);
            }

            var today = DateTime.UtcNow;
            //Filter consent according to parameters
            var consents = await context.Consents.AsNoTracking().Where(c =>
                    c.ClientCode == saveConsent.ClientCode
                    && c.RoleId == saveConsent.RoleId
                    && c.Scope == saveConsent.Scope
                    && c.UserTCKN == saveConsent.UserTCKN
                    && c.ConsentType == saveConsent.ConsentType)
                .ToListAsync();

            //Valid date ended consents
            var outDatedConsents = consents.Where(c => c.LastValidAccessDate != null
                                                       && c.LastValidAccessDate <= today
                                                       && c.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi)
                .ToList();
            if (outDatedConsents.Any())
            {
                //End consents
                outDatedConsents = outDatedConsents.Select(c =>
                {
                    c.State = OpenBankingConstants.RizaDurumu.YetkiSonlandirildi;
                    c.StateModifiedAt = today;
                    return c;
                }).ToList();
                context.Consents.UpdateRange(outDatedConsents);
            }

            var toBeCancelledConsents =
                consents.Where(c => c.State != OpenBankingConstants.RizaDurumu.YetkiSonlandirildi
                                    && c.State != OpenBankingConstants.RizaDurumu.YetkiIptal)
                    .ToList();
            if (toBeCancelledConsents.Any())
            {
                //End consents
                toBeCancelledConsents = toBeCancelledConsents.Select(c =>
                {
                    c.State = OpenBankingConstants.RizaDurumu.YetkiIptal;
                    c.StateCancelDetailCode = OpenBankingConstants.RizaIptalDetayKodu.YeniRizaTalebiIleIptal;
                    c.StateModifiedAt = today;
                    return c;
                }).ToList();
                context.Consents.UpdateRange(toBeCancelledConsents);
            }

            //Create desired consent
            var consent = new Consent
            {
                Scope = saveConsent.Scope,
                UserTCKN = saveConsent.UserTCKN,
                ConsentType = saveConsent.ConsentType,
                RoleId = saveConsent.RoleId,
                ClientCode = saveConsent.ClientCode,
                State = OpenBankingConstants.RizaDurumu.YetkiKullanildi,
                LastValidAccessDate = saveConsent.LastValidAccessDate,
                AdditionalData = string.Empty,
                CreatedAt = DateTime.UtcNow,
                StateModifiedAt = DateTime.UtcNow
            };
            context.Consents.Add(consent);

            await context.SaveChangesAsync();
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }
    
    private ApiResult IsDataValidToAuthorize(SaveConsentDto saveConsent)
    {
        ApiResult result = new();

        //Check consent type
        if (string.IsNullOrEmpty(saveConsent.ConsentType)
            || !ConstantHelper.GetConsentTypeList().Contains(saveConsent.ConsentType))
        {
            result.Result = false;
            result.Message = "Consent type is not valid.";
            return result;
        }
        var today = DateTime.UtcNow;
        if (saveConsent.LastValidAccessDate.HasValue
            && saveConsent.LastValidAccessDate < today)
        {
            result.Result = false;
            result.Message = "LastValidAccessDate is not valid.";
            return result;
        }
        return result;
    }
}