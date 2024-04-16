using System.Security.Cryptography;
using System.Text;
using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.DTO.Tag;
using amorphie.consent.core.DTO.Token;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;
using Jose;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace amorphie.consent.Helper;

public static class OBModuleHelper
{
    /// <summary>
    /// Keeps httpcontext header information into a headerdto object
    /// </summary>
    /// <param name="httpContext">Httpcontex object</param>
    /// <returns>Object keeping header keys</returns>
    public static RequestHeaderDto GetHeader(HttpContext httpContext)
    {
        RequestHeaderDto header = new();

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
        if (httpContext.Request.Headers.TryGetValue("openbanking_consent_id", out traceValue))
        {
            header.ConsentId = traceValue;
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
        using (StreamReader reader = new(pemFilePath))
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
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body)));
        return Convert.ToHexString(bytes);
    }
    
    /// <summary>
    /// Generates sha256 hash of body and xrequestId
    /// </summary>
    /// <returns>xRequestId|body sha256 hash</returns>
    public static string GetChecksumForXRequestIdSHA256(object body, string xRequestId)
    {
        string concatenatedData = $"{xRequestId}|{JsonConvert.SerializeObject(body)}";
        // Initialize a SHA256 hash object.
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(concatenatedData));
        // Convert the hash bytes to a hexadecimal string.
        return Convert.ToHexString(bytes);
    }

    /// <summary>
    /// Calculates hhsforwardingaddress according to kmlk data in consent.
    /// If tckn based consent, get data from tagservice and set according to On or Burgan user
    /// In any other cases, set token login url
    /// </summary>
    /// <returns>Hhsforwardingaddress</returns>
    public static async Task<string> GetHhsForwardingAddressAsync(IConfiguration configuration, KimlikDto kmlk,
        string consentId, ITagService tagService, IDeviceRecord deviceRecordService)
    { 
        //Set token login url
        string forwardingAddress = string.Format(configuration["HHSForwardingAddress"] ?? string.Empty, consentId);
        if (kmlk.kmlkTur == OpenBankingConstants.KimlikTur.TCKN)
        {//Tckn consent
            var getCustomerInfoResult = await tagService.GetCustomer(kmlk.kmlkVrs);//Get customer phonenumber
            if (!getCustomerInfoResult.Result || getCustomerInfoResult.Data == null) //Error in service
            {
                return forwardingAddress;
            }

            //Check phone number On user or Burgan User
                PhoneNumberDto? phoneNumber = (PhoneNumberDto?)getCustomerInfoResult.Data;
                if (phoneNumber != null)//Phone number is taken
                {
                    //Check if url will be set by operations system
                    bool.TryParse(configuration["TargetURLs:SetTargetUrlByOs"], out bool setUrlByOs);
                    bool isIos = false;
                    if (setUrlByOs)//Set url by operating system
                    {
                        //Get user operating system information
                        var deviceRecordData = await deviceRecordService.GetDeviceRecord(kmlk.kmlkVrs);
                        if (!deviceRecordData.Result || deviceRecordData.Data == null) //error from service
                        {
                            setUrlByOs = false;
                        }
                        else
                        {
                            GetDeviceRecordResponseDto deviceRecordResponse = (GetDeviceRecordResponseDto)deviceRecordData.Data;
                            isIos = deviceRecordResponse.os == OpenBankingConstants.OsType.Ios; 
                        }
                    }
                    
                    
                    if (phoneNumber.isOn == "X")//OnUser
                    {
                        if (setUrlByOs)
                        {
                            forwardingAddress = isIos
                                ? string.Format(configuration["HHSForwardingAddress:OnIOS"] ?? string.Empty, consentId)
                                : string.Format(configuration["HHSForwardingAddress:OnAndroid"] ?? string.Empty, consentId);
                        }
                        else
                        {
                              forwardingAddress = string.Format(configuration["HHSForwardingAddress:On"] ?? string.Empty, consentId);
                        }
                    }
                    else
                    {
                        if (setUrlByOs)
                        {
                            forwardingAddress = isIos
                                ? string.Format(configuration["HHSForwardingAddress:BurganIOS"] ?? string.Empty, consentId)
                                : string.Format(configuration["HHSForwardingAddress:BurganAndroid"] ?? string.Empty, consentId);
                        }
                        else
                        {
                            forwardingAddress = string.Format(configuration["HHSForwardingAddress:Burgan"] ?? string.Empty, consentId);
                        }
                        
                    }
                    return forwardingAddress;
                }
        }
       
        return forwardingAddress;
    }
    
    /// <summary>
    /// Get Account Consent by checking idempotency.
    /// </summary>
    /// <returns>Already responsed account consent data</returns>
    public static async Task<ApiResult> GetIdempotencyAccountConsent(HesapBilgisiRizaIstegiHHSDto rizaIstegi,
        RequestHeaderDto header, IOBAuthorizationService authorizationService)
    {
        ApiResult result = new(); 
        var checkSumRequest = GetChecksumForXRequestIdSHA256(rizaIstegi, header.XRequestID);//Generate checksum
        //Get db account consent
        var getConsentResult = await authorizationService.GetIdempotencyRecordOfAccountPaymentConsent(rizaIstegi.katilimciBlg.yosKod,
            ConsentConstants.ConsentType.OpenBankingAccount, checkSumRequest);
        if (!getConsentResult.Result)
        {//error in checking idempotency
            return getConsentResult;
        }
        if (getConsentResult is { Result: true, Data: not null })
        {//Idempotency occured. Return previous response
            result.Data= JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>((string)getConsentResult.Data);
        }
        return result;
    }
    
    /// <summary>
    /// Get Payment Consent by checking idempotency.
    /// </summary>
    /// <returns>Already responsed payment consent data</returns>
    public static async Task<ApiResult> GetIdempotencyPaymentConsent(OdemeEmriRizaIstegiHHSDto rizaIstegi,
        RequestHeaderDto header, IOBAuthorizationService authorizationService)
    {
        ApiResult result = new(); 
        var checkSumRequest = GetChecksumForXRequestIdSHA256(rizaIstegi, header.XRequestID);//Generate checksum
        //Get db account consent
        var getConsentResult = await authorizationService.GetIdempotencyRecordOfAccountPaymentConsent(rizaIstegi.katilimciBlg.yosKod,
            ConsentConstants.ConsentType.OpenBankingPayment, checkSumRequest);
        if (!getConsentResult.Result)
        {//error in checking idempotency
            return getConsentResult;
        }
        if (getConsentResult is { Result: true, Data: not null })
        {//Idempotency occured. Return previous response
            result.Data= JsonSerializer.Deserialize<OdemeEmriRizasiHHSDto>((string)getConsentResult.Data);
        }
        return result;
    }
    
    /// <summary>
    /// Get PaymentOrder record by checking idempotency.
    /// </summary>
    /// <returns>Already responsed paymentorder data</returns>
    public static async Task<ApiResult> GetIdempotencyPaymentOrder(OdemeEmriIstegiHHSDto odemeEmriIstegi,
        RequestHeaderDto header, IOBAuthorizationService authorizationService)
    {
        ApiResult result = new(); 
        var checkSumRequest = GetChecksumForXRequestIdSHA256(odemeEmriIstegi, header.XRequestID);//Generate checksum
        //Get db account consent
        var getConsentResult = await authorizationService.GetIdempotencyRecordOfPaymentOrder(odemeEmriIstegi.katilimciBlg.yosKod, checkSumRequest);
        if (!getConsentResult.Result)
        {//error in checking idempotency
            return getConsentResult;
        }
        if (getConsentResult is { Result: true, Data: not null })
        {//Idempotency occured. Return previous response
            result.Data= JsonSerializer.Deserialize<OdemeEmriHHSDto>((string)getConsentResult.Data);
        }
        return result;
    }

}