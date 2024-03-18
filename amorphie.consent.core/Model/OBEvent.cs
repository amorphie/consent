using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
using NpgsqlTypes;

namespace amorphie.consent.core.Model;

public class OBEvent : EntityBase
{
    [StringLength(10)]
    public string HHSCode { get; set; } = String.Empty;
    [StringLength(10)]
    public string YOSCode { get; set; } = String.Empty;
    public string EventNumber { get; set; } = String.Empty;
    public DateTime EventDate { get; set; }
    [StringLength(100)]
    public string EventType { get; set; } = String.Empty;
    [StringLength(100)]
    public string SourceType { get; set; } = String.Empty;
    public string SourceNumber { get; set; } = String.Empty;
    public string AdditionalData { get; set; } = String.Empty;
    [StringLength(10)]
    public string ModuleName { get; set; } = String.Empty;
    public int? ResponseCode { get; set; }
    public int? TryCount { get; set; }
    public DateTime? LastTryTime { get; set; }
    public int DeliveryStatus { get; set; }
}