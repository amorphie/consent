using amorphie.consent.core.DTO.OpenBanking;

namespace amorphie.consent.Helper;

public static class ModuleHelper
{
    /// <summary>
    /// Keeps httpcontext header information into a headerdto object
    /// </summary>
    /// <param name="httpContext">Httpcontex object</param>
    /// <returns>Object keeping header keys</returns>
    public static RequestHeaderDto GetHeader(HttpContext httpContext)
    {
        RequestHeaderDto header = new RequestHeaderDto();
        if (httpContext.Request.Headers.TryGetValue("X-Request-ID", out var traceValue))
        {
            header.XRequestID = traceValue;
        }
        if (httpContext.Request.Headers.TryGetValue("X-Group-ID", out traceValue))
        {
            header.XGroupID = traceValue;
        }
        if (httpContext.Request.Headers.TryGetValue("X-ASPSP-Code", out traceValue))
        {
            header.XASPSPCode = traceValue;
        }
        if (httpContext.Request.Headers.TryGetValue("X-TPP-Code", out traceValue))
        {
            header.XTPPCode = traceValue;
        }
        if (httpContext.Request.Headers.TryGetValue("PSU-Initiated", out traceValue))
        {
            header.PSUInitiated = traceValue;
        }
        return header;
    }

    /// <summary>
    /// Checks if header is valid by controlling;
    /// PSU Initiated value is in predefined values
    /// Required fields are set
    /// 
    /// </summary>
    /// <param name="header">Data to be checked</param>
    /// <param name="configuration">Configuration instance</param>
    /// <returns>If header is valid</returns>
    public static bool IsHeaderValid(RequestHeaderDto header,
        IConfiguration configuration)
    {

        if (string.IsNullOrEmpty(header.PSUInitiated)
            || string.IsNullOrEmpty(header.XGroupID)
            || string.IsNullOrEmpty(header.XASPSPCode)
            || string.IsNullOrEmpty(header.XRequestID)
            || string.IsNullOrEmpty(header.XTPPCode))
        {
            return false;
        }

        if (configuration["HHSCode"] != header.XASPSPCode)
        {//XASPSPCode value should be BurganBanks hhscode value
            return false;
        }

        if (ConstantHelper.GetPSUInitiatedValues().Contains(header.PSUInitiated) == false)
        {//Check psu initiated value
            return false;
        }
        return true;
    }
}