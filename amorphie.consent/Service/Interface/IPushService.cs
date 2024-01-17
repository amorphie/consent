using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.core.Base;

namespace amorphie.consent.Service.Interface;

public interface IPushService
{
    public Task<IResult> OpenBankingSendPush(KimlikDto data,string consentId);
}