using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
using NpgsqlTypes;

namespace amorphie.consent.core.Model;

public class OBEventTypeSourceTypeRelation : EntityBase
{

    public string EventType { get; set; }
    public string YOSRole { get; set; }
    public string SourceType { get; set; }
    public string EventCase { get; set; }
    public string SourceNumber { get; set; }
    public string APIToGetData { get; set; }
    public string EventNotificationReporter { get; set; }
    public string EventNotificationTime { get; set; }
    public string? RetryPolicy { get; set; }
    public int? RetryInMinute { get; set; }
    public int? RetryCount { get; set; }

}