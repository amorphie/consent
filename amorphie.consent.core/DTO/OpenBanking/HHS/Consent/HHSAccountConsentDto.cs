using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.core.Base;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class HHSAccountConsentDto : HHSConsentDto
{
    public HesapBilgisiRizasiHHSDto AdditionalData { get; set; }
    public ICollection<OBAccountReferenceDto> AccountReferences { get; set; }
}
