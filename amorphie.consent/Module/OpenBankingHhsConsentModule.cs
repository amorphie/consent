using System.Globalization;
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
using amorphie.consent.core.DTO;
using amorphie.core.Base;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.Helper;
using amorphie.consent.Service;
using amorphie.consent.Service.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Dapr;
using System.Linq;

namespace amorphie.consent.Module;

public class OpenBankingHHSConsentModule : BaseBBTRoute<OpenBankingConsentDto, Consent, ConsentDbContext>
{
    public OpenBankingHHSConsentModule(WebApplication app)
        : base(app)
    {
    }

    public override string[]? PropertyCheckList => new string[] { "ConsentType", "State" };

    public override string? UrlFragment => "OpenBankingConsentHHS";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        base.AddRoutes(routeGroupBuilder);
        routeGroupBuilder.MapGet("/search", SearchMethod);
        routeGroupBuilder.MapGet("/GetAuthorizedAccountConsents", GetAuthorizedAccountConsentsByUserTCKN);
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
        routeGroupBuilder.MapGet("/hesaplar/bakiye", GetAuthorizedBalances)
            .AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapGet("/hesaplar/{hspRef}/bakiye", GetAuthorizedBalanceByHspRef)
            .AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapGet("/hesaplar/{hspRef}/islemler", GetTransactionsByHspRef)
            .AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapDelete("/hesap-bilgisi-rizasi/{rizaNo}", DeleteAccountConsentFromYos)
            .AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapDelete("/DeleteAccountConsentFromHHS/{rizaNo}", DeleteAccountConsentFromHHS);
        routeGroupBuilder.MapPost("/hesap-bilgisi-rizasi", AccountInformationConsentPost)
            .AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapPost("/odeme-emri-rizasi", PaymentConsentPost)
            .AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapPost("/UpdateAccountConsentForAuthorization", UpdateAccountConsentForAuthorization);
        routeGroupBuilder.MapPost("/UpdatePaymentConsentForAuthorization", UpdatePaymentConsentForAuthorization);
        routeGroupBuilder.MapPost("/UpdateConsentStatusForUsage", UpdateConsentStatusForUsage);
        routeGroupBuilder.MapPost("odeme-emri", PaymentOrderPost).AddEndpointFilter<OBCustomResponseHeaderFilter>();
        routeGroupBuilder.MapPost("updatePaymentState", UpdatePaymentState);
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
            var header = ModuleHelper.GetHeader(httpContext);
            string userTCKN = header.UserReference; //get logged in user tckn
            if (string.IsNullOrEmpty(userTCKN))
            {
                return Results.Unauthorized();
            }

            ApiResult getConsentsResponse = await authorizationService.GetAuthUsedAccountConsentsOfUser(userTCKN);
            if (getConsentsResponse.Result == false)
            {
                //Error in getting consents
                return Results.Problem(getConsentsResponse.Message);
            }

            var userAccountConsents = (List<Consent>?)getConsentsResponse.Data;
            if (!(userAccountConsents?.Any() ?? false))
            {
                //No authorized account consent in the system
                return Results.NotFound();
            }

            //Get consent details
            var consentDetails = await GetAccountConsentDetails(userTCKN, userAccountConsents, dbContext, mapper);
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
    /// <param name="rizaNo">Riza No</param>
    /// <param name="context">Context DB object</param>
    /// <param name="mapper">Aoutomapper object</param>
    /// <param name="configuration">Configuration object</param>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <param name="httpContext">Httpcontext object</param>
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
            //Check header fields
            ApiResult headerValidation = await IsHeaderDataValid(httpContext, configuration, yosInfoService);
            if (!headerValidation.Result)
            {
                //Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }

            //Check consent
            await ProcessAccountConsentToCancelOrEnd(rizaNo, context);
            var entity = await context.Consents
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == rizaNo
                                          && c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount);
            ApiResult isDataValidResult = IsDataValidToGetAccountConsent(entity);
            if (!isDataValidResult.Result) //Error in data validation
            {
                return Results.BadRequest(isDataValidResult.Message);
            }

            var accountConsent = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity.AdditionalData);
            ModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, accountConsent);
            return Results.Ok(accountConsent);
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
    /// <param name="customerId">Customer Id</param>
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
            var header = ModuleHelper.GetHeader(httpContext);
            //Check header fields
            ApiResult headerValidation = await IsHeaderDataValid(httpContext, configuration, yosInfoService, header,
                isUserRequired: true);
            if (!headerValidation.Result)
            {
                //Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }

            ApiResult accountApiResult =
                await accountService.GetAuthorizedAccountByHspRef(header.UserReference, yosCode: header.XTPPCode,
                    hspRef); //Get data from service
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
            var header = ModuleHelper.GetHeader(httpContext);
            //Check header fields
            ApiResult headerValidation = await IsHeaderDataValid(httpContext, configuration, yosInfoService, header,
                isUserRequired: true);
            if (!headerValidation.Result)
            {
                //Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }

            //Get authorized accounts
            ApiResult accountApiResult =
                await accountService.GetAuthorizedAccounts(header.UserReference, header.XTPPCode, syfKytSayi, syfNo,
                    srlmKrtr, srlmYon);
            if (!accountApiResult.Result)
            {
                return Results.BadRequest(accountApiResult.Message);
            }

            ModuleHelper.SetLinkHeader(httpContext, configuration);
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
    /// <param name="customerId">Customer Id</param>
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
            var header = ModuleHelper.GetHeader(httpContext);
            //Check header fields
            ApiResult headerValidation = await IsHeaderDataValid(httpContext, configuration, yosInfoService, header,
                isUserRequired: true);
            if (!headerValidation.Result)
            {
                //Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }

            ApiResult accountApiResult =
                await accountService.GetAuthorizedBalanceByHspRef(header.UserReference, yosCode: header.XTPPCode,
                    hspRef); //Get data from service
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
    /// Get all balances from service 
    /// </summary>
    /// <param name="customerId">Customer Id</param>
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
            var header = ModuleHelper.GetHeader(httpContext);
            //Check header fields
            ApiResult headerValidation = await IsHeaderDataValid(httpContext, configuration, yosInfoService, header,
                isUserRequired: true);
            if (!headerValidation.Result)
            {
                //Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }

            ApiResult accountApiResult = await accountService.GetAuthorizedBalances(header.UserReference,
                header.XTPPCode, syfKytSayi, syfNo, srlmKrtr, srlmYon);
            if (!accountApiResult.Result)
            {
                return Results.BadRequest(accountApiResult.Message);
            }

            ModuleHelper.SetLinkHeader(httpContext, configuration);
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
    /// <param name="hspRef">Hesap ref</param>
    /// <param name="context">Context DB object</param>
    /// <param name="mapper">Aoutomapper object</param>
    /// <param name="accountService">Account service class</param>
    /// <param name="configuration">Configuration instance</param>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <param name="httpContext">Httpcontext object</param>
    /// <returns>account transactions- IslemBilgileriDto type of object</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    public async Task<IResult> GetTransactionsByHspRef(
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
            //Check header fields
            ApiResult headerValidation = await IsHeaderDataValid(httpContext, configuration, yosInfoService);
            if (!headerValidation.Result)
            {
                //Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }

            ApiResult accountApiResult = await accountService.GetTransactionsByHspRef(hspRef);
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
    /// Get consent additional data by Id casting to OdemeEmriRizasiHHSDto type of object
    /// </summary>
    /// <param name="rizaNo"></param>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="tokenService"></param>
    /// <param name="configuration">Configuration instance</param>
    /// <param name="httpContext">Httpcontext object</param>
    /// <returns>OdemeEmriRizasiHHSDto type of object</returns>
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
            //Check header fields
            ApiResult headerValidation = await IsHeaderDataValid(httpContext, configuration, yosInfoService);
            if (!headerValidation.Result)
            {
                //Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }

            //Check consent
            await ProcessPaymentConsentToCancelOrEnd(rizaNo, context, tokenService);
            var entity = await context.Consents
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == rizaNo
                                          && c.ConsentType == ConsentConstants.ConsentType.OpenBankingPayment);
            ApiResult isDataValidResult = IsDataValidToGetPaymentConsent(entity);
            if (!isDataValidResult.Result) //Error in data validation
            {
                return Results.BadRequest(isDataValidResult.Message);
            }

            var paymentConsent = JsonSerializer.Deserialize<OdemeEmriRizasiHHSDto>(entity.AdditionalData);
            ModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, paymentConsent);
            return Results.Ok(paymentConsent);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    /// <summary>
    /// Get consent additional data by Id casting to OdemeEmriHHSDto type of object
    /// </summary>
    /// <param name="odemeEmriNo"></param>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="configuration">Configuration instance</param>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <param name="httpContext">Httpcontext object</param>
    /// <returns>OdemeEmriHHSDto type of object</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    [AddSwaggerParameter("user_reference", ParameterLocation.Header, true)]
    public async Task<IResult> GetPaymentOrderConsentById(Guid odemeEmriNo,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            var header = ModuleHelper.GetHeader(httpContext);
            //Check header fields
            ApiResult headerValidation = await IsHeaderDataValid(httpContext, configuration, yosInfoService,header,isUserRequired: true);
            if (!headerValidation.Result)
            {
                //Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }

            var entity = await context.OBPaymentOrders
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == odemeEmriNo);
            ApiResult isDataValidResult = IsDataValidToGetPaymentOrderConsent(entity);
            if (!isDataValidResult.Result) //Error in data validation
            {
                return Results.BadRequest(isDataValidResult.Message);
            }

            var serializedData = JsonSerializer.Deserialize<OdemeEmriHHSDto>(entity.AdditionalData);
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
            var header = ModuleHelper.GetHeader(httpContext);
            if (string.IsNullOrEmpty(header.UserReference))
            {
                //Missing header fields
                return Results.BadRequest("Header userreference can not be empty");
            }

            //Check consent
            await ProcessConsentToCancelOrEnd(rizaNo, dbContext, tokenService);
            string userTCKN = header.UserReference; //get logged in user tckn
            List<string> consentTypes = new List<string>()
                { ConsentConstants.ConsentType.OpenBankingAccount, ConsentConstants.ConsentType.OpenBankingPayment };
            //Get consent
            var getConsentResult = await authorizationService.GetConsentReadonly(id: rizaNo, userTCKN: userTCKN,
                consentState: OpenBankingConstants.RizaDurumu.YetkiBekleniyor,
                consentTypes: consentTypes);

            if (getConsentResult.Result == false)
            {
                //Error in getting consents
                return Results.Problem(getConsentResult.Message);
            }

            var consent = (Consent?)getConsentResult.Data;
            if (consent == null)
            {
                //No consent in the system
                return Results.NotFound();
            }

            //Generate response
            ConsentWebViewInfoDto response = new ConsentWebViewInfoDto()
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
            url = string.Format(configuration["OB_AccountProjectURL"], id) ?? string.Empty;
        }
        else if (consentType == ConsentConstants.ConsentType.OpenBankingPayment)
        {
            url = string.Format(configuration["OB_PaymentProjectURL"], id) ?? string.Empty;
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
                return await UpdateAccountConsentStatusForUsage(updateConsentState, context, mapper);
            }
            else if (entity.ConsentType == ConsentConstants.ConsentType.OpenBankingPayment)
            {
                //Payment consent
                return await UpdatePaymentConsentStatusForUsage(updateConsentState, context, mapper, tokenService);
            }
            else
            {
                //Not related type
                return Results.BadRequest("Consent type not valid");
            }
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
    /// <param name="tokenService"></param>
    /// <returns></returns>
    private async Task<IResult> UpdatePaymentConsentStatusForUsage(UpdateConsentStateDto updateConsentState,
        ConsentDbContext context,
        IMapper mapper,
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
            var additionalData = JsonSerializer.Deserialize<OdemeEmriRizasiHHSDto>(entity.AdditionalData);
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

            //Set account reference
            var detail = entity.OBPaymentConsentDetails.FirstOrDefault();
            var additionalData = JsonSerializer.Deserialize<OdemeEmriRizasiWithMsrfTtrHHSDto>(entity.AdditionalData);
            //Check and set sender account
            if (additionalData.odmBsltm.gon == null
                || (string.IsNullOrEmpty(additionalData.odmBsltm.gon.hspNo)
                    && string.IsNullOrEmpty(additionalData.odmBsltm.gon.hspRef)))
            {
                additionalData.odmBsltm.gon = savePCStatusSenderAccount.SenderAccount;
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
                    OpenBankingConstants.OlayTip.AyrikGKDBasarili, OpenBankingConstants.KaynakTip.OdemeEmriRizasi);
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
        ConsentDbContext context,
        IMapper mapper)
    {
        try
        {
            //Check consent validity for cancel consent
            await ProcessAccountConsentToCancelOrEnd(updateConsentState.Id, context);
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == updateConsentState.Id
                                          && c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount);
            //Check consent validity
            ApiResult isDataValidResult = IsDataValidToUpdateAccountConsentStatusForUsage(entity, updateConsentState);
            if (!isDataValidResult.Result) //Error in data validation
            {
                return Results.BadRequest(isDataValidResult.Message);
            }

            //Set permissions
            var additionalData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity.AdditionalData);
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
            var additionalData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(consentEntity.AdditionalData);
            additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.Yetkilendirildi;
            additionalData.rzBlg.gnclZmn = DateTime.UtcNow;
            consentEntity.AdditionalData = JsonSerializer.Serialize(additionalData);
            consentEntity.State = OpenBankingConstants.RizaDurumu.Yetkilendirildi;
            consentEntity.StateModifiedAt = DateTime.UtcNow;
            consentEntity.ModifiedAt = DateTime.UtcNow;

            //Set account reference
            var detail = consentEntity.OBAccountConsentDetails.FirstOrDefault();
            detail.AccountReferences = saveAccountReference.AccountReferences;
            detail.ModifiedAt = DateTime.UtcNow;
            context.OBAccountConsentDetails.Update(detail);
            context.Consents.Update(consentEntity);
            await context.SaveChangesAsync();
            //If ayrikGKD, post olay-dinleme to YOS
            if (additionalData.gkd.yetYntm == OpenBankingConstants.GKDTur.Ayrik)
            {
                await obEventService.DoEventProcess(consentEntity.Id.ToString(), additionalData.katilimciBlg,
                    OpenBankingConstants.OlayTip.AyrikGKDBasarili, OpenBankingConstants.KaynakTip.HesapBilgisiRizasi);
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
    /// <param name="rizaIstegi">Request for account consent</param>
    /// <param name="context">DBContext object</param>
    /// <param name="mapper">Mapper object</param>
    /// <param name="configuration"></param>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    protected async Task<IResult> AccountInformationConsentPost([FromBody] HesapBilgisiRizaIstegiHHSDto rizaIstegi,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            //Check if post data is valid to process.
            var checkValidationResult =
                await IsDataValidToAccountConsentPost(rizaIstegi, configuration, yosInfoService, httpContext, context);
            if (!checkValidationResult.Result)
            {
                //Data not valid
                return Results.BadRequest(checkValidationResult.Message);
            }

            //Get user's active account consents from db
            var activeAccountConsents = await GetActiveAccountConsentsOfUser(rizaIstegi, context);
            if (AnyAuthAndUsedConsents(activeAccountConsents)) //Checks any authorized or authused state consent
            {
                return Results.BadRequest(
                    "TR.OHVPS.Resource.ConsentMismatch. There is already authorized account consent in system. First cancel the consent.");
            }

            //Cancel Yetki Bekleniyor state consents.
            CancelWaitingApproveConsents(context, activeAccountConsents);
            var header = ModuleHelper.GetHeader(httpContext);
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
            if (isAyrikGKD == false)
            {
                hesapBilgisiRizasi.gkd.hhsYonAdr = string.Format(configuration["HHSForwardingAddress"] ?? string.Empty,
                    consentEntity.Id.ToString());
            }

            hesapBilgisiRizasi.gkd.yetTmmZmn = DateTime.UtcNow.AddMinutes(5);
            consentEntity.AdditionalData = JsonSerializer.Serialize(hesapBilgisiRizasi);
            consentEntity.State = OpenBankingConstants.RizaDurumu.YetkiBekleniyor;
            consentEntity.StateModifiedAt = DateTime.UtcNow;
            consentEntity.ConsentType = ConsentConstants.ConsentType.OpenBankingAccount;
            consentEntity.Variant = hesapBilgisiRizasi.katilimciBlg.yosKod;
            consentEntity.ClientCode = string.Empty;
            consentEntity.OBAccountConsentDetails = new List<OBAccountConsentDetail>
            {
                GenerateAccountConsentDetailObject(hesapBilgisiRizasi, header)
            };
            context.Consents.Add(consentEntity);
            await context.SaveChangesAsync();
            if (isAyrikGKD)
            {
                //Send notification to user
                //TODO:Özlem call send notification
            }

            ModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, hesapBilgisiRizasi);
            return Results.Ok(hesapBilgisiRizasi);
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
            var header = ModuleHelper.GetHeader(httpContext);
            //Check header fields
            ApiResult headerValidation =
                await IsHeaderDataValid(httpContext, configuration, yosInfoService, header: header,
                    isUserRequired: true);
            if (!headerValidation.Result)
            {
                //Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }

            //Check consent
            await ProcessAccountConsentToCancelOrEnd(rizaNo, context);

            //get consent entity from db
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == rizaNo
                                          && c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                                          && c.Variant == header.XTPPCode);
            ApiResult dataValidationResult = IsDataValidToDeleteAccountConsent(entity); //Check data validation
            if (!dataValidationResult.Result)
            {
                //Data not valid
                return Results.BadRequest(dataValidationResult.Message);
            }

            //Update consent rıza bilgileri properties
            var additionalData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity.AdditionalData);
            additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiIptal;
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
        HttpContext httpContext)
    {
        try
        {
            //Get header fields
            var header = ModuleHelper.GetHeader(httpContext);
            //Check consent to cancel&/end
            await ProcessAccountConsentToCancelOrEnd(rizaNo, context);

            //get consent entity from db
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == rizaNo
                                          && c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount);
            ApiResult dataValidationResult =
                IsDataValidToDeleteAccountConsentFromHHS(entity, header.UserReference); //Check data validation
            if (!dataValidationResult.Result)
            {
                //Data not valid
                return Results.BadRequest(dataValidationResult.Message);
            }

            //Update consent rıza bilgileri properties
            var additionalData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity.AdditionalData);
            additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiIptal;
            additionalData.rzBlg.rizaIptDtyKod =
                OpenBankingConstants.RizaIptalDetayKodu.KullaniciIstegiIleHHSUzerindenIptal;
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


    /// <summary>
    /// Does payment information consent post process.
    /// odeme-emri-rizasi post method.
    /// Checks OdemeEmriRizaIstegi object data and generates OdemeEmriRizasi object and insert.
    /// </summary>
    /// <param name="rizaIstegi">Request object</param>
    /// <param name="context">DB Context</param>
    /// <param name="mapper">Mapping object</param>
    /// <param name="configuration">Configuration instance</param>
    /// <param name="paymentService">Payment service instance</param>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <param name="httpContext">Httpcontext object to get header data</param>
    /// <returns>OdemeEmriRizasi object</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    protected async Task<IResult> PaymentConsentPost([FromBody] OdemeEmriRizaIstegiHHSDto rizaIstegi,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IPaymentService paymentService,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            //Check if post data is valid to process.
            var dataValidationResult =
                await IsDataValidToPaymentConsentPost(rizaIstegi, configuration, yosInfoService, httpContext, context);
            if (!dataValidationResult.Result)
            {
                //Data not valid
                return Results.BadRequest(dataValidationResult.Message);
            }

            ApiResult paymentServiceResponse = await paymentService.SendOdemeEmriRizasi(rizaIstegi);
            if (!paymentServiceResponse.Result) //Error in service
                return Results.BadRequest(paymentServiceResponse.Message);

            var header = ModuleHelper.GetHeader(httpContext); //Get header
            var consentEntity = new Consent();
            context.Consents.Add(consentEntity);
            //Generate response object
            OdemeEmriRizasiWithMsrfTtrHHSDto odemeEmriRizasi =
                (OdemeEmriRizasiWithMsrfTtrHHSDto)paymentServiceResponse.Data;
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
            if (isAyrikGKD == false)
            {
                odemeEmriRizasi.gkd.hhsYonAdr = string.Format(configuration["HHSForwardingAddress"] ?? string.Empty,
                    consentEntity.Id.ToString());
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
                GeneratePaymentConsentDetailObject(odemeEmriRizasi, header)
            };

            context.Consents.Add(consentEntity);
            await context.SaveChangesAsync();
            if (isAyrikGKD)
            {
                //Send notification to user
                //TODO:Özlem call send notification
            }

            var resObject = mapper.Map<OdemeEmriRizasiHHSDto>(odemeEmriRizasi);
            ModuleHelper.SetXJwsSignatureHeader(httpContext, configuration, resObject);
            //Send consent to YOS without hhsmsrfttr property
            return Results.Ok(resObject);
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
    /// <param name="odemeEmriIstegi">Request object</param>
    /// <param name="context">DB Context</param>
    /// <param name="mapper">Mapping object</param>
    /// <param name="configuration">Configuration instance</param>
    /// <param name="paymentService"/>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <param name="httpContext">Httpcontext object</param>
    /// <returns>OdemeEmri object</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    protected async Task<IResult> PaymentOrderPost([FromBody] OdemeEmriIstegiHHSDto odemeEmriIstegi,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IPaymentService paymentService,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            //Check if post data is valid to process.
            var dataValidationResult = await IsDataValidToPaymentOrderPost(odemeEmriIstegi, context, yosInfoService,
                httpContext, configuration);
            if (!dataValidationResult.Result)
            {
                //Data not valid
                return Results.BadRequest(dataValidationResult.Message);
            }

            //Send payment order to payment service
            ApiResult paymentServiceResponse = await paymentService.SendOdemeEmri(odemeEmriIstegi);
            if (!paymentServiceResponse.Result) //Error in service
                return Results.BadRequest(paymentServiceResponse.Message);
            //TODO:Özlem error oluşma caseleri için konuş

            OdemeEmriHHSDto odemeEmriDto = (OdemeEmriHHSDto)paymentServiceResponse.Data;

            //Update consent state
            Consent paymentConsentEntity = (Consent)dataValidationResult.Data; //odemeemririzasi entity
            var additionalData = JsonSerializer.Deserialize<OdemeEmriRizasiHHSDto>(paymentConsentEntity.AdditionalData);
            additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiOdemeEmrineDonustu;
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

            var header = ModuleHelper.GetHeader(httpContext); //Get header
            orderEntity.XRequestId = header.XRequestID ?? string.Empty;
            orderEntity.XGroupId = header.XRequestID ?? string.Empty;
            context.OBPaymentOrders.Add(orderEntity);

            await context.SaveChangesAsync();
            return Results.Ok(odemeEmriDto);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }

    private static void SetPaymentSystemNumberFields(OdemeEmriHHSDto odemeEmriDto, OBPaymentOrder orderEntity)
    {
        var systemNumberItems = odemeEmriDto.odmBsltm.odmAyr.odmStmNo.Split('|').ToArray() ?? null;
        orderEntity.PSNDate = systemNumberItems?[0] ?? null;
        orderEntity.PSNYosCode = systemNumberItems?[1] ?? null;
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
        OpenBankingConstants.KafkaInformation.UpdatePaymentStatusTopicName, true)]
    [HttpPost]
    public async Task<IResult> UpdatePaymentState(
        [FromServices] ConsentDbContext context,
        [FromServices] IOBEventService obEventService,
        [FromServices] IYosInfoService yosInfoService,
        HttpContext httpContext)
    {
        try
        {
            //Get payment system record.
            PaymentRecordDto model = await httpContext.Deserialize<PaymentRecordDto>();
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
                        DateTime.ParseExact(o.PSNDate, "yyyy-MM-dd HH:mm:ss", null).Date == utcTranDate.Date);
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
                        additionalData.odmBsltm.odmAyr.odmDrm = paymentState;
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
                            && (bool)yosHasSubscription.Data)
                        {
                            //Yos has subscrition. Do event process.
                            await obEventService.DoEventProcess(paymentOrderEntity.Id.ToString(),
                                additionalData.katilimciBlg,
                                OpenBankingConstants.OlayTip.KaynakGuncellendi,
                                OpenBankingConstants.KaynakTip.OdemeEmri);
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
            }
        }

        return paymentState;
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
            .Include(c => c.Tokens)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(consentSearch.Keyword))
        {
            string keyword = consentSearch.Keyword.ToLower();
            query = query.AsNoTracking().Where(x => EF.Functions
                .ToTsVector("english", string.Join(" ", x.State, x.ConsentType, x.AdditionalData))
                .Matches(EF.Functions.PlainToTsQuery("english", consentSearch.Keyword)));
        }

        IList<Consent> resultList = await query.OrderBy(x => x.CreatedAt)
            .Skip(skipRecords)
            .Take(consentSearch.PageSize)
            .ToListAsync(token);

        return (resultList != null && resultList.Count > 0)
            ? Results.Ok(mapper.Map<IList<OpenBankingConsentDto>>(resultList))
            : Results.NoContent();
    }


    /// <summary>
    /// Checks if data is valid for account consent post process
    /// </summary>
    /// <param name="rizaIstegi">To be checked data</param>
    /// <param name="configuration">Config object</param>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <param name="httpContext">Context object to get header parameters</param>
    /// <exception cref="NotImplementedException"></exception>
    private async Task<ApiResult> IsDataValidToAccountConsentPost(HesapBilgisiRizaIstegiHHSDto rizaIstegi,
        IConfiguration configuration,
        IYosInfoService yosInfoService,
        HttpContext httpContext,
        ConsentDbContext dbContext)
    {
        //TODO:Ozlem Check if user is customer
        //TODO:Ozlem Check fields length and necessity

        ApiResult result = new();
        var header = ModuleHelper.GetHeader(httpContext);
        //Check header fields
        result = await IsHeaderDataValid(httpContext, configuration, yosInfoService, header,
            katilimciBlg: rizaIstegi.katilimciBlg);
        if (!result.Result)
        {
            //validation error in header fields
            return result;
        }

        //Check message required basic properties
        if (rizaIstegi.katilimciBlg is null
            || rizaIstegi.gkd == null
            || rizaIstegi.kmlk == null
            || rizaIstegi.hspBlg == null
            || rizaIstegi.hspBlg.iznBlg == null)
        {
            result.Result = false;
            result.Message =
                "katilimciBlg, gkd,odmBsltm, kmlk, hspBlg, spBlg.iznBlg should be in consent request message";
            return result;
        }

        //Check KatılımcıBilgisi
        if (string.IsNullOrEmpty(rizaIstegi.katilimciBlg.hhsKod) //Required fields
            || string.IsNullOrEmpty(rizaIstegi.katilimciBlg.yosKod)
            || configuration["HHSCode"] != rizaIstegi.katilimciBlg.hhsKod)
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. HHSKod YOSKod required";
            return result;
        }

        //Check GKD
        if (!IsGkdValid(rizaIstegi.gkd))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. GKD data not valid.";
            return result;
        }

        //Check if yos has subscription
        if (rizaIstegi.gkd.yetYntm == OpenBankingConstants.GKDTur.Ayrik
            && !await IsSubscsribed(rizaIstegi.katilimciBlg.yosKod, ConsentConstants.ConsentType.OpenBankingAccount,
                dbContext))
        {
            result.Result = false;
            result.Message = "Yos does not have subsription for Ayrik GKD";
            return result;
        }


        //Check Kimlik
        if (string.IsNullOrEmpty(rizaIstegi.kmlk.kmlkTur) //Check required fields
            || string.IsNullOrEmpty(rizaIstegi.kmlk.kmlkVrs)
            || (string.IsNullOrEmpty(rizaIstegi.kmlk.krmKmlkTur) != string.IsNullOrEmpty(rizaIstegi.kmlk.krmKmlkVrs))
            || string.IsNullOrEmpty(rizaIstegi.kmlk.ohkTur)
            || !ConstantHelper.GetKimlikTurList().Contains(rizaIstegi.kmlk.kmlkTur)
            || !ConstantHelper.GetOHKTurList().Contains(rizaIstegi.kmlk.ohkTur)
            || (!string.IsNullOrEmpty(rizaIstegi.kmlk.krmKmlkTur) &&
                !ConstantHelper.GetKurumKimlikTurList().Contains(rizaIstegi.kmlk.krmKmlkTur)))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. Kmlk data is not valid";
            return result;
        }

        //Check field constraints
        if ((rizaIstegi.kmlk.kmlkTur == OpenBankingConstants.KimlikTur.TCKN &&
             rizaIstegi.kmlk.kmlkVrs.Trim().Length != 11)
            || (rizaIstegi.kmlk.kmlkTur == OpenBankingConstants.KimlikTur.MNO &&
                rizaIstegi.kmlk.kmlkVrs.Trim().Length > 30)
            || (rizaIstegi.kmlk.kmlkTur == OpenBankingConstants.KimlikTur.YKN &&
                rizaIstegi.kmlk.kmlkVrs.Trim().Length != 11)
            || (rizaIstegi.kmlk.kmlkTur == OpenBankingConstants.KimlikTur.PNO &&
                (rizaIstegi.kmlk.kmlkVrs.Trim().Length < 7 || rizaIstegi.kmlk.kmlkVrs.Length > 9))
            || (rizaIstegi.kmlk.krmKmlkTur == OpenBankingConstants.KurumKimlikTur.TCKN &&
                rizaIstegi.kmlk.kmlkVrs.Trim().Length != 11)
            || (rizaIstegi.kmlk.krmKmlkTur == OpenBankingConstants.KurumKimlikTur.MNO &&
                rizaIstegi.kmlk.kmlkVrs.Trim().Length > 30)
            || (rizaIstegi.kmlk.krmKmlkTur == OpenBankingConstants.KurumKimlikTur.VKN &&
                rizaIstegi.kmlk.kmlkVrs.Trim().Length != 10))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. Kmlk data validation failed.";
            return result;
        }


        //Check HesapBilgisi
        //Check izinbilgisi properties
        if ((rizaIstegi.hspBlg.iznBlg.iznTur?.Any() ?? false) == false
            || rizaIstegi.hspBlg.iznBlg.iznTur.Any(i => !ConstantHelper.GetIzinTurList().Contains(i))
            || rizaIstegi.hspBlg.iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.TemelHesapBilgisi) == false
            || (rizaIstegi.hspBlg.iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.AyrintiliIslemBilgisi) &&
                !rizaIstegi.hspBlg.iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.TemelIslemBilgisi))
            || (rizaIstegi.hspBlg.iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.AnlikBakiyeBildirimi) &&
                !rizaIstegi.hspBlg.iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.BakiyeBilgisi)))
        {
            result.Result = false;
            result.Message =
                "TR.OHVPS.Resource.InvalidFormat. IznBld iznTur check failed. IznTur required and should contain TemelHesapBilgisi permission.";
            return result;
        }

        if (rizaIstegi.hspBlg.iznBlg.erisimIzniSonTrh == System.DateTime.MinValue
            || rizaIstegi.hspBlg.iznBlg.erisimIzniSonTrh > System.DateTime.UtcNow.AddMonths(6)
            || rizaIstegi.hspBlg.iznBlg.erisimIzniSonTrh < System.DateTime.UtcNow.AddDays(1))
        {
            result.Result = false;
            result.Message =
                "TR.OHVPS.Resource.InvalidFormat. IznBld erisimIzniSonTrh data check failed. It should be between tomorrow and 6 months later ";
            return result;
        }

        //Check işlem sorgulama başlangıç zamanı
        if (rizaIstegi.hspBlg.iznBlg.hesapIslemBslZmn.HasValue)
        {
            //Temel işlem bilgisi ve/veya ayrıntılı işlem bilgisi seçilmiş olması gerekir
            if (rizaIstegi.hspBlg.iznBlg?.iznTur?.Any(p => p == OpenBankingConstants.IzinTur.TemelIslemBilgisi
                                                           || p == OpenBankingConstants.IzinTur
                                                               .AyrintiliIslemBilgisi) == false)
            {
                result.Result = false;
                result.Message =
                    "TR.OHVPS.Resource.InvalidFormat. IznTur temelislem or ayrintiliIslem should be selected.";
                return result;
            }

            if (rizaIstegi.hspBlg.iznBlg?.hesapIslemBslZmn.Value < DateTime.UtcNow.AddMonths(-12)) //Data constraints
            {
                result.Result = false;
                result.Message =
                    "TR.OHVPS.Resource.InvalidFormat. hesapIslemBslZmn not valid. Maximum 12 months before.";
                return result;
            }
        }

        if (rizaIstegi.hspBlg.iznBlg.hesapIslemBtsZmn.HasValue) //Check işlem sorgulama bitiş zamanı
        {
            //Temel işlem bilgisi ve/veya ayrıntılı işlem bilgisi seçilmiş olması gerekir
            if (rizaIstegi.hspBlg.iznBlg?.iznTur?.Any(p => p == OpenBankingConstants.IzinTur.TemelIslemBilgisi
                                                           || p == OpenBankingConstants.IzinTur
                                                               .AyrintiliIslemBilgisi) == false)
            {
                result.Result = false;
                result.Message =
                    "TR.OHVPS.Resource.InvalidFormat IznTur temelislem or ayrintiliIslem should be selected.";
                return result;
            }

            if (rizaIstegi.hspBlg.iznBlg?.hesapIslemBtsZmn.Value > DateTime.UtcNow.AddMonths(12)) //Data constraints
            {
                result.Result = false;
                result.Message =
                    "TR.OHVPS.Resource.InvalidFormat. hesapIslemBtsZmn not valid. Maximum 12 months later.";
                return result;
            }
        }

        return result;
    }


    /// <summary>
    ///  Checks if data is valid for payment information consent post process
    /// </summary>
    /// <param name="rizaIstegi">To be checked data</param>
    /// <param name="configuration">Config file</param>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <param name="httpContext">HttpContext httpContext</param>
    /// <param name="dbContext"></param>
    /// <returns></returns>
    private async Task<ApiResult> IsDataValidToPaymentConsentPost(OdemeEmriRizaIstegiHHSDto rizaIstegi,
        IConfiguration configuration,
        IYosInfoService yosInfoService,
        HttpContext httpContext,
        ConsentDbContext dbContext)
    {
        //TODO:Ozlem update method
        ApiResult result = new();
        var header = ModuleHelper.GetHeader(httpContext); //Get header
        //Check header fields
        result = await IsHeaderDataValid(httpContext, configuration, yosInfoService, header,
            katilimciBlg: rizaIstegi.katilimciBlg);
        if (!result.Result)
        {
            //validation error in header fields
            return result;
        }

        //Check message required basic properties
        if (rizaIstegi is null
            || rizaIstegi.katilimciBlg is null
            || rizaIstegi.gkd == null
            || rizaIstegi.odmBsltm == null
            || rizaIstegi.odmBsltm.kmlk == null
            || rizaIstegi.odmBsltm.islTtr == null
            || rizaIstegi.odmBsltm.alc == null
            || rizaIstegi.odmBsltm.odmAyr == null)
        {
            result.Result = false;
            result.Message =
                "katilimciBlg, gkd,odmBsltm,odmBsltm-kmlk, odmBsltm-islttr, odmBsltm-alc, odmBsltm-odmAyr should be in consent request message";
            return result;
        }

        //Check KatılımcıBilgisi
        if (string.IsNullOrEmpty(rizaIstegi.katilimciBlg.hhsKod) //Required fields
            || string.IsNullOrEmpty(rizaIstegi.katilimciBlg.yosKod)
            || configuration["HHSCode"] != rizaIstegi.katilimciBlg.hhsKod)
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. HHSKod YOSKod required.";
            return result;
        }

        //Check GKD
        if (!IsGkdValid(rizaIstegi.gkd))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. GKD data not valid.";
            return result;
        }

        //Check if yos has subscription
        if (rizaIstegi.gkd.yetYntm == OpenBankingConstants.GKDTur.Ayrik
            && !await IsSubscsribed(rizaIstegi.katilimciBlg.yosKod, ConsentConstants.ConsentType.OpenBankingPayment,
                dbContext))
        {
            result.Result = false;
            result.Message = "Yos does not have subsription for Ayrik GKD";
            return result;
        }

        //Check odmBsltm  Kimlik field validities
        if (string.IsNullOrEmpty(rizaIstegi.odmBsltm.kmlk.ohkTur)
            || !ConstantHelper.GetOHKTurList().Contains(rizaIstegi.odmBsltm.kmlk.ohkTur)
            || (rizaIstegi.odmBsltm.kmlk.ohkTur == OpenBankingConstants.OHKTur.Bireysel
                && (string.IsNullOrEmpty(rizaIstegi.odmBsltm.kmlk.kmlkTur)
                    || string.IsNullOrEmpty(rizaIstegi.odmBsltm.kmlk.kmlkVrs)
                    || !ConstantHelper.GetKimlikTurList().Contains(rizaIstegi.odmBsltm.kmlk.kmlkTur)))
            || (rizaIstegi.odmBsltm.kmlk.ohkTur == OpenBankingConstants.OHKTur.Kurumsal
                && (string.IsNullOrEmpty(rizaIstegi.odmBsltm.kmlk.krmKmlkTur)
                    || string.IsNullOrEmpty(rizaIstegi.odmBsltm.kmlk.krmKmlkVrs)
                    || !ConstantHelper.GetKurumKimlikTurList().Contains(rizaIstegi.odmBsltm.kmlk.krmKmlkTur))))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. odmBsltm => Kmlk data is not valid";
            return result;
        }

        //Check odmBsltma Islem Tutarı
        if (string.IsNullOrEmpty(rizaIstegi.odmBsltm.islTtr.ttr) //Check required fields
            || string.IsNullOrEmpty(rizaIstegi.odmBsltm.islTtr.prBrm))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. odmBsltm => islTtr required fields empty";
            return result;
        }

        //Check odmBsltma Alıcı
        //Kolay Adres Sistemi kullanılmıyorsa zorunludur.
        if (rizaIstegi.odmBsltm.alc.kolas == null
            && (string.IsNullOrEmpty(rizaIstegi.odmBsltm.alc.unv)
                || string.IsNullOrEmpty(rizaIstegi.odmBsltm.alc.hspNo)))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. If kolas is null, unv and hspno is required";
            return result;
        }

        if (rizaIstegi.odmBsltm.alc.kolas != null
            && (string.IsNullOrEmpty(rizaIstegi.odmBsltm.alc.kolas.kolasDgr)
                || string.IsNullOrEmpty(rizaIstegi.odmBsltm.alc.kolas.kolasTur)
                || !ConstantHelper.GetKolasTurList().Contains(rizaIstegi.odmBsltm.alc.kolas.kolasTur)))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. alc-kolas-kolasDgr, alc-kolas-kolasTur required fields.";
            return result;
        }

        if (rizaIstegi.odmBsltm.alc.kolas != null
            && rizaIstegi.odmBsltm.kkod != null)
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. Kolas and KareKod can not be used at the same time";
            return result;
        }

        if (rizaIstegi.odmBsltm.kkod != null
            && (string.IsNullOrEmpty(rizaIstegi.odmBsltm.kkod.aksTur)
                || string.IsNullOrEmpty(rizaIstegi.odmBsltm.kkod.kkodUrtcKod)
                || !ConstantHelper.GetKolasTurList().Contains(rizaIstegi.odmBsltm.kkod.aksTur)))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. aksTur, kkodUrtcKod required fields.";
            return result;
        }

        //Check odemeayrintilari
        if (string.IsNullOrEmpty(rizaIstegi.odmBsltm.odmAyr.odmKynk)
            || string.IsNullOrEmpty(rizaIstegi.odmBsltm.odmAyr.odmAcklm))
        {
            result.Result = false;
            result.Message =
                "TR.OHVPS.Resource.InvalidFormat. odmBsltm-odmAyr-odmAcklm, odmBsltm-odmAyr-odmKynk required fields.";
            return result;
        }

        if (rizaIstegi.odmBsltm.odmAyr.odmKynk !=
            OpenBankingConstants.OdemeKaynak.AcikBankacilikAraciligiIleGonderilenOdemelerde)
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat.  odmBsltm-odmAyr-odmKynk must be O.";
            return result;
        }

        if (!ConstantHelper.GetOdemeAmaciList().Contains(rizaIstegi.odmBsltm.odmAyr.odmAmc))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat.  odmBsltm-odmAyr-odmAmc value is wrong.";
            return result;
        }

        //Check isyOdmBlg data
        if (rizaIstegi.isyOdmBlg != null
            && ((!string.IsNullOrEmpty(rizaIstegi.isyOdmBlg.isyKtgKod) && rizaIstegi.isyOdmBlg.isyKtgKod.Length != 4 )
                || (!string.IsNullOrEmpty(rizaIstegi.isyOdmBlg.altIsyKtgKod) && rizaIstegi.isyOdmBlg.altIsyKtgKod.Length != 4 )
                || (!string.IsNullOrEmpty(rizaIstegi.isyOdmBlg.genelUyeIsyeriNo) && rizaIstegi.isyOdmBlg.genelUyeIsyeriNo.Length != 8 )))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat.  isyOdmBlg value validation error.";
            return result;
        }

        return result;
    }

    /// <summary>
    /// Check if consent is valid to payment order post
    /// </summary>
    /// <param name="odemeEmriIstegi">request object</param>
    /// <param name="context">Context object</param>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <param name="httpContext">Httpcontext instance</param>
    /// <param name="configuration">Configuration instance</param>
    /// <returns></returns>
    private async Task<ApiResult> IsDataValidToPaymentOrderPost(OdemeEmriIstegiHHSDto odemeEmriIstegi,
        ConsentDbContext context,
        IYosInfoService yosInfoService,
        HttpContext httpContext,
        IConfiguration configuration)
    {
        ApiResult result = new();
        var header = ModuleHelper.GetHeader(httpContext); //Get header
        //Check header fields
        result = await IsHeaderDataValid(httpContext, configuration, yosInfoService, header,
            katilimciBlg: odemeEmriIstegi.katilimciBlg);
        if (!result.Result)
        {
            //validation error in header fields
            return result;
        }

        //Check message required basic properties
        if (odemeEmriIstegi == null
            || odemeEmriIstegi.rzBlg == null
            || odemeEmriIstegi.katilimciBlg is null
            || odemeEmriIstegi.gkd == null
            || odemeEmriIstegi.odmBsltm == null
            || odemeEmriIstegi.odmBsltm.kmlk == null
            || odemeEmriIstegi.odmBsltm.islTtr == null
            || odemeEmriIstegi.odmBsltm.alc == null
            || odemeEmriIstegi.odmBsltm.gon == null
            || odemeEmriIstegi.odmBsltm.odmAyr == null)
        {
            result.Result = false;
            result.Message =
                "rzBlg, katilimciBlg, gkd,odmBsltm,odmBsltm-kmlk, odmBsltm-islttr, odmBsltm-alc, odmBsltm-odmAyr should be in payment order message";
            return result;
        }

        //Check rzBlg
        if (string.IsNullOrEmpty(odemeEmriIstegi.rzBlg.rizaDrm) //Required fields
            || string.IsNullOrEmpty(odemeEmriIstegi.rzBlg.rizaNo)
            || !Guid.TryParse(odemeEmriIstegi.rzBlg.rizaNo, out Guid rizaNo)
            || odemeEmriIstegi.rzBlg.olusZmn == DateTime.MinValue || odemeEmriIstegi.rzBlg.olusZmn == null
            || !ConstantHelper.GetRizaDurumuList().Contains(odemeEmriIstegi.rzBlg.rizaDrm))
        {
            result.Result = false;
            result.Message =
                "TR.OHVPS.Resource.InvalidFormat. RZBlg rizaDrm, rizaNo, olusZmn values are required. rizaDrm should be in defined datas.";
            return result;
        }

        //Check KatılımcıBilgisi
        if (string.IsNullOrEmpty(odemeEmriIstegi.katilimciBlg.hhsKod) //Required fields
            || string.IsNullOrEmpty(odemeEmriIstegi.katilimciBlg.yosKod)
            || configuration["HHSCode"] != odemeEmriIstegi.katilimciBlg.hhsKod)
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. HHSKod YOSKod required / HHSKod is wrong.";
            return result;
        }

        //Check GKD
        if (!IsGkdValid(odemeEmriIstegi.gkd))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. GKD data not valid.";
            return result;
        }


        //Check odmBsltm  Kimlik
        if (string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.kmlk.ohkTur)
            || !ConstantHelper.GetOHKTurList().Contains(odemeEmriIstegi.odmBsltm.kmlk.ohkTur)
            || string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.kmlk.kmlkTur)
            || !ConstantHelper.GetKimlikTurList().Contains(odemeEmriIstegi.odmBsltm.kmlk.kmlkTur)
            || string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.kmlk.kmlkVrs)
            || (odemeEmriIstegi.odmBsltm.kmlk.ohkTur == OpenBankingConstants.OHKTur.Kurumsal
                && (string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.kmlk.krmKmlkTur)
                    || string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.kmlk.krmKmlkVrs)
                    || !ConstantHelper.GetKurumKimlikTurList().Contains(odemeEmriIstegi.odmBsltm.kmlk.krmKmlkTur))))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. ohkTur, kmlkTur, kmlkVrs required.";
            return result;
        }

        //Check odmBsltma Islem Tutarı
        if (string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.islTtr.ttr) //Check required fields
            || string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.islTtr.prBrm))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. odmBsltm => islTtr required fields empty";
            return result;
        }

        //Check odmBsltma gonHesap 
        if (string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.gon.unv))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. odmBsltma gon unv is require.";
            return result;
        }

        //Check odmBsltma alc 
        if (string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.alc.unv)
            || string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.alc.hspNo))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. odmBsltma alc unv, hspNo is require.";
            return result;
        }

        //Check odmBsltma alc kolas
        if (odemeEmriIstegi.odmBsltm.alc.kolas != null
            && (string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.alc.kolas.kolasDgr)
                || string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.alc.kolas.kolasTur)
                || !ConstantHelper.GetKolasTurList().Contains(odemeEmriIstegi.odmBsltm.alc.kolas.kolasTur)
                || odemeEmriIstegi.odmBsltm.alc.kolas.kolasRefNo == 0
                || string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.alc.kolas.kolasHspTur)
                || !ConstantHelper.GetKolasHspTurList().Contains(odemeEmriIstegi.odmBsltm.alc.kolas.kolasTur)))
        {
            result.Result = false;
            result.Message =
                "TR.OHVPS.Resource.InvalidFormat. alc-kolas-kolasDgr, alc-kolas-kolasTur,alc-kolas-kolasRefNo, alc-kolas-kolasHspTur required fields.";
            return result;
        }

        if (odemeEmriIstegi.odmBsltm.alc.kolas != null
            && odemeEmriIstegi.odmBsltm.kkod != null)
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. Kolas and KareKod can not be used at the same time";
            return result;
        }

        //Check odmBsltma karekod
        if (odemeEmriIstegi.odmBsltm.kkod != null
            && (string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.kkod.aksTur)
                || string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.kkod.kkodUrtcKod)
                || !ConstantHelper.GetKolasTurList().Contains(odemeEmriIstegi.odmBsltm.kkod.aksTur)))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. aksTur, kkodUrtcKod required fields.";
            return result;
        }

        //Check odmBsltma odemeayrintilari
        if (string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.odmAyr.odmKynk)
            || string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.odmAyr.odmAcklm)
            || string.IsNullOrEmpty(odemeEmriIstegi.odmBsltm.odmAyr.odmStm)
            || !ConstantHelper.GetOdemeSistemiList().Contains(odemeEmriIstegi.odmBsltm.odmAyr.odmStm))
        {
            result.Result = false;
            result.Message =
                "TR.OHVPS.Resource.InvalidFormat. odmBsltm-odmAyr-odmAcklm, odmBsltm-odmAyr-odmKynk, odmBsltm-odmAyr-odmStm required fields.";
            return result;
        }

        if (odemeEmriIstegi.odmBsltm.odmAyr.odmKynk !=
            OpenBankingConstants.OdemeKaynak.AcikBankacilikAraciligiIleGonderilenOdemelerde)
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat.  odmBsltm-odmAyr-odmKynk must be O.";
            return result;
        }

        if (!ConstantHelper.GetOdemeAmaciList().Contains(odemeEmriIstegi.odmBsltm.odmAyr.odmAmc))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat.  odmBsltm-odmAyr-odmAmc value is wrong.";
            return result;
        }

        //Do OdemeEmriRizasi validations
        var odemeEmriRizasiConsent = await context.Consents
            .FirstOrDefaultAsync(c => c.Id == new Guid(odemeEmriIstegi.rzBlg.rizaNo)
                                      && c.ConsentType == ConsentConstants.ConsentType.OpenBankingPayment);

        if (odemeEmriRizasiConsent == null) //No consent in db
        {
            result.Result = false;
            result.Message =
                $"Relational data is missing. No Payment Information consent in system with {odemeEmriIstegi.rzBlg.rizaNo}";
            return result;
        }

        //Check state
        if (odemeEmriRizasiConsent.State !=
            OpenBankingConstants.RizaDurumu.YetkiKullanildi) //State must be yetki kullanıldı
        {
            result.Result = false;
            result.Message = "Consent state not valid to process. Consent state have to be YetkiKullanildi";
            return result;
        }

        var odemeEmriRizasi = JsonSerializer.Deserialize<OdemeEmriRizasiHHSDto>(odemeEmriRizasiConsent.AdditionalData);
        if (odemeEmriRizasi == null)
        {
            result.Result = false;
            result.Message = $"Relational data is missing. No Payment Information consent additional data in system.";
            return result;
        }

        //odmBsltma Kmlk must be same
        if (odemeEmriRizasi.odmBsltm.kmlk.kmlkTur != odemeEmriIstegi.odmBsltm.kmlk.kmlkTur
            || odemeEmriRizasi.odmBsltm.kmlk.kmlkVrs != odemeEmriIstegi.odmBsltm.kmlk.kmlkVrs
            || (!string.IsNullOrEmpty(odemeEmriRizasi.odmBsltm.kmlk.krmKmlkVrs) &&
                odemeEmriRizasi.odmBsltm.kmlk.krmKmlkVrs != odemeEmriIstegi.odmBsltm.kmlk.krmKmlkVrs)
            || (!string.IsNullOrEmpty(odemeEmriRizasi.odmBsltm.kmlk.krmKmlkTur) &&
                odemeEmriRizasi.odmBsltm.kmlk.krmKmlkTur != odemeEmriIstegi.odmBsltm.kmlk.krmKmlkTur)
            || odemeEmriRizasi.odmBsltm.kmlk.ohkTur != odemeEmriIstegi.odmBsltm.kmlk.ohkTur)
        {
            result.Result = false;
            result.Message = "Kimlik data has to be equal with payment consent and payment order";
            return result;
        }

        //odmBsltma islTtr must be same
        if (odemeEmriRizasi.odmBsltm.islTtr.ttr != odemeEmriIstegi.odmBsltm.islTtr.ttr
            || odemeEmriRizasi.odmBsltm.islTtr.prBrm != odemeEmriIstegi.odmBsltm.islTtr.prBrm)
        {
            result.Result = false;
            result.Message = "islTtr data has to be equal with payment consent and payment order";
            return result;
        }

        //odmBsltma gon hesapnumarasi must be same
        if (odemeEmriRizasi.odmBsltm.gon.hspNo != odemeEmriIstegi.odmBsltm.gon.hspNo)
        {
            result.Result = false;
            result.Message = "gon hspNo data has to be equal with payment consent and payment order";
            return result;
        }

        //odmBsltma alc kolastur ve deger must be same
        if (odemeEmriRizasi.odmBsltm?.alc.kolas != null
            && (odemeEmriRizasi.odmBsltm.alc.kolas.kolasTur != odemeEmriIstegi.odmBsltm.alc.kolas?.kolasTur
                || odemeEmriRizasi.odmBsltm.alc.kolas.kolasDgr != odemeEmriIstegi.odmBsltm.alc.kolas?.kolasDgr))
        {
            result.Result = false;
            result.Message = "kolas kolasTur and kolasDgr data has to be equal with payment consent and payment order";
            return result;
        }

        //odmBsltma karekod akisturu ve referansı must be same
        if (odemeEmriRizasi.odmBsltm?.kkod != null
            && (odemeEmriRizasi.odmBsltm.kkod.aksTur != odemeEmriIstegi.odmBsltm.kkod?.aksTur
                || odemeEmriRizasi.odmBsltm.kkod.kkodRef != odemeEmriIstegi.odmBsltm.kkod?.kkodRef))
        {
            result.Result = false;
            result.Message = "kkod akstur and kkod kkodref data has to be equal with payment consent and payment order";
            return result;
        }

        //OdmAyr odmKynk,odmAmc, refBlg,odmStm must be same
        if (odemeEmriRizasi.odmBsltm?.odmAyr.odmKynk != odemeEmriIstegi.odmBsltm.odmAyr.odmKynk
            || odemeEmriRizasi.odmBsltm.odmAyr.odmAmc != odemeEmriIstegi.odmBsltm.odmAyr.odmAmc
            || odemeEmriRizasi.odmBsltm.odmAyr.refBlg != odemeEmriIstegi.odmBsltm.odmAyr.refBlg
            || odemeEmriRizasi.odmBsltm.odmAyr.odmStm != odemeEmriIstegi.odmBsltm.odmAyr.odmStm)
        {
            result.Result = false;
            result.Message =
                "odmKynk,odmAmc, refBlg,odmStm data has to be equal with payment consent and payment order";
            return result;
        }

        result.Data = odemeEmriRizasiConsent;
        return result;
    }

    /// <summary>
    /// Checks if consent is valid to get
    /// </summary>
    /// <param name="entity">To be checked entity</param>
    /// <returns>Validation result</returns>
    private ApiResult IsDataValidToGetAccountConsent(Consent? entity)
    {
        ApiResult result = new();
        if (entity == null || string.IsNullOrEmpty(entity.AdditionalData)) //No desired consent in system
        {
            result.Result = false;
            result.Message = "No desired consent in system";
            return result;
        }

        return result;
    }

    /// <summary>
    /// Checks if consent is valid to get
    /// </summary>
    /// <param name="entity">To be checked entity</param>
    /// <returns>Validation result</returns>
    private ApiResult IsDataValidToGetPaymentConsent(Consent? entity)
    {
        ApiResult result = new();
        if (entity == null)
        {
            result.Result = false;
            result.Message = "No desired consent in system";
            return result;
        }

        return result;
    }

    /// <summary>
    /// Checks if consent is valid to get
    /// </summary>
    /// <param name="entity">To be checked entity</param>
    /// <returns>Validation result</returns>
    private ApiResult IsDataValidToGetPaymentOrderConsent(OBPaymentOrder? entity)
    {
        ApiResult result = new();
        if (entity == null)
        {
            result.Result = false;
            result.Message = "No desired consent in system.";
            return result;
        }

        return result;
    }

    /// <summary>
    /// Check if consent is valid to be deleted
    /// </summary>
    /// <param name="entity">To be checked entity</param>
    /// <returns>Data validation result</returns>
    private ApiResult IsDataValidToDeleteAccountConsent(Consent? entity)
    {
        ApiResult result = new();
        if (entity == null)
        {
            result.Result = false;
            result.Message = "No desired consent in system.";
            return result;
        }

        if (!ConstantHelper.GetAccountConsentCanBeDeleteStatusList().Contains(entity.State))
        {
            //State not valid to set as deleted
            result.Result = false;
            result.Message = "Account consent status not valid to marked as deleted";
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
    private ApiResult IsDataValidToDeleteAccountConsentFromHHS(Consent? entity, string? userTCKN)
    {
        ApiResult result = new();
        if (entity == null)
        {
            result.Result = false;
            result.Message = "No desired consent in system.";
            return result;
        }

        if (!ConstantHelper.GetAccountConsentCanBeDeleteStatusList().Contains(entity.State))
        {
            //State not valid to set as deleted
            result.Result = false;
            result.Message = "Account consent status not valid to marked as deleted";
            return result;
        }

        var hesapBilgisiRizasi = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity.AdditionalData);
        if (hesapBilgisiRizasi?.kmlk.kmlkTur == OpenBankingConstants.KimlikTur.TCKN
            && hesapBilgisiRizasi.kmlk.kmlkVrs != userTCKN)
        {
            result.Result = false;
            result.Message = "Consent does not belong to this user.";
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
        bool isSenderAccountSet = !string.IsNullOrEmpty(additionalData.odmBsltm.gon?.hspNo) ||
                                  !string.IsNullOrEmpty(additionalData.odmBsltm.gon?.hspRef);
        if (!isSenderAccountSet
            && (savePcStatusSenderAccount.SenderAccount == null
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
    ///  Checks if header is varlid.
    /// Checks required fields.
    /// Checks hhskod yoskod if katilimciBlg parameter is set.
    /// </summary>
    /// <param name="context">Context</param>
    /// <param name="configuration">Configuration instance</param>
    /// <param name="yosInfoService">Yos service instance</param>
    /// <param name="header">Header object</param>
    /// <param name="katilimciBlg">Katilimci data object default value with null</param>
    /// <param name="isUserRequired">There should be userreference value in header. Optional parameter with default false value</param>
    /// <returns>Validation result</returns>
    private async Task<ApiResult> IsHeaderDataValid(HttpContext context,
        IConfiguration configuration,
        IYosInfoService yosInfoService,
        RequestHeaderDto? header = null,
        KatilimciBilgisiDto? katilimciBlg = null,
        bool? isUserRequired = false)
    {
        ApiResult result = new();
        if (header is null)
        {
            header = ModuleHelper.GetHeader(context);
        }

        if (!await ModuleHelper.IsHeaderValid(header, configuration, yosInfoService, isUserRequired: isUserRequired))
        {
            result.Result = false;
            result.Message = "There is a problem in header required values. Some key(s) can be missing or wrong.";
            return result;
        }

        //If there is katilimciBlg object, validate data in it with header
        if (katilimciBlg != null)
        {
            //Check header data and message data
            if (header.XASPSPCode != katilimciBlg.hhsKod)
            {
                //HHSCode must match with header x-aspsp-code
                result.Result = false;
                result.Message = "TR.OHVPS.Connection.InvalidASPSP. HHSKod must match with header x-aspsp-code";
                return result;
            }

            if (header.XTPPCode != katilimciBlg.yosKod)
            {
                //YOSCode must match with header x-tpp-code
                result.Result = false;
                result.Message = "TR.OHVPS.Connection.InvalidTPP. YosKod must match with header x-tpp-code";
                return result;
            }
        }

        return result;
    }


    /// <summary>
    /// Cancel waiting approve state consents
    /// </summary>
    /// <param name="context"></param>
    /// <param name="activeAccountConsents">Consents to be checked</param>
    private void CancelWaitingApproveConsents(ConsentDbContext context, List<Consent> activeAccountConsents)
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
                additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiIptal;
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
        }
    }

    /// <summary>
    /// Checks if any yetkilendirildi or yetkikullanildi state consents of user
    /// </summary>
    /// <param name="activeAccountConsents">Consents to be checked</param>
    /// <returns>Any consent that yetkilendirildi or yetkikullanildi states</returns>
    private bool AnyAuthAndUsedConsents(List<Consent> activeAccountConsents)
    {
        var authAndUsed = activeAccountConsents
            .Any(c => c.State == OpenBankingConstants.RizaDurumu.Yetkilendirildi
                      || c.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi);
        if (authAndUsed)
        {
            //TODO:Ozlem yetki süresi dolmuş ise iptal edilmeli
            return true;
        }

        return false;
    }


    /// <summary>
    /// Get active account consents of user
    /// Checks identity with consent identity properties
    /// </summary>
    /// <param name="rizaIstegi"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    private async Task<List<Consent>> GetActiveAccountConsentsOfUser(HesapBilgisiRizaIstegiHHSDto rizaIstegi,
        ConsentDbContext context)
    {
        var activeAccountConsentStatusList =
            ConstantHelper.GetActiveAccountConsentStatusList(); //Get active status list
        //Active account consents in db
        var activeAccountConsents = await context.Consents.Where(c =>
                c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                && activeAccountConsentStatusList.Contains(c.State)
                && c.OBAccountConsentDetails.Any(i => i.IdentityData == rizaIstegi.kmlk.kmlkVrs
                                                      && i.IdentityType == rizaIstegi.kmlk.kmlkTur
                                                      && i.UserType == rizaIstegi.kmlk.ohkTur
                                                      && i.YosCode == rizaIstegi.katilimciBlg.yosKod))
            .ToListAsync();
        return activeAccountConsents;
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

        var additionalData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity.AdditionalData);
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
            context.Consents.Update(entity);
            await context.SaveChangesAsync();
            return;
        }

        //comment from document
        //5 dakikadan uzun süredir “Yetki Bekleniyor” durumunda kalan kayıtların durumları güncellenir. 
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
        //5 dakikadan uzun süredir “Yetkilendirildi” durumunda kalan kayıtlar güncellenir. 
        //Yetkilendirildi ⇨ Rıza İptal / Süre Aşımı: Yetkilendirildi B ⇨ I / 05
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
    /// Checks if gkd data is valid
    /// </summary>
    /// <param name="gkd">To be checked data</param>
    /// <returns>Is gkd data valid</returns>
    private static bool IsGkdValid(GkdDto gkd)
    {
        return IsGkdValid(new GkdRequestDto() { ayrikGkd = gkd.ayrikGkd, yetYntm = gkd.yetYntm, yonAdr = gkd.yonAdr })
               && gkd.yetTmmZmn != DateTime.MinValue;
    }

    /// <summary>
    /// Checks if gkd data is valid
    /// </summary>
    /// <param name="gkd">To be checked data</param>
    /// <returns>Is gkd data valid</returns>
    private static bool IsGkdValid(GkdRequestDto gkd)
    {
        if (!string.IsNullOrEmpty(gkd.yetYntm)) //YetYntm is set
        {
            //Check data
            if (!ConstantHelper.GetGKDTurList().Contains(gkd.yetYntm))
            {
                //GDKTur value is not valid
                return false;
            }

            if ((gkd.yetYntm == OpenBankingConstants.GKDTur.Yonlendirmeli
                 && string.IsNullOrEmpty(gkd.yonAdr)))
            {
                //YonAdr should be set
                return false;
            }

            if (gkd.yetYntm == OpenBankingConstants.GKDTur.Ayrik)
            {
                //AyrikGKD object should be set
                if (gkd.ayrikGkd == null
                    || string.IsNullOrEmpty(gkd.ayrikGkd.ohkTanimDeger)
                    || string.IsNullOrEmpty(gkd.ayrikGkd.ohkTanimTip)
                    || !ConstantHelper.GetOhkTanimTipList().Contains(gkd.ayrikGkd.ohkTanimTip))
                    return false;
                //Check GKDTanımDeger values
                switch (gkd.ayrikGkd.ohkTanimTip)
                {
                    case OpenBankingConstants.OhkTanimTip.TCKN:
                        if (gkd.ayrikGkd.ohkTanimDeger.Trim().Length != 11
                            || !gkd.ayrikGkd.ohkTanimDeger.All(char.IsAsciiDigit))
                            return false;
                        break;
                    case OpenBankingConstants.OhkTanimTip.MNO:
                        if (gkd.ayrikGkd.ohkTanimDeger.Trim().Length >= 30)
                            return false;
                        break;
                    case OpenBankingConstants.OhkTanimTip.YKN:
                        if (gkd.ayrikGkd.ohkTanimDeger.Trim().Length != 11
                            || !gkd.ayrikGkd.ohkTanimDeger.All(char.IsAsciiDigit))
                            return false;
                        break;
                    case OpenBankingConstants.OhkTanimTip.PNO:
                        if (gkd.ayrikGkd.ohkTanimDeger.Trim().Length > 9)
                            return false;
                        break;
                    case OpenBankingConstants.OhkTanimTip.GSM:
                        if (gkd.ayrikGkd.ohkTanimDeger.Trim().Length != 10
                            || !gkd.ayrikGkd.ohkTanimDeger.All(char.IsAsciiDigit))
                            return false;
                        break;
                    case OpenBankingConstants.OhkTanimTip.IBAN:
                        if (gkd.ayrikGkd.ohkTanimDeger.Trim().Length != 26)
                            return false;
                        break;
                }
            }
        }
        else
        {
            //Yetkilendirme yontemi not set
            //TODO:Özlem. Set edilmeyebilir. Ama set edilmediği durumda nasıl ilerleyeceğimiz konuşulmalı. Patlamasın diye zorunluymuş gibi ilerltiyoruz.
            return false;
        }

        return true;
    }

    /// <summary>
    /// Check if yos has subscription for GKD
    /// </summary>
    /// <param name="yosKod"></param>
    /// <param name="consentType"></param>
    /// <param name="dbContext"></param>
    /// <returns></returns>
    private static async Task<bool> IsSubscsribed(string yosKod, string consentType, ConsentDbContext dbContext)
    {
        string sourceType = consentType == ConsentConstants.ConsentType.OpenBankingAccount
            ? OpenBankingConstants.KaynakTip.HesapBilgisiRizasi
            : OpenBankingConstants.KaynakTip.OdemeEmriRizasi;

        //HHS, YÖS'ün AYRIK_GKD_BASARILI ve AYRIK_GKD_BASARISIZ olay tipleri için olay aboneliğinin varlığını kontrol eder
        bool isSubscriped = await dbContext.OBEventSubscriptions.AsNoTracking().AnyAsync(s =>
            s.ModuleName == OpenBankingConstants.ModuleName.HHS
            && s.YOSCode == yosKod
            && s.OBEventSubscriptionTypes.Any(t =>
                t.SourceType == sourceType
                && t.EventType == OpenBankingConstants.OlayTip
                    .AyrikGKDBasarili)
            && s.OBEventSubscriptionTypes.Any(t =>
                t.SourceType == sourceType
                && t.EventType == OpenBankingConstants.OlayTip
                    .AyrikGKDBasarisiz));
        if (isSubscriped == false)
        {
            //Yos does not have subscription for ayrikGkd
            return false;
        }

        return true;
    }

    /// <summary>
    /// Generates accountconsentdetail object of account consent
    /// </summary>
    /// <param name="hesapBilgisiRizasi">Hesap bilgisi rizasi object</param>
    /// <param name="header">Request header</param>
    /// <returns>Accountconsentdetail object </returns>
    private static OBAccountConsentDetail GenerateAccountConsentDetailObject(
        HesapBilgisiRizasiHHSDto hesapBilgisiRizasi, RequestHeaderDto header)
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
            LastValidAccessDate = hesapBilgisiRizasi.hspBlg.iznBlg.erisimIzniSonTrh.ToUniversalTime(),
            TransactionInquiryStartTime = hesapBilgisiRizasi.hspBlg.iznBlg.hesapIslemBslZmn?.ToUniversalTime(),
            TransactionInquiryEndTime = hesapBilgisiRizasi.hspBlg.iznBlg.hesapIslemBtsZmn?.ToUniversalTime(),
            OhkMessage = hesapBilgisiRizasi.hspBlg.ayrBlg?.ohkMsj,
            XRequestId = header.XRequestID ?? string.Empty,
            XGroupId = header.XGroupID ?? string.Empty,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Generates paymentconsentdetail object of payment consent
    /// </summary>
    /// <param name="odemeEmriRizasi">Payment Consent</param>
    /// <param name="header">Request header</param>
    /// <returns>paymentconsentdetail object</returns>
    private static OBPaymentConsentDetail GeneratePaymentConsentDetailObject(
        OdemeEmriRizasiWithMsrfTtrHHSDto odemeEmriRizasi, RequestHeaderDto header)
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

            XRequestId = header.XRequestID ?? string.Empty,
            XGroupId = header.XGroupID ?? string.Empty,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }


    private async Task<List<ListAccountConsentDto>> GetAccountConsentDetails(string userTckn,
        List<Consent> userAccountConsents, ConsentDbContext dbContext, IMapper mapper)
    {
        List<ListAccountConsentDto> responseList = new List<ListAccountConsentDto>(); //Response list object
        ListAccountConsentDto detailedConsent;
        HesapBilgisiRizasiHHSDto hesapBilgisiRizasi;

        //Get yos informations by yos codes 
        var yosCodes = userAccountConsents.SelectMany(c => c.OBAccountConsentDetails.Select(d => d.YosCode))
            .Distinct()
            .ToList();
        var yosList = await dbContext.OBYosInfos.AsNoTracking().Where(y => yosCodes.Contains(y.Kod)).ToListAsync();
        var permissions = await dbContext.OBPermissionTypes.AsNoTracking()
            .Where(p => p.Language == "tr-TR")
            .ToListAsync();
        foreach (var consent in userAccountConsents)
        {
            //Generate consent detail object
            hesapBilgisiRizasi = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(consent.AdditionalData);
            detailedConsent = new ListAccountConsentDto()
            {
                ConsentId = consent.Id,
                AccountReferences = consent.OBAccountConsentDetails?.FirstOrDefault()?.AccountReferences ??
                                    new List<string>(),
                YosInfo = mapper.Map<OBYosInfoDto>(yosList.FirstOrDefault(y =>
                    y.Kod == hesapBilgisiRizasi.katilimciBlg.yosKod))
            };
            detailedConsent.PermissionDetail = new PermissionInformationDto()
            {
                LastValidAccessDate = hesapBilgisiRizasi.hspBlg.iznBlg.erisimIzniSonTrh,
                TransactionInquiryEndTime = hesapBilgisiRizasi.hspBlg.iznBlg.hesapIslemBtsZmn,
                TransactionInquiryStartTime = hesapBilgisiRizasi.hspBlg.iznBlg.hesapIslemBslZmn,
                PermissionType = permissions.Where(p => hesapBilgisiRizasi.hspBlg.iznBlg.iznTur.Contains(p.Code))
                    .ToDictionary(p => p.Code, p => p.Description)
            };

            responseList.Add(detailedConsent);
        }

        return responseList;
    }
}