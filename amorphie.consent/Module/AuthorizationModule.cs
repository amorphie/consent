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
            "/CheckAuthorizationForLogin/clientCode={clientCode}&roleId={roleId}&userTCKN={userTCKN}&scopeTCKN={scopeTCKN}",
            CheckAuthorizationForLogin);
        routeGroupBuilder.MapGet(
            "/CheckConsent/clientCode={clientCode}&userTCKN={userTCKN}&scopeTCKN={scopeTCKN}",
            CheckConsent);
        routeGroupBuilder.MapPost("/AuthorizeForLogin", AuthorizeForLogin);
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
                    && c.ScopeTCKN == scopeTCKN
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
    /// Checks clientCode,userTckn,scopeTckn
    /// </summary>
    /// <returns>Is any yetkikuıllanildi state of valid consent in system</returns>
    public async Task<IResult> CheckConsent(
        string clientCode,
        long userTCKN,
        long scopeTCKN,
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
                    && c.ScopeTCKN == scopeTCKN
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
    ///  If there isn't any in the system, Creates yetkikullanildi state iblogin type of consent with given variables
    /// </summary>
    /// <param name="saveConsent">Post data</param>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public async Task<IResult> AuthorizeForLogin([FromBody] SaveConsentForLoginDto saveConsent,
        [FromServices] ConsentDbContext context,
        [FromServices] IConfiguration configuration,
        HttpContext httpContext)
    {
        try
        {
            var today = DateTime.UtcNow;
            //Filter consent according to parameters
            var consents = await context.Consents.AsNoTracking().Where(c =>
                    c.ClientCode == saveConsent.ClientCode
                    && c.RoleId == saveConsent.RoleId
                    && c.ScopeTCKN == saveConsent.ScopeTCKN
                    && c.UserTCKN == saveConsent.UserTCKN
                    && c.ConsentType == ConsentConstants.ConsentType.IBLogin)
                .ToListAsync();

            //Valid date ended consents
            var outDatedConsents = consents.Where(c => c.LastValidAccessDate != null
                                                       && c.LastValidAccessDate <= today
                                                       && c.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi)
                .ToList();
            if (outDatedConsents?.Any() ?? false)
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
                && c.State != OpenBankingConstants.RizaDurumu.YetkiIptal).ToList();
            if (toBeCancelledConsents?.Any() ?? false)
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
                ScopeTCKN = saveConsent.ScopeTCKN,
                UserTCKN = saveConsent.UserTCKN,
                ConsentType = ConsentConstants.ConsentType.IBLogin,
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
}