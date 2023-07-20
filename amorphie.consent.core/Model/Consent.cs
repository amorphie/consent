using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
namespace amorphie.consent.core.Model;

public class Consent : EntityBase
{
    [ForeignKey("ConsentDefinition")]
    public Guid? ConsentDefinitionId { get; set; }
    
    public Guid UserId { get; set; }
    public string State { get; set; }
    public string ConsentType { get; set; }
    public string AdditionalData { get; set; }
    public ConsentDefinition ConsentDefinition { get; set; }
    public List<ConsentPermission> ConsentPermissions { get; set; }
}