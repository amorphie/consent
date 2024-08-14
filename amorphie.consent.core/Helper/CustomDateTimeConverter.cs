
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace amorphie.consent.core.Helper;
public class CustomDateTimeConverter : IsoDateTimeConverter
{
    public CustomDateTimeConverter()
    {
        DateTimeFormat = "yyyy-MM-dd'T'HH:mm:sszzz";
    }
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is DateTime dateTime)
        {
            // Convert UTC time to local time (assumed to be the server's local timezone)
            var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.Local);
            writer.WriteValue(localDateTime.ToString(DateTimeFormat));
        }
        else
        {
            base.WriteJson(writer, value, serializer);
        }
    }
}
