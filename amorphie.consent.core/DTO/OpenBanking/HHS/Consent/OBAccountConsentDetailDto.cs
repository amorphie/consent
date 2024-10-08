namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class OBAccountConsentDetailDto
{
    public Guid ConsentId { get; set; }
    public string IdentityType { get; set; } = string.Empty;
    public string IdentityData { get; set; } = string.Empty;
    public string? InstitutionIdentityType { get; set; }
    public string? InstitutionIdentityData { get; set; }

    public string? CustomerNumber { get; set; }
    public string? InstitutionCustomerNumber { get; set; }
    public string UserType { get; set; } = string.Empty;

    public string HhsCode { get; set; } = string.Empty;
    public string YosCode { get; set; } = string.Empty;
    public string? AuthMethod { get; set; }
    public string? ForwardingAddress { get; set; }
    public string? HhsForwardingAddress { get; set; }
    public string? DiscreteGKDDefinitionType { get; set; }
    public string? DiscreteGKDDefinitionValue { get; set; }
    public DateTime? AuthCompletionTime { get; set; }
    public List<string> PermissionTypes { get; set; } = new();
    public DateTime? TransactionInquiryStartTime { get; set; }
    public DateTime? TransactionInquiryEndTime { get; set; }
    public List<string>? AccountReferences { get; set; }
    public string? OhkMessage { get; set; }
    public string XRequestId { get; set; } = string.Empty;
    public string XGroupId { get; set; } = string.Empty;

    public int? SendToServiceTryCount { get; set; }
    public DateTime? SendToServiceLastTryTime { get; set; }
    public int? SendToServiceDeliveryStatus { get; set; }

}