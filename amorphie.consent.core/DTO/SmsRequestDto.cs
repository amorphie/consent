public class SmsRequestDto{
    public string Sender { get; set; }
    public string SmsType { get; set; }
    public PhoneInfoDto Phone { get; set; }
    public string Content { get; set; }
    public int CustomerNo { get; set; }
    public string CitizenshipNo { get; set; }
    public object Tags { get; set; }
    public bool InstantReminder { get; set; }
    public ProcessInfoDto Process { get; set; }
}

public class PhoneInfoDto
{
    public int CountryCode { get; set; }
    public int Prefix { get; set; }
    public int Number { get; set; }
}

public class ProcessInfoDto
{
    public string Name { get; set; }
    public object ItemId { get; set; }
    public object Action { get; set; }
    public string Identity { get; set; }
}