namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class PaymentStateChangedKafkaRecordDto
{
    public string? magic { get; set; }
    public string? type { get; set; }
    public object? headers { get; set; }
    public object? messageSchemaId { get; set; }
    public object? messageSchema { get; set; }
    public MessageDto message { get; set; }
}

public class MessageDto
{
    public DataDto data { get; set; }
    public object? beforeData { get; set; }
    public object? headers { get; set; }
}

public class DataDto
{
    public int ID { get; set; }
    public string TRAN_DATE { get; set; }
    public string CREATION_TIME { get; set; }
    public string UPDATE_DATE { get; set; }
    public string RIZANO { get; set; }
    public string EFT_RESPONSE { get; set; }
    public string PAYMENTPROCESSTYPE { get; set; }
  
}
