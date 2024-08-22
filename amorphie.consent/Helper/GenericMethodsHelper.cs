using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.DTO.Tag;
using amorphie.consent.core.DTO.Token;
using amorphie.consent.core.Enum;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;
using Jose;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace amorphie.consent.Helper;

public static class GenericMethodsHelper
{
   
    /// <summary>
    /// Converts string value to nullable long data
    /// </summary>
    /// <param name="stringData">To be parsed data</param>
    /// <returns>long? converted data</returns>
    public static long? ConvertStringToNullableLong(string?  stringData)
    {
        if (!string.IsNullOrEmpty(stringData) && long.TryParse(stringData, out long parsedValue))
        {
           return parsedValue;
        }
        return null;
    }



}
