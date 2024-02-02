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
    private readonly ConsentDbContext _context;

    public AccountService(IAccountClientService accountClientService,
        ConsentDbContext context)
    {
        _accountClientService = accountClientService;
        _context = context;
    }

    public async Task<ApiResult> GetAccounts(string customerId)
    {
        ApiResult result = new();
        try
        {
            //Get accounts of customer from service
            result.Data = await _accountClientService.GetAccounts(customerId);
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }

    public async Task<ApiResult> GetAuthorizedAccounts(string customerId)
    {
        ApiResult result = new();
        try
        {
            //Get accounts of customer from service
            List<HesapBilgileriDto> accounts = await _accountClientService.GetAccounts(customerId);
            if (!accounts?.Any() ?? false)
            {//No account
                result.Data = accounts;
                return result;
            }
            //Get account consent from db
            var activeConsent = await GetActiveAccountConsent(customerId, new List<string>()
                {OpenBankingConstants.IzinTur.TemelHesapBilgisi,
                    OpenBankingConstants.IzinTur.AyrintiliHesapBilgisi});
            if (activeConsent != null)
            {//filter accounts
                accounts = accounts.Where(a =>
                    activeConsent.OBAccountConsentDetails.Any(r => r.AccountReferences.Contains(a.hspTml.hspRef))).ToList();
                accounts = accounts.Select(a =>
                {
                    a.rizaNo = activeConsent.Id.ToString();
                    return a;
                }).ToList();
            }
            else
            {//User has got no authorized account consent
                accounts.Clear();
            }
            result.Data = accounts;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }


    public async Task<ApiResult> GetAccountByHspRef(string customerId, string hspRef)
    {
        ApiResult result = new();
        try
        {
            //Get account of customer from service
            result.Data = await _accountClientService.GetAccountByHspRef(customerId, hspRef);
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }

    public async Task<ApiResult> GetAuthorizedAccountByHspRef(string customerId, string hspRef)
    {
        ApiResult result = new();
        try
        {
            //Get account of customer from service
            HesapBilgileriDto? account = await _accountClientService.GetAccountByHspRef(customerId, hspRef);
            if (account == null)
            {//No account
                result.Data = account;
                return result;
            }
            var activeConsent = await GetActiveAccountConsent(customerId, new List<string>()
            {   OpenBankingConstants.IzinTur.TemelHesapBilgisi,
                OpenBankingConstants.IzinTur.AyrintiliHesapBilgisi
            }); //Get account consent from db
            if (activeConsent != null && activeConsent.OBAccountConsentDetails.Any(r => r.AccountReferences.Contains(account.hspTml.hspRef)))
            {//Set rizaNo
                account.rizaNo = activeConsent.Id.ToString();
            }
            else
            {//account is not authorized
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

    public async Task<ApiResult> GetAuthorizedBalances(string customerId)
    {
        ApiResult result = new();
        try
        {
            //Get balances of customer from service
            List<BakiyeBilgileriDto> balances = await _accountClientService.GetBalances(customerId);
            if (!balances?.Any() ?? false)
            {//No balance
                result.Data = balances;
                return result;
            }
            //Get account consent from db
            var activeConsent = await GetActiveAccountConsent(customerId, new List<string>()
            {
                OpenBankingConstants.IzinTur.BakiyeBilgisi
            });
            if (activeConsent != null)
            {//filter balances
                balances = balances.Where(b =>
                    activeConsent.OBAccountConsentDetails.Any(r => r.AccountReferences.Contains(b.hspRef))).ToList();
            }
            else
            {//User has got no authorized account consent
                balances.Clear();
            }
            result.Data = balances;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }

    public async Task<ApiResult> GetBalances(string customerId)
    {
        ApiResult result = new();
        try
        {
            //Get balances of customer from service
            List<BakiyeBilgileriDto> balances = await _accountClientService.GetBalances(customerId);
            result.Data = balances;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }

    public async Task<ApiResult> GetBalanceByHspRef(string customerId, string hspRef)
    {
        ApiResult result = new();
        try
        {
            //Get balance by account reference number of customer from service
            result.Data = await _accountClientService.GetBalanceByHspRef(customerId, hspRef);
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }

    public async Task<ApiResult> GetAuthorizedBalanceByHspRef(string customerId, string hspRef)
    {
        ApiResult result = new();
        try
        {
            //Get balance by account reference number of customer from service
            BakiyeBilgileriDto? balance = await _accountClientService.GetBalanceByHspRef(customerId, hspRef);
            if (balance == null)
            {//No balance
                result.Data = balance;
                return result;
            }
            var activeConsent = await GetActiveAccountConsent(customerId, new List<string>()
            {   OpenBankingConstants.IzinTur.BakiyeBilgisi
            }); //Get account consent from db
            if (activeConsent == null
                || activeConsent.OBAccountConsentDetails.Any(r => r.AccountReferences.Contains(balance.hspRef)) == false)
            {//No authorized account for balance
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

    private async Task<Consent?> GetActiveAccountConsent(string customerId, List<string> permissions)
    {
        var activeConsent = (await _context.Consents
            .Include(c => c.OBAccountConsentDetails)
            .Where(c => c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                                      && c.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi
                                      && c.OBAccountConsentDetails.Any(i => i.IdentityData == customerId)).AsNoTracking()
                                      .ToListAsync())
                                      ?.Where(c => c.OBAccountConsentDetails.Any(a => permissions.Any(a.PermissionTypes.Contains)))
            .FirstOrDefault();
        return activeConsent;
    }


}