using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.core.Base;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class HHSPaymentConsentDto : HHSConsentDto
{
    public OdemeEmriRizasiHHSDto AdditionalData { get; set; }
}
