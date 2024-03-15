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
    public PushService(IMessagingGateway postPushService, IMapper mapper, IDeviceRecord deviceRecord, ITagService tagService)
    {
        _postPushService = postPushService;
        _tagService = tagService;
        _deviceRecord = deviceRecord;
        _mapper = mapper;
    }
    public async Task<IResult> OpenBankingSendPush(KimlikDto data, Guid consentId)
    {
        string targetUrl;
        var templateParameters = new Dictionary<string, object>();
        var number = await _tagService.GetCustomer(data.kmlkVrs);
        PhoneNumberDto phoneNumber = new();
        var telNo = _mapper.Map(number, phoneNumber);
        if (telNo.isOn == "X")
        {
            targetUrl = $"onmobil://openbanking?consentno={consentId}";
        }
        else
        {
            targetUrl = $"burgan://openbanking?consentno={consentId}";
        }
        templateParameters["targetUrl"] = targetUrl;
        var deviceRecordData = await _deviceRecord.GetDeviceRecord(data.kmlkVrs);

        if (deviceRecordData.Result)
        {
            var sendPush = new SendPushDto
            {
                Sender = "AutoDetect",
                CitizenshipNo = data.kmlkVrs,
                Template = "OpenBankingTest",
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
            var result = await _postPushService.SendPush(sendPush);

            return Results.Ok();
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
            return Results.Ok();
        }
    }
}