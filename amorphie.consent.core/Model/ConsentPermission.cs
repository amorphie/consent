using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;

namespace amorphie.consent.core.Model;
public class ConsentPermission : EntityBase
{
    [ForeignKey("Consent")]
    public Guid ConsentId { get; set; }
    public string Permission { get; set; }
    public Consent Consent { get; set; }
}