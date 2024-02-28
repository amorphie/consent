using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
using NpgsqlTypes;

namespace amorphie.consent.core.Model;

public class OBPermissionType
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; }
    public string Description { get; set; }
    public string Language { get; set; }
}