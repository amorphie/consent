using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
using NpgsqlTypes;

namespace amorphie.consent.core.Model;

public class OBSystemEvent : EntityBase
{
    public string EventNumber { get; set; }
    public string HHSCode { get; set; }
    public string YOSCode { get; set; }
    public DateTime EventDate { get; set; }
    public string EventType { get; set; }
    public string SourceType { get; set; }
    public string SourceNumber { get; set; }
    public int? ResponseCode { get; set; }
    public int? TryCount { get; set; }
    public DateTime? LastTryTime { get; set; }
    public bool IsCompleted { get; set; }
    public string XRequestId { get; set; }
    public string ModuleName { get; set; }
}