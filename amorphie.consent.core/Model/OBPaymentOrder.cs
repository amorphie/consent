using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
namespace amorphie.consent.core.Model;
public class OBPaymentOrder : EntityBase
{
    public Guid ConsentId { get; set; }
    public Guid? UserId { get; set; }
    public string State { get; set; }= String.Empty;
    public string AdditionalData { get; set; }= String.Empty;

    public string HhsCode { get; set; }= String.Empty;
    public string YosCode { get; set; }= String.Empty;
    public string Currency { get; set; }= String.Empty;
    public string Amount { get; set; }= String.Empty;
    public string PaymentState { get; set; }= String.Empty;
    public string PaymentSystemNumber { get; set; }= String.Empty;
    public string? PSNDate { get; set; }
    public string? PSNYosCode { get; set; }
    public int? PSNRefNum { get; set; }
    public string PaymentSource { get; set; }= String.Empty;
    public string PaymentPurpose { get; set; }= String.Empty;
    public string? ReferenceInformation { get; set; }
    public string? PaymentDescription { get; set; }
    public string? OHKMessage { get; set; }
    public string PaymentSystem { get; set; }= String.Empty;
    public DateTime? ExpectedPaymentDate { get; set; }
    public string? WorkplaceCategoryCode { get; set; }
    public string? SubWorkplaceCategoryCode { get; set; }
    public string? GeneralWorkplaceNumber { get; set; }
    public string? PaymentServiceUpdateTime { get; set; }
    public string XRequestId { get; set; }= String.Empty;
    public string XGroupId { get; set; }= String.Empty;
    
    public string CheckSumValue { get; set; }= String.Empty;
    public DateTime CheckSumLastValiDateTime { get; set; }
    public string SaveResponseMessage { get; set; }= String.Empty;

    [ForeignKey("ConsentId")]
    public Consent Consent { get; set; }
}