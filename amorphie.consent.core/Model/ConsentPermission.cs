using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using amorphie.core.Base;
using NpgsqlTypes;

namespace amorphie.consent.core.Model;
public class ConsentPermission : EntityBase
{
    [ForeignKey("Consent")]
    public Guid ConsentId { get; set; }
    public string Permission { get; set; }
    public DateTime PermissionLastDate { get; set; }
    public DateTime? TransactionStartDate;
    public DateTime? TransactionEndDate;
    [JsonIgnore]
    public Consent Consent { get; set; }
    [NotMapped]
    public virtual NpgsqlTsVector SearchVector { get; set; }
}