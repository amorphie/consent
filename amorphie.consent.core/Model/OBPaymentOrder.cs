using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
namespace amorphie.consent.core.Model;
using NpgsqlTypes;
public class OBPaymentOrder : EntityBase
{
    public Guid ConsentId { get; set; }
    public Guid? UserId { get; set; }
    public string State { get; set; }
    public string ConsentDetailType { get; set; }
    public string AdditionalData { get; set; }
    [ForeignKey("ConsentId")]
    public Consent Consent { get; set; }
}