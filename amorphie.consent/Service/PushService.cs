using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.MessagingGateway;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.Service.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;

public class PushService : IPushService
{
    private readonly IMessagingGateway _postPushService;
    public PushService(IMessagingGateway postPushService)
    {
        _postPushService = postPushService;
    }
    public Task<IResult> OpenBankingSendPush(KimlikDto data, string consentId)
    {
        if (data.kmlkVrs != null)
        {
            var smsRequest = new SmsRequestDto
            {
                Sender = "AutoDetect",
                SmsType = "Otp",
                Phone = new PhoneInfoDto
                {
                    CountryCode = 90,
                    Prefix = 539,
                    Number = 2314593,
                },
                Content = "şifresi ile giriş yapabilirsiniz",
                Process = new ProcessInfoDto()
                {
                    Name = "OpenBanking Money Transfer",
                    Identity = "Otp Login"
                },
            };
            _postPushService.SendSms(smsRequest);
            return Task.FromResult(Results.Ok() as IResult);
        }
        else
        {
            var sendPush = new SendPushDto
            {
                Sender = "AutoDetect",
                CitizenshipNo = data.kmlkVrs,
                Template = "Test 1",
                TemplateParams = "",
                SaveInbox = false,
                Process = new ProcessInfo
                {
                    Name = "Açık Bankacılık",
                    ItemId = consentId,
                    Action = "Eylem",
                    Identity = "Kimlik"
                }
            };
            _postPushService.SendPush(sendPush);
            return Task.FromResult(Results.Ok() as IResult);
        }
    }
}