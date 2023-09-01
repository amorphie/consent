using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.core.Base;

namespace amorphie.consent.core.Model;

public class HhsConsentDto : DtoBase
{
    public string AdditionalData { get; set; }
    public List<TokenModel> Token { get; set; }
    public string description { get; set; }
    public string xGroupId { get; set; }
}
