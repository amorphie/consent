
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace amorphie.consent.core.Helper;
public class CustomDateTimeConverter : IsoDateTimeConverter
{
    public CustomDateTimeConverter()
    {
        DateTimeFormat = "yyyy-MM-dd'T'HH:mm:sszzz";
    }
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }
        if (value is DateTime dateTime)
        {
            DateTime localDateTime;
           // Check the DateTimeKind and convert accordingly
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                // Convert UTC to local time
                localDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.Local);
            }
            else if (dateTime.Kind == DateTimeKind.Local)
            {
                // No conversion needed, already local time
                localDateTime = dateTime;
            }
            else
            {
                // Unspecified kind - assume UTC and convert
                localDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc), TimeZoneInfo.Local);
            }

            writer.WriteValue(localDateTime.ToString(DateTimeFormat));
        }
        else
        {
            base.WriteJson(writer, value, serializer);
        }
    }
}
