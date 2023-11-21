using amorphie.consent.core.Model;
using amorphie.core.Base;

namespace amorphie.consent.core.DTO;
public class TokenDto : DtoBase
{
    public Guid ConsentId { get; set; }
    public string TokenValue { get; set; }
    public string TokenType { get; set; }
    public int ExpireTime { get; set; }
}