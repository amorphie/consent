using System.ComponentModel.DataAnnotations.Schema;
using amorphie.core.Base;
using NpgsqlTypes;

namespace amorphie.consent.core.Model;

public class OBEventSubscription : EntityBase
{
    public string HHSCode { get; set; }
    public string YOSCode { get; set; }
    public bool IsActive { get; set; }
    public string ModuleName { get; set; }
    public string XRequestId { get; set; }
    public virtual ICollection<OBEventSubscriptionType> OBEventSubscriptionTypes { get; set; }
}