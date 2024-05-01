using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.consent.Helper;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;
using AutoMapper;

namespace amorphie.consent.Service;

public class AccountService : IAccountService
{
    private readonly IAccountClientService _accountClientService;
    private readonly IOBAuthorizationService _authorizationService;
    private readonly ConsentDbContext _context;
    private readonly IMapper _mapper;

    public AccountService(IAccountClientService accountClientService,
        IOBAuthorizationService authorizationService,
        ConsentDbContext context,
        IMapper mapper)
    {
        _accountClientService = accountClientService;
        _authorizationService = authorizationService;
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResult> GetAuthorizedAccounts(HttpContext httpContext, string userTCKN, string consentId, string yosCode,List<OBErrorCodeDetail> errorCodeDetails, int? syfKytSayi, int? syfNo,
        string? srlmKrtr, string? srlmYon)
    {
        ApiResult result = new();
        try
        {

            //Get account consent from database
            var getConsentResult =
                await _authorizationService.GetAccountConsent(consentId, userTCKN, yosCode: yosCode,
                    permissions: new List<string>()
                    {
                        OpenBankingConstants.IzinTur.TemelHesapBilgisi,
                        OpenBankingConstants.IzinTur.AyrintiliHesapBilgisi
                    });
            var checkConsentResult = CheckAccountConsent(getConsentResult, httpContext, errorCodeDetails);
            if (!checkConsentResult.Result)//Consent in db is not valid
            {
                //Error
                return checkConsentResult;
            }
            

            var activeConsent = (Consent)getConsentResult.Data!;
            var consentDetail = activeConsent.OBAccountConsentDetails.FirstOrDefault()!;
            
            
            //Set header values
            bool havingDetailPermission = consentDetail.PermissionTypes.Contains(OpenBankingConstants.IzinTur.AyrintiliHesapBilgisi);
            var permissionType = havingDetailPermission ? OpenBankingConstants.AccountServiceParameters.izinTurDetay
                : OpenBankingConstants.AccountServiceParameters.izinTurTemel;
            //Create service request body object
            var requestObject = new GetByAccountRefRequestDto()
            {
                hspRefs = consentDetail.AccountReferences!
            };

            // Build account service parameters
            var (resolvedSyfKytSayi, resolvedSyfNo, resolvedSrlmKrtr, resolvedSrlmYon) = GetDefaultAccountServiceParameters(
                syfKytSayi,
                syfNo,
                srlmKrtr,
                srlmYon
            );
            //Check parameters.
            var checkValidationResult =
                OBConsentValidationHelper.IsParametersValidToGetAccountsBalances(httpContext,errorCodeDetails, 
                    resolvedSyfKytSayi,
                    resolvedSrlmKrtr,
                    resolvedSrlmYon);
            if (!checkValidationResult.Result)
            {
                //parameters not valid
                return checkValidationResult;
            }

            //Get accounts of customer from service
            var serviceResponse = await _accountClientService.GetAccounts(izinTur: permissionType,
                accountRefs: requestObject,
                customerId: userTCKN,
                syfKytSayi: resolvedSyfKytSayi,
                syfNo: resolvedSyfNo,
                srlmKrtr: resolvedSrlmKrtr,
                srlmYon: resolvedSrlmYon);

            if (serviceResponse?.error != null)
            {
                result.Result = false;
                result.Data = serviceResponse.error;
                return result;
            }
        
            List<HesapBilgileriDto>? accounts = serviceResponse?.data?.hesapBilgileri;
            accounts = accounts?.Select(a =>
            {
                a.rizaNo = activeConsent.Id.ToString();
                return a;
            }).ToList();
            result.Data = accounts;
            //Set header total count and link properties
            SetHeaderLinkForAccount(httpContext, serviceResponse?.data?.toplamHesapSayisi ?? 0, resolvedSyfKytSayi, resolvedSyfNo, resolvedSrlmKrtr, resolvedSrlmYon);
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }

    public async Task<ApiResult> GetAuthorizedAccountsForUI(string userTCKN, List<string> accountReferences, int? syfKytSayi, int? syfNo,
      string? srlmKrtr, string? srlmYon)
    {
        ApiResult result = new();
        try
        {

            //Create service request body object
            var requestObject = new GetByAccountRefRequestDto()
            {
                hspRefs = accountReferences
            };

            // Build account service parameters
            var (resolvedSyfKytSayi, resolvedSyfNo, resolvedSrlmKrtr, resolvedSrlmYon) = GetDefaultAccountServiceParameters(
                syfKytSayi,
                syfNo,
                srlmKrtr,
                srlmYon
            );

            //Get accounts of customer from service
            var serviceResponse = await _accountClientService.GetAccounts(izinTur: OpenBankingConstants.AccountServiceParameters.izinTurTemel,
                accountRefs: requestObject,
                customerId: userTCKN,
                syfKytSayi: resolvedSyfKytSayi,
                syfNo: resolvedSyfNo,
                srlmKrtr: resolvedSrlmKrtr,
                srlmYon: resolvedSrlmYon);
            
            if (serviceResponse?.error != null)//Error in service
            {
                result.Result = false;
                result.Data = serviceResponse.error;
                return result;
            }
            
            List<HesapBilgileriDto>? accounts = serviceResponse?.data?.hesapBilgileri;
            result.Data = accounts;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }


    public async Task<ApiResult> GetAuthorizedAccountByHspRef(HttpContext httpContext,string userTckn, string consentId, string yosCode, string hspRef,List<OBErrorCodeDetail> errorCodeDetails)
    {
        ApiResult result = new();
        try
        {
            var permissions = new List<string>()
            {
                OpenBankingConstants.IzinTur.TemelHesapBilgisi,
                OpenBankingConstants.IzinTur.AyrintiliHesapBilgisi
            };
            //Get account consent from database
            var getConsentResult =
                await _authorizationService.GetAccountConsentByAccountRef(userTckn: userTckn, consentId: consentId, yosCode: yosCode, accountRef: hspRef);
            var checkConsentResult = CheckAccountConsent(getConsentResult, httpContext, errorCodeDetails);
            if (!checkConsentResult.Result)//Consent in db is not valid
            {
                //Error
                return checkConsentResult;
            }
            
            var activeConsent = (Consent)getConsentResult.Data!;
            if (!activeConsent.OBAccountConsentDetails.Any(a => permissions.Any(a.PermissionTypes.Contains)))
            {//Consent permissions not valid to get response
                result.Result = false;
                result.Data = OBErrorResponseHelper.GetForbiddenError(httpContext, errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.InvalidPermissionGetAccount);
                return result;
            }
            //Get account of customer from service
            var serviceResponse = await _accountClientService.GetAccountByHspRef(userTckn, hspRef);
            if (serviceResponse?.error != null)//Error in service
            {
                result.Result = false;
                result.Data = serviceResponse.error;
                return result;
            }

            HesapBilgileriDto? account = serviceResponse?.data;
            //Set rizaNo
            if (account != null)
            {
                account.rizaNo = activeConsent.Id.ToString();
            }
            result.Data = account;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }

    public async Task<ApiResult> GetAuthorizedBalances(HttpContext httpContext, string userTCKN, string consentId, string yosCode,List<OBErrorCodeDetail> errorCodeDetails, int? syfKytSayi, int? syfNo,
        string? srlmKrtr, string? srlmYon)
    {
        ApiResult result = new();
        try
        {
            var permisssions = new List<string>()
            {
                OpenBankingConstants.IzinTur.BakiyeBilgisi
            };
            //Get account consent from database
            var getConsentResult =
                await _authorizationService.GetAccountConsent(userTckn: userTCKN, consentId:consentId, yosCode: yosCode,
                    permissions: permisssions);
            var checkConsentResult = CheckAccountConsent(getConsentResult, httpContext, errorCodeDetails);
            if (!checkConsentResult.Result)//Consent in db is not valid
            {
                //Error
                return checkConsentResult;
            }
            
            var activeConsent = (Consent)getConsentResult.Data!;
            var consentDetail = activeConsent.OBAccountConsentDetails.FirstOrDefault()!;

            // Build account service parameters
            var (resolvedSyfKytSayi, resolvedSyfNo, resolvedSrlmKrtr, resolvedSrlmYon) = GetDefaultAccountServiceParameters(
                syfKytSayi,
                syfNo,
                srlmKrtr,
                srlmYon
            );
            //Check parameters.
            var checkValidationResult =
                OBConsentValidationHelper.IsParametersValidToGetAccountsBalances(httpContext,errorCodeDetails, 
                    resolvedSyfKytSayi,
                    resolvedSrlmKrtr,
                    resolvedSrlmYon);
            if (!checkValidationResult.Result)
            {
                //parameters not valid
                return checkValidationResult;
            }
            
            //Create service request body object
            var requestObject = new GetByAccountRefRequestDto()
            {
                hspRefs = consentDetail.AccountReferences!
            };
            //Get balances of customer from service
            var serviceResponse = await _accountClientService.GetBalances(accountRefs: requestObject,
                customerId: userTCKN,
                syfKytSayi: resolvedSyfKytSayi,
                syfNo: resolvedSyfNo,
                srlmKrtr: resolvedSrlmKrtr,
                srlmYon: resolvedSrlmYon);
          
            if (serviceResponse?.error != null)//Error in service
            {
                result.Result = false;
                result.Data = serviceResponse.error;
                return result;
            }
            List<BakiyeBilgileriDto>? balances = serviceResponse?.data?.bakiyeBilgileri;
            result.Data = balances;
            //Set header total count and link properties
            SetHeaderLinkForBalance(httpContext, serviceResponse?.data?.toplamBakiyeSayisi ?? 0, resolvedSyfKytSayi, resolvedSyfNo, resolvedSrlmKrtr, resolvedSrlmYon);
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }

    public async Task<ApiResult> GetAuthorizedBalanceByHspRef(HttpContext httpContext,string userTCKN, string consentId, string yosCode, string hspRef,List<OBErrorCodeDetail> errorCodeDetails)
    {
        ApiResult result = new();
        try
        {
            var permissions = new List<string>()
            {
                OpenBankingConstants.IzinTur.BakiyeBilgisi
            };
            //Get account consent from database
            var getConsentResult =
                await _authorizationService.GetAccountConsentByAccountRef( userTckn:userTCKN, consentId:consentId, yosCode: yosCode,accountRef: hspRef);
            var checkConsentResult = CheckAccountConsent(getConsentResult, httpContext, errorCodeDetails);
            if (!checkConsentResult.Result)
            {
                //Error
                return checkConsentResult;
            }
            
            var activeConsent = (Consent)getConsentResult.Data!;
            if (!activeConsent.OBAccountConsentDetails.Any(a => permissions.Any(a.PermissionTypes.Contains)))
            {//Consent permissions not valid to get response
                result.Result = false;
                result.Data = OBErrorResponseHelper.GetForbiddenError(httpContext, errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.InvalidPermissionGetBalance);
                return result;
            }
        
            //Get balance by account reference number of customer from service
            var serviceResponse  = await _accountClientService.GetBalanceByHspRef(userTCKN, hspRef);
            if (serviceResponse?.error != null)//Error in service
            {
                result.Result = false;
                result.Data = serviceResponse.error;
                return result;
            }
            result.Data = serviceResponse?.data;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }
    
    public async Task<ApiResult> GetTransactionsByHspRef(HttpContext httpContext, string userTckn, string consentId, string yosCode,List<OBErrorCodeDetail> errorCodeDetails, string hspRef, string psuInitiated, DateTime hesapIslemBslTrh,
        DateTime hesapIslemBtsTrh,
        string? minIslTtr,
        string? mksIslTtr,
        string? brcAlc,
        int? syfKytSayi,
        int? syfNo,
        string? srlmKrtr,
        string? srlmYon)
    {
        ApiResult result = new();
        try
        {
            var permissions = new List<string>()
            {
                OpenBankingConstants.IzinTur.TemelIslemBilgisi,
                OpenBankingConstants.IzinTur.AyrintiliIslemBilgisi
            };
            //Get account consent from database
            var getConsentResult =
                await _authorizationService.GetAccountConsentByAccountRef(userTckn: userTckn, consentId:consentId, yosCode: yosCode, accountRef: hspRef);
            var checkConsentResult = CheckAccountConsent(getConsentResult, httpContext, errorCodeDetails);
            if (!checkConsentResult.Result)//Consent in db is not valid
            {
                //Error
                return checkConsentResult;
            }
            
            var activeConsent = (Consent)getConsentResult.Data!;
            var consentDetail = activeConsent.OBAccountConsentDetails.FirstOrDefault()!;
            if (!activeConsent.OBAccountConsentDetails.Any(a => permissions.Any(a.PermissionTypes.Contains)))
            {//Consent permissions not valid to get response
                result.Result = false;
                result.Data = OBErrorResponseHelper.GetForbiddenError(httpContext, errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.InvalidPermissionGetTransaction);
                return result;
            }
       
            //Get header values
            bool havingDetailPermission = activeConsent.OBAccountConsentDetails.Any(d =>
                d.PermissionTypes?.Contains(OpenBankingConstants.IzinTur.AyrintiliIslemBilgisi) ?? false);
            var permissionType = havingDetailPermission ? "D" : "T";
            string ohkTur = consentDetail.UserType;
            // Build account service parameters
            var (resolvedSyfKytSayi, resolvedSyfNo, resolvedSrlmKrtr, resolvedSrlmYon) = GetDefaultAccountServiceParameters(
                syfKytSayi,
                syfNo,
                srlmKrtr,
                srlmYon
            );
            //Check if post data is valid to process.
            var checkValidationResult =
                OBConsentValidationHelper.IsParametersValidToGetTransactionsByHspRef(httpContext,errorCodeDetails,activeConsent,
                    psuInitiated,
                    hesapIslemBslTrh,
                    hesapIslemBtsTrh,
                    minIslTtr,
                    mksIslTtr,
                    brcAlc,
                    resolvedSyfKytSayi,
                    resolvedSrlmKrtr,
                    resolvedSrlmYon);
            if (!checkValidationResult.Result)
            {
                //Data not valid
                return checkValidationResult;
            }
            minIslTtr = (minIslTtr == string.Empty) ? null : minIslTtr;
            mksIslTtr = (mksIslTtr == string.Empty) ? null : mksIslTtr;
            brcAlc = (brcAlc == string.Empty) ? null : brcAlc;
            var serviceResponse = await _accountClientService.GetTransactionsByHspRef(
                hspRef,
                hesapIslemBslTrh.ToString("o"),
                hesapIslemBtsTrh.ToString("o"),
                resolvedSyfKytSayi,
                resolvedSyfNo,
                resolvedSrlmKrtr,
                resolvedSrlmYon,
                minIslTtr,
                mksIslTtr,
                brcAlc,
                permissionType,
                ohkTur,
                psuInitiated);
            if (serviceResponse?.error != null)//Error in service
            {
                result.Result = false;
                result.Data = serviceResponse.error;
                return result;
            }
            result.Data =_mapper.Map<IslemBilgileriDto>(serviceResponse?.data);
            //Set header total count and link properties
            SetHeaderLinkForTransaction(httpContext, serviceResponse?.data?.toplamIslemSayisi ?? 0, hspRef, hesapIslemBslTrh, hesapIslemBtsTrh, resolvedSyfKytSayi, resolvedSyfNo, resolvedSrlmKrtr, resolvedSrlmYon, minIslTtr, mksIslTtr, brcAlc);

        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }

    public async Task<ApiResult> SendConsentToAccountService(List<string> accountRefs, string consentId, string instantBalanceNotificationPermission,
        string sharePermission)
    {
        ApiResult result = new();
        try
        {
           
           //Send account consent details to account service
            string serviceResponse = await _accountClientService.ChangeOpenBankingPermission(accountRefs, consentId, instantBalanceNotificationPermission,sharePermission);
            result.Data = serviceResponse;
            result.Result = serviceResponse.Trim('"') == "1";
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }


    private (int syfKytSayi, int syfNo, string srlmKrtr, string srlmYon) GetDefaultAccountServiceParameters(
        int? syfKytSayi,
        int? syfNo,
        string? srlmKrtr,
        string? srlmYon)
    {
        return (
            syfKytSayi ?? OpenBankingConstants.AccountServiceParameters.syfKytSayi,
            syfNo ?? OpenBankingConstants.AccountServiceParameters.syfNo,
            srlmKrtr ?? OpenBankingConstants.AccountServiceParameters.srlmKrtrAccountAndBalance,
            srlmYon ?? OpenBankingConstants.AccountServiceParameters.srlmYon
        );
    }
    
   
    /// <summary>
    /// Set header x-total-count and link properties
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="totalCount"></param>
    /// <param name="syfKytSayi"></param>
    /// <param name="syfNo"></param>
    /// <param name="srlmKrtr"></param>
    /// <param name="srlmYon"></param>
    private static void SetHeaderLinkForAccount(HttpContext httpContext, int totalCount, int syfKytSayi, int syfNo,
        string srlmKrtr, string srlmYon)
    {
        string basePath = $"ohvps/hbh/s1.1/hesaplar?srlmKrtr={srlmKrtr}&srlmYon={srlmYon}&syfKytSayi={syfKytSayi}";
        SetHeaderLink(basePath, httpContext, totalCount, syfKytSayi, syfNo);
    }

    /// <summary>
    /// Set header x-total-count and link properties
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="totalCount"></param>
    /// <param name="syfKytSayi"></param>
    /// <param name="syfNo"></param>
    /// <param name="srlmKrtr"></param>
    /// <param name="srlmYon"></param>
    private static void SetHeaderLinkForBalance(HttpContext httpContext, int totalCount, int syfKytSayi, int syfNo,
        string srlmKrtr, string srlmYon)
    {
        string basePath = $"ohvps/hbh/s1.1/bakiye?srlmKrtr={srlmKrtr}&srlmYon={srlmYon}&syfKytSayi={syfKytSayi}";
        SetHeaderLink(basePath, httpContext, totalCount, syfKytSayi, syfNo);
    }

    /// <summary>
    /// Set header x-total-count and link properties
    /// </summary>
    private static void SetHeaderLinkForTransaction(HttpContext httpContext, int totalCount, string hspRef, DateTime hesapIslemBslTrh, DateTime hesapIslemBtsTrh, int syfKytSayi, int syfNo, string srlmKrtr, string srlmYon, string? minIslTtr, string? mksIslTtr, string? brcAlc)
    {
        string basePath = GetTransactionBaseUrl(hspRef, hesapIslemBslTrh, hesapIslemBtsTrh, srlmKrtr, srlmYon,
            minIslTtr, mksIslTtr, brcAlc);
        SetHeaderLink(basePath, httpContext, totalCount, syfKytSayi, syfNo);
    }

    /// <summary>
    /// Set header x-total-count and link properties
    /// </summary>
    /// <param name="basePath"></param>
    /// <param name="httpContext"></param>
    /// <param name="totalCount"></param>
    /// <param name="syfKytSayi"></param>
    /// <param name="syfNo"></param>
    private static void SetHeaderLink(string basePath, HttpContext httpContext, int totalCount, int syfKytSayi, int syfNo)
    {
        httpContext.Response.Headers["x-total-count"] = totalCount.ToString();
        if (totalCount == 0)
        {//No record
            return;
        }

        int lastPageNumber = totalCount / syfKytSayi + (totalCount % syfKytSayi > 0 ? 1 : 0);//Calculte lastpage number
        // Construct the Link header value with conditional inclusion of "first" and "last" cases
        string linkHeaderValue = string.Join(", ",
            (syfNo != 1) ? $"</{basePath}&syfNo=1>;rel=\"first\"" : null,
            (syfNo > 1) ? $"</{basePath}&syfNo={syfNo - 1}>;rel=\"prev\"" : null,
            (syfNo < lastPageNumber) ? $"</{basePath}&syfNo={syfNo + 1}>;rel=\"next\"" : null,
            (syfNo != lastPageNumber) ? $"</{basePath}&syfNo={lastPageNumber}>;rel=\"last\"" : null);

        linkHeaderValue = linkHeaderValue.Replace(", ", "").Replace("\r", "").Replace("\n", "");
        httpContext.Response.Headers["Link"] = linkHeaderValue;
    }

    /// <summary>
    /// Generates transaction call base url according to query paramaters
    /// </summary>
    /// <param name="hspRef"></param>
    /// <param name="hesapIslemBslTrh"></param>
    /// <param name="hesapIslemBtsTrh"></param>
    /// <param name="resolvedSrlmKrtr"></param>
    /// <param name="resolvedSrlmYon"></param>
    /// <param name="minIslTtr"></param>
    /// <param name="mksIslTtr"></param>
    /// <param name="brcAlc"></param>
    /// <returns>Transaction get call base url</returns>
    private static string GetTransactionBaseUrl(string hspRef, DateTime hesapIslemBslTrh, DateTime hesapIslemBtsTrh, string resolvedSrlmKrtr, string resolvedSrlmYon, string? minIslTtr, string? mksIslTtr, string? brcAlc)
    {
        string requestUrl = $"ohvps/hbh/s1.1/hesaplar/{hspRef}/islemler?hesapIslemBslTrh={hesapIslemBslTrh}&hesapIslemBtsTrh={hesapIslemBtsTrh}&srlmKrtr={resolvedSrlmKrtr}&srlmYon={resolvedSrlmYon}";

        if (minIslTtr != null)
        {
            requestUrl += $"&minIslTtr={minIslTtr}";
        }

        if (mksIslTtr != null)
        {
            requestUrl += $"&mksIslTtr={mksIslTtr}";
        }

        if (brcAlc != null)
        {
            requestUrl += $"&brcAlc={brcAlc}";
        }

        return requestUrl;
    }
    
    /// <summary>
    /// Check getconsent result validity
    /// </summary>
    /// <returns>Consent check result</returns>
    private ApiResult CheckAccountConsent(ApiResult getConsentResult, HttpContext httpContext, List<OBErrorCodeDetail> errorCodeDetails)
    {
        ApiResult result = new();
        if (!getConsentResult.Result)
        {
            //Error
            return getConsentResult;
        }
        if (getConsentResult.Data == null)
        {
            //no consent in db
            result.Data = OBErrorResponseHelper.GetNotFoundError(httpContext, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.NotFound);
            return result;
        }
            
        var activeConsent = (Consent)getConsentResult.Data;
        var consentDetail = activeConsent.OBAccountConsentDetails.FirstOrDefault();
        if (consentDetail == null)
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetNotFoundError(httpContext, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.NotFound);
            return result;
        }
            
        //Check consent status
        if (consentDetail.LastValidAccessDate <  DateTime.UtcNow)
        {
            //Consent revoked
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(httpContext, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.ConsentRevokedStateEnd);
            return result;
        }
        
        if (consentDetail.AccountReferences == null
            || consentDetail.AccountReferences.Count == 0)
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetNotFoundError(httpContext, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.NotFound);
            return result;
        }

        if (activeConsent.State != OpenBankingConstants.RizaDurumu.YetkiKullanildi)
        {
            //Consent revoked
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(httpContext, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.ConsentRevokedStateNotAutUsed);
            return result;
        }

        return result;
    }


}