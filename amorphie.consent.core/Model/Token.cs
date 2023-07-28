using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using amorphie.core.Base;
namespace amorphie.consent.core.Model;

public class Token:EntityBase
{
    [ForeignKey("Consent")]
    public Guid ConsentId { get; set; }
    public string TokenValue { get; set; }
    public string TokenType { get; set; }
    public int ExpireTime { get; set; }
    [JsonIgnore]
    public Consent Consent { get; set; }
}