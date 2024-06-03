using Newtonsoft.Json;

namespace amorphie.consent.Helper;

public static class JsonExtensions
{
    public static string ToJsonString(this object? obj)
    {
        if (obj is not null)
        {
             return JsonConvert.SerializeObject(obj, Formatting.None);
        }
        return string.Empty;

    }
}