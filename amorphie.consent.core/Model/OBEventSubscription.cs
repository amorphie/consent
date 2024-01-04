using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
using NpgsqlTypes;

namespace amorphie.consent.core.Model;

public class OBEventSubscription : EntityBase
{
    public Guid SubscriptionNumber { get; set; }
    public string HHSCode { get; set; }
    public string YOSCode { get; set; }
    public string[,] SubsriptionTypes { get; set; }
    public bool IsActive { get; set; }
}