using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.Event;
using Refit;

public interface IBKMClientService
{
    [Headers("Content-Type: application/x-www-form-urlencoded", "Accept: application/x-www-form-urlencoded")]
    [Post("/oauth-provider/oauth2/token")]
    Task<HttpResponseMessage> GetToken([Body(BodySerializationMethod.UrlEncoded)] BKMTokenRequestDto request);

    [Headers("Accept: application/json")]
    [Get("/yos-api/s1.1/yos?srlmKrtr=kod&srlmYon=A")]
    Task<HttpResponseMessage> GetAllYos([Header("Authorization")] string authorization);

    [Headers("Accept: application/json")]
    [Get("/yos-api/s1.1/yos/{yosKod}")]
    Task<HttpResponseMessage> GetYos([Header("Authorization")] string authorization, string yosKod);

    [Headers("Accept: application/json")]
    [Get("/hhs-api/s1.0/hhs/{hhsKod}")]
    Task<HttpResponseMessage> GetHhs([Header("Authorization")] string authorization, string hhsKod);

    [Headers("Accept: application/json")]
    [Get("/hhs-api/s1.0/hhs")]
    Task<HttpResponseMessage> GetAllHhs([Header("Authorization")] string authorization);

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Post("/ohvps/ods/s1.1/olay-dinleme")]
    Task<HttpResponseMessage> SendEventToYos([Header("Authorization")] string authorization,
        [Header("X-Request-ID")] string xRequestId,
        [Header("X-ASPSP-Code")] string xAspspCode,
        [Header("X-TPP-Code")] string xTppCode,
        [Body] OlayIstegiDto olayIstegi);
}

