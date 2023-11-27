using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.consent.Helper;
using amorphie.core.Module.minimal_api;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace amorphie.consent.Module;

public class AuthorizationModule: BaseBBTRoute<ConsentDto, Consent, ConsentDbContext>
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
        routeGroupBuilder.MapGet("/CheckAuthorization/clientId={clientId}&userId={userId}&roleId={roleId}&scopeId={scopeId}&consentType={consentType}", CheckAuthorization);
    }

    /// <summary>
    /// Check if there is any valid consent with given parameters
    /// </summary>
    /// <param name="clientId">Client Id</param>
    /// <param name="userId">User Id</param>
    /// <param name="roleId">Role Id</param>
    /// <param name="scopeId">Scope Id</param>
    /// <param name="consentType">Consent Type</param>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="httpContext"></param>
    /// <returns>If there is any valid consent with given parameters</returns>
     public async Task<IResult> CheckAuthorization(
         Guid clientId,
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
                     c.ClientId == clientId
                     && c.UserId == userId
                     && c.RoleId == roleId
                     && c.ScopeId == scopeId
                     && c.ConsentType == consentType
                     && ((c.ConsentType == OpenBankingConstants.ConsentType.OpenBankingAccount 
                            && authAccountConsentStatusList.Contains(c.State) 
                            && c.OBAccountReferences.Any(r => r.LastValidAccessDate>= today))
                         || (c.ConsentType == OpenBankingConstants.ConsentType.OpenBankingPayment
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
}