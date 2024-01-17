using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
using NpgsqlTypes;

namespace amorphie.consent.core.Model;

public class OBEvent : EntityBase
{
    public string HHSCode { get; set; }
    public string YOSCode { get; set; }
    public string AdditionalData { get; set; }
    public string ModuleName { get; set; }
    public int? ResponseCode { get; set; }
    public int? TryCount { get; set; }
    public DateTime? LastTryTime { get; set; }
    public bool? IsUndeliverable { get; set; }
    public virtual ICollection<OBEventItem> OBEventItems { get; set; }
}