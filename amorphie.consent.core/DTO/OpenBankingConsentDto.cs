using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.core.Base;

namespace amorphie.consent.core.Model;

public class OpenBankingConsentDTO : DtoBase
{   
    public Guid? ConsentDefinitionId { get; set; }
    public Guid UserId { get; set; }
    public string State { get; set; }
    public string ConsentType { get; set; }
    public string AdditionalData { get; set; }
    public ConsentPermission ConsentPermission { get; set; }
    public List<Token>? Token { get; set; }
}
