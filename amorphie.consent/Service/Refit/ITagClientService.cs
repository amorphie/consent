using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.MessagingGateway;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.Tag;
using Refit;

public interface ITagClientService
{

    [Get("/tag/openbanking/openbankinguser/openbanking-customer/execute?reference={tckn}")]
    Task<PhoneNumberDto> GetCustomer(string tckn);
}
