public interface ILanguageService
{
    Task<string> GetLanguageAsync(HttpContext httpContext);
}

public class AcceptLanguageService : ILanguageService
{
    public async Task<string> GetLanguageAsync(HttpContext httpContext)
    {
        string acceptLanguageHeader = httpContext.Request.Headers["Accept-Language"].FirstOrDefault();
        
        string defaultLanguage = "en-US";
        
        if (!string.IsNullOrEmpty(acceptLanguageHeader))
        {
            var languageParts = acceptLanguageHeader.Split(',', ';');
            
            foreach (var part in languageParts)
            {
                var trimmedPart = part.Trim();
                if (!string.IsNullOrEmpty(trimmedPart))
                {
                    return trimmedPart;
                }
            }
        }
        
        return defaultLanguage;
    }
}