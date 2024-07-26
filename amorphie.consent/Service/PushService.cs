using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.MessagingGateway;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.Tag;
using amorphie.consent.core.DTO.Token;
using amorphie.consent.core.Enum;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;
using AutoMapper;
using Newtonsoft.Json;

namespace amorphie.consent.Service;

public class PushService : IPushService
{
    private readonly IMessagingGateway _postPushService;
    private readonly ITagService _tagService;
    private readonly IDeviceRecord _deviceRecord;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PushService> _logger;

    public PushService(IConfiguration configuration, IMessagingGateway postPushService, IMapper mapper,
        IDeviceRecord deviceRecord, ITagService tagService, ILogger<PushService> logger)
    {
        _postPushService = postPushService;
        _tagService = tagService;
        _deviceRecord = deviceRecord;
        _mapper = mapper;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ApiResult> OpenBankingSendPush(string userTckn, Guid consentId)
    {
        ApiResult result = new();
        try
        {
            string targetUrl;
            var templateParameters = new Dictionary<string, object>();
            var getCustomerInfoResult = await _tagService.GetCustomer(userTckn);

            if (!getCustomerInfoResult.Result || getCustomerInfoResult.Data == null) //error from service
            {
                _logger.LogError("Error in tagService.GetCustomer");
                result.Result = false;
                return result;
            }

            var deviceRecordData = await _deviceRecord.GetDeviceRecord(userTckn);
            if (!deviceRecordData.Result || deviceRecordData.Data == null) //error from service
            {
                _logger.LogError("Error in deviceRecord.GetDeviceRecord");
                result.Result = false;
                return result;
            }
            
            //Check phone number On user or Burgan User
            PhoneNumberDto phoneNumber = (PhoneNumberDto)getCustomerInfoResult.Data;
            GetDeviceRecordResponseDto deviceRecordResponse = (GetDeviceRecordResponseDto)deviceRecordData.Data;
            bool isIos = deviceRecordResponse.os == OpenBankingConstants.OsType.Ios; 
            bool.TryParse(_configuration["TargetURLs:SetTargetUrlByOs"], out bool setTargetUrlByOs);
            
            if (phoneNumber.isOn == "X") //OnUser
            {
                if (!setTargetUrlByOs)
                {
                    targetUrl = String.Format(_configuration["TargetURLs:OnMobileTargetUrl"] ?? String.Empty, consentId);
                }
                else if (isIos)
                {
                    targetUrl = String.Format(_configuration["TargetURLs:OnMobileTargetUrlIOS"] ?? String.Empty, consentId);
                }
                else
                {
                    targetUrl = String.Format(_configuration["TargetURLs:OnMobileTargetUrlAndroid"] ?? String.Empty, consentId);
                }
            }
            else//BurganUser
            {
                if (!setTargetUrlByOs)
                {
                    targetUrl = String.Format(_configuration["TargetURLs:BurganMobileTargetUrl"] ?? String.Empty, consentId);
                }
                else if (isIos)
                {
                    targetUrl = String.Format(_configuration["TargetURLs:BurganMobileTargetUrlIOS"] ?? String.Empty, consentId);
                }
                else
                {
                    targetUrl = String.Format(_configuration["TargetURLs:BurganMobileTargetUrlAndroid"] ?? String.Empty, consentId);
                }
                
            }

            templateParameters["targetUrl"] = targetUrl;
            var checkDeviceRecordData = _configuration["CheckDeviceRecordData"];
            if (checkDeviceRecordData != null)
            {
                deviceRecordData.Result = true;
            }
            if (deviceRecordData.Result)
            {
                var sendPush = new SendPushDto
                {
                    Sender = "AutoDetect",
                    CitizenshipNo = userTckn,
                    Template = _configuration["PushTemplateName"] ?? String.Empty,
                    TemplateParams = JsonConvert.SerializeObject(templateParameters),
                    CustomParameters = "",
                    SaveInbox = true,
                    Process = new ProcessInfo
                    {
                        Name = "Açık Bankacılık",
                        ItemId = consentId.ToString(),
                        Action = "Eylem",
                        Identity = "Kimlik"
                    }
                };
                await _postPushService.SendPush(sendPush);
            }
            else
            {
                var smsRequest = new SmsRequestDto
                {
                    Sender = "AutoDetect",
                    SmsType = "Otp",
                    Phone = new PhoneInfoDto
                    {
                        CountryCode = Int32.Parse(phoneNumber.country),
                        Prefix = Int32.Parse(phoneNumber.prefix),
                        Number = Int32.Parse(phoneNumber.number),
                    },
                    Content = "şifresi ile giriş yapabilirsiniz",
                    Process = new ProcessInfoDto()
                    {
                        Name = targetUrl,
                        Identity = "Otp Login"
                    },
                };
                await _postPushService.SendSms(smsRequest);
            }
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
            Console.WriteLine(e.ToString());
        }

        return result;
    }
}