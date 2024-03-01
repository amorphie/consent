using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
using NpgsqlTypes;

namespace amorphie.consent.core.Model;

public class OBEvent : EntityBase
{
    [StringLength(10)]
    public string HHSCode { get; set; }
    [StringLength(10)]
    public string YOSCode { get; set; }
    public string EventNumber { get; set; }
    public DateTime EventDate { get; set; }
    [StringLength(100)]
    public string EventType { get; set; }
    [StringLength(100)]
    public string SourceType { get; set; }
    public string SourceNumber { get; set; }
    public string AdditionalData { get; set; }
    [StringLength(10)]
    public string ModuleName { get; set; }
    public int? ResponseCode { get; set; }
    public int? TryCount { get; set; }
    public DateTime? LastTryTime { get; set; }
    public int DeliveryStatus { get; set; }
}