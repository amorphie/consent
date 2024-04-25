using amorphie.core.Base;

namespace amorphie.consent.core.Model;

public class OBSystemEvent : EntityBase
{
    public string EventNumber { get; set; } = String.Empty;
    public string HHSCode { get; set; } = String.Empty;
    public string YOSCode { get; set; } = String.Empty;
    public DateTime EventDate { get; set; }
    public string EventType { get; set; } = String.Empty;
    public string SourceType { get; set; } = String.Empty;
    public string SourceNumber { get; set; } = String.Empty;
    public int? ResponseCode { get; set; }
    public int? TryCount { get; set; }
    public DateTime? LastTryTime { get; set; }
    public bool IsCompleted { get; set; }
    public string XRequestId { get; set; } = String.Empty;
    public string ModuleName { get; set; } = String.Empty;
}