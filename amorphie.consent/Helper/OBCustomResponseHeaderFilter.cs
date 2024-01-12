using Microsoft.AspNetCore.Mvc.Filters;

namespace amorphie.consent.Helper;

public class OBCustomResponseHeaderFilter: IEndpointFilter
{
    /// <summary>
    /// Add open banking header properties to response message.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async ValueTask<object?> InvokeAsync(  
        EndpointFilterInvocationContext context,  
        EndpointFilterDelegate next)  
    {
        if (context.HttpContext.Request.Headers.TryGetValue("X-Request-ID", out var traceValue))
        {
            context.HttpContext.Response.Headers.Add("X-Request-ID", traceValue);
        }
        if (context.HttpContext.Request.Headers.TryGetValue("X-ASPSP-Code", out traceValue))
        {
            context.HttpContext.Response.Headers.Add("X-ASPSP-Code", traceValue);
        }
        if (context.HttpContext.Request.Headers.TryGetValue("X-TPP-Code", out traceValue))
        {
            context.HttpContext.Response.Headers.Add("X-TPP-Code", traceValue);
        }
        return await next(context);  
    }
    
}