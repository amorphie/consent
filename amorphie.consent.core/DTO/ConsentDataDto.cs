using amorphie.core.Base;

public class ConsentDataDto : DtoBase
{
    // Consent verileri
    public string ConsentType { get; set; }
    public string State { get; set; }
    public string AdditionalData { get; set; }
}