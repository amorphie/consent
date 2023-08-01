using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
using NpgsqlTypes;

namespace amorphie.consent.core.Model;

public class ConsentDefinition : EntityBase
{
    public string Name { get; set; }
    public string RoleAssignment { get; set; }
    public string[] Scope { get; set; }
    public string[] ClientId { get; set; }
    [NotMapped]
    public virtual NpgsqlTsVector SearchVector { get; set; }
}