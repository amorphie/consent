public class Consent:EntityBase
{
    [ForeignKey("ConsentDefinition")]
    public Guid? ConsentDefinitionId { get; set; }
    
    public Guid UserId { get; set; }
    public int State { get; set; }
    public int ConsentType { get; set; }
    public string AdditionalData { get; set; }
    public ConsentDefinition ConsentDefinition { get; set; }
    public List<ConsentPermission> ConsentPermissions { get; set; }
}