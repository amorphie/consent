using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.Event;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.consent.data.Migrations;
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

    public async Task<ApiResult> GetAuthorizedAccountConsent(string userTCKN, string yosCode, List<string> permissions)
    {
        ApiResult result = new();
        try
        {
            var consentState = OpenBankingConstants.RizaDurumu.YetkiKullanildi;
            var today = DateTime.UtcNow;
            var activeConsent = (await _context.Consents
                    .Include(c => c.OBAccountConsentDetails)
                    .AsNoTracking()
                    .Where(c => c.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount
                                && c.State == OpenBankingConstants.RizaDurumu.YetkiKullanildi
                                && c.Variant == yosCode
                                && c.OBAccountConsentDetails.Any(i => i.IdentityData == userTCKN
                                                                      && i.IdentityType ==
                                                                      OpenBankingConstants.KimlikTur.TCKN
                                                                      && i.LastValidAccessDate > today
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
}