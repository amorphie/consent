namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class BalanceChangedKafkaRecordDto
{
    public string? magic { get; set; }
    public string? type { get; set; }
    public object? headers { get; set; }
    public object? messageSchemaId { get; set; }
    public object? messageSchema { get; set; }
    public KafkaMessage? message { get; set; }
}

public class KafkaMessage
{
    public Data? data { get; set; }
    public object? beforeData { get; set; }
    public object? headers { get; set; }
}

public class Data
{
    public int ACCOUNT_BRANCH_CODE { get; set; }
    public int ACCOUNT_NUMBER { get; set; }
    public int ACCOUNT_SUFFIX { get; set; }
    public string? LAST_UPDATE_DATE { get; set; }
    public string? INSTANT_BALANCE_NOTIFICATION_PERMISSION { get; set; }
    public string? OPEN_BANKING_SHARE_PERMISSION { get; set; }
    public string? OPEN_BANKING_CONSENT_NUMBER { get; set; }
    public string? HESAP_REF { get; set; }
}
