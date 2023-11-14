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

namespace amorphie.consent.Module;

public class OpenBankingHHSConsentModule : BaseBBTRoute<OpenBankingConsentDTO, Consent, ConsentDbContext>
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
        routeGroupBuilder.MapGet("/hesap-bilgisi-rizasi/{rizaNo}", GetAccountConsentById);
        routeGroupBuilder.MapGet("/odeme-emri-rizasi/{rizaNo}", GetPaymentConsentById);
        routeGroupBuilder.MapGet("/odeme-emri/{odemeEmriNo}", GetPaymentOrderConsentById);
        routeGroupBuilder.MapGet("/GetAccountConsentById/{rizaNo}", GetAccountConsentByIdForUI);
        routeGroupBuilder.MapGet("/GetPaymentConsentById/{rizaNo}", GetPaymentConsentByIdForUI);
        routeGroupBuilder.MapGet("/hesaplar/{customerId}", GetAccounts);
        routeGroupBuilder.MapGet("/hesaplar/{customerId}/{hspRef}", GetAccountByHspRef);
        routeGroupBuilder.MapGet("/hesaplar/{customerId}/bakiye", GetBalances);
        routeGroupBuilder.MapGet("/hesaplar/{customerId}/{hspRef}/bakiye", GetBalanceByHspRef);
        routeGroupBuilder.MapGet("/hesaplar/{hspRef}/islemler", GetTransactionsByHspRef);
        routeGroupBuilder.MapDelete("/hesap-bilgisi-rizasi/{rizaNo}", DeleteAccountConsent);
        routeGroupBuilder.MapPost("/hesap-bilgisi-rizasi", AccountInformationConsentPost);
        routeGroupBuilder.MapPost("/odeme-emri-rizasi", PaymentConsentPost);
        routeGroupBuilder.MapPost("/UpdateAccountConsentForAuthorization", UpdateAccountConsentForAuthorization);
        routeGroupBuilder.MapPost("/UpdatePaymentConsentForAuthorization", UpdatePaymentConsentForAuthorization);
        routeGroupBuilder.MapPost("/UpdatePaymentConsentStatusForUsage", UpdatePaymentConsentStatusForUsage);
        routeGroupBuilder.MapPost("/UpdateAccountConsentStatusForUsage", UpdateAccountConsentStatusForUsage);
        routeGroupBuilder.MapPost("odeme-emri", PaymentOrderPost);
    }

    //hhs bizim bankamizi acacaklar. UI web ekranlarimiz


    #region HHS

    /// <summary>
    /// Get account consent additional data by rizano- consentId casting to HesapBilgisiRizasiHHSDto type of object
    /// </summary>
    /// <param name="rizaNo">Riza No</param>
    /// <param name="context">Context DB object</param>
    /// <param name="mapper">Aoutomapper object</param>
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
        HttpContext httpContext)
    {
        try
        {
            //Check header fields
            ApiResult headerValidation = IsHeaderDataValidForRequiredFields(httpContext);
            if (!headerValidation.Result)
            {//Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == rizaNo
                                        && c.ConsentType == OpenBankingConstants.ConsentType.OpenBankingAccount);
            ApiResult isDataValidResult = IsDataValidToGetAccountConsent(entity);
            if (!isDataValidResult.Result)//Error in data validation
            {
                return Results.BadRequest(isDataValidResult.Message);
            }
            var serializedData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity.AdditionalData);
            return Results.Ok(serializedData);
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
            //Get entity from db
            var entity = await context.Consents
                .Include(c => c.OBAccountReferences)
                .FirstOrDefaultAsync(c => c.Id == rizaNo
                                        && c.ConsentType == OpenBankingConstants.ConsentType.OpenBankingAccount);
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
    /// <param name="httpContext">Httpcontext object</param>
    /// <returns>account information of hspref - HesapBilgileriDto type of object</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    public async Task<IResult> GetAccountByHspRef(
        string customerId,
        string hspRef,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IAccountService accountService,
        HttpContext httpContext)
    {
        try
        {
            //Check header fields
            ApiResult headerValidation = IsHeaderDataValidForRequiredFields(httpContext);
            if (!headerValidation.Result)
            {//Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }
            ApiResult accountApiResult = await accountService.GetAccountByHspRef(customerId, hspRef);//Get data from service
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
    /// Get all accounts from service 
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <param name="context">Context DB object</param>
    /// <param name="mapper">Aoutomapper object</param>
    /// <param name="accountService">Account service class</param>
    /// <param name="httpContext">Httpcontext object</param>
    /// <returns>Account list of customer -  List of HesapBilgileriDto type of objects</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    public async Task<IResult> GetAccounts(string customerId,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IAccountService accountService,
        HttpContext httpContext)
    {
        try
        {
            //Check header fields
            ApiResult headerValidation = IsHeaderDataValidForRequiredFields(httpContext);
            if (!headerValidation.Result)
            {//Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }
            ApiResult accountApiResult = await accountService.GetAccounts(customerId);
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
    /// Get account's balance information from service with hspref 
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <param name="hspRef">Hesap ref</param>
    /// <param name="context">Context DB object</param>
    /// <param name="mapper">Aoutomapper object</param>
    /// <param name="accountService">Account service class</param>
    /// <param name="httpContext">Httpcontext object</param>
    /// <returns>account balance information of hspref - BakiyeBilgileriDto type of object</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    public async Task<IResult> GetBalanceByHspRef(
        string customerId,
        string hspRef,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IAccountService accountService,
        HttpContext httpContext)
    {
        try
        {
            //Check header fields
            ApiResult headerValidation = IsHeaderDataValidForRequiredFields(httpContext);
            if (!headerValidation.Result)
            {//Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }
            ApiResult accountApiResult = await accountService.GetBalanceByHspRef(customerId, hspRef);
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
    /// <param name="httpContext">Httpcontext object</param>
    /// <returns>Balance list of customer -  List of BakiyeBilgileriDto type of objects</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    public async Task<IResult> GetBalances(string customerId,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IAccountService accountService,
        HttpContext httpContext)
    {
        try
        {
            //Check header fields
            ApiResult headerValidation = IsHeaderDataValidForRequiredFields(httpContext);
            if (!headerValidation.Result)
            {//Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }
            ApiResult accountApiResult = await accountService.GetBalances(customerId);
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
    /// Get transactions from service with hspref 
    /// </summary>
    /// <param name="hspRef">Hesap ref</param>
    /// <param name="context">Context DB object</param>
    /// <param name="mapper">Aoutomapper object</param>
    /// <param name="accountService">Account service class</param>
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
        HttpContext httpContext)
    {
        try
        {
            //Check header fields
            ApiResult headerValidation = IsHeaderDataValidForRequiredFields(httpContext);
            if (!headerValidation.Result)
            {//Missing header fields
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
        HttpContext httpContext)
    {
        try
        {
            //Check header fields
            ApiResult headerValidation = IsHeaderDataValidForRequiredFields(httpContext);
            if (!headerValidation.Result)
            {//Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == rizaNo
                && c.ConsentType == OpenBankingConstants.ConsentType.OpenBankingPayment);
            ApiResult isDataValidResult = IsDataValidToGetPaymentConsent(entity);
            if (!isDataValidResult.Result)//Error in data validation
            {
                return Results.BadRequest(isDataValidResult.Message);
            }
            var serializedData = JsonSerializer.Deserialize<OdemeEmriRizasiHHSDto>(entity.AdditionalData);
            return Results.Ok(serializedData);
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
    /// <param name="httpContext">Httpcontext object</param>
    /// <returns>OdemeEmriHHSDto type of object</returns>
    [AddSwaggerParameter("X-Request-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-Group-ID", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-ASPSP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("X-TPP-Code", ParameterLocation.Header, true)]
    [AddSwaggerParameter("PSU-Initiated", ParameterLocation.Header, true)]
    public async Task<IResult> GetPaymentOrderConsentById(Guid odemeEmriNo,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        HttpContext httpContext)
    {
        try
        {
            //Check header fields
            ApiResult headerValidation = IsHeaderDataValidForRequiredFields(httpContext);
            if (!headerValidation.Result)
            {//Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == odemeEmriNo
                                     && c.ConsentType == OpenBankingConstants.ConsentType.OpenBankingPaymentOrder);
            ApiResult isDataValidResult = IsDataValidToGetPaymentOrderConsent(entity);
            if (!isDataValidResult.Result)//Error in data validation
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
    /// <returns>OdemeEmriRizaIstegiDto type of object</returns>
    public async Task<IResult> GetPaymentConsentByIdForUI(Guid rizaNo,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper)
    {
        try
        {
            //Get entity from db
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == rizaNo
                                    && c.ConsentType == OpenBankingConstants.ConsentType.OpenBankingPayment);
            var paymentConsent = mapper.Map<HHSPaymentConsentDto>(entity);
            return Results.Ok(paymentConsent);
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
    public async Task<IResult> UpdatePaymentConsentStatusForUsage([FromBody] UpdateConsentStateDto updateConsentState,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper)
    {
        try
        {
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == updateConsentState.Id
                && c.ConsentType == OpenBankingConstants.ConsentType.OpenBankingPayment);
            //Check consent validity
            ApiResult isDataValidResult = IsDataValidToUpdatePaymentConsentStatusForUsage(entity, updateConsentState);
            if (!isDataValidResult.Result)//Error in data validation
            {
                return Results.BadRequest(isDataValidResult.Message);
            }

            //Set permissions
            var additionalData = JsonSerializer.Deserialize<OdemeEmriRizasiHHSDto>(entity.AdditionalData);
            additionalData.rzBlg.rizaDrm = updateConsentState.State;
            additionalData.rzBlg.gnclZmn = DateTime.UtcNow;
            entity.AdditionalData = JsonSerializer.Serialize(additionalData);
            entity.State = updateConsentState.State;

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
    /// <returns></returns>
    protected async Task<IResult> UpdatePaymentConsentForAuthorization([FromBody] UpdatePCForAuthorizationDto savePCStatusSenderAccount,
      [FromServices] ConsentDbContext context,
      [FromServices] IMapper mapper)
    {
        var resultData = new Consent();
        try
        {
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == savePCStatusSenderAccount.Id
                                          && c.ConsentType == OpenBankingConstants.ConsentType.OpenBankingPayment);
            //Check consent validity
            ApiResult isDataValidResult = IsDataValidToUpdatePaymentConsentForAuth(entity, savePCStatusSenderAccount);
            if (!isDataValidResult.Result)//Error in data validation
            {
                return Results.BadRequest(isDataValidResult.Message);
            }

            var additionalData = JsonSerializer.Deserialize<OdemeEmriRizasiHHSDto>(entity.AdditionalData);
            //Check and set sender account
            if (additionalData.odmBsltm.gon == null
                || (string.IsNullOrEmpty(additionalData.odmBsltm.gon.hspNo)
                    && string.IsNullOrEmpty(additionalData.odmBsltm.gon.hspRef)))
            {
                additionalData.odmBsltm.gon = savePCStatusSenderAccount.SenderAccount;
            }

            //Set permission data
            additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.Yetkilendirildi;
            additionalData.rzBlg.gnclZmn = DateTime.UtcNow;
            entity.AdditionalData = JsonSerializer.Serialize(additionalData);
            entity.State = OpenBankingConstants.RizaDurumu.Yetkilendirildi;

            context.Consents.Update(entity);
            await context.SaveChangesAsync();
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
    public async Task<IResult> UpdateAccountConsentStatusForUsage([FromBody] UpdateConsentStateDto updateConsentState,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper)
    {
        try
        {
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == updateConsentState.Id
                                          && c.ConsentType == OpenBankingConstants.ConsentType.OpenBankingAccount);
            //Check consent validity
            ApiResult isDataValidResult = IsDataValidToUpdateAccountConsentStatusForUsage(entity, updateConsentState);
            if (!isDataValidResult.Result)//Error in data validation
            {
                return Results.BadRequest(isDataValidResult.Message);
            }

            //Set permissions
            var additionalData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity.AdditionalData);
            additionalData.rzBlg.rizaDrm = updateConsentState.State;
            additionalData.rzBlg.gnclZmn = DateTime.UtcNow;
            entity.AdditionalData = JsonSerializer.Serialize(additionalData);
            entity.State = updateConsentState.State;

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
    protected async Task<IResult> UpdateAccountConsentForAuthorization([FromBody] SaveAccountReferenceDto saveAccountReference,
      [FromServices] ConsentDbContext context,
      [FromServices] IMapper mapper)
    {
        try
        {
            //Get consent from db
            var consentEntity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == saveAccountReference.Id
                && c.ConsentType == OpenBankingConstants.ConsentType.OpenBankingAccount);

            //Check consent validity
            ApiResult isDataValidResult = IsDataValidToUpdateAccountConsentForAuthorization(consentEntity, saveAccountReference);
            if (!isDataValidResult.Result)//Error in data validation
            {
                return Results.BadRequest(isDataValidResult.Message);
            }

            //Update consent state and additional data
            var additionalData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(consentEntity.AdditionalData);
            additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.Yetkilendirildi;
            additionalData.rzBlg.gnclZmn = DateTime.UtcNow;
            consentEntity.AdditionalData = JsonSerializer.Serialize(additionalData);
            consentEntity.State = OpenBankingConstants.RizaDurumu.Yetkilendirildi;

            List<OBAccountReference> accountReferenceEntities = new List<OBAccountReference>();//Open banking account reference entity list
            string permissionType = string.Join(",", additionalData.hspBlg.iznBlg.iznTur);//Seperate permissiontypes with comma
            foreach (var accountReference in saveAccountReference.AccountReferences)//Generate account reference entity for each account
            {
                //Set account permission details
                accountReferenceEntities.Add(new OBAccountReference()
                {
                    ConsentId = consentEntity.Id,
                    AccountReference = accountReference,
                    PermissionType = permissionType,
                    LastValidAccessDate = additionalData.hspBlg.iznBlg.erisimIzniSonTrh,
                    TransactionInquiryStartTime = additionalData.hspBlg.iznBlg.hesapIslemBslZmn,
                    TransactionInquiryEndTime = additionalData.hspBlg.iznBlg.hesapIslemBtsZmn
                });
            }
            context.OBAccountReferences.AddRange(accountReferenceEntities);
            context.Consents.Update(consentEntity);
            await context.SaveChangesAsync();
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
        HttpContext httpContext)
    {
        try
        {
            //Check if post data is valid to process.
            var checkValidationResult = IsDataValidToAccountConsentPost(rizaIstegi, configuration, httpContext);
            if (!checkValidationResult.Result)
            {//Data not valid
                return Results.BadRequest(checkValidationResult.Message);
            }
            //Get user's active account consents from db
            var activeAccountConsents = await GetActiveAccountConsents(rizaIstegi, context);
            if (AnyAuthAndUsedConsents(activeAccountConsents))//Checks any authorized or authused state consent
            {
                return Results.BadRequest("TR.OHVPS.Resource.ConsentMismatch. There is already authorized account consent in system. First cancel the consent.");
            }
            //Cancel Yetki Bekleniyor state consents.
            CancelWaitingApproveConsents(context, activeAccountConsents);

            var consentEntity = new Consent();
            context.Consents.Add(consentEntity);
            //Generate response object
            HesapBilgisiRizasiHHSDto hesapBilgisiRizasi = mapper.Map<HesapBilgisiRizasiHHSDto>(rizaIstegi);
            //Set consent data
            hesapBilgisiRizasi.rzBlg = new RizaBilgileriDto()
            {
                rizaNo = consentEntity.Id.ToString(),
                olusZmn = DateTime.UtcNow,
                rizaDrm = OpenBankingConstants.RizaDurumu.YetkiBekleniyor
            };
            //Set gkd data
            hesapBilgisiRizasi.gkd.hhsYonAdr = string.Format(configuration["HHSForwardingAddress"] ?? string.Empty, consentEntity.Id.ToString());
            hesapBilgisiRizasi.gkd.yetTmmZmn = DateTime.UtcNow.AddMinutes(5);
            consentEntity.AdditionalData = JsonSerializer.Serialize(hesapBilgisiRizasi);
            consentEntity.State = OpenBankingConstants.RizaDurumu.YetkiBekleniyor; ;
            consentEntity.ConsentType = OpenBankingConstants.ConsentType.OpenBankingAccount;
            consentEntity.ObConsentIdentityInfos = new List<OBConsentIdentityInfo>
            {
                new OBConsentIdentityInfo()
                {//Get consent identity data to identity entity 
                    IdentityData = hesapBilgisiRizasi.kmlk.kmlkVrs,
                    IdentityType = hesapBilgisiRizasi.kmlk.kmlkTur,
                    InstitutionIdentityData = hesapBilgisiRizasi.kmlk.krmKmlkVrs,
                    InstitutionIdentityType = hesapBilgisiRizasi.kmlk.krmKmlkTur,
                    UserType = hesapBilgisiRizasi.kmlk.ohkTur
                }
            };
            context.Consents.Add(consentEntity);
            await context.SaveChangesAsync();
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
    protected async Task<IResult> DeleteAccountConsent(Guid id,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        HttpContext httpContext)
    {
        try
        {
            //Check header fields
            ApiResult headerValidation = IsHeaderDataValidForRequiredFields(httpContext);
            if (!headerValidation.Result)
            {//Missing header fields
                return Results.BadRequest(headerValidation.Message);
            }
            //get consent entity from db
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == id);
            ApiResult dataValidationResult = IsDataValidToDeleteAccountConsent(entity);//Check data validation
            if (!dataValidationResult.Result)
            {//Data not valid
                return Results.BadRequest(dataValidationResult.Message);
            }

            //Update consent rıza bilgileri properties
            var additionalData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity.AdditionalData);
            additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiIptal;
            additionalData.rzBlg.rizaIptDtyKod =
                OpenBankingConstants.RizaIptalDetayKodu.KullaniciIstegiIleHHSUzerindenIptal;
            additionalData.rzBlg.gnclZmn = DateTime.Now;
            entity.AdditionalData = JsonSerializer.Serialize(additionalData);
            entity.ModifiedAt = DateTime.UtcNow;
            entity.State = OpenBankingConstants.RizaDurumu.YetkiIptal;

            //TODO:Ozlem Erişim belirteci invalid hale getirilmeli
            context.Consents.Update(entity);
            await context.SaveChangesAsync();
            return Results.Ok();
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
    /// <param name="paymentService"/>
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
    HttpContext httpContext)
    {
        try
        {
            //Check if post data is valid to process.
            var dataValidationResult = await IsDataValidToPaymentConsentPost(rizaIstegi, configuration, httpContext);
            if (!dataValidationResult.Result)
            {//Data not valid
                return Results.BadRequest(dataValidationResult.Message);
            }

            ApiResult paymentServiceResponse = await paymentService.SendOdemeEmriRizasi(rizaIstegi);
            if (!paymentServiceResponse.Result)//Error in service
                return Results.BadRequest(paymentServiceResponse.Message);

            var consentEntity = new Consent();
            context.Consents.Add(consentEntity);
            //Generate response object
            OdemeEmriRizasiHHSDto odemeEmriRizasi = (OdemeEmriRizasiHHSDto)paymentServiceResponse.Data;
            //Set consent data
            odemeEmriRizasi.rzBlg = new RizaBilgileriDto()
            {
                rizaNo = consentEntity.Id.ToString(),
                olusZmn = DateTime.UtcNow,
                rizaDrm = OpenBankingConstants.RizaDurumu.YetkiBekleniyor
            };
            //Set gkd data
            odemeEmriRizasi.gkd.hhsYonAdr = string.Format(configuration["HHSForwardingAddress"] ?? string.Empty, consentEntity.Id.ToString());
            odemeEmriRizasi.gkd.yetTmmZmn = DateTime.UtcNow.AddMinutes(5);
            consentEntity.AdditionalData = JsonSerializer.Serialize(odemeEmriRizasi);
            consentEntity.State = OpenBankingConstants.RizaDurumu.YetkiBekleniyor;
            consentEntity.ConsentType = OpenBankingConstants.ConsentType.OpenBankingPayment;
            consentEntity.ObConsentIdentityInfos = new List<OBConsentIdentityInfo>
            {
                new OBConsentIdentityInfo()
                {//Get consent identity data to identity entity 
                    IdentityData = odemeEmriRizasi.odmBsltm.kmlk.kmlkVrs ?? string.Empty,
                    IdentityType = odemeEmriRizasi.odmBsltm.kmlk.kmlkTur ?? string.Empty,
                    InstitutionIdentityData = odemeEmriRizasi.odmBsltm.kmlk.krmKmlkVrs,
                    InstitutionIdentityType = odemeEmriRizasi.odmBsltm.kmlk.krmKmlkTur,
                    UserType = odemeEmriRizasi.odmBsltm.kmlk.ohkTur
                }
            };
            context.Consents.Add(consentEntity);
            await context.SaveChangesAsync();
            return Results.Ok(odemeEmriRizasi);
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
        HttpContext httpContext)
    {
        try
        {
            //Check if post data is valid to process.
            var dataValidationResult = await IsDataValidToPaymentOrderPost(odemeEmriIstegi, context, httpContext);
            if (!dataValidationResult.Result)
            {//Data not valid
                return Results.BadRequest(dataValidationResult.Message);
            }
          
            ApiResult paymentServiceResponse = await paymentService.SendOdemeEmri(odemeEmriIstegi);//Send payment order to service
            if (!paymentServiceResponse.Result)//Error in service
                return Results.BadRequest(paymentServiceResponse.Message);

            OdemeEmriHHSDto odemeEmriDto = (OdemeEmriHHSDto)paymentServiceResponse.Data;

            var orderEntity = new Consent();
            context.Consents.Add(orderEntity);//Add to get id

            //Set consent data
            odemeEmriDto.emrBlg = new EmirBilgileriDto()
            {
                odmEmriNo = orderEntity.Id.ToString(),
                odmEmriZmn = DateTime.UtcNow
            };
            odemeEmriDto.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiOdemeEmrineDonustu;
            orderEntity.AdditionalData = JsonSerializer.Serialize(odemeEmriDto);
            orderEntity.State = OpenBankingConstants.RizaDurumu.YetkiOdemeEmrineDonustu;
            orderEntity.ConsentType = OpenBankingConstants.ConsentType.OpenBankingPaymentOrder;
            context.Consents.Add(orderEntity);

            await context.SaveChangesAsync();
            return Results.Ok(odemeEmriDto);
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


    /// <summary>
    /// Checks if data is valid for account consent post process
    /// </summary>
    /// <param name="rizaIstegi">To be checked data</param>
    /// <param name="configuration">Config object</param>
    /// <param name="httpContext">Context object to get header parameters</param>
    /// <exception cref="NotImplementedException"></exception>
    private ApiResult IsDataValidToAccountConsentPost(HesapBilgisiRizaIstegiHHSDto rizaIstegi,
    IConfiguration configuration,
    HttpContext httpContext)
    {
        //TODO:Ozlem Check fields length and necessity
        //TODO:Ozlem Check if user is customer
        //TODO:Ozlem Check fields length and necessity

        ApiResult result = new();
        var header = ModuleHelper.GetHeader(httpContext);
        //Check header fields
        result = IsHeaderDataValidForRequiredFields(httpContext, header);
        if (!result.Result)
        {//validation error in header fields
            return result;
        }
        //Check KatılımcıBilgisi
        if (string.IsNullOrEmpty(rizaIstegi.katilimciBlg.hhsKod)//Required fields
            || string.IsNullOrEmpty(rizaIstegi.katilimciBlg.yosKod)
            || configuration["HHSCode"] != rizaIstegi.katilimciBlg.hhsKod)
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. HHSKod YOSKod required";
            return result;
        }

        if (header.XASPSPCode != rizaIstegi.katilimciBlg.hhsKod)
        {//HHSCode must match with header x-aspsp-code
            result.Result = false;
            result.Message = "TR.OHVPS.Connection.InvalidASPSP. HHSKod must match with header x-aspsp-code";
            return result;
        }
        if (header.XTPPCode != rizaIstegi.katilimciBlg.yosKod)
        {//YOSCode must match with header x-tpp-code
            result.Result = false;
            result.Message = "TR.OHVPS.Connection.InvalidTPP. YosKod must match with header x-tpp-code";
            return result;
        }

        //Check GKD
        if (!string.IsNullOrEmpty(rizaIstegi.gkd.yetYntm)
            && ((rizaIstegi.gkd.yetYntm == OpenBankingConstants.GKDTur.Yonlendirmeli
                && string.IsNullOrEmpty(rizaIstegi.gkd.yonAdr))
               || (rizaIstegi.gkd.yetYntm == OpenBankingConstants.GKDTur.Ayrik
                   && string.IsNullOrEmpty(rizaIstegi.gkd.bldAdr))))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. GKD data not valid.";
            return result;
        }

        //Check Kimlik
        if (string.IsNullOrEmpty(rizaIstegi.kmlk.kmlkTur)//Check required fields
            || string.IsNullOrEmpty(rizaIstegi.kmlk.kmlkVrs)
            || (string.IsNullOrEmpty(rizaIstegi.kmlk.krmKmlkTur) != string.IsNullOrEmpty(rizaIstegi.kmlk.krmKmlkVrs))
            || string.IsNullOrEmpty(rizaIstegi.kmlk.ohkTur))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. Kmlk data is not valid";
            return result;
        }

        //Check field constraints
        if ((rizaIstegi.kmlk.kmlkTur == OpenBankingConstants.KimlikTur.TCKN && rizaIstegi.kmlk.kmlkVrs.Trim().Length != 11)
            || (rizaIstegi.kmlk.kmlkTur == OpenBankingConstants.KimlikTur.MNO && rizaIstegi.kmlk.kmlkVrs.Trim().Length > 30)
             || (rizaIstegi.kmlk.kmlkTur == OpenBankingConstants.KimlikTur.YKN && rizaIstegi.kmlk.kmlkVrs.Trim().Length != 11)
            || (rizaIstegi.kmlk.kmlkTur == OpenBankingConstants.KimlikTur.PNO && (rizaIstegi.kmlk.kmlkVrs.Trim().Length < 7 || rizaIstegi.kmlk.kmlkVrs.Length > 9))
            || (rizaIstegi.kmlk.krmKmlkTur == OpenBankingConstants.KurumKimlikTur.TCKN && rizaIstegi.kmlk.kmlkVrs.Trim().Length != 11)
            || (rizaIstegi.kmlk.krmKmlkTur == OpenBankingConstants.KurumKimlikTur.MNO && rizaIstegi.kmlk.kmlkVrs.Trim().Length > 30)
            || (rizaIstegi.kmlk.krmKmlkTur == OpenBankingConstants.KurumKimlikTur.VKN && rizaIstegi.kmlk.kmlkVrs.Trim().Length != 10))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. Kmlk data validation failed.";
            return result;
        }


        //Check HesapBilgisi
        //Check izinbilgisi properties
        if ((rizaIstegi.hspBlg.iznBlg.iznTur?.Any() ?? false) == false
            || rizaIstegi.hspBlg.iznBlg.iznTur?.Contains(OpenBankingConstants.IzinTur.HesapBilgisi) == false)
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. IznBld iznTur check failed. IznTur required and should contain HesapBilgisi permission.";
            return result;
        }

        if (rizaIstegi.hspBlg.iznBlg.erisimIzniSonTrh == System.DateTime.MinValue
          || rizaIstegi.hspBlg.iznBlg.erisimIzniSonTrh > System.DateTime.UtcNow.AddMonths(6)
          || rizaIstegi.hspBlg.iznBlg.erisimIzniSonTrh < System.DateTime.UtcNow.AddDays(1))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. IznBld erisimIzniSonTrh data check failed. It should be between tomorrow and 6 months later ";
            return result;
        }

        //Check işlem sorgulama başlangıç zamanı
        if (rizaIstegi.hspBlg.iznBlg.hesapIslemBslZmn.HasValue)
        {
            //Temel işlem bilgisi ve/veya ayrıntılı işlem bilgisi seçilmiş olması gerekir
            if (rizaIstegi.hspBlg.iznBlg?.iznTur?.Any(p => p == OpenBankingConstants.IzinTur.TemelIslem
                                                        || p == OpenBankingConstants.IzinTur.AyrintiliIslem) == false)
            {
                result.Result = false;
                result.Message = "TR.OHVPS.Resource.InvalidFormat. IznTur temelislem or ayrintiliIslem should be selected.";
                return result;
            }
            if (rizaIstegi.hspBlg.iznBlg?.hesapIslemBslZmn.Value < DateTime.UtcNow.AddMonths(-12))//Data constraints
            {
                result.Result = false;
                result.Message = "TR.OHVPS.Resource.InvalidFormat. hesapIslemBslZmn not valid. Maximum 12 months before.";
                return result;
            }
        }
        if (rizaIstegi.hspBlg.iznBlg.hesapIslemBtsZmn.HasValue)//Check işlem sorgulama bitiş zamanı
        {
            //Temel işlem bilgisi ve/veya ayrıntılı işlem bilgisi seçilmiş olması gerekir
            if (rizaIstegi.hspBlg.iznBlg?.iznTur?.Any(p => p == OpenBankingConstants.IzinTur.TemelIslem
                                                        || p == OpenBankingConstants.IzinTur.AyrintiliIslem) == false)
            {
                result.Result = false;
                result.Message = "TR.OHVPS.Resource.InvalidFormat IznTur temelislem or ayrintiliIslem should be selected.";
                return result;
            }
            if (rizaIstegi.hspBlg.iznBlg?.hesapIslemBtsZmn.Value > DateTime.UtcNow.AddMonths(12))//Data constraints
            {
                result.Result = false;
                result.Message = "TR.OHVPS.Resource.InvalidFormat. hesapIslemBtsZmn not valid. Maximum 12 months later.";
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
    /// <param name="httpContext">HttpContext httpContext</param>
    /// <returns></returns>
    private async Task<ApiResult> IsDataValidToPaymentConsentPost(OdemeEmriRizaIstegiHHSDto rizaIstegi,
     IConfiguration configuration,
     HttpContext httpContext)
    {

        //TODO:Ozlem update method
        ApiResult result = new();
        var header = ModuleHelper.GetHeader(httpContext);//Get header
        //Check header fields
        result = IsHeaderDataValidForRequiredFields(httpContext, header);
        if (!result.Result)
        {//validation error in header fields
            return result;
        }
        //Check message required basic properties
        if (rizaIstegi.katilimciBlg is null
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
        if (string.IsNullOrEmpty(rizaIstegi.katilimciBlg.hhsKod)//Required fields
            || string.IsNullOrEmpty(rizaIstegi.katilimciBlg.yosKod)
            || configuration["HHSCode"] != rizaIstegi.katilimciBlg.hhsKod)
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. HHSKod YOSKod required.";
            return result;
        }

        if (header.XASPSPCode != rizaIstegi.katilimciBlg.hhsKod)
        {//HHSCode must match with header x-aspsp-code
            result.Result = false;
            result.Message = "TR.OHVPS.Connection.InvalidASPSP. HHSKod must match with header x-aspsp-code";
            return result;
        }
        if (header.XTPPCode != rizaIstegi.katilimciBlg.yosKod)
        {//YOSCode must match with header x-tpp-code
            result.Result = false;
            result.Message = "TR.OHVPS.Connection.InvalidTPP. YosKod must match with header x-tpp-code";
            return result;
        }

        //Check GKD
        if (!string.IsNullOrEmpty(rizaIstegi.gkd.yetYntm)
            && ((rizaIstegi.gkd.yetYntm == OpenBankingConstants.GKDTur.Yonlendirmeli
                && string.IsNullOrEmpty(rizaIstegi.gkd.yonAdr))
               || (rizaIstegi.gkd.yetYntm == OpenBankingConstants.GKDTur.Ayrik
                   && string.IsNullOrEmpty(rizaIstegi.gkd.bldAdr))))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. GKD data not valid.";
            return result;
        }
        //Check odmBsltm  Kimlik
        if (string.IsNullOrEmpty(rizaIstegi.odmBsltm.kmlk.ohkTur)//Check required fields
            || (rizaIstegi.odmBsltm.kmlk.ohkTur == OpenBankingConstants.OHKTur.Bireysel
                && (string.IsNullOrEmpty(rizaIstegi.odmBsltm.kmlk.kmlkTur) || string.IsNullOrEmpty(rizaIstegi.odmBsltm.kmlk.kmlkVrs)))
            || (rizaIstegi.odmBsltm.kmlk.ohkTur == OpenBankingConstants.OHKTur.Kurumsal
                && (string.IsNullOrEmpty(rizaIstegi.odmBsltm.kmlk.krmKmlkTur) || string.IsNullOrEmpty(rizaIstegi.odmBsltm.kmlk.krmKmlkVrs))))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. odmBsltm => Kmlk data is not valid";
            return result;
        }
        //Check odmBsltma Islem Tutarı
        if (string.IsNullOrEmpty(rizaIstegi.odmBsltm.islTtr.ttr)//Check required fields
            || string.IsNullOrEmpty(rizaIstegi.odmBsltm.islTtr.prBrm))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. odmBsltm => islTtr required fields empty";
            return result;
        }
        //TODO:Ozlem check gönderen hesap. 

        //Check odmBsltma Alıcı
        if (rizaIstegi.odmBsltm.alc.kolas == null
            && (string.IsNullOrEmpty(rizaIstegi.odmBsltm.alc.unv) || string.IsNullOrEmpty(rizaIstegi.odmBsltm.alc.hspNo)))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. If kolas is null, unv and hspno is required";
            return result;
        }

        if (rizaIstegi.odmBsltm.alc.kolas != null
        && rizaIstegi.odmBsltm.kkod != null)
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. Kolas and KareKod can not be used at the same time";
            return result;
        }

        if (rizaIstegi.odmBsltm.alc.kolas != null
            && (string.IsNullOrEmpty(rizaIstegi.odmBsltm.alc.kolas.kolasDgr)
                || string.IsNullOrEmpty(rizaIstegi.odmBsltm.alc.kolas.kolasTur)))
        {
            result.Result = false;
            result.Message = "TR.OHVPS.Resource.InvalidFormat. alc-kolas-kolasDgr, alc-kolas-kolasTur required fields.";
            return result;
        }

        if (rizaIstegi.odmBsltm.kkod != null
            && (string.IsNullOrEmpty(rizaIstegi.odmBsltm.kkod.aksTur)
            || string.IsNullOrEmpty(rizaIstegi.odmBsltm.kkod.kkodUrtcKod)))
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
            result.Message = "TR.OHVPS.Resource.InvalidFormat. odmBsltm-odmAyr-odmAcklm, odmBsltm-odmAyr-odmKynk required fields.";
            return result;
        }

        if (rizaIstegi.odmBsltm.odmAyr.odmKynk != OpenBankingConstants.OdemeKaynak.AcikBankacilikAraciligiIleGonderilenOdemelerde)
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
        return result;
    }

    /// <summary>
    /// Check if consent is valid to payment order post
    /// </summary>
    /// <param name="odemeEmriIstegi"></param>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    private async Task<ApiResult> IsDataValidToPaymentOrderPost(OdemeEmriIstegiHHSDto odemeEmriIstegi, ConsentDbContext context, HttpContext httpContext)
    {
        ApiResult result = new();
        //Check header fields
        result = IsHeaderDataValidForRequiredFields(httpContext);
        if (!result.Result)
        {//Missing header fields
            return result;
        }
        var odemeEmriRizasiConsent = await context.Consents
            .FirstOrDefaultAsync(c => c.Id == new Guid(odemeEmriIstegi.rzBlg.rizaNo)
                                      && c.ConsentType == OpenBankingConstants.ConsentType.OpenBankingPayment);

        if (odemeEmriRizasiConsent == null)//No consent in db
        {
            result.Result = false;
            result.Message = $"Relational data is missing. No Payment Information consent in system with {odemeEmriIstegi.rzBlg.rizaNo}";
            return result;
        }

        //Check state
        if (odemeEmriRizasiConsent.State != OpenBankingConstants.RizaDurumu.YetkiKullanildi)//State must be yetki kullanıldı
        {
            result.Result = false;
            result.Message = "Consent state not valid to process. Consent state have to be YetkiKullanildi";
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
        if (entity == null)//No desired consent in system
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
    private ApiResult IsDataValidToGetPaymentOrderConsent(Consent? entity)
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
    /// Check if consent is valid to be updated for authorization
    /// </summary>
    /// <param name="entity">To be checked entity</param>
    /// <param name="saveAccountReference">to be checked object</param>
    /// <returns>Data validation result</returns>
    private ApiResult IsDataValidToUpdateAccountConsentForAuthorization(Consent? entity, SaveAccountReferenceDto saveAccountReference)
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
    private ApiResult IsDataValidToUpdatePaymentConsentStatusForUsage(Consent? entity, UpdateConsentStateDto updateConsentState)
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
        return result;
    }

    /// <summary>
    ///  Check if consent is valid to be updated for usage
    /// </summary>
    /// <param name="entity">To be checked entity</param>
    /// <param name="updateConsentState">to be checked object</param>
    /// <returns></returns>
    private ApiResult IsDataValidToUpdateAccountConsentStatusForUsage(Consent? entity, UpdateConsentStateDto updateConsentState)
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
        return result;
    }

    /// <summary>
    /// Check if consent is valid to be updated for authorization
    /// </summary>
    /// <param name="entity">To be checked entity</param>
    /// <param name="savePcStatusSenderAccount">To be checked object</param>
    /// <returns></returns>
    private ApiResult IsDataValidToUpdatePaymentConsentForAuth(Consent? entity, UpdatePCForAuthorizationDto savePcStatusSenderAccount)
    {
        ApiResult result = new();
        if (entity == null)//No consent in db
        {
            result.Result = false;
            result.Message = "No consent in the system.";
            return result;
        }

        if (entity.State != OpenBankingConstants.RizaDurumu.YetkiBekleniyor)//State must be yetki bekleniyor
        {
            result.Result = false;
            result.Message = "Consent state not valid to process. Only YetkiBekleniyor state consent can be authorized.";
            return result;
        }
        //Check if sender account is already selected in db
        var additionalData = JsonSerializer.Deserialize<OdemeEmriRizasiHHSDto>(entity.AdditionalData);
        bool isSenderAccountSet = string.IsNullOrEmpty(additionalData.odmBsltm.gon.hspNo) || string.IsNullOrEmpty(additionalData.odmBsltm.gon.hspRef);
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
    ///  Checks if header required values are set.
    /// </summary>
    /// <param name="context">Context</param>
    /// <param name="header">Header object</param>
    /// <returns>Validation result</returns>
    private ApiResult IsHeaderDataValidForRequiredFields(HttpContext context, RequestHeaderDto header = null)
    {
        ApiResult result = new();
        if (header is null)
        {
            header = ModuleHelper.GetHeader(context);
        }
        if (!ModuleHelper.IsHeaderRequiredValuesCheckSuccess(header))
        {
            result.Result = false;
            result.Message = "There is a problem in header required values. Some key(s) can be missing or wrong.";
            return result;
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
        {//If any, cancel all of them
            foreach (var waitingAporove in waitingAporoves)
            {
                //Update consent rıza bilgileri properties
                var additionalData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(waitingAporove.AdditionalData);
                additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiIptal;
                additionalData.rzBlg.rizaIptDtyKod =
                    OpenBankingConstants.RizaIptalDetayKodu.YeniRizaTalebiIleIptal;
                additionalData.rzBlg.gnclZmn = DateTime.Now;
                waitingAporove.AdditionalData = JsonSerializer.Serialize(additionalData);
                waitingAporove.ModifiedAt = DateTime.UtcNow;
                waitingAporove.State = OpenBankingConstants.RizaDurumu.YetkiIptal;
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
    private async Task<List<Consent>> GetActiveAccountConsents(HesapBilgisiRizaIstegiHHSDto rizaIstegi, ConsentDbContext context)
    {
        var activeAccountConsentStatusList = ConstantHelper.GetActiveAccountConsentStatusList(); //Get active status list
        //Active account consents in db
        var activeAccountConsents = await context.Consents.Where(c =>
                c.ConsentType == OpenBankingConstants.ConsentType.OpenBankingAccount
                && activeAccountConsentStatusList.Contains(c.State)
                && c.ObConsentIdentityInfos.Any(i => i.IdentityData == rizaIstegi.kmlk.kmlkVrs
                                                     && i.IdentityType == rizaIstegi.kmlk.kmlkTur
                                                     && i.UserType == rizaIstegi.kmlk.ohkTur))
            .ToListAsync();
        return activeAccountConsents;
    }


}
