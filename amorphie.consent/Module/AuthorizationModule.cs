using amorphie.consent.core.DTO;
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
        routeGroupBuilder.MapPost("/CheckAuthorizationForLogin/clientCode={clientCode}&roleId={roleId}&userTCKN={userTCKN}", CheckAuthorizationForLogin);
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
                           && c.OBAccountReferences.Any(r => r.LastValidAccessDate >= today))
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
    /// Check authorization for login process by integrated with contract service
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
    /// <returns>The value if user is authorized, If not, give to be approved documents</returns>
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

            Consent consent;
            Boolean isNewConsent = false;
            if (consents?.Any() ?? false)//Update consent
            {
                consent = consents.First();
                consent.State = OpenBankingConstants.RizaDurumu.YetkiBekleniyor;
                consent.ModifiedAt = DateTime.UtcNow;
                consent.StateModifiedAt = DateTime.UtcNow;
                context.Consents.Update(consent);
            }
            else//If there is no consent in db, insert consent
            {
                isNewConsent = true;
                consent = new Consent();
                consent.ScopeTCKN = scopeTCKN;
                consent.UserTCKN = userTCKN;
                consent.ConsentType = ConsentConstants.ConsentType.IBLogin;
                consent.RoleId = roleId;
                consent.ClientCode = clientCode;
                consent.State = OpenBankingConstants.RizaDurumu.YetkiBekleniyor;
                consent.AdditionalData = string.Empty;
                consent.CreatedAt = DateTime.UtcNow;
                consent.StateModifiedAt = DateTime.UtcNow;
                context.Consents.Add(consent);
            }
            //Get document list. Call constractinstance method
            InstanceRequestDto instanceRequest = new InstanceRequestDto(userTCKN.ToString(), "logindocs");
            ApiResult contractApiResult = await contractService.ContractInstance(instanceRequest);//Get data from service
            if (!contractApiResult.Result)
            {//Error in getting documents info
                return Results.BadRequest(contractApiResult.Message);
            }

            if (contractApiResult.Data == null
                || !(((InstanceResponseDto)contractApiResult.Data).document?.Any(i => i is { required: true, status: "not-started" }) ?? false))
            {//All documents approved.  Authorized user
                consent.State = OpenBankingConstants.RizaDurumu.YetkiKullanildi;
                if (isNewConsent)
                {
                    context.Consents.Add(consent);
                }
                else
                {
                    context.Consents.Update(consent);
                }

                await context.SaveChangesAsync();
                return Results.Ok(response);
            }
            InstanceResponseDto instanceResponse = (InstanceResponseDto)contractApiResult.Data;
            //Get document data for each document 
            //call templaterender method
            ApiResult renderResult;
            response.ContractDocuments = new List<ContractDocumentDto>();
            response.IsAuthorized = false;
            string templateEngineUrl = configuration["ServiceURLs:TemplateEngineRendePdfURL"] ?? string.Empty;
            foreach (var documentInfo in instanceResponse.document.Where(i => i is { required: true, status: "not-started" }))
            {
                if (documentInfo.onlineSign != null
                    && (documentInfo.onlineSign.documentModelTemplate?.Any() ?? false))
                {
                    var template = documentInfo.onlineSign.documentModelTemplate[0];
                    Guid templateRenderId = Guid.NewGuid();
                    TemplateRenderRequestDto renderRequest = new TemplateRenderRequestDto(templateRenderId, template.name, template.minVersion);
                    renderResult = await contractService.TemplateRender(renderRequest);//Render the template
                    if (renderResult.Result && renderResult.Data != null)
                    {
                        response.ContractDocuments.Add(new ContractDocumentDto()
                        {
                            FilePath = string.Format(templateEngineUrl, templateRenderId),
                            FileType = "application/pdf",
                            FileContextType = "base64",
                            FileName = $"{documentInfo.code}.pdf",
                            DocumentCode = documentInfo.code,
                            DocumentVersion = template.minVersion,
                            Reference = userTCKN.ToString(),
                            Owner = userTCKN.ToString()
                        });
                    }
                    else
                    {//Error in getting rendered file
                        return Results.BadRequest(renderResult.Message);
                    }
                }
            }
            await context.SaveChangesAsync();
            //Not authorized
            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

}