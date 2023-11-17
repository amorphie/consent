using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
namespace amorphie.consent.core.Model;
using NpgsqlTypes;
public class Consent : EntityBase
{
    public Guid? UserId { get; set; }
    public string State { get; set; }
    public string? Description { get; set; }
    public string Scope { get; set; }
    public string Role { get; set; }
    public string Client { get; set; }
    public string Variant { get; set; }
    public string? xGroupId { get; set; }
    public string ConsentType { get; set; }
    public string AdditionalData { get; set; }
    public List<Token> Token { get; set; }
    public virtual ICollection<OBAccountReference> OBAccountReferences { get; set; }
    public virtual ICollection<OBConsentIdentityInfo> ObConsentIdentityInfos { get; set; }
    public virtual ICollection<ConsentDetail> ConsentDetails { get; set; }

    [NotMapped]
    public virtual NpgsqlTsVector SearchVector { get; set; }


}