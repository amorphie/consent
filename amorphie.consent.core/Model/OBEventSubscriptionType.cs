using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
using NpgsqlTypes;

namespace amorphie.consent.core.Model;

public class OBEventSubscriptionType : EntityBase
{
    public Guid OBEventSubscriptionId { get; set; }
    public string EventType { get; set; }
    public string SourceType { get; set; }
    [ForeignKey("OBEventSubscriptionId")]
    public OBEventSubscription OBEventSubscription { get; set; }
}