using amorphie.consent.core.DTO;
using Refit;

namespace amorphie.consent.Service.Refit;

public interface ITagService
{
    ApiResult GetCustomer(string tckn);
}
