using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
using NpgsqlTypes;

namespace amorphie.consent.core.Model;

public class OBPaymentConsentDetail : EntityBase
{
    public Guid ConsentId { get; set; }
    public string? IdentityType { get; set; }
    public string? IdentityData { get; set; }
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
    
    public string Currency { get; set; }
    public string Amount { get; set; }
    public string? SenderTitle { get; set; }
    public string? SenderAccountNumber { get; set; }
    public string? SenderAccountReference { get; set; }
    
    public string ReceiverTitle { get; set; }
    public string ReceiverAccountNumber { get; set; }
    public string? KolasType { get; set; }
    public string? KolasValue { get; set; }
    public long? KolasRefNum { get; set; }
    public string? KolasAccountType { get; set; }
    public string? QRCodeFlowType { get; set; }
    public string? QRCodeRef { get; set; }
    public string? QRCodeProducerCode { get; set; }
    
    public string PaymentSource { get; set; }
    public string PaymentPurpose { get; set; }
    public string? ReferenceInformation { get; set; }
    public string? PaymentDescription { get; set; }
    public string? OHKMessage { get; set; }
    public string PaymentSystem { get; set; }
    public DateTime? ExpectedPaymentDate { get; set; }
    public string? WorkplaceCategoryCode  { get; set; }
    public string? SubWorkplaceCategoryCode  { get; set; }
    public string? GeneralWorkplaceNumber  { get; set; }
    
    public string XRequestId { get; set; }
    public string XGroupId { get; set; }
    [ForeignKey("ConsentId")]
    public Consent Consent { get; set; }
}