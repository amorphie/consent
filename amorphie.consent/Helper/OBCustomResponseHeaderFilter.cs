namespace amorphie.consent.Helper;

public class OBCustomResponseHeaderFilter : IEndpointFilter
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
            context.HttpContext.Response.Headers["X-Request-ID"]= traceValue;
        }
        if (context.HttpContext.Request.Headers.TryGetValue("X-ASPSP-Code", out traceValue))
        {
            context.HttpContext.Response.Headers["X-ASPSP-Code"]= traceValue;
        }
        if (context.HttpContext.Request.Headers.TryGetValue("X-TPP-Code", out traceValue))
        {
            context.HttpContext.Response.Headers["X-TPP-Code"]= traceValue;
        }
        if (context.HttpContext.Request.Headers.TryGetValue("X-Group-ID", out traceValue))
        {
            context.HttpContext.Response.Headers["X-Group-ID"]= traceValue;
        }
        return await next(context);
    }

}