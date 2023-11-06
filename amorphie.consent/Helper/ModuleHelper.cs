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
    /// Checks if header required values are set.
    /// PSU Initiated value is correct
    /// </summary>
    /// <param name="header">Data to be checked</param>
    /// <returns>If header required values are set</returns>
    public static bool IsHeaderRequiredValuesCheckSuccess(RequestHeaderDto header)
    {

        if (string.IsNullOrEmpty(header.PSUInitiated)
            || string.IsNullOrEmpty(header.XGroupID)
            || string.IsNullOrEmpty(header.XASPSPCode)
            || string.IsNullOrEmpty(header.XRequestID)
            || string.IsNullOrEmpty(header.XTPPCode))
        {
            return false;
        }

        if (ConstantHelper.GetPSUInitiatedValues().Contains(header.PSUInitiated) == false)
        {//Check psu initiated value
            return false;
        }
        return true;
    }
}