using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
using NpgsqlTypes;

namespace amorphie.consent.core.Model;

public class OBConsentIdentityInfo : EntityBase
{
    public Guid ConsentId { get; set; }
    public string IdentityType { get; set; }
    public string IdentityData { get; set; }
    public string? InstitutionIdentityType { get; set; }
    public string? InstitutionIdentityData { get; set; }
    public string UserType { get; set; }
    [ForeignKey("ConsentId")]
    public Consent Consent { get; set; }

}