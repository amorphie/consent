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
        _logger.LogInformation("Request: {Method} {Uri}", request.Method, request.RequestUri);
        _logger.LogInformation("Request Headers: {Headers}", request.Headers);
        if (request.Content != null)
        {
            var requestContent = await request.Content.ReadAsStringAsync();
            _logger.LogInformation("Request Content: {Content}", requestContent);
        }

        // Send request
        var response = await base.SendAsync(request, cancellationToken);

        // Log response
        _logger.LogInformation("Response Status: {StatusCode}", response.StatusCode);
        _logger.LogInformation("Response Headers: {Headers}", response.Headers);
        if (response.Content != null)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Response Content: {Content}", responseContent);
        }

        return response;
    }
}