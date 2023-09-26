using Dapr.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITranslationService
{
    Task<Dictionary<string, string>> GetTranslationsForLanguageAsync(string language);
    Task<string> GetTranslatedMessageAsync(string language, string key);
}

public class TranslationService : ITranslationService
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<TranslationService> _logger;

    public TranslationService(DaprClient daprClient, ILogger<TranslationService> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public async Task<Dictionary<string, string>> GetTranslationsForLanguageAsync(string language)
    {
        try
        {
            var jsonData = await _daprClient.GetStateAsync<string>("amorphie-state", "messages");
            var translations = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonData);
            
            if (translations.TryGetValue(language, out var translatedMessages))
            {
                return translatedMessages;
            }
            
            return new Dictionary<string, string>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving translations for language {Language}", language);
            return new Dictionary<string, string>();
        }
    }

    public async Task<string> GetTranslatedMessageAsync(string language, string key)
{
    try
    {
        if (string.IsNullOrWhiteSpace(language) || string.IsNullOrWhiteSpace(key))
        {
            return "Invalid language or key.";
        }

        if (string.Equals(language, "en-EN", StringComparison.OrdinalIgnoreCase))
        {
            language = "en-EN";
        }

        var translations = await GetTranslationsForLanguageAsync(language);
        if (translations.TryGetValue(key, out var message))
        {
            return message;
        }
        
        return $"Translation for key '{key}' not found.";
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "An error occurred while retrieving the translated message for language {language} and key {key}", language, key);
        return "An error occurred while retrieving the translated message.";
    }
}
}