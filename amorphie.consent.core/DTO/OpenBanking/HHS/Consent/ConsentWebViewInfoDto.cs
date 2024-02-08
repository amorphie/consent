using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.core.Base;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class ConsentWebViewInfoDto
{
    public Guid RizaNo { get;set; }
    public string ConsentType { get; set; }
    public string ForwardingUrl { get; set; }
}

