using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.core.Base;

namespace amorphie.consent.core.DTO.OpenBanking.YOS;

public class YOSConsentDto : DtoBase
{
    public string AdditionalData { get; set; }
    public OpenBankingTokenDto? Token { get; set; }
    public string Description { get; set; }
    public string XGroupId { get; set; }
}
