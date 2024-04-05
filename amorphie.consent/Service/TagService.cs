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
    public async Task<ApiResult> GetCustomer(string tckn)
    {
        ApiResult apiResult = new();
        try
        {
            var customerNumber = await _tagService.GetCustomerFromTag(tckn);
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
