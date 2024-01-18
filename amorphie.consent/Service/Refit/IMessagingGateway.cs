using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using Refit;

public interface IMessagingGateway
{
    [Post("/api/v2/Messaging/push-notification/templated")]
    Task<SendPushDto> SendPush([Body] SendPushDto sendPush);

    [Post("/api/v2/Messaging/sms/message")]
    Task<SmsResponseDto> SendSms(SmsRequestDto request);
}
