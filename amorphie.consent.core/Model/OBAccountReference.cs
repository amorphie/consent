using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
using NpgsqlTypes;

namespace amorphie.consent.core.Model;

public class OBAccountReference : EntityBase
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
    [ForeignKey("ConsentId")]
    public Consent Consent { get; set; }
}