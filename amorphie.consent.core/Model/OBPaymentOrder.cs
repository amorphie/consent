using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
namespace amorphie.consent.core.Model;
using NpgsqlTypes;
public class OBPaymentOrder : EntityBase
{
    public Guid ConsentId { get; set; }
    public Guid? UserId { get; set; }
    public string State { get; set; }
    public string AdditionalData { get; set; }

    public string HhsCode { get; set; }
    public string YosCode { get; set; }
    public string Currency { get; set; }
    public string Amount { get; set; }
    public string PaymentState { get; set; }
    public string PaymentSystemNumber { get; set; }
    public string? PSNDate { get; set; }
    public string? PSNYosCode { get; set; }
    public int? PSNRefNum { get; set; }
    public string PaymentSource { get; set; }
    public string PaymentPurpose { get; set; }
    public string? ReferenceInformation { get; set; }
    public string? PaymentDescription { get; set; }
    public string? OHKMessage { get; set; }
    public string PaymentSystem { get; set; }
    public DateTime? ExpectedPaymentDate { get; set; }
    public string? WorkplaceCategoryCode { get; set; }
    public string? SubWorkplaceCategoryCode { get; set; }
    public string? GeneralWorkplaceNumber { get; set; }
    public string? PaymentServiceUpdateTime { get; set; }
    public string XRequestId { get; set; }
    public string XGroupId { get; set; }

    [ForeignKey("ConsentId")]
    public Consent Consent { get; set; }
}