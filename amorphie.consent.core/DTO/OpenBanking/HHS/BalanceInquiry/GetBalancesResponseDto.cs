﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace amorphie.consent.core.DTO.OpenBanking.HHS;
public class GetBalancesResponseDto
{
    public AccountServiceErrorResponseDto? error { get; set; }
    public ListBakiyeBilgileriDto? data { get; set; }
}
