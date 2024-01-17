using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
using NpgsqlTypes;

namespace amorphie.consent.core.Model;

public class OBEvent : EntityBase
{
    public Guid EventNumber { get; set; }
    public string HHSCode { get; set; }
    public string YOSCode { get; set; }
    public DateTime EventDate { get; set; }
    public string EventType { get; set; }
    public string SourceType { get; set; }
    public string SourceNumber { get; set; }
    public string AdditionalData { get; set; }
    public string Source { get; set; }
    public int? ResponseCode { get; set; }
    public int? TryCount { get; set; }
    public DateTime? LastTryTime { get; set; }
    public bool? IsUndeliverable { get; set; }
}