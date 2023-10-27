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
}