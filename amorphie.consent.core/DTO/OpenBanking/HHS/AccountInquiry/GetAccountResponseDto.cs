using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace amorphie.consent.core.DTO.OpenBanking.HHS;
public class GetAccountResponseDto
{
    public AccountServiceErrorResponseDto? error { get; set; }
    public HesapBilgileriDto? data { get; set; }
}
