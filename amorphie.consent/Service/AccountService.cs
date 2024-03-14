using System.Globalization;
using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.consent.Helper;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;

namespace amorphie.consent.Service;

public class AccountService : IAccountService
{
    private readonly IAccountClientService _accountClientService;
    private readonly IOBAuthorizationService _authorizationService;
    private readonly ConsentDbContext _context;

    public AccountService(IAccountClientService accountClientService,
        IOBAuthorizationService authorizationService,
        ConsentDbContext context)
    {
        _accountClientService = accountClientService;
        _authorizationService = authorizationService;
        _context = context;
    }

    public async Task<ApiResult> GetAuthorizedAccounts(HttpContext httpContext, string userTCKN, string yosCode, int? syfKytSayi, int? syfNo,
        string? srlmKrtr, string? srlmYon)
    {
        ApiResult result = new();
        try
        {
            var permisssions = new List<string>()
            {
                OpenBankingConstants.IzinTur.TemelHesapBilgisi,
                OpenBankingConstants.IzinTur.AyrintiliHesapBilgisi
            };
            //Get account consent from database
            var authConsentResult =
                await _authorizationService.GetAuthorizedAccountConsent(userTCKN, yosCode: yosCode,
                    permissions: permisssions);
            if (authConsentResult.Result == false
                || authConsentResult.Data == null)
            {
                //Error or no consent in db
                return authConsentResult;
            }

            var activeConsent = (Consent)authConsentResult.Data;
            //Set header values
            bool havingDetailPermission = activeConsent.OBAccountConsentDetails.Any(d =>
                d.PermissionTypes?.Contains(OpenBankingConstants.IzinTur.AyrintiliHesapBilgisi) ?? false);
            var permissionType = havingDetailPermission ? "D" : "T";
            // Build account service parameters
            var (resolvedSyfKytSayi, resolvedSyfNo, resolvedSrlmKrtr, resolvedSrlmYon) = GetDefaultAccountServiceParameters(
                syfKytSayi,
                syfNo,
                srlmKrtr,
                srlmYon
            );

            //Get accounts of customer from service
            var serviceResponse = await _accountClientService.GetAccounts(userTCKN, permissionType,
                resolvedSyfKytSayi, resolvedSyfNo, resolvedSrlmKrtr, resolvedSrlmYon);
            if (serviceResponse is null)
            {
                //No account
                result.Data = null;
                return result;
            }

            List<HesapBilgileriDto>? accounts = serviceResponse.hesapBilgileri;
            //filter accounts
            accounts = accounts?.Where(a =>
                    activeConsent.OBAccountConsentDetails.Any(d =>
                        d.AccountReferences?.Contains(a.hspTml.hspRef) ?? false))
                .ToList();
            accounts = accounts?.Select(a =>
            {
                a.rizaNo = activeConsent.Id.ToString();
                return a;
            }).ToList();
            result.Data = accounts;
            //Set header total count and link properties
            SetHeaderLinkForAccount(httpContext, serviceResponse.toplamHesapSayisi, resolvedSyfKytSayi, resolvedSyfNo, resolvedSrlmKrtr, resolvedSrlmYon);
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }

    public async Task<ApiResult> GetAuthorizedAccountByHspRef(string userTCKN, string yosCode, string hspRef)
    {
        ApiResult result = new();
        try
        {
            var permisssions = new List<string>()
            {
                OpenBankingConstants.IzinTur.TemelHesapBilgisi,
                OpenBankingConstants.IzinTur.AyrintiliHesapBilgisi
            };
            //Get account consent from database
            var authConsentResult =
                await _authorizationService.GetAuthorizedAccountConsent(userTCKN, yosCode: yosCode,
                    permissions: permisssions, hspRef);
            if (authConsentResult.Result == false
                || authConsentResult.Data == null)
            {
                //Error or no consent in db
                return authConsentResult;
            }

            //Get account of customer from service
            HesapBilgileriDto? account = await _accountClientService.GetAccountByHspRef(userTCKN, hspRef);
            if (account == null)
            {
                //No account
                result.Data = account;
                return result;
            }

            var activeConsent = (Consent)authConsentResult.Data;
            //Set rizaNo
            account.rizaNo = activeConsent.Id.ToString();
            result.Data = account;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }

    public async Task<ApiResult> GetAuthorizedBalances(HttpContext httpContext, string userTCKN, string yosCode, int? syfKytSayi, int? syfNo,
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
            var authConsentResult =
                await _authorizationService.GetAuthorizedAccountConsent(userTCKN, yosCode: yosCode,
                    permissions: permisssions);
            if (authConsentResult.Result == false
                || authConsentResult.Data == null)
            {
                //Error or no consent in db
                return authConsentResult;
            }

            // Build account service parameters
            var (resolvedSyfKytSayi, resolvedSyfNo, resolvedSrlmKrtr, resolvedSrlmYon) = GetDefaultAccountServiceParameters(
                syfKytSayi,
                syfNo,
                srlmKrtr,
                srlmYon
            );
            //Get balances of customer from service
            var serviceResponse = await _accountClientService.GetBalances(userTCKN, resolvedSyfKytSayi, resolvedSyfNo, resolvedSrlmKrtr, resolvedSrlmYon);
            if (serviceResponse is null)
            {
                //No balance
                result.Data = null;
                return result;
            }

            List<BakiyeBilgileriDto>? balances = serviceResponse.bakiyeBilgileri;
            //Get account consent from db
            var activeConsent = (Consent)authConsentResult.Data;

            //filter balances
            balances = balances?.Where(b =>
                    activeConsent.OBAccountConsentDetails.Any(r => r.AccountReferences?.Contains(b.hspRef) ?? false))
                .ToList();
            result.Data = balances;
            //Set header total count and link properties
            SetHeaderLinkForBalance(httpContext, serviceResponse.toplamBakiyeSayisi, resolvedSyfKytSayi, resolvedSyfNo, resolvedSrlmKrtr, resolvedSrlmYon);
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }

    public async Task<ApiResult> GetAuthorizedBalanceByHspRef(string userTCKN, string yosCode, string hspRef)
    {
        ApiResult result = new();
        try
        {
            var permisssions = new List<string>()
            {
                OpenBankingConstants.IzinTur.BakiyeBilgisi
            };
            //Get account consent from database
            var authConsentResult =
                await _authorizationService.GetAuthorizedAccountConsent(userTCKN, yosCode: yosCode,
                    permissions: permisssions, accountRef: hspRef);
            if (authConsentResult.Result == false
                || authConsentResult.Data == null)
            {
                //Error or no consent in db
                return authConsentResult;
            }

            //Get balance by account reference number of customer from service
            BakiyeBilgileriDto? balance = await _accountClientService.GetBalanceByHspRef(userTCKN, hspRef);
            if (balance == null)
            {
                //No balance
                result.Data = balance;
                return result;
            }
            result.Data = balance;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }

    public async Task<ApiResult> GetTransactionsByHspRef(HttpContext httpContext, string userTCKN, string yosCode, string hspRef, string psuInitiated, DateTime hesapIslemBslTrh,
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
            var permisssions = new List<string>()
            {
                OpenBankingConstants.IzinTur.TemelIslemBilgisi,
                OpenBankingConstants.IzinTur.AyrintiliIslemBilgisi
            };
            //Get account consent from database
            var authConsentResult =
                await _authorizationService.GetAuthorizedAccountConsent(userTCKN, yosCode: yosCode,
                    permissions: permisssions, accountRef: hspRef);
            if (authConsentResult.Result == false
                || authConsentResult.Data == null)
            {
                //Error or no consent in db
                return authConsentResult;
            }

            var activeConsent = (Consent)authConsentResult.Data;
            //Check if post data is valid to process.
            var checkValidationResult =
                IsDataValidToGetTransactionsByHspRef(activeConsent,
                    psuInitiated,
                    hesapIslemBslTrh,
                    hesapIslemBtsTrh,
                    minIslTtr,
                    mksIslTtr,
                    brcAlc,
                    syfKytSayi,
                    srlmKrtr,
                    srlmYon);
            if (!checkValidationResult.Result)
            {
                //Data not valid
                return checkValidationResult;
            }

            //Get header values
            bool havingDetailPermission = activeConsent.OBAccountConsentDetails.Any(d =>
                d.PermissionTypes?.Contains(OpenBankingConstants.IzinTur.AyrintiliIslemBilgisi) ?? false);
            var permissionType = havingDetailPermission ? "D" : "T";
            string ohkTur = activeConsent.OBAccountConsentDetails.FirstOrDefault()?.UserType ?? OpenBankingConstants.OHKTur.Bireysel;
            // Build account service parameters
            var (resolvedSyfKytSayi, resolvedSyfNo, resolvedSrlmKrtr, resolvedSrlmYon) = GetDefaultAccountServiceParameters(
                syfKytSayi,
                syfNo,
                srlmKrtr,
                srlmYon
            );
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

            if (serviceResponse is null)
            {
                //No transaction
                result.Data = null;
                return result;
            }
            result.Data = serviceResponse;
            //Set header total count and link properties
            SetHeaderLinkForTransaction(httpContext, serviceResponse.toplamIslemSayisi, hspRef, hesapIslemBslTrh, hesapIslemBtsTrh, resolvedSyfKytSayi, resolvedSyfNo, resolvedSrlmKrtr, resolvedSrlmYon, minIslTtr, mksIslTtr, brcAlc);

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
            srlmKrtr ?? OpenBankingConstants.AccountServiceParameters.srlmKrtrAccount,
            srlmYon ?? OpenBankingConstants.AccountServiceParameters.srlmYon
        );
    }



    /// <summary>
    /// Checks if parameters valid to get transactions
    /// </summary>
    /// <param name="consent"></param>
    /// <param name="psuInitiated"></param>
    /// <param name="hesapIslemBslTrh"></param>
    /// <param name="hesapIslemBtsTrh"></param>
    /// <param name="minIslTtr"></param>
    /// <param name="mksIslTtr"></param>
    /// <param name="brcAlc"></param>
    /// <param name="syfKytSayi"></param>
    /// <param name="srlmKrtr"></param>
    /// <param name="srlmYon"></param>
    /// <returns></returns>
    private ApiResult IsDataValidToGetTransactionsByHspRef(Consent consent,
        string psuInitiated,
        DateTime hesapIslemBslTrh,
        DateTime hesapIslemBtsTrh,
        string? minIslTtr,
        string? mksIslTtr,
        string? brcAlc,
        int? syfKytSayi,
        string? srlmKrtr,
        string? srlmYon)
    {
        ApiResult result = new();
        var today = DateTime.UtcNow;
        if (hesapIslemBtsTrh == DateTime.MinValue
            || hesapIslemBslTrh == DateTime.MinValue) //required parameters
        {
            result.Result = false;
            result.Message = "hesapIslemBtsTrh,hesapIslemBslTrh values not valid ";
            return result;
        }

        //Check hesapIslemBtsTrh
        if (hesapIslemBtsTrh > today)
        {
            result.Result = false;
            result.Message = "hesapIslemBtsTrh can not be later than enquiry datetime.";
            return result;
        }

        if (hesapIslemBtsTrh < hesapIslemBslTrh)
        {
            result.Result = false;
            result.Message = "hesapIslemBtsTrh can not be early than hesapIslemBslTrh.";
            return result;
        }

        //ÖHK tarafından tetiklenen sorgularda; hesapIslemBslTrh ve hesapIslemBtsTrh arası fark bireysel ÖHK’lar için en fazla 1 ay,kurumsal ÖHK’lar için ise en fazla 1 hafta olabilir.
        if (psuInitiated == OpenBankingConstants.PSUInitiated.OHKStarted)
        {
            if (consent.OBAccountConsentDetails.FirstOrDefault()?.UserType == OpenBankingConstants.OHKTur.Bireysel)
            {
                if (hesapIslemBslTrh.AddMonths(1) < hesapIslemBtsTrh)
                {
                    result.Result = false;
                    result.Message = "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 month.";
                    return result;
                }
            }
            else if (consent.OBAccountConsentDetails.FirstOrDefault()?.UserType == OpenBankingConstants.OHKTur.Kurumsal)
            {
                if (hesapIslemBslTrh.AddDays(7) < hesapIslemBtsTrh)
                {
                    result.Result = false;
                    result.Message = "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 week.";
                    return result;
                }
            }
        }

        //YÖS tarafından sistemsel yapılan sorgulamalarda hem bireysel, hem de kurumsal ÖHK’lar için;son 24 saat sorgulanabilir. Bu yüzden hesapIslemBtsTrh-24 saat’ten daha uzun bir aralık sorgulanamaz olmalıdır.
        if (psuInitiated == OpenBankingConstants.PSUInitiated.SystemStarted
            && (hesapIslemBtsTrh - hesapIslemBslTrh).TotalHours > 24)
        {
            result.Result = false;
            result.Message = "hesapIslemBtsTrh hesapIslemBslTrh can not be later than enquiry datetime.";
            return result;
        }

        if (!string.IsNullOrEmpty(brcAlc) && !ConstantHelper.GetBrcAlcList().Contains(brcAlc))
        {
            result.Result = false;
            result.Message = "brcAlc value is not valid.";
            return result;
        }

        if (syfKytSayi is > OpenBankingConstants.AccountServiceParameters.syfKytSayi)
        {
            result.Result = false;
            result.Message = $"syfKytSayi value is not valid. syfKytSayi can be maximum: {OpenBankingConstants.AccountServiceParameters.syfKytSayi} ";
            return result;
        }

        if (!string.IsNullOrEmpty(minIslTtr) && !ConstantHelper.IsValidAmount(minIslTtr))
        {
            result.Result = false;
            result.Message = "minIslTtr value is not valid.";
            return result;
        }
        if (!string.IsNullOrEmpty(mksIslTtr) && !ConstantHelper.IsValidAmount(mksIslTtr))
        {
            result.Result = false;
            result.Message = "mksIslTtr value is not valid.";
            return result;
        }

        if (!string.IsNullOrEmpty(srlmKrtr)
            && OpenBankingConstants.AccountServiceParameters.srlmKrtrTransaction != srlmKrtr)
        {
            result.Result = false;
            result.Message = "srlmKrtr value is not valid.";
            return result;
        }

        if (!string.IsNullOrEmpty(srlmYon)
            && !ConstantHelper.GetSrlmYonList().Contains(srlmYon))
        {
            result.Result = false;
            result.Message = "srlmYon value is not valid.";
            return result;
        }

        return result;
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
    /// <param name="httpContext"></param>
    /// <param name="totalCount"></param>
    /// <param name="hesapIslemBtsTrh"></param>
    /// <param name="syfKytSayi"></param>
    /// <param name="syfNo"></param>
    /// <param name="srlmKrtr"></param>
    /// <param name="srlmYon"></param>
    /// <param name="hspRef"></param>
    /// <param name="hesapIslemBslTrh"></param>
    /// <param name="minIslTtr"></param>
    /// <param name="mksIslTtr"></param>
    /// <param name="brcAlc"></param>
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

}