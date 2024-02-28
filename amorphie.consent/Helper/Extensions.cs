using System.Text.Json;

namespace amorphie.consent.Helper;

public static class HttpContextExtensions
{
    /// <summary>
    /// Deserializes to httpcontext body to given type
    /// </summary>
    /// <param name="httpContext">HttpContext</param>
    /// <typeparam name="TModel">Serialized object type</typeparam>
    /// <returns>Serialized object</returns>
    public static async Task<TModel> Deserialize<TModel>(this HttpContext httpContext)
    {
        TModel model = default(TModel);
        using (StreamReader reader = new StreamReader(httpContext.Request.Body))
        {
            string jsonContent = await reader.ReadToEndAsync();
            model = JsonSerializer.Deserialize<TModel>(jsonContent);
        }
        return model;
    }
}