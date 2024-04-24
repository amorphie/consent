using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.Event;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.consent.data.Migrations;
using amorphie.consent.Helper;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace amorphie.consent.Service;

public class OBAuthorizationService : IOBAuthorizationService
{
    private readonly ConsentDbContext _context;
    private readonly IMapper _mapper;

    public OBAuthorizationService(ConsentDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<ApiResult> GetAuthUsedAccountConsentsOfUser(string userTCKN)
    {
        ApiResult result = new();
        try
        {
            var consentState = OpenBankingConstants.RizaDurumu.YetkiKullanildi;
            var today = DateTime.UtcNow;
            //Active account consents in db
            var activeAccountConsents = await _context.Consents
                .Include(c => c.OBAccountConsentDetails)
                .AsNoTracking()
                .Where(c =>
                    c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                    && c.State == consentState
                    && c.OBAccountConsentDetails.Any(i => i.IdentityData == userTCKN
                                                          && i.IdentityType == OpenBankingConstants.KimlikTur.TCKN
                                                          && i.LastValidAccessDate > today
                                                          && i.UserType == OpenBankingConstants.OHKTur.Bireysel))
                .ToListAsync();
            result.Data = activeAccountConsents;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }

    public async Task<ApiResult> GetAuthUsedAccountConsent(string consentId, string accountRef,
        List<string> permissions)
    {
        ApiResult result = new();
        try
        {
            var consentState = OpenBankingConstants.RizaDurumu.YetkiKullanildi;
            var today = DateTime.UtcNow;
            var activeConsent = (await _context.Consents
                    .Include(c => c.OBAccountConsentDetails)
                    .AsNoTracking()
                    .Where(c => c.Id.ToString() == consentId
                                && c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                                && c.State == consentState
                                && c.OBAccountConsentDetails.Any(i =>
                                    i.LastValidAccessDate > today
                                    && i.UserType == OpenBankingConstants.OHKTur.Bireysel
                                    && i.AccountReferences != null
                                    && i.AccountReferences.Contains(accountRef)))
                    .ToListAsync())
                ?.Where(c => c.OBAccountConsentDetails.Any(a => permissions.Any(a.PermissionTypes.Contains)))
                .FirstOrDefault();
            result.Data = activeConsent;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }


    public async Task<ApiResult> GetAccountConsent(string consentId, string userTckn, string yosCode, List<string> permissions)
    {
        ApiResult result = new();
        try
        {
            var activeConsent = (await _context.Consents
                    .Include(c => c.OBAccountConsentDetails)
                    .AsNoTracking()
                    .Where(c => c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                                && c.Variant == yosCode
                                && c.Id.ToString() == consentId
                                && c.OBAccountConsentDetails.Any(i => i.IdentityData == userTckn
                                                                      && i.IdentityType ==
                                                                      OpenBankingConstants.KimlikTur.TCKN
                                                                      && i.UserType == OpenBankingConstants.OHKTur
                                                                          .Bireysel))
                    .ToListAsync())
                ?.Where(c => c.OBAccountConsentDetails.Any(a => permissions.Any(a.PermissionTypes.Contains)))
                .FirstOrDefault();
            result.Data = activeConsent;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }

    public async Task<ApiResult> GetAccountConsentByAccountRef(string userTckn,string consentId, string yosCode, List<string> permissions,
        string accountRef)
    {
        ApiResult result = new();
        try
        {
            var consent = (await _context.Consents
                    .Include(c => c.OBAccountConsentDetails)
                    .AsNoTracking()
                    .Where(c => c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                                && c.Variant == yosCode
                                && c.Id.ToString() == consentId
                                && c.OBAccountConsentDetails.Any(i => i.IdentityData == userTckn
                                                                      && i.IdentityType ==
                                                                      OpenBankingConstants.KimlikTur.TCKN
                                                                      && i.UserType == OpenBankingConstants.OHKTur
                                                                          .Bireysel
                                                                      && i.AccountReferences != null
                                                                      && i.AccountReferences.Contains(accountRef)))
                    .ToListAsync())
                ?.Where(c => c.OBAccountConsentDetails.Any(a => permissions.Any(a.PermissionTypes.Contains)))
                .FirstOrDefault();
            result.Data = consent;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }


    public async Task<ApiResult> GetActiveAccountConsentsOfUser(KimlikDto identity, string yosCode)
    {
        ApiResult result = new();
        try
        {
            var activeAccountConsentStatusList =
                ConstantHelper.GetActiveAccountConsentStatusList(); //Get active status list
            //Active account consents in db
            var activeAccountConsents = await _context.Consents.Where(c =>
                    c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                    && activeAccountConsentStatusList.Contains(c.State)
                    && c.OBAccountConsentDetails.Any(i => i.IdentityData == identity.kmlkVrs
                                                          && i.IdentityType == identity.kmlkTur
                                                          && i.UserType == identity.ohkTur
                                                          && i.YosCode == yosCode))
                .ToListAsync();
            result.Data = activeAccountConsents;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }


    public async Task<ApiResult> GetConsentReadonly(Guid id, string userTCKN,
        List<string> consentTypes)
    {
        ApiResult result = new();
        try
        {
            var today = DateTime.UtcNow;
            var activeConsent = await _context.Consents
                .Include(c => c.OBAccountConsentDetails)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id
                                          && consentTypes.Contains(c.ConsentType)
                                          && (c.OBAccountConsentDetails.Any(i => i.IdentityData == userTCKN
                                                  && i.IdentityType ==
                                                  OpenBankingConstants.KimlikTur.TCKN
                                                  && i.LastValidAccessDate > today
                                                  && i.UserType == OpenBankingConstants.OHKTur
                                                      .Bireysel
                                                  && i.Consent.ConsentType ==
                                                  ConsentConstants.ConsentType.OpenBankingAccount)
                                              || c.OBPaymentConsentDetails.Any(i => i.IdentityData == userTCKN
                                                  && i.IdentityType ==
                                                  OpenBankingConstants.KimlikTur.TCKN
                                                  && i.Consent.ConsentType ==
                                                  ConsentConstants.ConsentType.OpenBankingPayment
                                                  && i.UserType == OpenBankingConstants.OHKTur
                                                      .Bireysel)));

            result.Data = activeConsent;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }

    public async Task<ApiResult> GetIdempotencyRecordOfAccountPaymentConsent(string yosCode,
        string consentType, string checkSumValue)
    {
        ApiResult result = new();
        try
        {
            var today = DateTime.UtcNow;
            var activeConsent = await _context.Consents
                .Include(c => c.OBAccountConsentDetails)
                .Include(c => c.OBPaymentConsentDetails)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ConsentType == consentType
                                          && (c.OBAccountConsentDetails.Any(i =>
                                                  i.LastValidAccessDate > today
                                                  && i.YosCode == yosCode
                                                  && i.CheckSumValue == checkSumValue
                                                  && i.CheckSumLastValiDateTime >= today
                                                  && i.Consent.ConsentType ==
                                                  ConsentConstants.ConsentType.OpenBankingAccount)
                                              || c.OBPaymentConsentDetails.Any(i =>
                                                  i.YosCode == yosCode
                                                  && i.CheckSumValue == checkSumValue
                                                  && i.CheckSumLastValiDateTime >= today
                                                  && i.Consent.ConsentType ==
                                                  ConsentConstants.ConsentType.OpenBankingPayment)
                                          ));
            //Set account consent data
            if (consentType == ConsentConstants.ConsentType.OpenBankingAccount
                && (activeConsent?.OBAccountConsentDetails?.Any() ?? false))
            {
                result.Data = activeConsent.OBAccountConsentDetails.First().SaveResponseMessage;
            }
            //Set payment consent data
            else if (consentType == ConsentConstants.ConsentType.OpenBankingPayment
                && (activeConsent?.OBPaymentConsentDetails?.Any() ?? false))
            {
                result.Data = activeConsent.OBPaymentConsentDetails.First().SaveResponseMessage;
            }
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }
    
    
    public async Task<ApiResult> GetIdempotencyRecordOfPaymentOrder(string yosCode, string checkSumValue)
    {
        ApiResult result = new();
        try
        {
            var today = DateTime.UtcNow;
            var paymentOrder = await _context.OBPaymentOrders
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.YosCode == yosCode
                                                  && i.CheckSumValue == checkSumValue
                                                  && i.CheckSumLastValiDateTime >= today);
            //Set payment order data
            if (paymentOrder != null)
            {
                result.Data = paymentOrder.SaveResponseMessage;
            }
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }
}