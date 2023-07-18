using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;

public class ConsentPermission:EntityBase
{
    [ForeignKey("Consent")]
    public Guid ConsentId { get; set; }
    public string Permission { get; set; }
    public Consent Consent { get; set; }
}