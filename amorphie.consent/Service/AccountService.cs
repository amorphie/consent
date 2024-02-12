using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;
using Microsoft.EntityFrameworkCore;

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
    
    public async Task<ApiResult> GetAuthorizedAccounts(string userTCKN, string yosCode,int? syfKytSayi,int? syfNo,string? srlmKrtr,string? srlmYon)
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
            bool havingDetailPermission = activeConsent.OBAccountConsentDetails.Any(d => d.PermissionTypes?.Contains( OpenBankingConstants.IzinTur.AyrintiliHesapBilgisi) ?? false);
            
            // Build account service parameters
            SetDefaultAccountServiceParameters(ref syfKytSayi, ref syfNo, ref srlmKrtr, ref srlmYon);
            
            //Get accounts of customer from service
            List<HesapBilgileriDto> accounts = await _accountClientService.GetAccounts(userTCKN, syfKytSayi.Value,syfNo.Value,srlmKrtr,srlmYon, permissionType: havingDetailPermission ? "D": null);
            if (!accounts?.Any() ?? false)
            {
                //No account
                result.Data = accounts;
                return result;
            }
            
            //filter accounts
            accounts = accounts.Where(a =>
                    activeConsent.OBAccountConsentDetails.Any(d =>
                        d.AccountReferences?.Contains(a.hspTml.hspRef) ?? false))
                .ToList();
            accounts = accounts.Select(a =>
            {
                a.rizaNo = activeConsent.Id.ToString();
                return a;
            }).ToList();

            result.Data = accounts;
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
                    permissions: permisssions);
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
            if (activeConsent != null &&
                activeConsent.OBAccountConsentDetails.Any(r =>
                    r.AccountReferences?.Contains(account.hspTml.hspRef) ?? false))
            {
                //Set rizaNo
                account.rizaNo = activeConsent.Id.ToString();
            }
            else
            {
                //account is not authorized
                account = null;
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

    public async Task<ApiResult> GetAuthorizedBalances(string userTCKN, string yosCode,int? syfKytSayi,int? syfNo,string? srlmKrtr,string? srlmYon)
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
            SetDefaultAccountServiceParameters(ref syfKytSayi, ref syfNo, ref srlmKrtr, ref srlmYon);
            //Get balances of customer from service
            List<BakiyeBilgileriDto> balances = await _accountClientService.GetBalances(userTCKN, syfKytSayi.Value,syfNo.Value,srlmKrtr,srlmYon);
            if (!balances?.Any() ?? false)
            {
                //No balance
                result.Data = balances;
                return result;
            }

            //Get account consent from db
            var activeConsent = (Consent)authConsentResult.Data;

            //filter balances
            balances = balances.Where(b =>
                    activeConsent.OBAccountConsentDetails.Any(r => r.AccountReferences?.Contains(b.hspRef) ?? false))
                .ToList();

            result.Data = balances;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }

    public async Task<ApiResult> GetAuthorizedBalanceByHspRef(string userTCKN , string yosCode,  string hspRef)
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
            
            //Get balance by account reference number of customer from service
            BakiyeBilgileriDto? balance = await _accountClientService.GetBalanceByHspRef(userTCKN, hspRef);
            if (balance == null)
            {
                //No balance
                result.Data = balance;
                return result;
            }

            var activeConsent = (Consent)authConsentResult.Data;
            if (!(activeConsent != null
                && activeConsent.OBAccountConsentDetails.Any(r => r.AccountReferences?.Contains(balance.hspRef) ?? false)))
            {
                //No authorized account for balance
                balance = null;
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

    public async Task<ApiResult> GetTransactionsByHspRef(string hspRef)
    {
        ApiResult result = new();
        try
        {
            //Get transactions by account reference number from service
            result.Data = await _accountClientService.GetTransactionsByHspRef(hspRef);
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }
    
    private void SetDefaultAccountServiceParameters(
        ref int? syfKytSayi,
        ref int? syfNo,
        ref string? srlmKrtr,
        ref string? srlmYon)
    {
        syfKytSayi ??= OpenBankingConstants.AccountServiceParameters.syfKytSayi;
        syfNo ??= OpenBankingConstants.AccountServiceParameters.syfNo;
        srlmKrtr ??= OpenBankingConstants.AccountServiceParameters.srlmKrtr;
        srlmYon ??= OpenBankingConstants.AccountServiceParameters.srlmYon;
    }
}