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

public class CustomerService : ICustomerService
{
    private readonly ICustomerClientService _customerClientService;
    private readonly IOBAuthorizationService _authorizationService;
    private readonly ConsentDbContext _context;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerClientService customerClientService,
        ConsentDbContext context,
        IMapper mapper)
    {
        _customerClientService = customerClientService;
        _context = context;
        _mapper = mapper;
    }

    public Task<ApiResult> GetCustomerInformations(HttpContext httpContext, string userTCKN, string consentId, string yosCode,
        List<OBErrorCodeDetail> errorCodeDetails, int? syfKytSayi, int? syfNo, string? srlmKrtr, string? srlmYon)
    {
        ApiResult result = new();
        try
        {
//Bireysel rızada Kişinin rıza kimlik turu tckn değilse K => kişinin tckn sine ulaşmak
//Kurumsal rıza, bireysel müşteri numarası, kurumsal müşteri numarası
           
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return null;
    }
}