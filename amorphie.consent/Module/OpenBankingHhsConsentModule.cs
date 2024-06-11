using System.Globalization;
using System.Net;
using amorphie.core.Module.minimal_api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using amorphie.core.Swagger;
using Microsoft.OpenApi.Models;
using amorphie.consent.data;
using amorphie.consent.core.Model;
using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.Helper;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;
using Dapr;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace amorphie.consent.Module;

public class OpenBankingHHSConsentModule : BaseBBTRoute<OpenBankingConsentDto, Consent, ConsentDbContext>
{
    private IOBErrorCodeDetailService _errorCodeDetailService;
    private List<OBErrorCodeDetail> _errorCodeDetails;
    public OpenBankingHHSConsentModule(WebApplication app)
     : base(app)
    {
        InitializeErrorCodeDetails(app); // Get error code details
    }

    private async void InitializeErrorCodeDetails(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        // Resolve the IOBErrorCodeDetailService dependency from the service provider
        _errorCodeDetailService = scope.ServiceProvider.GetRequiredService<IOBErrorCodeDetailService>();
        _errorCodeDetails = await _errorCodeDetailService.GetErrorCodeDetailsAsync();
    }

    public override string[] PropertyCheckList => new[] { "ConsentType", "State" };

    public override string UrlFragment => "OpenBankingConsentHHS";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapGet("/GetAuthorizedAccountConsents", GetAuthorizedAccountConsentsByUserTCKN);
        routeGroupBuilder.MapGet("/GetInstitutionAuthorizedAccountConsents", GetInstitutionAuthorizedAccountConsents);
        routeGroupBuilder.MapGet("/GetConsentWebViewInfo/{rizaNo}", GetConsentWebViewInfo);
        routeGroupBuilder.MapGet("/hesap-bilgisi-rizasi/{rizaNo}", GetAccountConsentById)
            .AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapGet("/odeme-emri-rizasi/{rizaNo}", GetPaymentConsentById)
            .AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapGet("/odeme-emri/{odemeEmriNo}", GetPaymentOrderConsentById)
            .AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapGet("/GetAccountConsentById/{rizaNo}", GetAccountConsentByIdForUI);
        routeGroupBuilder.MapGet("/GetPaymentConsentById/{rizaNo}", GetPaymentConsentByIdForUI);
        routeGroupBuilder.MapGet("/hesaplar", GetAuthorizedAccounts).AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapGet("/hesaplar/{hspRef}", GetAuthorizedAccountByHspRef)
            .AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapGet("/bakiye", GetAuthorizedBalances)
            .AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapGet("/hesaplar/{hspRef}/bakiye", GetAuthorizedBalanceByHspRef)
            .AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapGet("/hesaplar/{hspRef}/islemler", GetTransactionsByHspRef)
            .AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapDelete("/hesap-bilgisi-rizasi/{rizaNo}", DeleteAccountConsentFromYos)
            .AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapDelete("/DeleteAccountConsentFromHHS/{rizaNo}", DeleteAccountConsentFromHHS);
        routeGroupBuilder.MapDelete("/DeleteAccountConsentFromHHSInstitution", DeleteAccountConsentFromHHSInstitution);
        routeGroupBuilder.MapPost("/hesap-bilgisi-rizasi", AccountInformationConsentPost)
            .AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapPost("/odeme-emri-rizasi", PaymentConsentPost)
            .AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapPost("/UpdateAccountConsentForAuthorization", UpdateAccountConsentForAuthorization);
        routeGroupBuilder.MapPost("/UpdatePaymentConsentForAuthorization", UpdatePaymentConsentForAuthorization);
        routeGroupBuilder.MapPost("/UpdateConsentStatusForUsage", UpdateConsentStatusForUsage);
        routeGroupBuilder.MapDelete("/Cancel", CancelConsent);
        routeGroupBuilder.MapPost("odeme-emri", PaymentOrderPost).AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapPost("PaymentStateChanged", PaymentStateChanged);
        routeGroupBuilder.MapPost("BalanceChanged", BalanceChanged);
    }

    //hhs bizim bankamizi acacaklar. UI web ekranlarimiz


    #region HHS

    /// <summary>
    /// Get users account consents from all yos
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="mapper"></param>
    /// <param name="accountService"></param>
    /// <param name="configuration"></param>
    /// <param name="yosInfoService"></param>
    /// <param name="httpContext"></param>
    /// <returns>Account consent list of user</returns>
    [AddSwaggerParameter("user_reference", ParameterLocation.Header, true)]
    public async Task<IResult> GetAuthorizedAccountConsentsByUserTCKN(
        [FromServices] ConsentDbContext dbContext,
        [FromServices] IMapper mapper,
        [FromServices] IAccountService accountService,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        [FromServices] IOBAuthorizationService authorizationService,
        HttpContext httpContext)
    {
        try
        {
            var header = OBModuleHelper.GetHeader(httpContext);
            if (string.IsNullOrEmpty(header.UserReference))
            {
                //Missing header fields
                return Results.BadRequest("Header user_reference can not be empty");
            }

            string userTCKN = header.UserReference; //get logged in user tckn
            ApiResult getConsentsResponse = await authorizationService.GetAuthUsedAccountConsentsOfUser(userTCKN);
            if (!getConsentsResponse.Result)
            {
                //Error in getting consents
                return Results.Problem(getConsentsResponse.Message);
            }

            var userAccountConsents = (List<Consent>?)getConsentsResponse.Data;
            if (!(userAccountConsents?.Any() ?? false))
            {
                //No authorized account consent in the system
                //MObile ekip NoContenti yakalayamadığı için Ok ve boş liste dönüldü
                return Results.Ok(new List<ListAccountConsentDto>());
            }

            //Get consent details
            var consentDetails = await GetAccountConsentDetails(userTCKN, userAccountConsents, dbContext, mapper, accountService);
            return Results.Ok(consentDetails);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    public async Task<IResult> GetInstitutionAuthorizedAccountConsents(
        string customerNumber,
        string institutionCustomerNumber,
        [FromServices] ConsentDbContext dbContext,
        [FromServices] IMapper mapper,
        [FromServices] IAccountService accountService,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        [FromServices] IOBAuthorizationService authorizationService,
        HttpContext httpContext)
    {
        try
        {
            if (string.IsNullOrEmpty(customerNumber) || string.IsNullOrEmpty(institutionCustomerNumber))
            {
                return Results.BadRequest("customerNumber and institutionCustomerNumber can not be empty");
            }

            var getConsentsResponse = await authorizationService
                                            .GetAuthUsedAccountConsentsOfInstitutionUser(customerNumber, institutionCustomerNumber);

            if (!getConsentsResponse.Result)
            {
                //Error in getting consents
                return Results.Problem(getConsentsResponse.Message);
            }

            var userAccountConsents = (List<Consent>?)getConsentsResponse.Data;
            if (!(userAccountConsents?.Any() ?? false))
            {//NO consent in system
                return Results.NoContent();
            }

            //Get consent details
            var consentDetails = await GetAccountConsentDetails
                                    (userAccountConsents[0].UserTCKN.ToString()!, userAccountConsents, dbContext, mapper, accountService);
            return Results.Ok(consentDetails);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    /// <summary>
    /// Get account consent additional data by rizano- consentId casting to HesapBilgisiRizasiHHSDto type of object
    /// </summary>
    /// <returns>HesapBilgisiRizasiHHSDto type of object</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    public async Task<IResult> GetAccountConsentById(
        Guid rizaNo,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            var header = OBModuleHelper.GetHeader(httpContext);
            //Check header fields
            ApiResult headerValidation = await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, errorCodeDetails: _errorCodeDetails, header: header);
            if (!headerValidation.Result)
            {
                //Missing header fields
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, headerValidation.Data);
                //Data not valid
                return Results.Content(headerValidation.Data.ToJsonString(),"application/json", statusCode: HttpStatusCode.BadRequest.GetHashCode());
            }

            //Check consent
            await ProcessAccountConsentToCancelOrEnd(rizaNo, context);
            var entity = await context.Consents
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == rizaNo
                                          && c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                                          && c.OBAccountConsentDetails.Any(d => d.YosCode == header.XTPPCode));

            if (entity == null
                || string.IsNullOrEmpty(entity.AdditionalData)) //No desired consent in system
            {
                var errorResponse = OBErrorResponseHelper.GetNotFoundError(httpContext, _errorCodeDetails,
                     OBErrorCodeConstants.ErrorCodesEnum.NotFound);
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, errorResponse);
                return Results.NotFound(errorResponse);
            }

            var accountConsent = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity.AdditionalData);
            OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, accountConsent);
            return Results.Content(accountConsent.ToJsonString(),"application/json" ,statusCode: HttpStatusCode.OK.GetHashCode());
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    /// <summary>
    /// Get account type consent by Id casting to HHSAccountConsentDto type of object
    /// </summary>
    /// <param name="rizaNo"></param>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <returns>HHSAccountConsentDto type of object</returns>
    public async Task<IResult> GetAccountConsentByIdForUI(
        Guid rizaNo,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper)
    {
        try
        {
            //Check consent
            await ProcessAccountConsentToCancelOrEnd(rizaNo, context);
            //Get entity from db
            var entity = await context.Consents
                .AsNoTracking()
                .Include(c => c.OBAccountConsentDetails)
                .FirstOrDefaultAsync(c => c.Id == rizaNo
                                          && c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount);
            var accountConsent = mapper.Map<HHSAccountConsentDto>(entity);
            return Results.Ok(accountConsent);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    /// <summary>
    /// Get account information from service with hspref 
    /// </summary>
    /// <param name="hspRef">Hesap ref</param>
    /// <param name="context">Context DB object</param>
    /// <param name="mapper">Aoutomapper object</param>
    /// <param name="accountService">Account service class</param>
    /// <param name="configuration">Configuration object</param>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <param name="httpContext">Httpcontext object</param>
    /// <returns>account information of hspref - HesapBilgileriDto type of object</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    [AddSwaggerParameter("user_reference", ParameterLocation.Header, true)]
    [AddSwaggerParameter("openbanking_consent_id", ParameterLocation.Header, true)]
    public async Task<IResult> GetAuthorizedAccountByHspRef(
        string hspRef,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IAccountService accountService,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            var header = OBModuleHelper.GetHeader(httpContext);
            //Check header fields
            ApiResult headerValidation = await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, header,
                isUserRequired: true, isConsentIdRequired: true, errorCodeDetails: _errorCodeDetails);
            if (!headerValidation.Result)
            {
                //Missing header fields
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, headerValidation.Data);
                return Results.BadRequest(headerValidation.Data);
            }

            ApiResult accountApiResult =
                await accountService.GetAuthorizedAccountByHspRef(httpContext, header.UserReference!, header.ConsentId!, yosCode: header.XTPPCode,
                    hspRef, _errorCodeDetails); //Get data from service
            if (!accountApiResult.Result)
            {
                return Results.BadRequest(accountApiResult.Message);
            }

            return Results.Ok(accountApiResult.Data);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    /// <summary>
    /// Get authorized accounts from service 
    /// </summary>
    /// <param name="syfKytSayi"></param>
    /// <param name="syfNo"></param>
    /// <param name="srlmKrtr"></param>
    /// <param name="srlmYon"></param>
    /// <param name="context">Context DB object</param>
    /// <param name="mapper">Automapper object</param>
    /// <param name="accountService">Account service class</param>
    /// <param name="configuration">Configuration object</param>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <param name="httpContext">Httpcontext object</param>
    /// <returns>Account list of customer -  List of HesapBilgileriDto type of objects</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    [AddSwaggerParameter("user_reference", ParameterLocation.Header, true)]
    [AddSwaggerParameter("openbanking_consent_id", ParameterLocation.Header, true)]
    public async Task<IResult> GetAuthorizedAccounts([FromQuery] int? syfKytSayi,
        [FromQuery] int? syfNo,
        [FromQuery] string? srlmKrtr,
        [FromQuery] string? srlmYon,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IAccountService accountService,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            var header = OBModuleHelper.GetHeader(httpContext);
            //Check header fields
            ApiResult headerValidation = await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, header,
                isUserRequired: true, isConsentIdRequired: true, errorCodeDetails: _errorCodeDetails);
            if (!headerValidation.Result)
            {
                //Missing header fields
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, headerValidation.Data);
                return Results.BadRequest(headerValidation.Data);
            }

            //Get authorized accounts
            ApiResult accountApiResult =
                await accountService.GetAuthorizedAccounts(httpContext, userTCKN: header.UserReference!, consentId: header.ConsentId!, header.XTPPCode, _errorCodeDetails, syfKytSayi, syfNo,
                    srlmKrtr, srlmYon);
            if (!accountApiResult.Result)
            {
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, accountApiResult.Data);
                return Results.BadRequest(accountApiResult.Data);
            }
            return Results.Ok(accountApiResult.Data);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    /// <summary>
    /// Get account's balance information from service with hspref 
    /// </summary>
    /// <param name="hspRef">Hesap ref</param>
    /// <param name="context">Context DB object</param>
    /// <param name="mapper">Aoutomapper object</param>
    /// <param name="accountService">Account service class</param>
    /// <param name="configuration">Configuration object</param>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <param name="httpContext">Httpcontext object</param>
    /// <returns>account balance information of hspref - BakiyeBilgileriDto type of object</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    [AddSwaggerParameter("user_reference", ParameterLocation.Header, true)]
    [AddSwaggerParameter("openbanking_consent_id", ParameterLocation.Header, true)]
    public async Task<IResult> GetAuthorizedBalanceByHspRef(
        string hspRef,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IAccountService accountService,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            var header = OBModuleHelper.GetHeader(httpContext);
            //Check header fields
            ApiResult headerValidation = await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, header,
                isUserRequired: true, isConsentIdRequired: true, errorCodeDetails: _errorCodeDetails);
            if (!headerValidation.Result)
            {
                //Missing header fields
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, headerValidation.Data);
                return Results.BadRequest(headerValidation.Data);
            }

            ApiResult accountApiResult =
                await accountService.GetAuthorizedBalanceByHspRef(httpContext, userTCKN: header.UserReference!, yosCode: header.XTPPCode,
                   hspRef: hspRef, consentId: header.ConsentId!, errorCodeDetails: _errorCodeDetails); //Get data from service
            if (!accountApiResult.Result)
            {
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, accountApiResult.Data);
                return Results.BadRequest(accountApiResult.Data);
            }

            return Results.Ok(accountApiResult.Data);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    /// <summary>
    /// Get all balances from service, filter by authorized accounts
    /// </summary>
    /// <param name="syfKytSayi">Page size</param>
    /// <param name="syfNo">Page Nunber</param>
    /// <param name="srlmKrtr">Sorting By</param>
    /// <param name="srlmYon">Order By</param>
    /// <param name="context">Context DB object</param>
    /// <param name="mapper">Aoutomapper object</param>
    /// <param name="accountService">Account service class</param>
    /// <param name="configuration">Configuration instance</param>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <param name="httpContext">Httpcontext object</param>
    /// <returns>Balance list of customer -  List of BakiyeBilgileriDto type of objects</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    [AddSwaggerParameter("user_reference", ParameterLocation.Header, true)]
    [AddSwaggerParameter("openbanking_consent_id", ParameterLocation.Header, true)]
    public async Task<IResult> GetAuthorizedBalances([FromQuery] int? syfKytSayi,
        [FromQuery] int? syfNo,
        [FromQuery] string? srlmKrtr,
        [FromQuery] string? srlmYon,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IAccountService accountService,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            var header = OBModuleHelper.GetHeader(httpContext);
            //Check header fields
            ApiResult headerValidation = await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, header,
                isUserRequired: true, isConsentIdRequired: true, errorCodeDetails: _errorCodeDetails);
            if (!headerValidation.Result)
            {
                //Missing header fields
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, headerValidation.Data);
                return Results.BadRequest(headerValidation.Data);
            }

            ApiResult accountApiResult = await accountService.GetAuthorizedBalances(httpContext, userTCKN: header.UserReference!,
             consentId: header.ConsentId!, yosCode: header.XTPPCode, _errorCodeDetails, syfKytSayi, syfNo, srlmKrtr, srlmYon);
            if (!accountApiResult.Result)
            {
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, accountApiResult.Data);
                return Results.BadRequest(accountApiResult.Data);
            }
            return Results.Ok(accountApiResult.Data);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Get transactions from service with hspref 
    /// </summary>
    /// <returns>account transactions- IslemBilgileriDto type of object</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    [AddSwaggerParameter("user_reference", ParameterLocation.Header, true)]
    [AddSwaggerParameter("openbanking_consent_id", ParameterLocation.Header, true)]
    public async Task<IResult> GetTransactionsByHspRef(
        string hspRef,
        [FromQuery] DateTime hesapIslemBslTrh,
        [FromQuery] DateTime hesapIslemBtsTrh,
        [FromQuery] string? minIslTtr,
        [FromQuery] string? mksIslTtr,
        [FromQuery] string? brcAlc,
        [FromQuery] int? syfKytSayi,
        [FromQuery] int? syfNo,
        [FromQuery] string? srlmKrtr,
        [FromQuery] string? srlmYon,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IAccountService accountService,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            var header = OBModuleHelper.GetHeader(httpContext);
            //Check header fields
            ApiResult headerValidation = await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, header,
                isUserRequired: true, isConsentIdRequired: true, errorCodeDetails: _errorCodeDetails);
            if (!headerValidation.Result)
            {
                //Missing header fields
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, headerValidation.Data);
                return Results.BadRequest(headerValidation.Data);
            }

            //Get transactions from service
            ApiResult accountApiResult = await accountService.GetTransactionsByHspRef(httpContext, userTCKN: header.UserReference!,
              consentId: header.ConsentId!, yosCode: header.XTPPCode, _errorCodeDetails,
                hspRef, header.PSUInitiated, hesapIslemBslTrh, hesapIslemBtsTrh, minIslTtr, mksIslTtr, brcAlc,
                syfKytSayi, syfNo,
                srlmKrtr, srlmYon);
            if (!accountApiResult.Result)
            {
                //Error in service
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, accountApiResult.Data);
                return Results.BadRequest(accountApiResult.Data);
            }

            return Results.Ok(accountApiResult.Data);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    /// <summary>
    /// Get consent additional data by Id casting to OdemeEmriRizasiHHSDto type of object
    /// </summary>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    public async Task<IResult> GetPaymentConsentById(Guid rizaNo,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] ITokenService tokenService,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            var header = OBModuleHelper.GetHeader(httpContext);
            //Check header fields
            ApiResult headerValidation = await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, errorCodeDetails: _errorCodeDetails, header: header);
            if (!headerValidation.Result)
            {
                //Missing header fields
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, headerValidation.Data);
                return Results.BadRequest(headerValidation.Data);
            }

            //Check consent
            await ProcessPaymentConsentToCancelOrEnd(rizaNo, context, tokenService);
            var entity = await context.Consents
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == rizaNo
                                          && c.ConsentType == ConsentConstants.ConsentType.OpenBankingPayment
                                          && c.OBPaymentConsentDetails.Any(d => d.YosCode == header.XTPPCode));
            if (entity == null
                || string.IsNullOrEmpty(entity.AdditionalData)) //No desired consent in system
            {
                var errorResponse = OBErrorResponseHelper.GetNotFoundError(httpContext, _errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.NotFound);
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, errorResponse);
                return Results.NotFound(errorResponse);
            }

            var paymentConsent = JsonSerializer.Deserialize<OdemeEmriRizasiHHSDto>(entity.AdditionalData);
            OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, paymentConsent);
            return Results.Ok(paymentConsent);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    /// <summary>
    /// Get odeme emri rizası consent
    /// </summary>
    /// <returns>OdemeEmriHHSDto type of object</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    [AddSwaggerParameter("user_reference", ParameterLocation.Header, true)]
    [AddSwaggerParameter("openbanking_consent_id", ParameterLocation.Header, true)]
    public async Task<IResult> GetPaymentOrderConsentById(Guid odemeEmriNo,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        [FromServices] IPushService pushService,
        HttpContext httpContext)
    {
        try
        {
            var header = OBModuleHelper.GetHeader(httpContext);
            //Check header fields
            ApiResult headerValidation = await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, header,
                isUserRequired: true);
            if (!headerValidation.Result)
            {
                //Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }

            var entity = await context.OBPaymentOrders
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == odemeEmriNo
                && c.ConsentId.ToString() == header.ConsentId
                && c.YosCode == header.XTPPCode);


            if (entity == null
                || string.IsNullOrEmpty(entity.AdditionalData)) //No desired consent in system
            {
                var errorResponse = OBErrorResponseHelper.GetNotFoundError(httpContext, _errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.NotFound);
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, errorResponse);
                return Results.NotFound(errorResponse);
            }

            var serializedData = JsonSerializer.Deserialize<OdemeEmriHHSDto>(entity.AdditionalData);
            OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, serializedData);
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
    /// <param name="rizaNo"></param>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="tokenService"></param>
    /// <returns>OdemeEmriRizaIstegiDto type of object</returns>
    public async Task<IResult> GetPaymentConsentByIdForUI(Guid rizaNo,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] ITokenService tokenService)
    {
        try
        {
            //Check consent
            await ProcessPaymentConsentToCancelOrEnd(rizaNo, context, tokenService);
            //Get entity from db
            var entity = await context.Consents
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == rizaNo
                                          && c.ConsentType == ConsentConstants.ConsentType.OpenBankingPayment);
            var paymentConsent = mapper.Map<HHSPaymentConsentDto>(entity);
            return Results.Ok(paymentConsent);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    [AddSwaggerParameter("user_reference", ParameterLocation.Header, true)]
    public async Task<IResult> GetConsentWebViewInfo(
        Guid rizaNo,
        [FromServices] ConsentDbContext dbContext,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        [FromServices] IOBAuthorizationService authorizationService,
        [FromServices] ITokenService tokenService,
        HttpContext httpContext)
    {
        try
        {
            var header = OBModuleHelper.GetHeader(httpContext);
            if (string.IsNullOrEmpty(header.UserReference))
            {
                //Missing header fields
                return Results.BadRequest("Header user_reference can not be empty");
            }

            //Check consent
            await ProcessConsentToCancelOrEnd(rizaNo, dbContext, tokenService);

            string userTCKN = header.UserReference; //get logged in user tckn
            List<string> consentTypes = new List<string>()
                { ConsentConstants.ConsentType.OpenBankingAccount, ConsentConstants.ConsentType.OpenBankingPayment };
            var consentState = OpenBankingConstants.RizaDurumu.YetkiBekleniyor;
            //Get consent
            var getConsentResult = await authorizationService.GetConsentReadonly(id: rizaNo, userTckn: userTCKN,
                consentTypes: consentTypes);

            if (!getConsentResult.Result)
            {
                //Error in getting consents
                return Results.Problem(getConsentResult.Message);
            }

            var consent = (Consent?)getConsentResult.Data;
            if (consent == null)
            {//No consent in system
                return new CustomStatusCodeResult(454, "Invalid process. There is no consent in the system.");
            }

            if (consent.State != consentState)
            {//Consent durumu uygun değil.
                if (consent.State == OpenBankingConstants.RizaDurumu.YetkiIptal)
                {
                    return new CustomStatusCodeResult(454, "Consent is cancelled. It should be completed within 5 minutes. Try again.");
                }
                return new CustomStatusCodeResult(454, "Invalid consent to process. Try again.");
            }
            //Generate response
            ConsentWebViewInfoDto response = new()
            {
                RizaNo = consent.Id,
                ConsentType = consent.ConsentType,
                ForwardingUrl = GetConsentForwardingAddress(consent.ConsentType, consent.Id, configuration)
            };
            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    private string GetConsentForwardingAddress(string consentType, Guid id, IConfiguration configuration)
    {
        string url = string.Empty;
        //Set forwarding address according to consent type
        if (consentType == ConsentConstants.ConsentType.OpenBankingAccount)
        {
            url = string.Format(configuration["OB_AccountProjectURL"] ?? string.Empty, id);
        }
        else if (consentType == ConsentConstants.ConsentType.OpenBankingPayment)
        {
            url = string.Format(configuration["OB_PaymentProjectURL"] ?? string.Empty, id);
        }

        return url;
    }


    /// <summary>
    /// Updates consent state for authorization usage
    /// </summary>
    /// <param name="updateConsentState">To be updated consent data</param>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="tokenService"></param>
    /// <returns></returns>
    public async Task<IResult> UpdateConsentStatusForUsage([FromBody] UpdateConsentStateDto updateConsentState,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] ITokenService tokenService)
    {
        try
        {
            //Get entity from db
            var entity = await context.Consents
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == updateConsentState.Id);
            if (entity == null)
            {
                return Results.NoContent();
            }

            if (entity.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount)
            {
                //Account consent
                return await UpdateAccountConsentStatusForUsage(updateConsentState, context);
            }
            if (entity.ConsentType == ConsentConstants.ConsentType.OpenBankingPayment)
            {
                //Payment consent
                return await UpdatePaymentConsentStatusForUsage(updateConsentState, context, tokenService);
            }
            //Not related type
            return Results.BadRequest("Consent type not valid");
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates consent state for authorization usage
    /// </summary>
    /// <param name="updateConsentState">To be updated consent data</param>
    /// <param name="context"></param>
    /// <param name="tokenService"></param>
    /// <returns></returns>
    private async Task<IResult> UpdatePaymentConsentStatusForUsage(UpdateConsentStateDto updateConsentState,
        ConsentDbContext context,
        ITokenService tokenService)
    {
        try
        {
            //Check consent
            await ProcessPaymentConsentToCancelOrEnd(updateConsentState.Id, context, tokenService);
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == updateConsentState.Id
                                          && c.ConsentType == ConsentConstants.ConsentType.OpenBankingPayment);
            //Check consent validity
            ApiResult isDataValidResult = IsDataValidToUpdatePaymentConsentStatusForUsage(entity, updateConsentState);
            if (!isDataValidResult.Result) //Error in data validation
            {
                return Results.BadRequest(isDataValidResult.Message);
            }

            //Set permissions
            var additionalData = JsonSerializer.Deserialize<OdemeEmriRizasiHHSDto>(entity!.AdditionalData);
            if (additionalData == null)
            {
                return Results.BadRequest("Payment consent data can not be deserialized.");
            }
            additionalData.rzBlg.rizaDrm = updateConsentState.State;
            additionalData.rzBlg.gnclZmn = DateTime.UtcNow;
            entity.AdditionalData = JsonSerializer.Serialize(additionalData);
            entity.State = updateConsentState.State;
            entity.StateModifiedAt = DateTime.UtcNow;
            entity.ModifiedAt = DateTime.UtcNow;

            context.Consents.Update(entity);
            await context.SaveChangesAsync();
            return Results.Ok(true);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Update payment type of consent to be authorized
    /// </summary>
    /// <param name="savePCStatusSenderAccount">To be updated consent data</param>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="tokenService"></param>
    /// <returns></returns>
    protected async Task<IResult> UpdatePaymentConsentForAuthorization(
        [FromBody] UpdatePCForAuthorizationDto savePCStatusSenderAccount,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] ITokenService tokenService,
        [FromServices] IOBEventService obEventService)
    {
        var resultData = new Consent();
        try
        {
            //Check consent
            await ProcessPaymentConsentToCancelOrEnd(savePCStatusSenderAccount.Id, context, tokenService);
            var entity = await context.Consents
                .Include(c => c.OBPaymentConsentDetails)
                .FirstOrDefaultAsync(c => c.Id == savePCStatusSenderAccount.Id
                                          && c.ConsentType == ConsentConstants.ConsentType.OpenBankingPayment);
            //Check consent validity
            ApiResult isDataValidResult = IsDataValidToUpdatePaymentConsentForAuth(entity, savePCStatusSenderAccount);
            if (!isDataValidResult.Result) //Error in data validation
            {
                return Results.BadRequest(isDataValidResult.Message);
            }

            var additionalData = JsonSerializer.Deserialize<OdemeEmriRizasiWithMsrfTtrHHSDto>(entity!.AdditionalData);
            //Check and set sender account
            if (additionalData!.odmBsltm.gon == null
                || (string.IsNullOrEmpty(additionalData.odmBsltm.gon.hspNo)
                    && string.IsNullOrEmpty(additionalData.odmBsltm.gon.hspRef)))
            {
                additionalData.odmBsltm.gon = savePCStatusSenderAccount.SenderAccount;
                //Set account reference
                var detail = entity!.OBPaymentConsentDetails.FirstOrDefault();
                if (detail == null)
                {
                    return Results.Problem("There is no payment consent detail data in the system.");
                }
                detail.SenderTitle = savePCStatusSenderAccount.SenderAccount?.unv;
                detail.SenderAccountNumber = savePCStatusSenderAccount.SenderAccount?.hspNo;
                detail.SenderAccountReference = savePCStatusSenderAccount.SenderAccount?.hspRef;
                detail.ModifiedAt = DateTime.UtcNow;
                context.OBPaymentConsentDetails.Update(detail);
            }

            //Set permission data
            additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.Yetkilendirildi;
            additionalData.rzBlg.gnclZmn = DateTime.UtcNow;
            entity.AdditionalData = JsonSerializer.Serialize(additionalData);
            entity.State = OpenBankingConstants.RizaDurumu.Yetkilendirildi;
            entity.StateModifiedAt = DateTime.UtcNow;
            entity.ModifiedAt = DateTime.UtcNow;
            context.Consents.Update(entity);

            await context.SaveChangesAsync();
            //If ayrikGKD, post olay-dinleme to YOS
            if (additionalData.gkd.yetYntm == OpenBankingConstants.GKDTur.Ayrik)
            {
                await obEventService.DoEventProcess(entity.Id.ToString(), additionalData.katilimciBlg,
                    OpenBankingConstants.OlayTip.AyrikGKDBasarili, OpenBankingConstants.KaynakTip.OdemeEmriRizasi, entity.Id.ToString());
            }
            return Results.Ok(resultData);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    /// <summary>
    /// Updates consent state for authorization usage
    /// </summary>
    /// <param name="updateConsentState">To be updated consent data</param>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    private async Task<IResult> UpdateAccountConsentStatusForUsage(UpdateConsentStateDto updateConsentState,
        ConsentDbContext context)
    {
        try
        {
            //Check consent validity for cancel consent
            await ProcessAccountConsentToCancelOrEnd(updateConsentState.Id, context);
            var entity = await context.Consents
                .Include(c => c.OBAccountConsentDetails)
                .FirstOrDefaultAsync(c => c.Id == updateConsentState.Id
                                          && c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount);
            //Check consent validity
            ApiResult isDataValidResult = IsDataValidToUpdateAccountConsentStatusForUsage(entity, updateConsentState);
            if (!isDataValidResult.Result) //Error in data validation
            {
                return Results.BadRequest(isDataValidResult.Message);
            }

            //Set permissions
            var additionalData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity!.AdditionalData);
            if (additionalData == null)
            {
                return Results.BadRequest("Consent additional data is empty");
            }
            var consentDetail = entity.OBAccountConsentDetails.FirstOrDefault();
            if (consentDetail is null)
            {
                return Results.BadRequest("Consent detail is empty");
            }
            additionalData.rzBlg.rizaDrm = updateConsentState.State;
            additionalData.rzBlg.gnclZmn = DateTime.UtcNow;
            consentDetail.SendToServiceTryCount = 0;
            consentDetail.SendToServiceDeliveryStatus = OpenBankingConstants.RecordDeliveryStatus.Processing;
            entity.AdditionalData = JsonSerializer.Serialize(additionalData);
            entity.State = updateConsentState.State;
            entity.StateModifiedAt = DateTime.UtcNow;
            entity.ModifiedAt = DateTime.UtcNow;

            context.OBAccountConsentDetails.Update(consentDetail);
            context.Consents.Update(entity);
            await context.SaveChangesAsync();
            return Results.Ok(true);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    /// <summary>
    /// Update account type of consent to be set authorized
    /// </summary>
    /// <param name="saveAccountReference"></param>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    protected async Task<IResult> UpdateAccountConsentForAuthorization(
        [FromBody] SaveAccountReferenceDto saveAccountReference,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IOBEventService obEventService)
    {
        try
        {
            //Check consent validity for cancel consent
            await ProcessAccountConsentToCancelOrEnd(saveAccountReference.Id, context);
            //Get consent from db
            var consentEntity = await context.Consents
                .Include(c => c.OBAccountConsentDetails)
                .FirstOrDefaultAsync(c => c.Id == saveAccountReference.Id
                                          && c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount);

            //Check consent validity For Authorization
            ApiResult isDataValidResult =
                IsDataValidToUpdateAccountConsentForAuthorization(consentEntity, saveAccountReference);
            if (!isDataValidResult.Result) //Error in data validation
            {
                return Results.BadRequest(isDataValidResult.Message);
            }

            //Update consent state and additional data
            var additionalData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(consentEntity!.AdditionalData);
            if (additionalData == null)
            {
                return Results.BadRequest("Consent additional data is empty");
            }
            additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.Yetkilendirildi;
            additionalData.rzBlg.gnclZmn = DateTime.UtcNow;
            consentEntity.AdditionalData = JsonSerializer.Serialize(additionalData);
            consentEntity.State = OpenBankingConstants.RizaDurumu.Yetkilendirildi;
            consentEntity.StateModifiedAt = DateTime.UtcNow;
            consentEntity.ModifiedAt = DateTime.UtcNow;

            //Set account reference
            var detail = consentEntity.OBAccountConsentDetails.FirstOrDefault();
            if (detail is null)
            {
                return Results.BadRequest("Consent detail is empty");
            }
            detail.AccountReferences = saveAccountReference.AccountReferences;
            detail.ModifiedAt = DateTime.UtcNow;
            context.OBAccountConsentDetails.Update(detail);
            context.Consents.Update(consentEntity);
            await context.SaveChangesAsync();
            //If ayrikGKD, post olay-dinleme to YOS
            if (additionalData.gkd.yetYntm == OpenBankingConstants.GKDTur.Ayrik)
            {
                await obEventService.DoEventProcess(consentEntity.Id.ToString(), additionalData.katilimciBlg,
                    OpenBankingConstants.OlayTip.AyrikGKDBasarili, OpenBankingConstants.KaynakTip.HesapBilgisiRizasi, consentEntity.Id.ToString());
            }
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    public async Task<IResult> CancelConsent([FromBody] CancelConsentDto cancelData,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] ITokenService tokenService)
    {
        try
        {
            //Check data validity
            ApiResult isDataValidResult = IsDataValidToCancelConsent(cancelData);
            if (!isDataValidResult.Result) //Error in data validation
            {
                return Results.BadRequest(isDataValidResult.Message);
            }
            //State list can be cancelled
            var canBeCancelledStates = new List<string>()
            {
                OpenBankingConstants.RizaDurumu.YetkiBekleniyor,
                OpenBankingConstants.RizaDurumu.Yetkilendirildi
            };
            //Get entity from db
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == cancelData.ConsentId);
            if (entity == null)
            {
                return Results.NoContent();
            }
            if (!canBeCancelledStates.Contains(entity.State))
            {
                return Results.BadRequest("Consent state not valid to be cancelled.");
            }

            if (entity.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount)
            {
                //Account consent
                //Update consent rıza bilgileri properties
                var additionalData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity.AdditionalData);
                additionalData!.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiIptal;
                additionalData.rzBlg.rizaIptDtyKod = cancelData.CancelDetailCode;
                additionalData.rzBlg.gnclZmn = DateTime.UtcNow;
                entity.AdditionalData = JsonSerializer.Serialize(additionalData);
                entity.ModifiedAt = DateTime.UtcNow;
                entity.State = OpenBankingConstants.RizaDurumu.YetkiIptal;
                entity.StateModifiedAt = DateTime.UtcNow;
                entity.StateCancelDetailCode = additionalData.rzBlg.rizaIptDtyKod;
                context.Consents.Update(entity);
                await context.SaveChangesAsync();
            }
            else if (entity.ConsentType == ConsentConstants.ConsentType.OpenBankingPayment)
            {
                //Payment consent
                //Update consent rıza bilgileri properties
                var additionalData = JsonSerializer.Deserialize<OdemeEmriRizasiWithMsrfTtrHHSDto>(entity.AdditionalData);
                additionalData!.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiIptal;
                additionalData.rzBlg.rizaIptDtyKod = cancelData.CancelDetailCode;
                additionalData.rzBlg.gnclZmn = DateTime.UtcNow;
                entity.AdditionalData = JsonSerializer.Serialize(additionalData);
                entity.ModifiedAt = DateTime.UtcNow;
                entity.State = OpenBankingConstants.RizaDurumu.YetkiIptal;
                entity.StateModifiedAt = DateTime.UtcNow;
                entity.StateCancelDetailCode = additionalData.rzBlg.rizaIptDtyKod;
                context.Consents.Update(entity);
                await context.SaveChangesAsync();
            }
            else
            {
                //Not related type
                return Results.BadRequest("Consent type not valid");
            }
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// hesap-bilgisi-rizasi post. Does account consent process.
    /// </summary>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-JWS-Signature", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Fraud-Check", ParameterLocation.Header)]
    protected async Task<IResult> AccountInformationConsentPost(
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        [FromServices] IOBAuthorizationService authorizationService,
        [FromServices] IOBEventService eventService,
        HttpContext httpContext,
        [FromServices] IPushService pushService,
        [FromServices] ITagService tagService,
        [FromServices] IDeviceRecord deviceRecordService,
        [FromServices] ICustomerService customerService
)
    {
        try
        {
            var requestBody = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
            var rizaIstegi = JsonSerializer.Deserialize<HesapBilgisiRizaIstegiHHSDto>(requestBody);
            if (rizaIstegi == null)
            {
                var nullError = OBErrorResponseHelper.GetBadRequestError(httpContext, _errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.InvalidContent);
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, nullError);
                httpContext.Response.ContentType = "application/json";
                return Results.Content(nullError.ToJsonString(), "application/json", statusCode: HttpStatusCode.BadRequest.GetHashCode());
            }
            var header = OBModuleHelper.GetHeader(httpContext);
            //Check if post data is valid to process.
            var checkValidationResult =
                await IsDataValidToAccountConsentPost(rizaIstegi, requestBody, configuration, yosInfoService, eventService, httpContext, context);

            if (!checkValidationResult.Result)
            {
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, checkValidationResult.Data);
                //Data not valid
                return Results.Content(checkValidationResult.Data.ToJsonString(), "application/json", statusCode: HttpStatusCode.BadRequest.GetHashCode());
            }

            //Check customer
            var checkCustomerResult =
                await OBConsentValidationHelper.CheckCustomerInformation(customerService, rizaIstegi.kmlk, httpContext,
                    _errorCodeDetails);
            if (!checkCustomerResult.Result)
            {//Error in customer service
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, checkCustomerResult.Data);
                //Data not valid
                return Results.Content(checkCustomerResult.Data.ToJsonString(), "application/json", statusCode: HttpStatusCode.BadRequest.GetHashCode());
            }

            //Check Idempotency
            var getIdempotencyConsentResult = await OBModuleHelper.GetIdempotencyAccountConsent(rizaIstegi, header, authorizationService);
            if (!getIdempotencyConsentResult.Result)
            {
                //Get 500 error response
                var errorResponse = OBErrorResponseHelper.GetInternalServerError(httpContext, _errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.InternalServerErrorCheckingIdempotency);
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, errorResponse);
                return Results.Content(errorResponse.ToJsonString(), "application/json", statusCode: HttpStatusCode.BadRequest.GetHashCode());
            }
            if (getIdempotencyConsentResult.Data != null)
            {//Idempotency occured. Return previous response
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, getIdempotencyConsentResult.Data);
                return Results.Content(getIdempotencyConsentResult.Data.ToJsonString(), "application/json", statusCode: HttpStatusCode.OK.GetHashCode());
            }

            //Get user's active account consents from db and process them
            var checkAccountConsentResult = await CheckAccountConsents(authorizationService, rizaIstegi, context, httpContext);
            if (!checkAccountConsentResult.Result)
            {
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, checkAccountConsentResult.Data);
                return Results.Content(checkAccountConsentResult.Data.ToJsonString(), "application/json", statusCode: HttpStatusCode.BadRequest.GetHashCode());
            }
            
            GetCustomerResponseDto customerResponse = (GetCustomerResponseDto)checkCustomerResult.Data!;

            var consentEntity = new Consent();
            context.Consents.Add(consentEntity);
            //Generate response object
            HesapBilgisiRizasiHHSDto hesapBilgisiRizasi = mapper.Map<HesapBilgisiRizasiHHSDto>(rizaIstegi);
            //Set consent data
            hesapBilgisiRizasi.rzBlg = new RizaBilgileriDto()
            {
                rizaNo = consentEntity.Id.ToString(),
                olusZmn = DateTime.UtcNow,
                gnclZmn = DateTime.UtcNow,
                rizaDrm = OpenBankingConstants.RizaDurumu.YetkiBekleniyor
            };

            bool isAyrikGKD = hesapBilgisiRizasi.gkd.yetYntm == OpenBankingConstants.GKDTur.Ayrik;
            //Set gkd data
            //Set hhsYonAdr in Yonlendirmeli GKD
            if (!isAyrikGKD)
            {
                hesapBilgisiRizasi.gkd.hhsYonAdr = await OBModuleHelper.GetHhsForwardingAddressAsync(configuration, hesapBilgisiRizasi.kmlk, consentEntity.Id.ToString(), tagService, deviceRecordService);
            }

            hesapBilgisiRizasi.gkd.yetTmmZmn = DateTime.UtcNow.AddMinutes(5);
            consentEntity.AdditionalData = JsonSerializer.Serialize(hesapBilgisiRizasi);
            consentEntity.State = OpenBankingConstants.RizaDurumu.YetkiBekleniyor;
            consentEntity.StateModifiedAt = DateTime.UtcNow;
            consentEntity.ConsentType = ConsentConstants.ConsentType.OpenBankingAccount;
            consentEntity.Variant = hesapBilgisiRizasi.katilimciBlg.yosKod;
            consentEntity.ClientCode = string.Empty;
            consentEntity.LastValidAccessDate = hesapBilgisiRizasi.hspBlg.iznBlg.erisimIzniSonTrh.ToUniversalTime();
            consentEntity.UserTCKN = GenericMethodsHelper.ConvertStringToNullableLong(customerResponse.citizenshipNumber);
            consentEntity.OBAccountConsentDetails = new List<OBAccountConsentDetail>
            {
                GenerateAccountConsentDetailObject(hesapBilgisiRizasi, rizaIstegi, header, customerResponse)
            };
            context.Consents.Add(consentEntity);
            await context.SaveChangesAsync();
            if (isAyrikGKD)
            {
                //Send notification to user
                await pushService.OpenBankingSendPush(hesapBilgisiRizasi.kmlk, consentEntity.Id);
            }

            OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, hesapBilgisiRizasi);
            return Results.Content(hesapBilgisiRizasi.ToJsonString(), "application/json", statusCode: HttpStatusCode.OK.GetHashCode());
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }



    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    [AddSwaggerParameter("user_reference", ParameterLocation.Header, true)]
    protected async Task<IResult> DeleteAccountConsentFromYos(Guid rizaNo,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] ITokenService tokenService,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            List<OBErrorCodeDetail> errorCodeDetails = await context.OBErrorCodeDetails.AsNoTracking().ToListAsync();
            var header = OBModuleHelper.GetHeader(httpContext);
            //Check header fields
            ApiResult headerValidation =
                await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, header: header,
                    isUserRequired: true, errorCodeDetails: errorCodeDetails);
            if (!headerValidation.Result)
            {
                //Missing header fields
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, headerValidation.Data);
                return Results.BadRequest(headerValidation.Data);
            }

            //Check consent
            await ProcessAccountConsentToCancelOrEnd(rizaNo, context);

            //get consent entity from db
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == rizaNo
                                          && c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                                          && c.Variant == header.XTPPCode);

            if (entity == null
               || string.IsNullOrEmpty(entity.AdditionalData)) //No desired consent in system
            {
                var errorResponse = OBErrorResponseHelper.GetNotFoundError(httpContext, errorCodeDetails,
                     OBErrorCodeConstants.ErrorCodesEnum.NotFound);
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, errorResponse);
                return Results.NotFound(errorResponse);
            }
            ApiResult dataValidationResult = IsDataValidToDeleteAccountConsent(entity, errorCodeDetails, httpContext); //Check data validation
            if (!dataValidationResult.Result)
            {
                //Data not valid
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, dataValidationResult.Data);
                return Results.BadRequest(dataValidationResult.Data);
            }

            //Update consent rıza bilgileri properties
            var additionalData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity.AdditionalData);
            additionalData!.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiIptal;
            additionalData.rzBlg.rizaIptDtyKod =
                OpenBankingConstants.RizaIptalDetayKodu.KullaniciIstegiIleYOSUzerindenIptal;
            additionalData.rzBlg.gnclZmn = DateTime.UtcNow;
            entity.AdditionalData = JsonSerializer.Serialize(additionalData);
            entity.ModifiedAt = DateTime.UtcNow;
            entity.State = OpenBankingConstants.RizaDurumu.YetkiIptal;
            entity.StateModifiedAt = DateTime.UtcNow;
            entity.StateCancelDetailCode = additionalData.rzBlg.rizaIptDtyKod;
            context.Consents.Update(entity);
            await context.SaveChangesAsync();

            //Revoke token
            await tokenService.RevokeConsentToken(rizaNo);
            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    [AddSwaggerParameter("user_reference", ParameterLocation.Header, true)]
    protected async Task<IResult> DeleteAccountConsentFromHHS(Guid rizaNo,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] ITokenService tokenService,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        [FromServices] IOBEventService eventService,
        HttpContext httpContext)
    {
        try
        {
            //Get header fields
            var header = OBModuleHelper.GetHeader(httpContext);
            if (string.IsNullOrEmpty(header.UserReference))
            {
                //Missing header fields
                return Results.BadRequest("Header user_reference can not be empty");
            }
            //Check consent to cancel&/end
            await ProcessAccountConsentToCancelOrEnd(rizaNo, context);

            //get consent entity from db
            var entity = await context.Consents
                .Include(c => c.OBAccountConsentDetails)
                .FirstOrDefaultAsync(c => c.Id == rizaNo
                                          && c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount);
            ApiResult dataValidationResult =
                IsDataValidToDeleteAccountConsentFromHHS(entity, header.UserReference); //Check data validation
            if (!dataValidationResult.Result)
            {
                //Data not valid
                return Results.BadRequest(dataValidationResult.Message);
            }
            await CancelAccountConsent(context, tokenService, eventService, entity!);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }



    protected async Task<IResult> DeleteAccountConsentFromHHSInstitution(Guid consentId,
        string customerNumber,
        string institutionCustomerNumber,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] ITokenService tokenService,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        [FromServices] IOBEventService eventService,
        HttpContext httpContext)
    {
        try
        {
            //Check consent to cancel&/end
            await ProcessAccountConsentToCancelOrEnd(consentId, context);

            //get consent entity from db
            var entity = await context.Consents
                .Include(c => c.OBAccountConsentDetails)
                .FirstOrDefaultAsync(c => c.Id == consentId
                                          && c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                                          && c.OBAccountConsentDetails.Any(i => i.CustomerNumber == customerNumber
                                                          && i.InstitutionCustomerNumber == institutionCustomerNumber
                                                          && i.UserType == OpenBankingConstants.OHKTur.Kurumsal));
            ApiResult dataValidationResult =
                IsDataValidToDeleteAccountConsentFromHHSInstitution(entity); //Check data validation
            if (!dataValidationResult.Result)
            {
                //Data not valid
                return Results.BadRequest(dataValidationResult.Message);
            }

            //Update consent rıza bilgileri properties
            await CancelAccountConsent(context, tokenService, eventService, entity!);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    private static async Task CancelAccountConsent(ConsentDbContext context, ITokenService tokenService,
        IOBEventService eventService, Consent entity)
    {
        //Update consent rıza bilgileri properties
        var additionalData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity!.AdditionalData);
        additionalData!.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiIptal;
        additionalData.rzBlg.rizaIptDtyKod =
            OpenBankingConstants.RizaIptalDetayKodu.KullaniciIstegiIleHHSUzerindenIptal;
        additionalData.rzBlg.gnclZmn = DateTime.UtcNow;
        entity.AdditionalData = JsonSerializer.Serialize(additionalData);
        entity.ModifiedAt = DateTime.UtcNow;
        entity.State = OpenBankingConstants.RizaDurumu.YetkiIptal;
        entity.StateModifiedAt = DateTime.UtcNow;
        entity.StateCancelDetailCode = additionalData.rzBlg.rizaIptDtyKod;
        //Update consent detail to send consent information to account service.
        var consentDetail = entity.OBAccountConsentDetails.FirstOrDefault();
        if (consentDetail is not null)
        {
            consentDetail.SendToServiceTryCount = 0;
            consentDetail.SendToServiceDeliveryStatus = OpenBankingConstants.RecordDeliveryStatus.Processing;
            context.OBAccountConsentDetails.Update(consentDetail);
        }

        context.Consents.Update(entity);
        await context.SaveChangesAsync();

        //Revoke token
        await tokenService.RevokeConsentToken(entity.Id);

        //Send event to yos
        await eventService.DoEventProcess(entity.Id.ToString(),
            additionalData.katilimciBlg,
            eventType: OpenBankingConstants.OlayTip.KaynakGuncellendi,
            sourceType: OpenBankingConstants.KaynakTip.HesapBilgisiRizasi,
            sourceNumber: entity.Id.ToString());
    }


    /// <summary>
    /// Does payment information consent post process.
    /// odeme-emri-rizasi post method.
    /// Checks OdemeEmriRizaIstegi object data and generates OdemeEmriRizasi object and insert.
    /// </summary>
    /// <returns>OdemeEmriRizasi object</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-JWS-Signature", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Fraud-Check", ParameterLocation.Header)]
    protected async Task<IResult> PaymentConsentPost([FromBody] OdemeEmriRizaIstegiHHSDto rizaIstegi,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IPaymentService paymentService,
        [FromServices] IYosInfoService yosInfoService,
        [FromServices] IOBEventService eventService,
        [FromServices] IPushService pushService,
        [FromServices] ITagService tagService,
        [FromServices] IOBAuthorizationService authorizationService,
        [FromServices] IDeviceRecord deviceRecordService,
        HttpContext httpContext)
    {
        try
        {
            var header = OBModuleHelper.GetHeader(httpContext);
            //Check if post data is valid to process.
            var dataValidationResult =
                await IsDataValidToPaymentConsentPost(rizaIstegi, header, configuration, yosInfoService, eventService, httpContext, context);
            if (!dataValidationResult.Result)
            {
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, dataValidationResult.Data);
                //Data not valid
                httpContext.Response.ContentType = "application/json";
                return Results.Content(dataValidationResult.Data.ToJsonString(), "application/json", statusCode: HttpStatusCode.BadRequest.GetHashCode());
            }

            //Check Idempotency
            var getIdempotencyConsentResult = await OBModuleHelper.GetIdempotencyPaymentConsent(rizaIstegi, header, authorizationService);
            if (!getIdempotencyConsentResult.Result)
            {
                //Get 500 error response
                var errorResponse = OBErrorResponseHelper.GetBadRequestError(httpContext, _errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.InternalServerErrorCheckingIdempotency);
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, errorResponse);
                return Results.Content(errorResponse.ToJsonString(), "application/json", statusCode: HttpStatusCode.BadRequest.GetHashCode());
            }
            if (getIdempotencyConsentResult.Data != null)
            {//Idempotency occured. Return previous response
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, getIdempotencyConsentResult.Data);
                return Results.Content(getIdempotencyConsentResult.Data.ToJsonString(), "application/json", statusCode: HttpStatusCode.OK.GetHashCode());
            }

            ApiResult paymentServiceResponse = await paymentService.SendOdemeEmriRizasi(rizaIstegi);
            if (!paymentServiceResponse.Result) //Error in service
            {
                return Results.Content(paymentServiceResponse.Data.ToJsonString(), "application/json", statusCode: HttpStatusCode.BadRequest.GetHashCode());
            }

            var consentEntity = new Consent();
            context.Consents.Add(consentEntity);
            //Generate response object
            OdemeEmriRizasiWithMsrfTtrHHSDto odemeEmriRizasi =
                (OdemeEmriRizasiWithMsrfTtrHHSDto)paymentServiceResponse.Data!;
            //Set consent data
            odemeEmriRizasi.rzBlg = new RizaBilgileriDto()
            {
                rizaNo = consentEntity.Id.ToString(),
                olusZmn = DateTime.UtcNow,
                gnclZmn = DateTime.UtcNow,
                rizaDrm = OpenBankingConstants.RizaDurumu.YetkiBekleniyor
            };

            bool isAyrikGKD = odemeEmriRizasi.gkd.yetYntm == OpenBankingConstants.GKDTur.Ayrik;
            //Set gkd data
            //Set hhsYonAdr in Yonlendirmeli GKD
            if (!isAyrikGKD)
            {
                odemeEmriRizasi.gkd.hhsYonAdr = await OBModuleHelper.GetHhsForwardingAddressAsync(configuration, odemeEmriRizasi.odmBsltm.kmlk, consentEntity.Id.ToString(), tagService, deviceRecordService);
            }

            odemeEmriRizasi.gkd.yetTmmZmn = DateTime.UtcNow.AddMinutes(5);
            consentEntity.AdditionalData = JsonSerializer.Serialize(odemeEmriRizasi);
            consentEntity.State = OpenBankingConstants.RizaDurumu.YetkiBekleniyor;
            consentEntity.StateModifiedAt = DateTime.UtcNow;
            consentEntity.ConsentType = ConsentConstants.ConsentType.OpenBankingPayment;
            consentEntity.Variant = odemeEmriRizasi.katilimciBlg.yosKod;
            consentEntity.ClientCode = string.Empty;

            consentEntity.OBPaymentConsentDetails = new List<OBPaymentConsentDetail>
            {
                GeneratePaymentConsentDetailObject(odemeEmriRizasi, rizaIstegi, header)
            };

            context.Consents.Add(consentEntity);
            await context.SaveChangesAsync();
            if (isAyrikGKD)
            {
                //Send notification to user
                await pushService.OpenBankingSendPush(odemeEmriRizasi.odmBsltm.kmlk, consentEntity.Id);
            }

            var resObject = mapper.Map<OdemeEmriRizasiHHSDto>(odemeEmriRizasi); //Send consent to YOS without hhsmsrfttr property
            OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, resObject);
            string jsonResponse = JsonConvert.SerializeObject(resObject, Formatting.None);
            httpContext.Response.ContentType = "application/json";
            return Results.Content(jsonResponse, "application/json", statusCode: 200);

        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    /// <summary>
    /// Payment order consent post process.
    /// odeme-emri post method.
    /// Checks OdemeEmriIstegi object data, service integration and returns OdemeEmri object
    /// </summary>
    /// <returns>OdemeEmri object</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    [AddSwaggerParameter("user_reference", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-JWS-Signature", ParameterLocation.Header, true)]
    [AddSwaggerParameter("openbanking_consent_id", ParameterLocation.Header, true)]
    protected async Task<IResult> PaymentOrderPost([FromBody] OdemeEmriIstegiHHSDto odemeEmriIstegi,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IPaymentService paymentService,
        [FromServices] IYosInfoService yosInfoService,
        [FromServices] IOBEventService eventService,
        [FromServices] IOBAuthorizationService authorizationService,
        [FromServices] ITokenService tokenService,
        HttpContext httpContext)
    {
        try
        {
            var header = OBModuleHelper.GetHeader(httpContext); //Get header
            //Check if post data is valid to process.
            var dataValidationResult = await IsDataValidToPaymentOrderPost(odemeEmriIstegi, header, context, yosInfoService, eventService: eventService, tokenService: tokenService,
                httpContext, configuration);
            if (!dataValidationResult.Result)
            {
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, dataValidationResult.Data);
                //Data not valid
                return Results.BadRequest(dataValidationResult.Data);
            }

            if (dataValidationResult.Data == null)
            {
                //Odeme emri rizası entity can not be taken
                return Results.BadRequest("Payment Order Consent can not be found in the system");
            }

            //Check Idempotency
            var getIdempotencyConsentResult = await OBModuleHelper.GetIdempotencyPaymentOrder(odemeEmriIstegi, header, authorizationService);
            if (!getIdempotencyConsentResult.Result)
            {
                //Get 500 error response
                var errorResponse = OBErrorResponseHelper.GetBadRequestError(httpContext, _errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.InternalServerErrorCheckingIdempotency);
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, errorResponse);
                return Results.BadRequest(errorResponse.ToJsonString());
            }
            if (getIdempotencyConsentResult.Data != null)
            {//Idempotency occured. Return previous response
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, getIdempotencyConsentResult.Data);
                return Results.Ok(getIdempotencyConsentResult.Data.ToJsonString());
            }

            //Send payment order to payment service
            ApiResult paymentServiceResponse = await paymentService.SendOdemeEmri(odemeEmriIstegi);
            if (paymentServiceResponse.Data == null)
            {
                //TODO:Özlem ödeme servisinden cevap gelmediği zaman ne olacak.
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, "No response from payment system");
                return Results.Problem("No response from payment system");
            }
            if (!paymentServiceResponse.Result) //Error in service
            {
                OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, paymentServiceResponse.Data);
                return Results.BadRequest(paymentServiceResponse.Data.ToJsonString());
            }

            //TODO:Özlem error oluşma caseleri için konuş

            OdemeEmriHHSDto odemeEmriDto = (OdemeEmriHHSDto)paymentServiceResponse.Data;

            //Update consent state
            Consent paymentConsentEntity = (Consent)dataValidationResult.Data; //odemeemririzasi entity
            var additionalData = JsonSerializer.Deserialize<OdemeEmriRizasiHHSDto>(paymentConsentEntity.AdditionalData);
            additionalData!.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiOdemeEmrineDonustu;
            additionalData.rzBlg.gnclZmn = DateTime.UtcNow;
            paymentConsentEntity.AdditionalData = JsonSerializer.Serialize(additionalData);
            paymentConsentEntity.State = OpenBankingConstants.RizaDurumu.YetkiOdemeEmrineDonustu;
            paymentConsentEntity.StateModifiedAt = DateTime.UtcNow;
            context.Consents.Update(paymentConsentEntity);

            var orderEntity = new OBPaymentOrder();
            context.OBPaymentOrders.Add(orderEntity); //Add to get id

            //Set consent data
            odemeEmriDto.emrBlg = new EmirBilgileriDto()
            {
                odmEmriNo = orderEntity.Id.ToString(),
                odmEmriZmn = DateTime.UtcNow
            };
            odemeEmriDto.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiOdemeEmrineDonustu;
            orderEntity.ConsentId = paymentConsentEntity.Id;
            orderEntity.AdditionalData = JsonSerializer.Serialize(odemeEmriDto);
            orderEntity.State = OpenBankingConstants.RizaDurumu.YetkiOdemeEmrineDonustu;
            orderEntity.HhsCode = odemeEmriDto.katilimciBlg.hhsKod;
            orderEntity.YosCode = odemeEmriDto.katilimciBlg.yosKod;
            orderEntity.Currency = odemeEmriDto.odmBsltm.islTtr.prBrm;
            orderEntity.Amount = odemeEmriDto.odmBsltm.islTtr.ttr;
            orderEntity.PaymentState = odemeEmriDto.odmBsltm.odmAyr.odmDrm;
            orderEntity.PaymentSource = odemeEmriDto.odmBsltm.odmAyr.odmKynk;
            orderEntity.PaymentPurpose = odemeEmriDto.odmBsltm.odmAyr.odmAmc;
            orderEntity.ReferenceInformation = odemeEmriDto.odmBsltm.odmAyr.refBlg;
            orderEntity.OHKMessage = odemeEmriDto.odmBsltm.odmAyr.ohkMsj;
            orderEntity.PaymentDescription = odemeEmriDto.odmBsltm.odmAyr.odmAcklm;
            orderEntity.PaymentSystem = odemeEmriDto.odmBsltm.odmAyr.odmStm;
            orderEntity.ExpectedPaymentDate = odemeEmriDto.odmBsltm.odmAyr.bekOdmZmn;
            orderEntity.PaymentSystemNumber = odemeEmriDto.odmBsltm.odmAyr.odmStmNo;
            SetPaymentSystemNumberFields(odemeEmriDto, orderEntity);
            if (odemeEmriDto.isyOdmBlg != null)
            {
                orderEntity.WorkplaceCategoryCode = odemeEmriDto.isyOdmBlg.isyKtgKod;
                orderEntity.SubWorkplaceCategoryCode = odemeEmriDto.isyOdmBlg.altIsyKtgKod;
                orderEntity.GeneralWorkplaceNumber = odemeEmriDto.isyOdmBlg.genelUyeIsyeriNo;
            }

            orderEntity.XRequestId = header.XRequestID;
            orderEntity.XGroupId = header.XGroupID;
            context.OBPaymentOrders.Add(orderEntity);
            await context.SaveChangesAsync();//Save order
            OBModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, odemeEmriDto);
            return Results.Ok(odemeEmriDto.ToJsonString());
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    private static void SetPaymentSystemNumberFields(OdemeEmriHHSDto odemeEmriDto, OBPaymentOrder orderEntity)
    {
        var systemNumberItems = odemeEmriDto.odmBsltm.odmAyr.odmStmNo.Split('|').ToArray();
        orderEntity.PSNDate = systemNumberItems?[0];
        orderEntity.PSNYosCode = systemNumberItems?[1];
        if (systemNumberItems?[2] == null
            || string.IsNullOrEmpty(systemNumberItems[2]))
        {
            orderEntity.PSNRefNum = null;
        }
        else
        {
            if (Int32.TryParse(systemNumberItems[2], out int result))
            {
                // Successfully parsed
                orderEntity.PSNRefNum = result;
            }
            else
            {
                orderEntity.PSNRefNum = null;
            }
        }
    }


    [Topic(OpenBankingConstants.KafkaInformation.KafkaName,
        OpenBankingConstants.KafkaInformation.TopicName_PaymentStatusUpdated, true)]
    [HttpPost]
    public async Task<IResult> PaymentStateChanged(
        [FromServices] ConsentDbContext context,
        [FromServices] IOBEventService obEventService,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            //Get payment system record.
            PaymentRecordDto? model = await httpContext.Deserialize<PaymentRecordDto>();
            if (model != null
                && model.message?.data?.TRAN_BRANCH_CODE ==
                OpenBankingConstants.PaymentServiceInformation.PaymentServiceBranchCode
                && !string.IsNullOrEmpty(model.message.data.TRAN_DATE)
                && !string.IsNullOrEmpty(model.message.data.RECORD_STATUS))
            {
                //Check must fields
                string recordStatus = model.message.data.RECORD_STATUS; //New status
                DateTime tranDate = DateTime.ParseExact(model.message.data.TRAN_DATE, "yyyy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture);
                DateTime utcTranDate = DateTime.SpecifyKind(tranDate, DateTimeKind.Utc);
                DateTime lastUpdateDate = DateTime.ParseExact(model.message.data.LAST_UPDATE_DATE,
                    "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                int refNum = model.message.data.REF_NUM;
                //Get related payment order entity
                var paymentOrderEntity = context.OBPaymentOrders.Where(o => o.PSNRefNum == refNum
                                                                            && o.PSNDate != null).AsEnumerable()
                    .FirstOrDefault(o =>
                        o.PSNDate != null && DateTime.ParseExact(o.PSNDate, "yyyy-MM-dd HH:mm:ss", null).Date == utcTranDate.Date);
                if (paymentOrderEntity != null)
                {
                    if (paymentOrderEntity.PaymentServiceUpdateTime != null
                        && DateTime.ParseExact(paymentOrderEntity.PaymentServiceUpdateTime, "yyyy-MM-dd HH:mm:ss.fff",
                            null) > lastUpdateDate)
                    {
                        //Payment order updated with latest record
                        //TODO:Ozlem log this case
                        return Results.Ok();
                    }

                    //Calculate new state
                    var paymentState = GetPaymentState(recordStatus, paymentOrderEntity.PaymentSystem,
                        paymentOrderEntity.PaymentState);
                    if (paymentState != paymentOrderEntity.PaymentState) //Payment state changed
                    {
                        //Update payment message
                        var additionalData =
                            JsonSerializer.Deserialize<OdemeEmriHHSDto>(paymentOrderEntity.AdditionalData);
                        additionalData!.odmBsltm.odmAyr.odmDrm = paymentState;
                        //Update payment order data
                        paymentOrderEntity.AdditionalData = JsonSerializer.Serialize(additionalData);
                        paymentOrderEntity.PaymentServiceUpdateTime = model.message.data.LAST_UPDATE_DATE;
                        paymentOrderEntity.PaymentState = paymentState;
                        paymentOrderEntity.ModifiedAt = DateTime.UtcNow;
                        context.OBPaymentOrders.Update(paymentOrderEntity);
                        await context.SaveChangesAsync();
                        //If YOS has subscription, Do event process
                        ApiResult yosHasSubscription = await yosInfoService.IsYosSubscsribed(paymentOrderEntity.YosCode,
                            OpenBankingConstants.OlayTip.KaynakGuncellendi, OpenBankingConstants.KaynakTip.OdemeEmri);
                        if (yosHasSubscription.Result
                            && yosHasSubscription.Data != null
                           && (bool)yosHasSubscription.Data)
                        {
                            //Yos has subscrition. Do event process.
                            await obEventService.DoEventProcess(paymentOrderEntity.Id.ToString(),
                                additionalData.katilimciBlg,
                                OpenBankingConstants.OlayTip.KaynakGuncellendi,
                                OpenBankingConstants.KaynakTip.OdemeEmri,
                                paymentOrderEntity.Id.ToString());
                        }
                    }
                }
            }

            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    [Topic(OpenBankingConstants.KafkaInformation.KafkaName,
          OpenBankingConstants.KafkaInformation.TopicName_BalanceUpdated, true)]
    [HttpPost]
    public async Task<IResult> BalanceChanged(
          [FromServices] ConsentDbContext context,
          [FromServices] IOBEventService obEventService,
          [FromServices] IOBAuthorizationService authorizationService,
          [FromServices] ILogger<OpenBankingHHSConsentModule> logger,
          HttpContext httpContext)
    {
        try
        {
            //Get payment system record.
            BalanceChangedKafkaRecordDto? kafkaRecord = await httpContext.Deserialize<BalanceChangedKafkaRecordDto>();
            if (kafkaRecord is { message.data: not null }
                && !string.IsNullOrEmpty(kafkaRecord.message.data.OPEN_BANKING_CONSENT_NUMBER)
                && !string.IsNullOrEmpty(kafkaRecord.message.data.HESAP_REF)
                && !string.IsNullOrEmpty(kafkaRecord.message.data.INSTANT_BALANCE_NOTIFICATION_PERMISSION)
                && kafkaRecord.message.data.INSTANT_BALANCE_NOTIFICATION_PERMISSION == OpenBankingConstants.BalanceChangedServiceYesNo.Yes
                && !string.IsNullOrEmpty(kafkaRecord.message.data.OPEN_BANKING_SHARE_PERMISSION)
                && kafkaRecord.message.data.OPEN_BANKING_SHARE_PERMISSION == OpenBankingConstants.BalanceChangedServiceYesNo.Yes)
            {
                //Get active account consent
                var getConsentResult = await authorizationService.GetAuthUsedAccountConsent(
                    kafkaRecord.message.data.OPEN_BANKING_CONSENT_NUMBER,
                    kafkaRecord.message.data.HESAP_REF,
                    new List<string>() { OpenBankingConstants.IzinTur.AnlikBakiyeBildirimi });

                if (!getConsentResult.Result)
                {//Error in service
                    return Results.Problem(getConsentResult.Message);
                }

                if (getConsentResult.Data == null)
                {//No consent in system
                    logger.LogWarning("Message read from Kafka. But no related consent data in system. {@KafkaRecord}", kafkaRecord);
                    return Results.NoContent();
                }
                var consent = (Consent)getConsentResult.Data;
                var consentDetail = consent.OBAccountConsentDetails.FirstOrDefault();
                if (consentDetail != null)
                {//Do event process
                    await obEventService.DoEventProcess(consent.Id.ToString(), new KatilimciBilgisiDto() { hhsKod = consentDetail.HhsCode, yosKod = consentDetail.YosCode },
                        OpenBankingConstants.OlayTip.KaynakGuncellendi, OpenBankingConstants.KaynakTip.Bakiye,
                        kafkaRecord.message.data.HESAP_REF);
                }

            }
            else
            {//Kafka record data is not  valid
                logger.LogWarning("Message read from Kafka. But not valid. {@KafkaRecord}", kafkaRecord);
                return Results.BadRequest();
            }

            return Results.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing BalanceChanged request.");
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }



    private string GetPaymentState(string recordStatus, string paymentSystem, string currentPaymentState)
    {
        string paymentState = currentPaymentState;
        if (paymentSystem == OpenBankingConstants.OdemeSistemi.Fast)
        {
            switch (recordStatus)
            {
                case "TM":
                    paymentState = OpenBankingConstants.OdemeDurumu.Gerceklesti;
                    break;
                case "HM":
                case "FR":
                case "IT":
                case "I":
                case "FI":
                case "EM":
                case "T":
                case "W":
                case "Y":
                case "G":
                    paymentState = OpenBankingConstants.OdemeDurumu.Gerceklesmedi;
                    break;
                default:
                    paymentState = currentPaymentState;
                    break;
            }
        }
        else if (paymentSystem == OpenBankingConstants.OdemeSistemi.EFT_POS)
        {
            switch (recordStatus)
            {
                case "T":
                case "B":
                case "O":
                    paymentState = OpenBankingConstants.OdemeDurumu.Gerceklesti;
                    break;
                case "I":
                case "Q":
                case "U":
                    paymentState = OpenBankingConstants.OdemeDurumu.Gerceklesmedi;
                    break;
                case "G":
                    paymentState = OpenBankingConstants.OdemeDurumu.Gonderildi;
                    break;
                default:
                    paymentState = currentPaymentState;
                    break;

            }
        }

        return paymentState;
    }

    #endregion


    /// <summary>
    /// Checks if data is valid for account consent post process
    /// </summary>
    /// <param name="rizaIstegi">To be checked data</param>
    /// <param name="configuration">Config object</param>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <param name="eventService"></param>
    /// <param name="httpContext">Context object to get header parameters</param>
    /// <exception cref="NotImplementedException"></exception>
    private async Task<ApiResult> IsDataValidToAccountConsentPost(HesapBilgisiRizaIstegiHHSDto rizaIstegi,
        string requestBody,
        IConfiguration configuration,
        IYosInfoService yosInfoService,
        IOBEventService eventService,
        HttpContext httpContext,
        ConsentDbContext dbContext)
    {
        //TODO:Ozlem Check if user is customer

        ApiResult result = new();
        var header = OBModuleHelper.GetHeader(httpContext);
        //Check header fields
        result = await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, header, isXJwsSignatureRequired: true,
            katilimciBlg: rizaIstegi.katilimciBlg, errorCodeDetails: _errorCodeDetails, body: requestBody);
        if (!result.Result)
        {
            //validation error in header fields
            return result;
        }
        string objectName = OBErrorCodeConstants.ObjectNames.HesapBilgisiRizasiIstegi;
        //Check message required basic properties/objects
        if (!OBConsentValidationHelper.PrepareAndCheckInvalidFormatProperties_HBRObject(rizaIstegi, httpContext, _errorCodeDetails, out var errorResponse, objectName:objectName))
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

       
        //Check KatılımcıBilgisi
        result = OBConsentValidationHelper.IsKatilimciBlgDataValid(httpContext, configuration,
            katilimciBlg: rizaIstegi.katilimciBlg, errorCodeDetails: _errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            //validation error in katiliciBilgisi data fields
            return result;
        }

        //Check GKD
        result = await OBConsentValidationHelper.IsGkdValid(rizaIstegi.gkd, rizaIstegi.kmlk, rizaIstegi.katilimciBlg.yosKod, httpContext, _errorCodeDetails, eventService, yosInfoService: yosInfoService, objectName: objectName);
        if (!result.Result)
        {
            return result;
        }


        //Check Kimlik
        result = OBConsentValidationHelper.CheckKmlkData(rizaIstegi.kmlk, httpContext, _errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            return result;
        }

        result = OBConsentValidationHelper.CheckIznBlgTur(rizaIstegi.hspBlg.iznBlg, httpContext, _errorCodeDetails, objectName: objectName);
        return result;
    }


    /// <summary>
    ///  Checks if data is valid for payment information consent post process
    /// </summary>
    /// <returns></returns>
    private async Task<ApiResult> IsDataValidToPaymentConsentPost(OdemeEmriRizaIstegiHHSDto rizaIstegi,
        RequestHeaderDto header,
        IConfiguration configuration,
        IYosInfoService yosInfoService,
        IOBEventService eventService,
        HttpContext httpContext,
        ConsentDbContext dbContext)
    {
        ApiResult result = new();
        //Check header fields
        result = await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, header, isXJwsSignatureRequired: true,
            katilimciBlg: rizaIstegi.katilimciBlg, errorCodeDetails: _errorCodeDetails);
        if (!result.Result)
        {
            //validation error in header fields
            return result;
        }

        //Check message required basic properties/objects
        if (!OBConsentValidationHelper.PrepareAndCheckInvalidFormatProperties_OERObject(rizaIstegi, httpContext, _errorCodeDetails, out var errorResponse))
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }
        string objectName = OBErrorCodeConstants.ObjectNames.OdemeEmriRizasiIstegi;
        //Check KatılımcıBilgisi
        result = OBConsentValidationHelper.IsKatilimciBlgDataValid(httpContext, configuration,
            katilimciBlg: rizaIstegi.katilimciBlg, errorCodeDetails: _errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            //validation error in katiliciBilgisi data fields
            return result;
        }

        //Check GKD
        result = await OBConsentValidationHelper.IsGkdValid(rizaIstegi.gkd, rizaIstegi.odmBsltm.kmlk, rizaIstegi.katilimciBlg.yosKod, httpContext, _errorCodeDetails, eventService, yosInfoService: yosInfoService, objectName: objectName);
        if (!result.Result)
        {
            return result;
        }
        //Check One time payment
        result = OBConsentValidationHelper.CheckOneTimePayment(rizaIstegi.odmBsltm.kmlk, rizaIstegi.odmBsltm.alc.kolas,
            rizaIstegi.odmBsltm.gon?.unv, header, httpContext, _errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            return result;
        }

        //Check odmBsltm  Kimlik field validities
        result = OBConsentValidationHelper.CheckKmlkData(rizaIstegi.odmBsltm.kmlk, httpContext, _errorCodeDetails, objectName: objectName, checkOneTimePayment: true);
        if (!result.Result)
        {
            return result;
        }

        //Check odmBsltma Islem Tutarı object
        result = OBConsentValidationHelper.CheckTtrData(rizaIstegi.odmBsltm.islTtr, httpContext, _errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            return result;
        }

        //Check odmBsltma Alıcı,kkod,kolas
        result = OBConsentValidationHelper.CheckKolasKarekodAlici(rizaIstegi.odmBsltm, httpContext, _errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            return result;
        }

        //Check odemeayrintilari
        result = OBConsentValidationHelper.CheckOdemeAyrinti(rizaIstegi.odmBsltm.odmAyr, httpContext, _errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            return result;
        }

        //Check isyOdmBlg data
        result = OBConsentValidationHelper.CheckIsyeriOdemeBilgileri(rizaIstegi.isyOdmBlg, httpContext, _errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            return result;
        }


        return result;
    }

    /// <summary>
    /// Check if consent is valid to payment order post
    /// </summary>
    private async Task<ApiResult> IsDataValidToPaymentOrderPost(OdemeEmriIstegiHHSDto odemeEmriIstegi,
        RequestHeaderDto header,
        ConsentDbContext context,
        IYosInfoService yosInfoService,
        IOBEventService eventService,
        ITokenService tokenService,
        HttpContext httpContext,
        IConfiguration configuration)
    {
        ApiResult result = new();
        //Check header fields
        result = await OBConsentValidationHelper.IsHeaderDataValid(httpContext, configuration, yosInfoService, header, isXJwsSignatureRequired: true,
            katilimciBlg: odemeEmriIstegi.katilimciBlg, isUserRequired: true, isConsentIdRequired: true, errorCodeDetails: _errorCodeDetails);
        if (!result.Result)
        {
            //validation error in header fields
            return result;
        }

        //Check message required basic properties/objects
        if (!OBConsentValidationHelper.PrepareAndCheckInvalidFormatProperties_OEIObject(odemeEmriIstegi, httpContext, _errorCodeDetails, out var errorResponse))
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }
        string objectName = OBErrorCodeConstants.ObjectNames.OdemeEmriIstegi;

        //Check rzBlg
        result = OBConsentValidationHelper.CheckRizaBlg(odemeEmriIstegi.rzBlg, consentId: header.ConsentId, httpContext, errorCodeDetails: _errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            //validation error in katiliciBilgisi data fields
            return result;
        }

        //Check KatılımcıBilgisi
        result = OBConsentValidationHelper.IsKatilimciBlgDataValid(httpContext, configuration,
            katilimciBlg: odemeEmriIstegi.katilimciBlg, errorCodeDetails: _errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            //validation error in katiliciBilgisi data fields
            return result;
        }

        //Check GKD
        result = await OBConsentValidationHelper.IsGkdValid(odemeEmriIstegi.gkd, odemeEmriIstegi.odmBsltm.kmlk, odemeEmriIstegi.katilimciBlg.yosKod, httpContext, _errorCodeDetails, eventService, yosInfoService: yosInfoService, objectName: objectName);
        if (!result.Result)
        {
            return result;
        }

        //Check odmBsltm  Kimlik field validities
        result = OBConsentValidationHelper.CheckKmlkData(odemeEmriIstegi.odmBsltm.kmlk, httpContext, _errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            return result;
        }

        //Check odmBsltma Islem Tutarı object
        result = OBConsentValidationHelper.CheckTtrData(odemeEmriIstegi.odmBsltm.islTtr, httpContext, _errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            return result;
        }

        //Check odmBsltma gonHesap 
        result = OBConsentValidationHelper.CheckGonderen(odemeEmriIstegi.odmBsltm.gon, httpContext, _errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            return result;
        }

        //Check odmBsltma alc,kolas, kkod 
        result = OBConsentValidationHelper.CheckKolasKarekodAlici(odemeEmriIstegi.odmBsltm, httpContext, _errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            return result;
        }

        //Check odmBsltma odemeayrintilari
        result = OBConsentValidationHelper.CheckOdemeAyrinti(odemeEmriIstegi.odmBsltm.odmAyr, httpContext, _errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            return result;
        }
        //Check isyOdmBlg data
        result = OBConsentValidationHelper.CheckIsyeriOdemeBilgileri(odemeEmriIstegi.isyOdmBlg, httpContext, _errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            return result;
        }


        //TODO:Özlem update when user service finished
        if (!string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.kmlk.kmlkTur)
            && odemeEmriIstegi.odmBsltm.kmlk.kmlkTur == OpenBankingConstants.KimlikTur.TCKN
            && odemeEmriIstegi.odmBsltm.kmlk.kmlkVrs != header.UserReference)
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(httpContext, _errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidContentProcessingUserNotInKmlData);
            return result;
        }

        //Check consent
        await ProcessPaymentConsentToCancelOrEnd(new Guid(odemeEmriIstegi.rzBlg.rizaNo), context, tokenService);

        //TODO:Özlem select e işlem yapan kullanıcı bilgilerini de servis geliştirme bitince ekle
        var odemeEmriRizasiConsent = await context.Consents
            .FirstOrDefaultAsync(c => c.Id.ToString() == odemeEmriIstegi.rzBlg.rizaNo
                                      && c.ConsentType == ConsentConstants.ConsentType.OpenBankingPayment);

        //Do OdemeEmriRizasi validations
        if (odemeEmriRizasiConsent == null) //No consent in db
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(httpContext, _errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.NotFoundPaymentConsentToPaymentOrder);
            return result;
        }

        //Check state
        if (odemeEmriRizasiConsent.State !=
            OpenBankingConstants.RizaDurumu.YetkiKullanildi) //State must be yetki kullanıldı
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(httpContext, _errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.ConsentMismatchStatusNotValidToPaymentOrder);
            return result;
        }


        var odemeEmriRizasi = JsonSerializer.Deserialize<OdemeEmriRizasiHHSDto>(odemeEmriRizasiConsent.AdditionalData);
        if (odemeEmriRizasi == null)
        {
            result.Result = false;
            result.Message = "Relational data is missing. No Payment Information consent additional data in system.";
            return result;
        }

        //Check odeme emri rizasi and odeme emri istegi
        result = OBConsentValidationHelper.CheckOdemeEmriRizasiOdemeEmri(odemeEmriRizasi, odemeEmriIstegi, httpContext, _errorCodeDetails, objectName: objectName);
        if (!result.Result)
        {
            return result;
        }
        result.Data = odemeEmriRizasiConsent;
        return result;
    }



    /// <summary>
    /// Check if consent is valid to be deleted
    /// </summary>
    /// <returns>Data validation result</returns>
    private ApiResult IsDataValidToDeleteAccountConsent(Consent entity, List<OBErrorCodeDetail> errorCodeDetails, HttpContext httpContext)
    {
        ApiResult result = new();
        var deleteStatusList = new List<string>() { OpenBankingConstants.RizaDurumu.Yetkilendirildi,
            OpenBankingConstants.RizaDurumu.YetkiBekleniyor,
            OpenBankingConstants.RizaDurumu.YetkiKullanildi
        };
        if (!deleteStatusList.Contains(entity.State))
        {
            //State not valid to set as deleted
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(httpContext, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.ConsentMismatchStateNotValidToDelete);
            return result;
        }
        return result;
    }

    /// <summary>
    /// Check if consent is valid to be deleted
    /// </summary>
    /// <param name="entity">To be checked entity</param>
    /// <param name="userTCKN">Processing user tckn</param>
    /// <returns>Data validation result</returns>
    private ApiResult IsDataValidToDeleteAccountConsentFromHHS(Consent? entity, string userTCKN)
    {
        ApiResult result = new();
        if (entity == null)
        {
            result.Result = false;
            result.Message = "No desired consent in system.";
            return result;
        }
        var deleteStatusList = new List<string>() { OpenBankingConstants.RizaDurumu.Yetkilendirildi,
            OpenBankingConstants.RizaDurumu.YetkiBekleniyor,
            OpenBankingConstants.RizaDurumu.YetkiKullanildi
        };
        if (!deleteStatusList.Contains(entity.State))
        {
            //State not valid to set as deleted
            result.Result = false;
            result.Message = "Account consent status not valid to marked as deleted";
            return result;
        }

        var hesapBilgisiRizasi = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity.AdditionalData);
        if (hesapBilgisiRizasi == null)
        {
            result.Result = false;
            result.Message = "HesapBilgisiRizasi data in system is null";
            return result;
        }
        if (hesapBilgisiRizasi.kmlk.kmlkTur == OpenBankingConstants.KimlikTur.TCKN
            && hesapBilgisiRizasi.kmlk.kmlkVrs != userTCKN)
        {
            result.Result = false;
            result.Message = "Consent does not belong to this user.";
            return result;
        }

        return result;
    }

    private ApiResult IsDataValidToDeleteAccountConsentFromHHSInstitution(Consent? entity)
    {
        ApiResult result = new();
        if (entity == null)
        {
            result.Result = false;
            result.Message = "No desired consent in system.";
            return result;
        }
        var deleteStatusList = new List<string>() { OpenBankingConstants.RizaDurumu.Yetkilendirildi,
            OpenBankingConstants.RizaDurumu.YetkiBekleniyor,
            OpenBankingConstants.RizaDurumu.YetkiKullanildi
        };
        if (!deleteStatusList.Contains(entity.State))
        {
            //State not valid to set as deleted
            result.Result = false;
            result.Message = "Account consent status not valid to marked as deleted";
            return result;
        }

        var hesapBilgisiRizasi = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity.AdditionalData);
        if (hesapBilgisiRizasi == null)
        {
            result.Result = false;
            result.Message = "HesapBilgisiRizasi data in system is null";
            return result;
        }
        return result;
    }

    /// <summary>
    /// Check if consent is valid to be updated for authorization
    /// </summary>
    /// <param name="entity">To be checked entity</param>
    /// <param name="saveAccountReference">to be checked object</param>
    /// <returns>Data validation result</returns>
    private ApiResult IsDataValidToUpdateAccountConsentForAuthorization(Consent? entity,
        SaveAccountReferenceDto saveAccountReference)
    {
        ApiResult result = new();
        if (entity == null)
        {
            result.Result = false;
            result.Message = "No desired consent in system.";
            return result;
        }

        if (entity.State != OpenBankingConstants.RizaDurumu.YetkiBekleniyor)
        {
            result.Result = false;
            result.Message = "Consent state not valid to process";
            return result;
        }

        if (!(saveAccountReference.AccountReferences?.Any() ?? false))
        {
            result.Result = false;
            result.Message = "Account reference not set";
            return result;
        }

        return result;
    }

    /// <summary>
    ///  Check if consent is valid to be updated for usage
    /// </summary>
    /// <param name="entity">To be checked entity</param>
    /// <param name="updateConsentState">to be checked object</param>
    /// <returns></returns>
    private ApiResult IsDataValidToUpdatePaymentConsentStatusForUsage(Consent? entity,
        UpdateConsentStateDto updateConsentState)
    {
        ApiResult result = new();
        if (entity == null)
        {
            result.Result = false;
            result.Message = $"No consent in system with id: {updateConsentState.Id}";
            return result;
        }

        if (entity.State != OpenBankingConstants.RizaDurumu.Yetkilendirildi)
        {
            result.Result = false;
            result.Message = "Consent state not valid to process";
            return result;
        }

        //Consent state parameter not valid
        if (string.IsNullOrEmpty(updateConsentState.State)
            || !ConstantHelper.GetConsentNexStepFromAuthorizedStatusList().Contains(updateConsentState.State))
        {
            result.Result = false;
            result.Message = "Consent state parameter value is not valid";
            return result;
        }

        return result;
    }

    /// <summary>
    ///  Check if consent is valid to be updated for usage
    /// </summary>
    /// <param name="entity">To be checked entity</param>
    /// <param name="updateConsentState">to be checked object</param>
    /// <returns></returns>
    private ApiResult IsDataValidToUpdateAccountConsentStatusForUsage(Consent? entity,
        UpdateConsentStateDto updateConsentState)
    {
        ApiResult result = new();
        if (entity == null)
        {
            result.Result = false;
            result.Message = "BadRequest.";
            return result;
        }

        if (entity.State != OpenBankingConstants.RizaDurumu.Yetkilendirildi)
        {
            result.Result = false;
            result.Message = "Consent state not valid to process";
            return result;
        }

        //Consent state parameter not valid
        if (string.IsNullOrEmpty(updateConsentState.State)
            || !ConstantHelper.GetConsentNexStepFromAuthorizedStatusList().Contains(updateConsentState.State))
        {
            result.Result = false;
            result.Message = "Consent state parameter value is not valid";
            return result;
        }

        return result;
    }

    /// <summary>
    /// Check if consent is valid to be updated for authorization
    /// </summary>
    /// <param name="entity">To be checked entity</param>
    /// <param name="savePcStatusSenderAccount">To be checked object</param>
    /// <returns></returns>
    private ApiResult IsDataValidToUpdatePaymentConsentForAuth(Consent? entity,
        UpdatePCForAuthorizationDto savePcStatusSenderAccount)
    {
        ApiResult result = new();
        if (entity == null) //No consent in db
        {
            result.Result = false;
            result.Message = "No consent in the system.";
            return result;
        }

        if (entity.State != OpenBankingConstants.RizaDurumu.YetkiBekleniyor) //State must be yetki bekleniyor
        {
            result.Result = false;
            result.Message =
                "Consent state not valid to process. Only YetkiBekleniyor state consent can be authorized.";
            return result;
        }

        //Check if sender account is already selected in db
        var additionalData = JsonSerializer.Deserialize<OdemeEmriRizasiHHSDto>(entity.AdditionalData);
        if (additionalData == null)
        {
            result.Result = false;
            result.Message = "Consent additional data is empty";
            return result;
        }
        //Check if sender account is set in db
        bool isSenderAccountSet = !string.IsNullOrEmpty(additionalData.odmBsltm.gon?.hspNo) ||
                                  !string.IsNullOrEmpty(additionalData.odmBsltm.gon?.hspRef);
        if (!isSenderAccountSet
            && (savePcStatusSenderAccount.SenderAccount == null
                || string.IsNullOrEmpty(savePcStatusSenderAccount.SenderAccount.unv)
                || (string.IsNullOrEmpty(savePcStatusSenderAccount.SenderAccount.hspRef)
                    && string.IsNullOrEmpty(savePcStatusSenderAccount.SenderAccount.hspNo))))
        {
            result.Result = false;
            result.Message = "Sender account information should be sent.";
            return result;
        }

        return result;
    }




    /// <summary>
    /// Cancel waiting approve state consents
    /// </summary>
    /// <param name="context"></param>
    /// <param name="activeAccountConsents">Consents to be checked</param>
    private async Task CancelWaitingApproveConsents(ConsentDbContext context, List<Consent> activeAccountConsents)
    {
        //Waiting approve state consents
        var waitingAporoves = activeAccountConsents
            .Where(c => c.State == OpenBankingConstants.RizaDurumu.YetkiBekleniyor).ToList();
        if (waitingAporoves?.Any() ?? false)
        {
            //If any, cancel all of them
            foreach (var waitingAporove in waitingAporoves)
            {
                //Update consent rıza bilgileri properties
                var additionalData =
                    JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(waitingAporove.AdditionalData);
                additionalData!.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiIptal;
                additionalData.rzBlg.rizaIptDtyKod =
                    OpenBankingConstants.RizaIptalDetayKodu.YeniRizaTalebiIleIptal;
                additionalData.rzBlg.gnclZmn = DateTime.UtcNow;
                waitingAporove.AdditionalData = JsonSerializer.Serialize(additionalData);
                waitingAporove.ModifiedAt = DateTime.UtcNow;
                waitingAporove.State = OpenBankingConstants.RizaDurumu.YetkiIptal;
                waitingAporove.StateCancelDetailCode = additionalData.rzBlg.rizaIptDtyKod;
                waitingAporove.StateModifiedAt = DateTime.UtcNow;
            }

            context.Consents.UpdateRange(waitingAporoves);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Checks if any yetkilendirildi or yetkikullanildi state consents of user
    /// </summary>
    /// <param name="activeAccountConsents">Consents to be checked</param>
    /// <returns>Any consent that yetkilendirildi or yetkikullanildi states</returns>
    private bool AnyAuthAndUsedConsents(List<Consent>? activeAccountConsents)
    {
        var authAndUsed = activeAccountConsents?
            .Any(c => c.State == OpenBankingConstants.RizaDurumu.Yetkilendirildi
                      || c.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi) ?? false;
        if (authAndUsed)
        {
            return true;
        }
        return false;
    }



    /// <summary>
    /// Process account consent states
    /// Get active account consents. Cancel Yetki Bekleniyor state consents.
    /// Check yetki kullanıldı, yetkilendirildi state consents validity
    /// </summary>
    /// <returns></returns>
    private async Task<ApiResult> CheckAccountConsents(IOBAuthorizationService authorizationService,
        HesapBilgisiRizaIstegiHHSDto rizaIstegi,
        ConsentDbContext context,
        HttpContext httpContext)
    {
        ApiResult result = new();
        var getConsentsResult =
            await authorizationService.GetActiveAccountConsentsOfUser(rizaIstegi.kmlk, rizaIstegi.katilimciBlg.yosKod);
        if (getConsentsResult.Result == false)
        {
            //Error getting consents
            result.Result = false;
            result.Message = "Error checking current consents";
            return result;
        }

        //Active account consents in db
        var activeAccountConsents = (List<Consent>?)getConsentsResult.Data;
        if (!(activeAccountConsents?.Any() ?? false))
        {
            //No active consent
            return result;
        }

        await ProcessAccountConsentToCancelOrEnd(activeAccountConsents, context);
        //Cancel Yetki Bekleniyor state consents.
        await CancelWaitingApproveConsents(context, activeAccountConsents);
        if (AnyAuthAndUsedConsents(activeAccountConsents)) //Checks any yetkilendirildi, yetki kullanıldı state consent
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(httpContext, _errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.ConsentMismatchAccountPostAlreadyAuthroized);
            return result;
        }

        return result;
    }

    /// <summary>
    /// Check account consent to end or cancel
    /// according to state, state modified date, last valid date
    /// If last valid date comes, end consent
    /// If state waiting time passes, cancel consent
    /// </summary>
    /// <param name="rizaNo">To be checked consent id</param>
    /// <param name="context">Context Object</param>
    private async Task ProcessAccountConsentToCancelOrEnd(Guid rizaNo, ConsentDbContext context)
    {
        var entity = await context.Consents
            .Include(c => c.OBAccountConsentDetails)
            .FirstOrDefaultAsync(c => c.Id == rizaNo
                                      && c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount);
        var today = DateTime.UtcNow;
        if (entity == null
            || string.IsNullOrEmpty(entity.AdditionalData)
            || entity.State == OpenBankingConstants.RizaDurumu.YetkiIptal
            || entity.State == OpenBankingConstants.RizaDurumu.YetkiSonlandirildi)
        {
            //Consent life ended. There is nothing to do.
            return;
        }

        await ProcessAccountConsentToCancelOrEnd(new List<Consent>() { entity }, context);
    }


    private async Task ProcessAccountConsentToCancelOrEnd(List<Consent> consents, ConsentDbContext context)
    {
        var today = DateTime.UtcNow;

        foreach (var entity in consents)
        {
            if (entity == null
                || string.IsNullOrEmpty(entity.AdditionalData)
                || entity.State == OpenBankingConstants.RizaDurumu.YetkiIptal
                || entity.State == OpenBankingConstants.RizaDurumu.YetkiSonlandirildi)
            {
                //Consent life ended. There is nothing to do.
                continue;
            }

            var additionalData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity.AdditionalData);
            if (additionalData == null)
            {
                continue;
            }
            //comment from document
            //Erişimin Geçerli Olduğu Son Tarih geldiğinde Rıza durumu Yetki Kullanıldı’dan Yetki Sonlandırıldı durumuna çekilmelidir. K ⇨ S
            if (entity.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi
                && additionalData.hspBlg.iznBlg.erisimIzniSonTrh < today)
            {
                //Consent given time is up. End the consent
                additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiSonlandirildi;
                additionalData.rzBlg.gnclZmn = today;
                entity.AdditionalData = JsonSerializer.Serialize(additionalData);
                entity.ModifiedAt = today;
                entity.State = OpenBankingConstants.RizaDurumu.YetkiSonlandirildi;
                entity.StateModifiedAt = today;
                var consentDetail = entity.OBAccountConsentDetails.FirstOrDefault();
                if (consentDetail is not null)
                {
                    consentDetail.SendToServiceTryCount = 0;
                    consentDetail.SendToServiceDeliveryStatus = OpenBankingConstants.RecordDeliveryStatus.Processing;
                    context.OBAccountConsentDetails.Update(consentDetail);
                }
                context.Consents.Update(entity);
                await context.SaveChangesAsync();
            }

            //comment from document
            //5 dakikadan uzun süredir “Yetki Bekleniyor” durumunda kalan kayıtların durumları güncellenir. 
            //Yetki Bekleniyor ⇨ Rıza İptal / Süre Aşımı : Yetki Bekleniyor B ⇨ I / 04 
            else if (entity.State == OpenBankingConstants.RizaDurumu.YetkiBekleniyor
                     && additionalData.rzBlg.gnclZmn.AddMinutes(5) < today)
            {
                //Consent is in yetki bekleniyor state more than 5 minutes
                additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiIptal;
                additionalData.rzBlg.rizaIptDtyKod =
                    OpenBankingConstants.RizaIptalDetayKodu.SureAsimi_YetkiBekleniyor;
                additionalData.rzBlg.gnclZmn = today;
                entity.AdditionalData = JsonSerializer.Serialize(additionalData);
                entity.ModifiedAt = today;
                entity.State = OpenBankingConstants.RizaDurumu.YetkiIptal;
                entity.StateModifiedAt = today;
                entity.StateCancelDetailCode = additionalData.rzBlg.rizaIptDtyKod;
                context.Consents.Update(entity);
                await context.SaveChangesAsync();
            }

            //comment from document
            //5 dakikadan uzun süredir “Yetkilendirildi” durumunda kalan kayıtlar güncellenir. 
            //Yetkilendirildi ⇨ Rıza İptal / Süre Aşımı: Yetkilendirildi B ⇨ I / 05
            else if (entity.State == OpenBankingConstants.RizaDurumu.Yetkilendirildi
                     && additionalData.rzBlg.gnclZmn.AddMinutes(5) < today)
            {
                //Consent is in yetkilendirildi state more than 5 minutes
                additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiIptal;
                additionalData.rzBlg.rizaIptDtyKod =
                    OpenBankingConstants.RizaIptalDetayKodu.SureAsimi_Yetkilendirildi;
                additionalData.rzBlg.gnclZmn = today;
                entity.AdditionalData = JsonSerializer.Serialize(additionalData);
                entity.ModifiedAt = today;
                entity.State = OpenBankingConstants.RizaDurumu.YetkiIptal;
                entity.StateModifiedAt = today;
                entity.StateCancelDetailCode = additionalData.rzBlg.rizaIptDtyKod;
                context.Consents.Update(entity);
                await context.SaveChangesAsync();
                //There is no access token, so no need to call revoke.
            }
        }
    }


    /// <summary>
    /// Check payment consent to end or cancel
    /// If state waiting time passes, cancel consent
    /// </summary>
    /// <param name="rizaNo">To be checked consent id</param>
    /// <param name="context">Context Object</param>
    /// <param name="tokenService">Token service instance</param>
    private async Task ProcessPaymentConsentToCancelOrEnd(Guid rizaNo, ConsentDbContext context,
        ITokenService tokenService)
    {
        var entity = await context.Consents
            .FirstOrDefaultAsync(c => c.Id == rizaNo
                                      && c.ConsentType == ConsentConstants.ConsentType.OpenBankingPayment);
        var today = DateTime.UtcNow;
        if (entity == null
            || string.IsNullOrEmpty(entity.AdditionalData)
            || entity.State == OpenBankingConstants.RizaDurumu.YetkiIptal
            || entity.State == OpenBankingConstants.RizaDurumu.YetkiSonlandirildi)
        {
            //Consent life ended. There is nothing to do.
            return;
        }

        var additionalData = JsonSerializer.Deserialize<OdemeEmriRizasiHHSDto>(entity.AdditionalData);
        if (additionalData == null)
        {
            return;
        }

        //comment from document
        //5 dakikadan uzun süredir Yetki Bekleniyor'da kalan kayıtların durumları güncellenir.
        //Yetki Bekleniyor ⇨ Rıza İptal / Süre Aşımı : Yetki Bekleniyor B ⇨ I / 04
        if (entity.State == OpenBankingConstants.RizaDurumu.YetkiBekleniyor
            && additionalData.rzBlg.gnclZmn.AddMinutes(5) < today)
        {
            //Consent is in yetki bekleniyor state more than 5 minutes
            additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiIptal;
            additionalData.rzBlg.rizaIptDtyKod =
                OpenBankingConstants.RizaIptalDetayKodu.SureAsimi_YetkiBekleniyor;
            additionalData.rzBlg.gnclZmn = today;
            entity.AdditionalData = JsonSerializer.Serialize(additionalData);
            entity.ModifiedAt = today;
            entity.State = OpenBankingConstants.RizaDurumu.YetkiIptal;
            entity.StateModifiedAt = today;
            entity.StateCancelDetailCode = additionalData.rzBlg.rizaIptDtyKod;
            context.Consents.Update(entity);
            await context.SaveChangesAsync();
            return;
        }

        //comment from document
        //5 dakikadan uzun süredir Yetkilendirildi'de kalan kayıtlar durumları güncellenir.
        //Yetkilendirildi ⇨ Rıza İptal / Süre Aşımı : Yetkilendirildi B ⇨ I / 05
        if (entity.State == OpenBankingConstants.RizaDurumu.Yetkilendirildi
            && additionalData.rzBlg.gnclZmn.AddMinutes(5) < today)
        {
            //Consent is in yetkilendirildi state more than 5 minutes
            additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiIptal;
            additionalData.rzBlg.rizaIptDtyKod =
                OpenBankingConstants.RizaIptalDetayKodu.SureAsimi_Yetkilendirildi;
            additionalData.rzBlg.gnclZmn = today;
            entity.AdditionalData = JsonSerializer.Serialize(additionalData);
            entity.ModifiedAt = today;
            entity.State = OpenBankingConstants.RizaDurumu.YetkiIptal;
            entity.StateModifiedAt = today;
            entity.StateCancelDetailCode = additionalData.rzBlg.rizaIptDtyKod;
            context.Consents.Update(entity);
            await context.SaveChangesAsync();
            //There is no access token, so no need to call revoke.
            return;
        }

        //comment from document
        //5 dakikadan uzun süredir Yetki kullanıldı'da kalan kayıtlar durumları güncellenir.
        //Yetki kullanıldı ⇨ Rıza İptal / Süre Aşımı : Yetki Ödemeye Dönüşmedi B ⇨ I / 06
        if (entity.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi
            && additionalData.rzBlg.gnclZmn.AddMinutes(5) < today)
        {
            //Consent is in yetkilendirildi state more than 5 minutes
            additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiIptal;
            additionalData.rzBlg.rizaIptDtyKod =
                OpenBankingConstants.RizaIptalDetayKodu.SureAsimi_YetkiOdemeyeDonusmedi;
            additionalData.rzBlg.gnclZmn = today;
            entity.AdditionalData = JsonSerializer.Serialize(additionalData);
            entity.ModifiedAt = today;
            entity.State = OpenBankingConstants.RizaDurumu.YetkiIptal;
            entity.StateModifiedAt = today;
            entity.StateCancelDetailCode = additionalData.rzBlg.rizaIptDtyKod;
            context.Consents.Update(entity);
            await context.SaveChangesAsync();
            //There is a token, revoke the token
            await tokenService.RevokeConsentToken(rizaNo);
            return;
        }

        //comment from document
        //Yenileme belirteci Son Tarih geldiğinde rıza durumu Yetki ödeme emrine dönüştü’den Yetki Sonlandırıldı'ya güncellenir.
        // E ⇨ S
        //TODO:Özlem bu durum nasıl handle edilecek bilmiyorum
    }

    private async Task ProcessConsentToCancelOrEnd(Guid rizaNo, ConsentDbContext context,
        ITokenService tokenService)
    {
        var consent = await context.Consents.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == rizaNo);
        if (consent != null)
        {
            if (consent.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount)
            {
                await ProcessAccountConsentToCancelOrEnd(rizaNo: rizaNo, context: context);
            }
            else if (consent.ConsentType == ConsentConstants.ConsentType.OpenBankingPayment)
            {
                await ProcessPaymentConsentToCancelOrEnd(rizaNo: rizaNo, context: context, tokenService: tokenService);
            }
        }
    }

    /// <summary>
    /// Checks cancel consent data
    /// </summary>
    /// <param name="cancelData">To be checked data</param>
    /// <returns>Data validation result</returns>
    private ApiResult IsDataValidToCancelConsent(CancelConsentDto cancelData)
    {
        ApiResult result = new();

        //check clientcode
        if (string.IsNullOrEmpty(cancelData.CancelDetailCode))
        {
            result.Result = false;
            result.Message = "Client detail code parameter is required.";
            return result;
        }
        //check tckn
        if (!ConstantHelper.GetRizaIptalDetayKoduList().Contains(cancelData.CancelDetailCode))
        {
            result.Result = false;
            result.Message = "Cancel detail code is not valid.";
            return result;
        }

        return result;
    }


    /// <summary>
    /// Generates accountconsentdetail object of account consent
    /// </summary>
    /// <returns>Accountconsentdetail object </returns>
    private static OBAccountConsentDetail GenerateAccountConsentDetailObject(
        HesapBilgisiRizasiHHSDto hesapBilgisiRizasi,
        HesapBilgisiRizaIstegiHHSDto rizaIstegi,
        RequestHeaderDto header,
        GetCustomerResponseDto customerInfo
        )
    {
        return new()
        {
            //Set consent detail 
            IdentityData = hesapBilgisiRizasi.kmlk.kmlkVrs,
            IdentityType = hesapBilgisiRizasi.kmlk.kmlkTur,
            InstitutionIdentityData = hesapBilgisiRizasi.kmlk.krmKmlkVrs,
            InstitutionIdentityType = hesapBilgisiRizasi.kmlk.krmKmlkTur,
            UserType = hesapBilgisiRizasi.kmlk.ohkTur,
            HhsCode = hesapBilgisiRizasi.katilimciBlg.hhsKod,
            YosCode = hesapBilgisiRizasi.katilimciBlg.yosKod,
            AuthMethod = hesapBilgisiRizasi.gkd.yetYntm,
            ForwardingAddress = hesapBilgisiRizasi.gkd.yonAdr,
            HhsForwardingAddress = hesapBilgisiRizasi.gkd.hhsYonAdr,
            DiscreteGKDDefinitionType = hesapBilgisiRizasi.gkd.ayrikGkd?.ohkTanimTip,
            DiscreteGKDDefinitionValue = hesapBilgisiRizasi.gkd.ayrikGkd?.ohkTanimDeger,
            AuthCompletionTime = hesapBilgisiRizasi.gkd.yetTmmZmn,
            PermissionTypes = hesapBilgisiRizasi.hspBlg.iznBlg.iznTur.ToList(),
            TransactionInquiryStartTime = hesapBilgisiRizasi.hspBlg.iznBlg.hesapIslemBslZmn?.ToUniversalTime(),
            TransactionInquiryEndTime = hesapBilgisiRizasi.hspBlg.iznBlg.hesapIslemBtsZmn?.ToUniversalTime(),
            OhkMessage = hesapBilgisiRizasi.hspBlg.ayrBlg?.ohkMsj,
            CheckSumLastValiDateTime = DateTime.UtcNow.AddMinutes(5),
            SaveResponseMessage = JsonSerializer.Serialize(hesapBilgisiRizasi),
            CheckSumValue = OBModuleHelper.GetChecksumForXRequestIdSHA256(rizaIstegi, header.XRequestID),
            XRequestId = header.XRequestID,
            XGroupId = header.XGroupID,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            CustomerNumber = customerInfo.customerNumber,
            InstitutionCustomerNumber = customerInfo.krmCustomerNumber
        };
    }

    /// <summary>
    /// Generates paymentconsentdetail object of payment consent
    /// </summary>
    /// <returns>paymentconsentdetail object</returns>
    private static OBPaymentConsentDetail GeneratePaymentConsentDetailObject(
        OdemeEmriRizasiWithMsrfTtrHHSDto odemeEmriRizasi, OdemeEmriRizaIstegiHHSDto rizaIstegi, RequestHeaderDto header)
    {
        return new()
        {
            //Set consent detail 
            IdentityData = odemeEmriRizasi.odmBsltm.kmlk.kmlkVrs,
            IdentityType = odemeEmriRizasi.odmBsltm.kmlk.kmlkTur,
            InstitutionIdentityData = odemeEmriRizasi.odmBsltm.kmlk.krmKmlkVrs,
            InstitutionIdentityType = odemeEmriRizasi.odmBsltm.kmlk.krmKmlkTur,
            UserType = odemeEmriRizasi.odmBsltm.kmlk.ohkTur,
            HhsCode = odemeEmriRizasi.katilimciBlg.hhsKod,
            YosCode = odemeEmriRizasi.katilimciBlg.yosKod,
            AuthMethod = odemeEmriRizasi.gkd.yetYntm,
            ForwardingAddress = odemeEmriRizasi.gkd.yonAdr,
            HhsForwardingAddress = odemeEmriRizasi.gkd.hhsYonAdr,
            DiscreteGKDDefinitionType = odemeEmriRizasi.gkd.ayrikGkd?.ohkTanimTip,
            DiscreteGKDDefinitionValue = odemeEmriRizasi.gkd.ayrikGkd?.ohkTanimDeger,
            AuthCompletionTime = odemeEmriRizasi.gkd.yetTmmZmn,

            Currency = odemeEmriRizasi.odmBsltm.islTtr.prBrm,
            Amount = odemeEmriRizasi.odmBsltm.islTtr.ttr,

            SenderTitle = odemeEmriRizasi.odmBsltm.gon?.unv,
            SenderAccountNumber = odemeEmriRizasi.odmBsltm.gon?.hspNo,
            SenderAccountReference = odemeEmriRizasi.odmBsltm.gon?.hspRef,

            ReceiverTitle = odemeEmriRizasi.odmBsltm.alc.unv,
            ReceiverAccountNumber = odemeEmriRizasi.odmBsltm.alc.hspNo,
            KolasType = odemeEmriRizasi.odmBsltm.alc.kolas?.kolasTur,
            KolasValue = odemeEmriRizasi.odmBsltm.alc.kolas?.kolasDgr,
            KolasRefNum = odemeEmriRizasi.odmBsltm.alc.kolas?.kolasRefNo,
            KolasAccountType = odemeEmriRizasi.odmBsltm.alc.kolas?.kolasHspTur,

            QRCodeRef = odemeEmriRizasi.odmBsltm.kkod?.kkodRef,
            QRCodeFlowType = odemeEmriRizasi.odmBsltm.kkod?.kkodRef,
            QRCodeProducerCode = odemeEmriRizasi.odmBsltm.kkod?.kkodUrtcKod,

            PaymentSource = odemeEmriRizasi.odmBsltm.odmAyr.odmKynk,
            PaymentPurpose = odemeEmriRizasi.odmBsltm.odmAyr.odmAmc,
            ReferenceInformation = odemeEmriRizasi.odmBsltm.odmAyr.refBlg,
            OHKMessage = odemeEmriRizasi.odmBsltm.odmAyr.ohkMsj,
            PaymentSystem = odemeEmriRizasi.odmBsltm.odmAyr.odmStm,
            ExpectedPaymentDate = odemeEmriRizasi.odmBsltm.odmAyr.bekOdmZmn,
            PaymentDescription = odemeEmriRizasi.odmBsltm.odmAyr.odmAcklm,

            WorkplaceCategoryCode = odemeEmriRizasi.isyOdmBlg?.isyKtgKod,
            SubWorkplaceCategoryCode = odemeEmriRizasi.isyOdmBlg?.altIsyKtgKod,
            GeneralWorkplaceNumber = odemeEmriRizasi.isyOdmBlg?.genelUyeIsyeriNo,

            CheckSumLastValiDateTime = DateTime.UtcNow.AddMinutes(5),
            SaveResponseMessage = JsonSerializer.Serialize(odemeEmriRizasi),
            CheckSumValue = OBModuleHelper.GetChecksumForXRequestIdSHA256(rizaIstegi, header.XRequestID),

            XRequestId = header.XRequestID,
            XGroupId = header.XGroupID,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }


    private async Task<List<ListAccountConsentDto>> GetAccountConsentDetails(string userTckn,
        List<Consent> userAccountConsents, ConsentDbContext dbContext, IMapper mapper, IAccountService accountService)
    {
        List<ListAccountConsentDto> responseList = new List<ListAccountConsentDto>(); //Response list object
        ListAccountConsentDto detailedConsent;
        HesapBilgisiRizasiHHSDto? hesapBilgisiRizasi;

        //Get yos informations by yos codes 
        var yosCodes = userAccountConsents.SelectMany(c => c.OBAccountConsentDetails.Select(d => d.YosCode))
            .Distinct()
            .ToList();
        var yosList = await dbContext.OBYosInfos.AsNoTracking().Where(y => yosCodes.Contains(y.Kod)).ToListAsync();
        var permissions = await dbContext.OBPermissionTypes.AsNoTracking()
            .Where(p => p.Language == "tr-TR")
            .ToListAsync();
        var accountRefs = userAccountConsents.SelectMany(c => c.OBAccountConsentDetails.SelectMany(d => d.AccountReferences ?? Enumerable.Empty<string>()))
            .Distinct()
            .ToList();
        List<HesapBilgileriDto>? accounts = null;
        if (accountRefs?.Any() ?? false)
        {
            var getAccountInfoResult = await accountService.GetAuthorizedAccountsForUI(userTckn, accountRefs, null, null, null, null);
            if (getAccountInfoResult.Result)
            {
                accounts = (List<HesapBilgileriDto>?)getAccountInfoResult.Data;
            }
        }


        foreach (var consent in userAccountConsents)
        {
            //Generate consent detail object
            hesapBilgisiRizasi = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(consent.AdditionalData);
            if (hesapBilgisiRizasi is null)
            {
                continue;
            }

            detailedConsent = new ListAccountConsentDto()
            {
                ConsentId = consent.Id,
                CreatedAt = consent.CreatedAt,
                AccountReferences = consent.OBAccountConsentDetails?.FirstOrDefault()?.AccountReferences?.Select(aRef =>
                    new AccountRefDetailDto
                    {
                        AccountReference = aRef,
                        AccountName = accounts?.FirstOrDefault(a => a.hspTml.hspRef == aRef)?.hspTml.kisaAd
                    }).ToList() ?? new List<AccountRefDetailDto>(),
                YosInfo = mapper.Map<OBYosInfoDto>(yosList.FirstOrDefault(y =>
                    y.Kod == hesapBilgisiRizasi.katilimciBlg.yosKod))
            };
            detailedConsent.PermissionInformation = new PermissionInformationDto()
            {
                LastValidAccessDate = hesapBilgisiRizasi.hspBlg.iznBlg.erisimIzniSonTrh,
                TransactionInquiryEndTime = hesapBilgisiRizasi.hspBlg.iznBlg.hesapIslemBtsZmn,
                TransactionInquiryStartTime = hesapBilgisiRizasi.hspBlg.iznBlg.hesapIslemBslZmn,
                PermissionTypes = permissions.Where(p => hesapBilgisiRizasi.hspBlg.iznBlg.iznTur.Contains(p.Code))
                    .GroupBy(p => p.GroupId)
                    .Select(g => new PermissionTypeResponseDto()
                    {
                        GroupName = g.First().GroupName,
                        GroupId = g.Key,
                        PermissionNames = g.Select(p => p.Description).ToList()
                    })
                        .ToList()
            };

            responseList.Add(detailedConsent);
        }

        return responseList;
    }

}