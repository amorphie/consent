using amorphie.consent.core.DTO.OpenBanking;
using Refit;

public interface IMessagingGateway
{
    [Post("/api/v2/Messaging/push-notification/templated")]
    Task<SendPush> SendPush([Body] SendPush sendPush);
    
    [Post("/api/v2/Messaging/sms/message")]
    Task<SmsResponseDto> SendSms(SmsRequestDto request);
}
