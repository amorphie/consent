using System.Text.Json.Serialization;
using amorphie.consent.core.DTO;
using amorphie.consent.Service.Refit;
using Newtonsoft.Json;
namespace amorphie.consent.Service;
public class TagService : ITagService
{
    private readonly ITagClientService _tagService;
    public TagService(ITagClientService tagService)
    {
        _tagService = tagService;
    }
    public ApiResult GetCustomer(string tckn)
    {
        ApiResult apiResult = new();
        try
        {
            Console.WriteLine(JsonConvert.SerializeObject(_tagService));
            var customerNumber = _tagService.GetCustomer(tckn);
            apiResult.Data = customerNumber;

            Console.WriteLine(JsonConvert.SerializeObject(customerNumber));
        }
        catch (Exception e)
        {
            apiResult.Result = false;
            apiResult.Message = e.Message;
            Console.WriteLine("GetCustomerException" + e.ToString());
        }
        return apiResult;
    }
}
