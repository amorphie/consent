using Google.Protobuf.Reflection;

namespace amorphie.consent.core.DTO;

public class ApiResult
{
    public ApiResult(bool result = true)
    {
        Result = result;
    }
    public bool Result { get; set; }
    public string? Message { get; set; }
    public Object? Data { get; set; }
}