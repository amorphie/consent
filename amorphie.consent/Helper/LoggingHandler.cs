namespace amorphie.consent.Helper;

public class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler> _logger;

    public LoggingHandler(ILogger<LoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Log request
        await LogRequestAsync(request);

        // Send request
        var response = await base.SendAsync(request, cancellationToken);

        // Log response
        await LogResponseAsync(response);

        return response;
    }

    private async Task LogRequestAsync(HttpRequestMessage request)
    {
        var headers = string.Join("\n", request.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"));
        _logger.LogInformation("Request: {Method} {Uri}\nHeaders:\n{Headers}",
            request.Method,
            request.RequestUri,
            headers);

        if (request.Content != null)
        {
            var requestContent = await request.Content.ReadAsStringAsync();
            _logger.LogInformation("Request Content: {Content}", requestContent);
        }
    }

    private async Task LogResponseAsync(HttpResponseMessage response)
    {
        var headers = string.Join("\n", response.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"));
        _logger.LogInformation("Response Status: {StatusCode}\nHeaders:\n{Headers}",
            response.StatusCode,
            headers);

        if (response.Content != null)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Response Content: {Content}", responseContent);
        }
    }
}