using Newtonsoft.Json;

namespace amorphie.consent.Helper;

public static class JsonExtensions
{
    public static string ToJsonString(this object? obj)
    {
        if (obj is not null)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore // This will remove null properties
            };
             return JsonConvert.SerializeObject(obj, Formatting.None,settings);
        }
        return string.Empty;

    }
}