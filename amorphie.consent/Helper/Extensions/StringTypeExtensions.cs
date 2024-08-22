using Newtonsoft.Json;

namespace amorphie.consent.Helper;

public static class StringTypeExtensions
{
    /// <summary>
    /// Converts string value to nullable long data
    /// </summary>
    /// <param name="stringData">To be parsed data</param>
    /// <returns>long? converted data</returns>
    public static long? ToNullableLong(this string stringData)
    {
        if (!string.IsNullOrEmpty(stringData) && long.TryParse(stringData, out long parsedValue))
        {
            return parsedValue;
        }
        return null;
    }
}