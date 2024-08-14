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
                    && c.LastValidAccessDate > today
                    && c.State == consentState
                    && c.UserTCKN != null
                    && c.UserTCKN.ToString() == userTCKN
                    && c.OBAccountConsentDetails.Any(i => i.UserType == OpenBankingConstants.OHKTur.Bireysel))
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

    public async Task<ApiResult> GetAuthUsedAccountConsentsOfInstitutionUser(string customerNumber, string institutionCustomerNumber)
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
                    && c.LastValidAccessDate > today
                    && c.State == consentState
                    && c.OBAccountConsentDetails.Any(i => i.CustomerNumber == customerNumber
                                                          && i.InstitutionCustomerNumber == institutionCustomerNumber
                                                          && i.UserType == OpenBankingConstants.OHKTur.Kurumsal))
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
                                && c.LastValidAccessDate > today
                                && c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                                && c.State == consentState
                                && c.OBAccountConsentDetails.Any(i => i.AccountReferences != null
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


    public async Task<ApiResult> GetAccountConsent(string consentId, string userTckn, string yosCode, List<string>? permissions)
    {
        ApiResult result = new();
        try
        {
            var query =  _context.Consents
                    .Include(c => c.OBAccountConsentDetails)
                    .AsNoTracking()
                    .Where(c => c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                                && c.Variant == yosCode
                                && c.Id.ToString() == consentId
                                && c.UserTCKN != null
                                && c.UserTCKN.ToString() == userTckn);
            // If permissions is not null, apply the additional filtering
            if (permissions != null)
            {
                query = query.Where(c => c.OBAccountConsentDetails.Any(a => permissions.Any(a.PermissionTypes.Contains)));
            }
            // Execute the query and take the first matching result
            var activeConsent = await query.FirstOrDefaultAsync();
            result.Data = activeConsent;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }

    public async Task<ApiResult> GetAccountConsentByAccountRef(string consentId,string userTckn, string yosCode, string accountRef)
    {
        ApiResult result = new();
        try
        {
            var consent = await _context.Consents
                    .Include(c => c.OBAccountConsentDetails)
                    .AsNoTracking()
                    .Where(c => c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                                && c.Variant == yosCode
                                && c.Id.ToString() == consentId
                                && c.UserTCKN != null
                                && c.UserTCKN.ToString() == userTckn
                                && c.OBAccountConsentDetails.Any(i => i.AccountReferences != null
                                                                      && i.AccountReferences.Contains(accountRef)))
                    .FirstOrDefaultAsync();
            result.Data = consent;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }


    public async Task<ApiResult> GetActiveAccountConsentsOfUser(KimlikDto identity, 
    string yosCode, 
    long? userTckn,
     string? customerNumber, 
     string? institutionCustomerNumber)
    {
        ApiResult result = new();
        try
        {
            var activeAccountConsentStatusList =
                ConstantHelper.GetActiveAccountConsentStatusList(); //Get active status list
            //Active account consents in db
            var activeAccountConsents = await _context.Consents.Where(c =>
                    c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount 
                    && c.UserTCKN != null
                       && c.UserTCKN == userTckn
                       && activeAccountConsentStatusList.Contains(c.State)
                       && ((identity.ohkTur == OpenBankingConstants.OHKTur.Bireysel  
                            && c.OBAccountConsentDetails.Any(i => i.YosCode == yosCode))
                           ||(identity.ohkTur == OpenBankingConstants.OHKTur.Kurumsal 
                              && c.OBAccountConsentDetails.Any(i => i.CustomerNumber == customerNumber
                                                                    && i.InstitutionCustomerNumber == institutionCustomerNumber
                                                                    && i.YosCode == yosCode)))
                  )
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
                                          && c.UserTCKN != null && c.UserTCKN.ToString()== userTCKN
                                          && (c.LastValidAccessDate == null
                                              || (c.LastValidAccessDate != null && c.LastValidAccessDate >= today))
                                    );

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
                                          && (c.LastValidAccessDate == null
                                              || (c.LastValidAccessDate != null && c.LastValidAccessDate > today))
                                          && (c.OBAccountConsentDetails.Any(i =>
                                                  i.YosCode == yosCode
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