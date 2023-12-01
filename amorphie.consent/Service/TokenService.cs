using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;
using Microsoft.EntityFrameworkCore;

namespace amorphie.consent.Service;

public class TokenService : ITokenService
{
    private readonly ITokenClientService _tokenClientService;
    private readonly ConsentDbContext _context;

    public TokenService(ITokenClientService tokenClientService,
        ConsentDbContext context)
    {
        _tokenClientService = tokenClientService;
        _context = context;
    }
    
 
    public async Task<ApiResult> RevokeConsentToken(Guid consentId)
    {
        ApiResult result = new();
        try
        {
            //call revoke token service
            await _tokenClientService.RevokeConsentToken(consentId);
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }


}