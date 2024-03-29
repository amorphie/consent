using System.ComponentModel.DataAnnotations;

namespace amorphie.consent.core.Model;

public class OBErrorCodeDetail
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public int InternalCode { get; set; }
    public string MessageTr { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string BkmCode { get; set; } = string.Empty;
}