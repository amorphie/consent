using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.core.Base;

namespace amorphie.consent.core.DTO;

public class ConsentDto : DtoBase
{
    public Guid? UserId { get; set; }
    public Guid? ScopeId { get; set; }
    public Guid? RoleId { get; set; }
    public string ClientCode { get; set; }
    public long? UserTCKN { get; set; }
    public string? Scope { get; set; }
    public string? Variant { get; set; }
    public string Description { get; set; }
    public string State { get; set; }
    public DateTime StateModifiedAt { get; set; }
    public string? StateCancelDetailCode { get; set; }
    public DateTime? LastValidAccessDate { get; set; }
    public string ConsentType { get; set; }
    public string AdditionalData { get; set; }
}
