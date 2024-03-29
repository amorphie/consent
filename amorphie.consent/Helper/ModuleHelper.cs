using System.Net;
using System.Security.Cryptography;
using System.Text;
using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.Service.Interface;
using Jose;
using Newtonsoft.Json;

namespace amorphie.consent.Helper;

public static class ModuleHelper
{
    /// <summary>
    /// Keeps httpcontext header information into a headerdto object
    /// </summary>
    /// <param name="httpContext">Httpcontex object</param>
    /// <returns>Object keeping header keys</returns>
    public static RequestHeaderDto GetHeader(HttpContext httpContext)
    {
        RequestHeaderDto header = new RequestHeaderDto();

        if (httpContext.Request.Headers.TryGetValue("X-Request-ID", out var traceValue))
        {
            header.XRequestID = traceValue.ToString();
        }

        if (httpContext.Request.Headers.TryGetValue("X-Group-ID", out traceValue))
        {
            header.XGroupID = traceValue.ToString();
        }

        if (httpContext.Request.Headers.TryGetValue("X-ASPSP-Code", out traceValue))
        {
            header.XASPSPCode = traceValue.ToString();
        }

        if (httpContext.Request.Headers.TryGetValue("X-TPP-Code", out traceValue))
        {
            header.XTPPCode = traceValue.ToString();
        }

        if (httpContext.Request.Headers.TryGetValue("PSU-Initiated", out traceValue))
        {
            header.PSUInitiated = traceValue.ToString();
        }

        if (httpContext.Request.Headers.TryGetValue("user_reference", out traceValue))
        {
            header.UserReference = traceValue;
        }

        return header;
    }

    /// <summary>
    /// Checks if header is valid by controlling;
    /// PSU Initiated value is in predefined values
    /// Required fields are checked
    /// XASPSPCode is equal with BurganBank hhscode
    /// </summary>
    /// <param name="header">Data to be checked</param>
    /// <param name="configuration">Configuration instance</param>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <param name="context"></param>
    /// <param name="errorCodeDetails"></param>
    /// <param name="isUserRequired">There should be userreference value in header. Optional parameter with default false value</param>
    /// <returns>If header is valid</returns>
    public static async Task<ApiResult> IsHeaderValid(RequestHeaderDto header,
        IConfiguration configuration,
        IYosInfoService yosInfoService,
        HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails,
        bool? isUserRequired = false)
    {
        ApiResult result = new();

        // Separate method to prepare and check header properties
        if (!OBErrorResponseHelper.PrepareAndCheckHeaderInvalidFormatProperties(header, context, errorCodeDetails, out var errorResponse))
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        if (configuration["HHSCode"] != header.XASPSPCode)
        {
            //XASPSPCode value should be BurganBanks hhscode value
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.InvalidAspsp);
            return result;
        }

        if (ConstantHelper.GetPSUInitiatedValues().Contains(header.PSUInitiated) == false)
        {
            //Check psu initiated value
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.InvalidContentPsuInitiated);
            return result;
        }

        //Check setted yos value
        var yosCheckResult = await yosInfoService.IsYosInApplication(header.XTPPCode);
        if (yosCheckResult.Result == false
            || yosCheckResult.Data == null
            || (bool)yosCheckResult.Data == false)
        {
            //No yos data in the system
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.InvalidTpp);
            return result;
        }

        if (isUserRequired.HasValue
            && isUserRequired.Value
            && string.IsNullOrEmpty(header.UserReference))
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.InvalidContentUserReference);
            return result;
        }

        return result;
    }

    /// <summary>
    /// Checks if header is valid by controlling;
    /// PSU Initiated value is in predefined values
    /// Required fields are checked
    /// XASPSPCode is equal with BurganBank hhscode
    /// </summary>
    /// <param name="header">Data to be checked</param>
    /// <param name="configuration">Configuration instance</param>
    /// <param name="yosInfoService">YosInfoService object</param>
    /// <returns>If header is valid</returns>
    public static async Task<bool> IsHeaderValidForEvents(RequestHeaderDto header,
        IConfiguration configuration,
        IYosInfoService yosInfoService)
    {
        if (string.IsNullOrEmpty(header.XASPSPCode)
            || string.IsNullOrEmpty(header.XRequestID)
            || string.IsNullOrEmpty(header.XTPPCode))
        {
            return false;
        }

        if (configuration["HHSCode"] != header.XASPSPCode)
        {
            //XASPSPCode value should be BurganBanks hhscode value
            return false;
        }

        //Check setted yos value
        var yosCheckResult = await yosInfoService.IsYosInApplication(header.XTPPCode);
        if (yosCheckResult.Result == false
            || yosCheckResult.Data == null
            || (bool)yosCheckResult.Data == false)
        {
            //No yos data in the system
            return false;
        }

        return true;
    }

    /// <summary>
    /// Set X-JWS-Signature header property
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="configuration"></param>
    /// <param name="body">Message body</param>
    public static void SetXJwsSignatureHeader(HttpContext httpContext, IConfiguration configuration, object? body)
    {
        if (body != null)
        {
            var headerPropData = GetXJwsSignature(body, configuration);
            // Check if the header already exists before adding
            httpContext.Response.Headers["X-JWS-Signature"] = headerPropData;
        }
    }

    public static string GetXJwsSignature(object body, IConfiguration configuration)
    {
        //JWT header-payload-signature

        // Create JWS header
        var jwtHeader = new Dictionary<string, object>()
        {
            { "alg", "RS256" },
            { "typ", "JWT" }
        };

        // Create JWS payload
        //From Document:
        //Payload kısmında özel olarak oluşturulacak olan “body” claim alanına istek gövdesi (request body) verisinin SHA256 hash değeri karşılığı yazılmalıdır.
        var data = new Dictionary<string, object>()
        {
            { "iss", "https://apigw.bkm.com.tr" },
            { "exp", ((DateTimeOffset)DateTime.UtcNow.AddMinutes(60)).ToUnixTimeSeconds() },
            { "iat", ((DateTimeOffset)DateTime.UtcNow.AddMinutes(-5)).ToUnixTimeSeconds() },
            { "body", GetChecksumSHA256(body) }
        };

        // Load private key from file
        var key = LoadPrivateKeyFromVault(configuration);
        return JWT.Encode(payload: data, key: key, JwsAlgorithm.RS256, extraHeaders: jwtHeader);
    }

    private static RSA LoadPrivateKeyFromPemFile(string pemFilePath)
    {
        string pemContents;
        using (StreamReader reader = new StreamReader(pemFilePath))
        {
            pemContents = reader.ReadToEnd();
        }

        var key = RSA.Create();
        key.ImportFromPem(pemContents);
        return key;
    }

    /// <summary>
    /// Loads hhsprivate key from vault
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns>RSA Key</returns>
    private static RSA LoadPrivateKeyFromVault(IConfiguration configuration)
    {
        string? pemContents = configuration["HHS_PrivateKey"];
        var key = RSA.Create();
        key.ImportFromPem(pemContents);
        return key;
    }

    /// <summary>
    /// Generates sha256 hash of body
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    private static string GetChecksumSHA256(object body)
    {
        // Initialize a SHA256 hash object.
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body)));
            return Convert.ToHexString(bytes);
        }
    }

    public static void SetLinkHeader(HttpContext httpContext, IConfiguration configuration)
    {
        string linkHeaderValue = string.Join(", ",
            "</ohvps/hbh/s1.1/hesaplar/hspref/islemler?hesapIslemBslTrh=2022-01-01T00:00:00+03:00&hesapIslemBtsTrh=2023-12-12T23:59:59+03:00&srlmKrtr=islGrckZaman&srlmYon=Y&syfNo=6&syfKytSayi=100>; rel=\"next\"",
            "</ohvps/hbh/s1.1/hesaplar/hspref/islemler?hesapIslemBslTrh=2022-01-01T00:00:00+03:00&hesapIslemBtsTrh=2023-12-12T23:59:59+03:00&srlmKrtr=islGrckZaman&srlmYon=Y&syfNo=4&syfKytSayi=100>; rel=\"prev\"",
            "</ohvps/hbh/s1.1/hesaplar/hspref/islemler?hesapIslemBslTrh=2022-01-01T00:00:00+03:00&hesapIslemBtsTrh=2023-12-12T23:59:59+03:00&srlmKrtr=islGrckZaman&srlmYon=Y&syfNo=14&syfKytSayi=100>; rel=\"last\"",
            "</ohvps/hbh/s1.1/hesaplar/hspref/islemler?hesapIslemBslTrh=2022-01-01T00:00:00+03:00&hesapIslemBtsTrh=2023-12-12T23:59:59+03:00&srlmKrtr=islGrckZaman&srlmYon=Y&syfNo=0&syfKytSayi=100>; rel=\"first\"");

        linkHeaderValue = linkHeaderValue.Replace("\r", "").Replace("\n", "");
        httpContext.Response.Headers["Link"] = linkHeaderValue;
    }
}