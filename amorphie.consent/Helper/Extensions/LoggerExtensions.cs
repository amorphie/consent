namespace amorphie.consent.Helper;

public static class LoggerExtensions
{
    public static async Task LogHttpResponseAsync(this ILogger logger, HttpResponseMessage response, string context = null)
    {
        string content = await response.Content.ReadAsStringAsync();
        var headers = response.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value));
        var contentHeaders = response.Content.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value));

        if (context != null)
        {
            logger.LogInformation("Context: {Context}", context);
        }

        logger.LogInformation("Response Content: {Content}", content);
        logger.LogInformation("Response Headers: {Headers}", headers);
        logger.LogInformation("Content Headers: {ContentHeaders}", contentHeaders);
    }
}
