using amorphie.consent.core.Model;
using amorphie.core.Base;

public class TokenDto : DtoBase
{
    public Guid ConsentId { get; set; }
    public string TokenValue { get; set; }
    public int TokenType { get; set; }
    public int ExpireTime { get; set; }
    // public Consent Consent { get; set; }
}