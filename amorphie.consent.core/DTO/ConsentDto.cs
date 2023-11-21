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
    public Guid ClientId { get; set; }
    public string? Variant { get; set; }
    public string Description { get; set; }
    public string State { get; set; }
    public string ConsentType { get; set; }
    public string AdditionalData { get; set; }
}
