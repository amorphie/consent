using System.ComponentModel.DataAnnotations;

namespace amorphie.consent.core.Model;

public class OBPermissionType
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; }= string.Empty;
    public int GroupId { get; set; }
    public string GroupName { get; set; }= string.Empty;
    public string Language { get; set; }= string.Empty;
}