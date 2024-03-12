using amorphie.consent.core.DTO;
using amorphie.consent.Service.Refit;

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
            var customerNumber = await _tagService.GetCustomer(tckn);
            apiResult.Data = customerNumber;
            Console.WriteLine(customerNumber);
        }
        catch (Exception e)
        {
            apiResult.Result = false;
            apiResult.Message = e.Message;
        }
        return apiResult;
    }
}
