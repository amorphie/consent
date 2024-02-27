using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
using NpgsqlTypes;

namespace amorphie.consent.core.Model;

public class OBAccountConsentDetail : EntityBase
{
    public Guid ConsentId { get; set; }
    public string IdentityType { get; set; }
    public string IdentityData { get; set; }
    public string? InstitutionIdentityType { get; set; }
    public string? InstitutionIdentityData { get; set; }
    public string UserType { get; set; }

    public string HhsCode { get; set; }
    public string YosCode { get; set; }
    public string? AuthMethod { get; set; }
    public string? ForwardingAddress { get; set; }
    public string? HhsForwardingAddress { get; set; }
    public string? DiscreteGKDDefinitionType { get; set; }
    public string? DiscreteGKDDefinitionValue { get; set; }
    public DateTime? AuthCompletionTime { get; set; }
    public List<string> PermissionTypes { get; set; }
    public DateTime LastValidAccessDate { get; set; }
    public DateTime? TransactionInquiryStartTime { get; set; }
    public DateTime? TransactionInquiryEndTime { get; set; }
    public List<string>? AccountReferences { get; set; }
    public string? OhkMessage { get; set; }
    public string XRequestId { get; set; }
    public string XGroupId { get; set; }
    [ForeignKey("ConsentId")]
    public Consent Consent { get; set; }
}