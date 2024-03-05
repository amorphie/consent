using Microsoft.AspNetCore.Mvc;

namespace amorphie.consent.Helper;

public class CustomStatusCodeResult : IResult
{
    private readonly int _statusCode;
    private readonly string _content;

    public CustomStatusCodeResult(int statusCode, string content)
    {
        _statusCode = statusCode;
        _content = content;
    }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = _statusCode;

        if (!string.IsNullOrEmpty(_content))
        {
            httpContext.Response.ContentType = "text/plain";
            await httpContext.Response.WriteAsync(_content);
        }
    }
}