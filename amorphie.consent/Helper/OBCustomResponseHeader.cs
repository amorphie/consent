using Microsoft.AspNetCore.Mvc.Filters;

namespace amorphie.consent.Helper;

public class OBCustomResponseHeader : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
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
            context.HttpContext.Response.Headers.Add("X-Request-ID", traceValue);
        }

        base.OnResultExecuting(context);
    }
}