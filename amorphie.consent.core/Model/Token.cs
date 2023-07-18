using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
namespace amorphie.consent.core.Model;

public class Token:EntityBase
{
    [ForeignKey("Consent")]
    public Guid ConsentId { get; set; }
    public string TokenValue { get; set; }
    public int TokenType { get; set; }
    public int ExpireTime { get; set; }
    public Consent Consent { get; set; }
}