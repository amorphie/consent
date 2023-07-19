using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.core.Base;

namespace amorphie.consent.core.Model;

public class ConsentDTO : DtoBase
{   
    public Guid? ConsentDefinitionId { get; set; }
    public Guid UserId { get; set; }
    public int State { get; set; }
    public int ConsentType { get; set; }
    public string AdditionalData { get; set; }
}
