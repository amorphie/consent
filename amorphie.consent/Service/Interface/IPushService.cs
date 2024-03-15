using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.Service.Refit;
using amorphie.core.Base;
using Microsoft.AspNetCore.Mvc;

namespace amorphie.consent.Service.Interface;

public interface IPushService
{
    public Task<ApiResult> OpenBankingSendPush(KimlikDto data, Guid consentId);
}