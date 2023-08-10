using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
namespace amorphie.consent.core.Model;
using NpgsqlTypes;
public class Consent : EntityBase
{
    [ForeignKey("ConsentDefinition")]
    public Guid? ConsentDefinitionId { get; set; }

    public Guid UserId { get; set; }
    public string State { get; set; }
    public string Description { get; set; }
    public string ConsentType { get; set; }
    public string AdditionalData { get; set; }
    public List<Token> Token { get; set; }
    public ConsentPermission ConsentPermission { get; set; }
    [NotMapped]
    public virtual NpgsqlTsVector SearchVector { get; set; }

    
}