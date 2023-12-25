using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.Contract;
using amorphie.consent.core.DTO.Contract.ContractInstance;
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
        routeGroupBuilder.MapGet("/CheckAuthorization/clientId={clientId}&userId={userId}&roleId={roleId}&scopeId={scopeId}&consentType={consentType}", CheckAuthorization);
        routeGroupBuilder.MapPost("/CheckAuthorizationForLogin/clientId={clientId}&roleId={roleId}&userTCKN={userTCKN}",CheckAuthorizationForLogin);
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
                           && c.OBAccountReferences.Any(r => r.LastValidAccessDate >= today))
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

      public async Task<IResult> CheckAuthorizationForLogin(
        Guid clientId,
        Guid roleId,
        string userTCKN,
        string scopeTCKN,
        [FromServices] ConsentDbContext context,
        [FromServices] IContractService contractService,
        [FromServices] IMapper mapper,
        HttpContext httpContext)
    {
        try
        {
            var response = new ContractResponseDto();
            //Filter consent according to parameters
            var consents = await context.Consents.AsNoTracking().Where(c =>
                    c.ClientId == clientId
                    && c.RoleId == roleId
                    && c.ScopeTCKN == scopeTCKN
                    && c.UserTCKN == userTCKN
                    && c.ConsentType == OpenBankingConstants.ConsentType.IBLogin)
                .ToListAsync();

            if (consents.Any(c => c.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi))
            {//Authorized user
                return Results.Ok(response);
            }
            //If there is no consent in db, insert consent
            //Get document list. Call constractinstance method
            InstanceRequestDto instanceRequest = new InstanceRequestDto(userTCKN);
            ApiResult contractApiResult = await contractService.ContractInstance(instanceRequest);//Get data from service
            if (!contractApiResult.Result)
            {//Error in getting documents info
                return Results.BadRequest(contractApiResult.Message);
            }

            if (contractApiResult.Data == null)
            {//All documents approved. Authorized user
                //TODO:Ozlem update consent
                return Results.Ok(response);
            }
            InstanceResponseDto instanceResponse = (InstanceResponseDto)contractApiResult.Data;
            if (instanceResponse.document?.Any() ?? false)
            {
                //Get document data for each document 
                //call templaterender method
                ApiResult renderResult;
                response.Contracts = new List<ContractDto>();
                response.IsAuthorized = false;
                foreach (var documentInfo in instanceResponse.document)
                {
                    if (documentInfo.onlineSign != null && (documentInfo.onlineSign.documentModelTemplate?.Any() ?? false))
                    {
                        var template = documentInfo.onlineSign.documentModelTemplate[0];
                        TemplateRenderRequestDto renderRequest = new TemplateRenderRequestDto(template.name,template.minVersion);
                       renderResult = await contractService.TemplateRender(renderRequest);
                       if (renderResult.Result && renderResult.Data != null)
                       {
                           response.Contracts.Add(new ContractDto()
                           {
                               FileContext = (string)renderResult.Data,
                               FileType = "pdf",
                               FileContextType = "base64",
                               FileName = documentInfo.code,
                               DocumentCode = documentInfo.code,
                               DocumentVersion = template.minVersion,
                               Reference = userTCKN,
                               Owner = userTCKN
                           });
                       }
                       else
                       {//Error in getting file
                           //TODO:Ozlem ask what to do
                       }
                    }
                   
                }
                //Not authorized
                return Results.Ok(response);
            }
            else
            {//No document. Authorized user
                //TODO:Ozlem update consent
                return Results.Ok(response);
            }
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }
}