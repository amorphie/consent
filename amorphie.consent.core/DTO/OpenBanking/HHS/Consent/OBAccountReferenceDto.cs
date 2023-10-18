using amorphie.core.Base;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class OBAccountReferenceDto : DtoBase
{
    public Guid ConsentId { get; set; }
    public string AccountReference { get; set; }
    /// <summary>
    /// Permission types joined by comma
    /// </summary>
    public string PermissionType { get; set; }
    public DateTime LastValidAccessDate { get; set; }
    public DateTime? TransactionInquiryStartTime { get; set; }
    public DateTime? TransactionInquiryEndTime { get; set; }
}