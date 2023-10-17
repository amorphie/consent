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
        routeGroupBuilder.MapGet("/GetAccountConsentById/{rizaNo}", GetAccountConsentByIdForUI);
        routeGroupBuilder.MapGet("/GetPaymentConsentById/{rizaNo}", GetPaymentConsentByIdForUI);
        routeGroupBuilder.MapGet("/hesaplar/{customerId}", GetAccounts);
        routeGroupBuilder.MapGet("/hesaplar/{customerId}/{hspRef}", GetAccountByHspRef);
        routeGroupBuilder.MapGet("/hesaplar/{customerId}/bakiye", GetBalances);
        routeGroupBuilder.MapGet("/hesaplar/{customerId}/{hspRef}/bakiye", GetBalanceByHspRef);
        routeGroupBuilder.MapGet("/hesaplar/{hspRef}/islemler", GetTransactionsByHspRef);
        routeGroupBuilder.MapDelete("/hesap-bilgisi-rizasi/{rizaNo}", DeleteAccountConsent);
        routeGroupBuilder.MapPost("/hesap-bilgisi-rizasi", AccountInformationConsentPost);
        routeGroupBuilder.MapPost("/odeme-emri-rizasi", PaymentInformationConsentPost);
        routeGroupBuilder.MapPost("/UpdateAccountConsentForAuthorization", UpdateAccountConsentForAuthorization);
        routeGroupBuilder.MapPost("/UpdatePaymentConsentStatus/{consentId}/{status}", UpdatePaymentConsentStatus);
        routeGroupBuilder.MapPost("/UpdatePaymentConsentForAuthorization", UpdatePaymentConsentForAuthorization);
        //TODO:Ozlem /odeme-emri/{odemeEmriNo} bu metod eklenecek
    }

    //hhs bizim bankamizi acacaklar. UI web ekranlarimiz


    #region HHS

    /// <summary>
    /// Get account consent additional data by rizano- consentId casting to HesapBilgisiRizasiHHSDto type of object
    /// </summary>
    /// <param name="rizaNo">Riza No</param>
    /// <param name="context">Context DB object</param>
    /// <param name="mapper">Aoutomapper object</param>
    /// <returns>HesapBilgisiRizasiHHSDto type of object</returns>
    public async Task<IResult> GetAccountConsentById(
        Guid rizaNo,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper)
    {
        try
        {
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == rizaNo);
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
            //Get data from db
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == rizaNo);
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
    /// <returns>account information of hspref - HesapBilgileriDto type of object</returns>
    public async Task<IResult> GetAccountByHspRef(
        string customerId,
        string hspRef,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IAccountService accountService)
    {
        try
        {
            ApiResult accountApiResult = await accountService.GetAccountByHspRef(customerId, hspRef);
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
    /// <returns>Account list of customer -  List of HesapBilgileriDto type of objects</returns>
    public async Task<IResult> GetAccounts(string customerId,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IAccountService accountService)
    {
        try
        {
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
    /// <returns>account balance information of hspref - BakiyeBilgileriDto type of object</returns>
    public async Task<IResult> GetBalanceByHspRef(
        string customerId,
        string hspRef,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IAccountService accountService)
    {
        try
        {
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
    /// <returns>Balance list of customer -  List of BakiyeBilgileriDto type of objects</returns>
    public async Task<IResult> GetBalances(string customerId,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IAccountService accountService)
    {
        try
        {
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
    /// <returns>account transactions- IslemBilgileriDto type of object</returns>
    public async Task<IResult> GetTransactionsByHspRef(
        string hspRef,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper,
        [FromServices] IAccountService accountService)
    {
        try
        {
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
    /// <returns>OdemeEmriRizasiHHSDto type of object</returns>
    public async Task<IResult> GetPaymentConsentById(Guid rizaNo,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper)
    {
        try
        {
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == rizaNo);
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
            var entity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == rizaNo);
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

    protected async Task<IResult> UpdateAccountConsentForAuthorization([FromBody] SaveAccountReferenceDto saveAccountReference,
      [FromServices] ConsentDbContext context,
      [FromServices] IMapper mapper)
    {
        var returnData = new Consent();
        try
        {
            //Get consent from db
            var consentEntity = await context.Consents
                .FirstOrDefaultAsync(c => c.Id == saveAccountReference.Id);

            ApiResult isDataValidResult = IsDataValidToUpdateAccountConsentForAuthorization(consentEntity, saveAccountReference);

            if (!isDataValidResult.Result)//Error in data validation
            {
                return Results.BadRequest(isDataValidResult.Message);
            }

            //Update consent state and additional data
            consentEntity.State = OpenBankingConstants.RizaDurumu.Yetkilendirildi;
            var additionalData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(consentEntity.AdditionalData);
            additionalData.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.Yetkilendirildi;
            consentEntity.AdditionalData = JsonSerializer.Serialize(additionalData);
            consentEntity.ModifiedAt = DateTime.UtcNow;

            List<OBAccountReference> accountReferenceEntities = new List<OBAccountReference>();//Open banking account reference entity list
            string permissionType = string.Join(",", additionalData.hspBlg.iznBlg.iznTur);//Seperate permissiontypes with comma
            foreach (var accountReference in saveAccountReference.AccountReferences)//Generate account reference entity for each account
            {
                accountReferenceEntities.Add(new OBAccountReference()
                {
                    ConsentId = consentEntity.Id,
                    AccountReference = accountReference,
                    PermissionType = permissionType
                });
            }
            context.OBAccountReferences.AddRange(accountReferenceEntities);
            context.Consents.Update(consentEntity);
            await context.SaveChangesAsync();
            return Results.Ok(consentEntity);

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
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    protected async Task<IResult> AccountInformationConsentPost([FromBody] HesapBilgisiRizaIstegiHHSDto rizaIstegi,
   [FromServices] ConsentDbContext context,
   [FromServices] IMapper mapper,
   [FromServices] IConfiguration configuration)
    {
        try
        {
            //Check if post data is valid to process.
            var checkValidationResult = IsDataValidToAccountConsentPost(rizaIstegi, configuration);
            if (checkValidationResult != Results.Ok())//Not valid
            {
                return checkValidationResult;
            }
            var consentEntity = mapper.Map<Consent>(rizaIstegi);
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
            hesapBilgisiRizasi.gkd.hhsYonAdr = configuration["HHSForwardingAddress"] ?? string.Empty;
            hesapBilgisiRizasi.gkd.yetTmmZmn = DateTime.UtcNow.AddMinutes(5);
            consentEntity.AdditionalData = JsonSerializer.Serialize(hesapBilgisiRizasi);
            consentEntity.State = OpenBankingConstants.RizaDurumu.YetkiBekleniyor; ;
            consentEntity.ConsentType = OpenBankingConstants.ConsentType.OpenBankingAccount;

            context.Consents.Add(consentEntity);

            await context.SaveChangesAsync();
            return Results.Ok(hesapBilgisiRizasi);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
    }


    protected async Task<IResult> DeleteAccountConsent(Guid id,
        [FromServices] ConsentDbContext context,
        [FromServices] IMapper mapper)
    {
        try
        {
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
    /// <returns>OdemeEmriRizasi object</returns>
    protected async Task<IResult> PaymentInformationConsentPost([FromBody] OdemeEmriRizaIstegiHHSDto rizaIstegi,
      [FromServices] ConsentDbContext context,
      [FromServices] IMapper mapper,
        [FromServices] IConfiguration configuration,
        [FromServices] IPaymentService paymentService)
    {
        try
        {
            ApiResult paymentServiceResponse = await paymentService.SendOdemeEmriRizasi(rizaIstegi);
            if (!paymentServiceResponse.Result)//Error in service
                return Results.BadRequest(paymentServiceResponse.Message);

            //Check if post data is valid to process.
            var checkValidationResult = await IsDataValidToPaymentInformationConsentPost(rizaIstegi, configuration, paymentService);
            if (checkValidationResult != Results.Ok())//Not valid
            {
                return checkValidationResult;
            }
            var consentEntity = mapper.Map<Consent>(rizaIstegi);
            context.Consents.Add(consentEntity);
            //Generate response object
            OdemeEmriRizasiHHSDto odemeEmriRizasi = mapper.Map<OdemeEmriRizasiHHSDto>(rizaIstegi);
            //Set consent data
            odemeEmriRizasi.rzBlg = new RizaBilgileriDto()
            {
                rizaNo = consentEntity.Id.ToString(),
                olusZmn = DateTime.UtcNow,
                rizaDrm = OpenBankingConstants.RizaDurumu.YetkiBekleniyor
            };
            //Set gkd data
            odemeEmriRizasi.gkd.hhsYonAdr = configuration["HHSForwardingAddress"] ?? string.Empty;
            odemeEmriRizasi.gkd.yetTmmZmn = DateTime.UtcNow.AddMinutes(5);
            consentEntity.AdditionalData = JsonSerializer.Serialize(odemeEmriRizasi);
            consentEntity.State = OpenBankingConstants.RizaDurumu.YetkiBekleniyor;
            consentEntity.ConsentType = OpenBankingConstants.ConsentType.OpenBankingPayment;

            context.Consents.Add(consentEntity);

            await context.SaveChangesAsync();
            return Results.Ok(odemeEmriRizasi);
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
    /// <exception cref="NotImplementedException"></exception>
    private IResult IsDataValidToAccountConsentPost(HesapBilgisiRizaIstegiHHSDto rizaIstegi,
    IConfiguration configuration)
    {
        //TODO:Ozlem check status, if any other consent.
        //TODO:Ozlem Check Header
        //TODO:Ozlem Check fields length and necessity
        //TODO:Ozlem Check if user is customer
        //TODO:Ozlem Check fields length and necessity

        //Check KatılımcıBilgisi
        if (string.IsNullOrEmpty(rizaIstegi.katilimciBlg.hhsKod)//Required fields
            || string.IsNullOrEmpty(rizaIstegi.katilimciBlg.yosKod)
            || configuration["HHSCode"] != rizaIstegi.katilimciBlg.hhsKod)
        {
            return Results.BadRequest("TR.OHVPS.Resource.InvalidFormat. HHSKod YOSKod required");
        }
        //TODO:Ozlem hhskod, yoskod check validaty


        //Check GKD
        if (!string.IsNullOrEmpty(rizaIstegi.gkd.yetYntm)
            && ((rizaIstegi.gkd.yetYntm == OpenBankingConstants.GKDTur.Yonlendirmeli
                && string.IsNullOrEmpty(rizaIstegi.gkd.yonAdr))
               || (rizaIstegi.gkd.yetYntm == OpenBankingConstants.GKDTur.Ayrik
                   && string.IsNullOrEmpty(rizaIstegi.gkd.bldAdr))))
        {
            return Results.BadRequest("TR.OHVPS.Resource.InvalidFormat. GKD data not valid.");
        }

        //Check Kimlik
        if (string.IsNullOrEmpty(rizaIstegi.kmlk.kmlkTur)//Check required fields
            || string.IsNullOrEmpty(rizaIstegi.kmlk.kmlkVrs)
            || (string.IsNullOrEmpty(rizaIstegi.kmlk.krmKmlkTur) != string.IsNullOrEmpty(rizaIstegi.kmlk.krmKmlkVrs))
            || string.IsNullOrEmpty(rizaIstegi.kmlk.ohkTur))
        {
            return Results.BadRequest("TR.OHVPS.Resource.InvalidFormat. Kmlk data is not valid");
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
            return Results.BadRequest("TR.OHVPS.Resource.InvalidFormat. Kmlk data validation failed.");
        }


        //Check HesapBilgisi
        //Check izinbilgisi properties
        if (rizaIstegi.hspBlg.iznBlg.iznTur?.Any() == false
            || rizaIstegi.hspBlg.iznBlg.erisimIzniSonTrh == System.DateTime.MinValue
            || rizaIstegi.hspBlg.iznBlg.erisimIzniSonTrh > System.DateTime.UtcNow.AddMonths(6)
            || rizaIstegi.hspBlg.iznBlg.erisimIzniSonTrh < System.DateTime.UtcNow.AddDays(1))
        {
            return Results.BadRequest("TR.OHVPS.Resource.InvalidFormat. IznBld data check failed.");
        }

        //Check işlem sorgulama başlangıç zamanı
        if (rizaIstegi.hspBlg.iznBlg.hesapIslemBslZmn.HasValue)
        {
            //Temel işlem bilgisi ve/veya ayrıntılı işlem bilgisi seçilmiş olması gerekir
            if (rizaIstegi.hspBlg.iznBlg.iznTur.Any(p => p != OpenBankingConstants.IzinTur.TemelIslem && p != OpenBankingConstants.IzinTur.AyrintiliIslem))
            {
                return Results.BadRequest("TR.OHVPS.Resource.InvalidFormat. hesapIslemBslZmn related iznTur not valid.");
            }
            if (rizaIstegi.hspBlg.iznBlg.hesapIslemBslZmn.Value < DateTime.UtcNow.AddMonths(-12))//Data constraints
            {
                return Results.BadRequest("TR.OHVPS.Resource.InvalidFormat. hesapIslemBslZmn not valid.");
            }
        }
        if (rizaIstegi.hspBlg.iznBlg.hesapIslemBtsZmn.HasValue)//Check işlem sorgulama bitiş zamanı
        {
            //Temel işlem bilgisi ve/veya ayrıntılı işlem bilgisi seçilmiş olması gerekir
            if (rizaIstegi.hspBlg.iznBlg.iznTur.All(p => p != OpenBankingConstants.IzinTur.TemelIslem && p != OpenBankingConstants.IzinTur.AyrintiliIslem))
            {
                return Results.BadRequest("TR.OHVPS.Resource.InvalidFormat hesapIslemBtsZmn related iznTur not valid.");
            }
            if (rizaIstegi.hspBlg.iznBlg.hesapIslemBtsZmn.Value > DateTime.UtcNow.AddMonths(12))//Data constraints
            {
                return Results.BadRequest("TR.OHVPS.Resource.InvalidFormat. hesapIslemBtsZmn not valid.");
            }
        }
        return Results.Ok();
    }

    /// <summary>
    ///  Checks if data is valid for payment information consent post process
    /// </summary>
    /// <param name="rizaIstegi">To be checked data</param>
    /// <param name="configuration">Config file</param>
    /// <returns></returns>
    private async Task<IResult> IsDataValidToPaymentInformationConsentPost(OdemeEmriRizaIstegiHHSDto rizaIstegi,
     IConfiguration configuration,
     IPaymentService paymentService)
    {

        //TODO:Ozlem update method
        //Check KatılımcıBilgisi
        if (string.IsNullOrEmpty(rizaIstegi.katilimciBlg.hhsKod)//Required fields
            || string.IsNullOrEmpty(rizaIstegi.katilimciBlg.yosKod)
            || configuration["HHSCode"] != rizaIstegi.katilimciBlg.hhsKod)
        {
            return Results.BadRequest("TR.OHVPS.Resource.InvalidFormat. HHSKod YOSKod required");
        }
        //TODO:Ozlem hhskod, yoskod check validaty

        //Check GKD
        if (!string.IsNullOrEmpty(rizaIstegi.gkd.yetYntm)
            && ((rizaIstegi.gkd.yetYntm == OpenBankingConstants.GKDTur.Yonlendirmeli
                && string.IsNullOrEmpty(rizaIstegi.gkd.yonAdr))
               || (rizaIstegi.gkd.yetYntm == OpenBankingConstants.GKDTur.Ayrik
                   && string.IsNullOrEmpty(rizaIstegi.gkd.bldAdr))))
        {
            return Results.BadRequest("TR.OHVPS.Resource.InvalidFormat. GKD data not valid.");
        }
        //Check odmBsltm  Kimlik
        if (string.IsNullOrEmpty(rizaIstegi.odmBsltm.kmlk.ohkTur)//Check required fields
            || (rizaIstegi.odmBsltm.kmlk.ohkTur == OpenBankingConstants.OHKTur.Bireysel
                && (string.IsNullOrEmpty(rizaIstegi.odmBsltm.kmlk.kmlkTur) || string.IsNullOrEmpty(rizaIstegi.odmBsltm.kmlk.kmlkVrs)))
            || (rizaIstegi.odmBsltm.kmlk.ohkTur == OpenBankingConstants.OHKTur.Kurumsal
                && (string.IsNullOrEmpty(rizaIstegi.odmBsltm.kmlk.krmKmlkTur) || string.IsNullOrEmpty(rizaIstegi.odmBsltm.kmlk.krmKmlkVrs))))
        {
            return Results.BadRequest("TR.OHVPS.Resource.InvalidFormat. odmBsltm => Kmlk data is not valid");
        }
        //Check odmBsltma Islem Tutarı
        if (string.IsNullOrEmpty(rizaIstegi.odmBsltm.islTtr.ttr)//Check required fields
            || string.IsNullOrEmpty(rizaIstegi.odmBsltm.islTtr.prBrm))
        {
            return Results.BadRequest("TR.OHVPS.Resource.InvalidFormat. odmBsltm => islTtr required fields empty");
        }
        //TODO:Ozlem check gönderen hesap If different than system record. Do not accept consent

        //Check odmBsltma Alıcı
        if (rizaIstegi.odmBsltm.alc.kolas == null
            && (string.IsNullOrEmpty(rizaIstegi.odmBsltm.alc.unv) || string.IsNullOrEmpty(rizaIstegi.odmBsltm.alc.hspNo)))
        {
            return Results.BadRequest("TR.OHVPS.Resource.InvalidFormat. If kolas is null, unv and hspno is required");
        }

        if (rizaIstegi.odmBsltm.alc.kolas != null
        && rizaIstegi.odmBsltm.kkod != null)
        {
            return Results.BadRequest("TR.OHVPS.Resource.InvalidFormat. Kolas and KareKod can not be used at the same time");
        }

        if (rizaIstegi.odmBsltm.kkod != null
            && (string.IsNullOrEmpty(rizaIstegi.odmBsltm.kkod.aksTur)
            || string.IsNullOrEmpty(rizaIstegi.odmBsltm.kkod.kkodUrtcKod)))
        {
            return Results.BadRequest("TR.OHVPS.Resource.InvalidFormat. aksTur, kkodUrtcKod required fields.");
        }

        return Results.Ok();
    }

    private ApiResult IsDataValidToGetAccountConsent(Consent entity)
    {
        ApiResult result = new ApiResult();
        if (entity == null)
        {
            result.Result = false;
            result.Message = "BadRequest.";
        }
        return result;
    }

    private ApiResult IsDataValidToGetPaymentConsent(Consent entity)
    {
        ApiResult result = new ApiResult();
        if (entity == null)
        {
            result.Result = false;
            result.Message = "BadRequest.";
        }
        return result;
    }

    /// <summary>
    /// Check if consent is valid to be deleted
    /// </summary>
    /// <param name="entity">To be checked entity</param>
    /// <returns>Data validation result</returns>
    private ApiResult IsDataValidToDeleteAccountConsent(Consent entity)
    {
        ApiResult result = new ApiResult();
        if (entity == null)
        {
            result.Result = false;
            result.Message = "BadRequest.";
        }

        if (!ConstantHelper.GetAccountConsentCanBeDeleteStatusList().Contains(entity.State))
        {
            //State not valid to set as deleted
            result.Result = false;
            result.Message = "Account consent status not valid to marked as deleted";
        }
        return result;
    }

    /// <summary>
    /// Check if consent is valid to be updated for authorization
    /// </summary>
    /// <param name="entity">To be checked entity</param>
    /// <param name="saveAccountReference">to be checked object</param>
    /// <returns>Data validation result</returns>
    private ApiResult IsDataValidToUpdateAccountConsentForAuthorization(Consent entity, SaveAccountReferenceDto saveAccountReference)
    {
        ApiResult result = new ApiResult();
        if (entity == null)
        {
            result.Result = false;
            result.Message = "BadRequest.";
        }

        if (entity.State != OpenBankingConstants.RizaDurumu.YetkiBekleniyor)
        {
            result.Result = false;
            result.Message = "Consent state not valid to process";
        }

        return result;
    }
}
