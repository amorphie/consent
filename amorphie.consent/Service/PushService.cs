using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.MessagingGateway;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.Tag;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class PushService : IPushService
{
    private readonly IMessagingGateway _postPushService;
    private readonly ITagService _tagService;
    private readonly IDeviceRecord _deviceRecord;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    public PushService(IConfiguration configuration, IMessagingGateway postPushService, IMapper mapper, IDeviceRecord deviceRecord, ITagService tagService)
    {
        _postPushService = postPushService;
        _tagService = tagService;
        _deviceRecord = deviceRecord;
        _mapper = mapper;
        _configuration = configuration;
    }
    public async Task<ApiResult> OpenBankingSendPush(KimlikDto data, Guid consentId)
    {
        ApiResult result = new();
        try
        {
            string targetUrl;
            var templateParameters = new Dictionary<string, object>();
            var number = await _tagService.GetCustomer(data.kmlkVrs);
            PhoneNumberDto phoneNumber = new();
            var telNo = _mapper.Map(number, phoneNumber);
            if (telNo.isOn == "X")
            {
                targetUrl = String.Format(_configuration["OnMobileTargetUrl"] ?? String.Empty, consentId);
            }
            else
            {
                targetUrl = String.Format(_configuration["BurganMobileTargetUrl"] ?? String.Empty, consentId);
            }
            templateParameters["targetUrl"] = targetUrl;
            var deviceRecordData = await _deviceRecord.GetDeviceRecord(data.kmlkVrs);

            if (deviceRecordData.Result)
            {
                var sendPush = new SendPushDto
                {
                    Sender = "AutoDetect",
                    CitizenshipNo = data.kmlkVrs,
                    Template = _configuration["PushTemplateName"] ?? String.Empty,
                    TemplateParams = JsonConvert.SerializeObject(templateParameters),
                    CustomParameters = "",
                    SaveInbox = false,
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
                        CountryCode = Int32.Parse(telNo.country),
                        Prefix = Int32.Parse(telNo.prefix),
                        Number = Int32.Parse(telNo.number),
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
        }
        return result;
    }
}