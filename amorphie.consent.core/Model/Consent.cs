using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
namespace amorphie.consent.core.Model;
using NpgsqlTypes;
public class Consent : EntityBase
{
    public Guid? UserId { get; set; }
    public Guid? ScopeId { get; set; }
    public Guid? RoleId { get; set; }
    public string ClientCode { get; set; }
    public long? UserTCKN { get; set; }
    public long? ScopeTCKN { get; set; }
    public string? Variant { get; set; }
    public string State { get; set; }
    public string? Description { get; set; }
    public string? XGroupId { get; set; }
    public string ConsentType { get; set; }
    public string AdditionalData { get; set; }
    public DateTime StateModifiedAt { get; set; }
    public string? StateCancelDetailCode { get; set; }
    public virtual ICollection<Token> Tokens { get; set; }
    public virtual ICollection<OBAccountConsentDetail> OBAccountConsentDetails { get; set; }
    public virtual ICollection<OBPaymentConsentDetail> OBPaymentConsentDetails { get; set; }
    // public virtual ICollection<OBAccountReference> OBAccountReferences { get; set; }
    // public virtual ICollection<OBConsentIdentityInfo> ObConsentIdentityInfos { get; set; }
    public virtual ICollection<OBPaymentOrder> PaymentOrders { get; set; }

    [NotMapped]
    public virtual NpgsqlTsVector SearchVector { get; set; }


}