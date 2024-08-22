using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.DTO.Tag;
using amorphie.consent.core.DTO.Token;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.data;
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

        if (httpContext.Request.Headers.TryGetValue("X-JWS-Signature", out traceValue))
        {
            header.XJWSSignature = traceValue;
        }

        if (httpContext.Request.Headers.TryGetValue("PSU-Fraud-Check", out traceValue))
        {
            header.PSUFraudCheck = traceValue;
        }
        if (httpContext.Request.Headers.TryGetValue("PSU-Session-ID", out traceValue))
        {
            header.PSUSessionId = traceValue;
        }

        return header;
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
    public static string GetChecksumSHA256(object body)
    {
        // Initialize a SHA256 hash object.
        using SHA256 sha256Hash = SHA256.Create();
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore // This will remove null properties
        };
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body,settings)));
        return Convert.ToHexString(bytes);
    }


    /// <summary>
    /// Generates sha256 hash of body
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    public static string GetChecksumSHA256(string body)
    {
        // Initialize a SHA256 hash object.
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(body));
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
    public static async Task<string> GetHhsForwardingAddressAsync(IConfiguration configuration, string? customerCitizenshipNumber,
        string consentId, ITagService tagService, IDeviceRecord deviceRecordService)
    {
        //Set token login url
        string forwardingAddress = string.Format(configuration["HHSForwardingAddress"] ?? string.Empty, consentId);
        if (customerCitizenshipNumber is null)
        {
            return forwardingAddress;
        }
        var getCustomerInfoResult = await tagService.GetCustomer(customerCitizenshipNumber); //Get customer phonenumber
        if (!getCustomerInfoResult.Result || getCustomerInfoResult.Data == null) //Error in service
        {
            return forwardingAddress;
        }

        //Check phone number On user or Burgan User
        PhoneNumberDto? phoneNumber = (PhoneNumberDto?)getCustomerInfoResult.Data;
        if (phoneNumber != null) //Phone number is taken
        {
            //Check if url will be set by operations system
            bool.TryParse(configuration["TargetURLs:SetTargetUrlByOs"], out bool setUrlByOs);
            bool isIos = false;
            if (setUrlByOs) //Set url by operating system
            {
                //Get user operating system information
                var deviceRecordData = await deviceRecordService.GetDeviceRecord(customerCitizenshipNumber);
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
            if (phoneNumber.isOn == "X") //OnUser
            {
                if (setUrlByOs)
                {
                    forwardingAddress = isIos
                        ? string.Format(configuration["HHSForwardingAddress:OnIOS"] ?? string.Empty, consentId)
                        : string.Format(configuration["HHSForwardingAddress:OnAndroid"] ?? string.Empty, consentId);
                }
                else
                {
                    forwardingAddress =
                        string.Format(configuration["HHSForwardingAddress:On"] ?? string.Empty, consentId);
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
                    forwardingAddress = string.Format(configuration["HHSForwardingAddress:Burgan"] ?? string.Empty,
                        consentId);
                }
            }

            return forwardingAddress;
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
            result.Data = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>((string)getConsentResult.Data);
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
            result.Data = JsonSerializer.Deserialize<OdemeEmriRizasiHHSDto>((string)getConsentResult.Data);
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
            result.Data = JsonSerializer.Deserialize<OdemeEmriHHSDto>((string)getConsentResult.Data);
        }
        return result;
    }
    
    /// <summary>
    /// Set header x-total-count and link properties
    /// </summary>
    /// <param name="basePath"></param>
    /// <param name="httpContext"></param>
    /// <param name="totalCount"></param>
    /// <param name="syfKytSayi"></param>
    /// <param name="syfNo"></param>
    public static void SetHeaderLink(string basePath, HttpContext httpContext, int totalCount, int syfKytSayi, int syfNo)
    {
        httpContext.Response.Headers["x-total-count"] = totalCount.ToString();
        
        // Calculate last page number
        int lastPageNumber = totalCount > 0 ? (totalCount / syfKytSayi + (totalCount % syfKytSayi > 0 ? 1 : 0)) : 1;

        // Ensure first and last links are correctly set even when totalCount is 0
        // Construct the Link header value with conditional inclusion of "first" and "last"
        var links = new List<string>
        {
            $"</{basePath}&syfNo=1>; rel=\"first\"",
            $"</{basePath}&syfNo={lastPageNumber}>; rel=\"last\""
        };
        // Include "prev" and "next" links based on current page number
        if (totalCount > 0)
        {
            if (syfNo > 1)
            {
                links.Add($"</{basePath}&syfNo={syfNo - 1}>; rel=\"prev\"");
            }

            if (syfNo < lastPageNumber)
            {
                links.Add($"</{basePath}&syfNo={syfNo + 1}>; rel=\"next\"");
            }
        }
        // Join the links with commas and set the header
        httpContext.Response.Headers["Link"] = string.Join(", ", links);
    }
    
   
    /// <summary>
    /// Cancels institution ohk type consent
    /// </summary>
    /// <returns>Cancel process result</returns>
    public static async Task<ApiResult> CancelInstitutionConsentUnAuthorized(ConsentDbContext context, ITokenService tokenService,
        IOBEventService eventService, IYosInfoService yosInfoService, Consent entity, string cancelDetailCode)

    {
        ApiResult result = new();
        //State list can be cancelled from login
        var canBeCancelledStates = new List<string>()
        {
            OpenBankingConstants.RizaDurumu.YetkiBekleniyor,
            OpenBankingConstants.RizaDurumu.Yetkilendirildi,
            OpenBankingConstants.RizaDurumu.YetkiKullanildi
        };
       
        if (!canBeCancelledStates.Contains(entity.State))
        {
            result.Result = false;
            result.Message = "Consent state not valid to be cancelled.";
            return result;
        }
        return await CancelConsent(context, tokenService, eventService, yosInfoService, entity,cancelDetailCode);
    }
    
    /// <summary>
    /// Cancel consent generic method to cancel consent without any control.
    /// Checks consent type and cancels according to consent type
    /// </summary>
    /// <returns></returns>
    public static async Task<ApiResult> CancelConsent(ConsentDbContext context, ITokenService tokenService,
        IOBEventService eventService, IYosInfoService yosInfoService, Consent entity, string cancelDetailCode )
    {
        ApiResult result = new();
        if (entity.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount)
        {
            //Account consent
            //Update consent rıza bilgileri properties
            await  CancelAccountConsent(context, tokenService, eventService: eventService, yosInfoService, entity,
                cancelDetailCode);
        }
        else if (entity.ConsentType == ConsentConstants.ConsentType.OpenBankingPayment)
        {
            //Payment consent
            //Update consent rıza bilgileri properties
            await  CancelPaymentConsent(context, tokenService, entity,
                cancelDetailCode);
        }
        else
        {
            //Not related type
            result.Result = false;
            result.Message = "Consent type not valid";
        }
        return result;
    }



    /// <summary>
    /// Cancels account consent.
    /// Updates additionaldata, consent state, send to account serive clms.
    /// If there is token, revokes the token
    /// If event subscription, send event to yos
    /// </summary>
    public static async Task CancelAccountConsent(ConsentDbContext context, ITokenService tokenService,
        IOBEventService eventService, IYosInfoService yosInfoService, Consent entity, string cancelDetailCode )
    {
       var currentState = entity.State;//current consent state
        //Update consent rıza bilgileri properties
        var additionalData = JsonSerializer.Deserialize<HesapBilgisiRizasiHHSDto>(entity!.AdditionalData);
        additionalData!.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiIptal;
        additionalData.rzBlg.rizaIptDtyKod = cancelDetailCode;
        additionalData.rzBlg.gnclZmn = DateTime.UtcNow;
        entity.AdditionalData = JsonSerializer.Serialize(additionalData);
        entity.ModifiedAt = DateTime.UtcNow;
        entity.State = OpenBankingConstants.RizaDurumu.YetkiIptal;
        entity.StateModifiedAt = DateTime.UtcNow;
        entity.StateCancelDetailCode = additionalData.rzBlg.rizaIptDtyKod;
        //Update consent detail to send consent information to account service.
        var consentDetail = entity.OBAccountConsentDetails.FirstOrDefault();
        if (consentDetail is not null)
        {
            consentDetail.SendToServiceTryCount = 0;
            consentDetail.SendToServiceDeliveryStatus = OpenBankingConstants.RecordDeliveryStatus.Processing;
            context.OBAccountConsentDetails.Update(consentDetail);
        }

        context.Consents.Update(entity);
        await context.SaveChangesAsync();

        if (currentState == OpenBankingConstants.RizaDurumu.YetkiKullanildi)
        {
            //Revoke token
            await tokenService.RevokeConsentToken(entity.Id);
        }
        
        //If YOS has subscription, Do event process
        ApiResult yosHasSubscription = await yosInfoService.IsYosSubscsribed(entity.Variant!,
            OpenBankingConstants.OlayTip.KaynakGuncellendi, OpenBankingConstants.KaynakTip.HesapBilgisiRizasi);
        if (yosHasSubscription.Result
            && yosHasSubscription.Data != null
            && (bool)yosHasSubscription.Data)
        {

            //Send event to yos
            await eventService.DoEventProcess(entity.Id.ToString(),
                additionalData.katilimciBlg,
                eventType: OpenBankingConstants.OlayTip.KaynakGuncellendi,
                sourceType: OpenBankingConstants.KaynakTip.HesapBilgisiRizasi,
                sourceNumber: entity.Id.ToString());
        }
    }

    
    /// <summary>
    /// Cancels payment consent.
    /// Updates additionaldata, consent state
    /// </summary>
      public static async Task CancelPaymentConsent(ConsentDbContext context, ITokenService tokenService,
          Consent entity, string cancelDetailCode )
    {
       var currentState = entity.State;//current consent state
        //Update consent rıza bilgileri properties
        var additionalData = JsonSerializer.Deserialize<OdemeEmriRizasiWithMsrfTtrHHSDto>(entity.AdditionalData);
        additionalData!.rzBlg.rizaDrm = OpenBankingConstants.RizaDurumu.YetkiIptal;
        additionalData.rzBlg.rizaIptDtyKod = cancelDetailCode;
        additionalData.rzBlg.gnclZmn = DateTime.UtcNow;
        entity.AdditionalData = JsonSerializer.Serialize(additionalData);
        entity.ModifiedAt = DateTime.UtcNow;
        entity.State = OpenBankingConstants.RizaDurumu.YetkiIptal;
        entity.StateModifiedAt = DateTime.UtcNow;
        entity.StateCancelDetailCode = additionalData.rzBlg.rizaIptDtyKod;
        context.Consents.Update(entity);
        await context.SaveChangesAsync();

        if (currentState == OpenBankingConstants.RizaDurumu.YetkiKullanildi)
        {
            //Revoke token
            await tokenService.RevokeConsentToken(entity.Id);
        }
    }

  


}
