using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
using NpgsqlTypes;

namespace amorphie.consent.core.Model;

public class OBEventItem : EntityBase
{
    public Guid OBEventId { get; set; }
    public string EventNumber { get; set; }
    public DateTime EventDate { get; set; }
    public string EventType { get; set; }
    public string SourceType { get; set; }
    public string SourceNumber { get; set; }
    [ForeignKey("OBEventId")]
    public OBEvent OBEvent { get; set; }
}