using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace amorphie.consent.core.DTO.OpenBanking.HHS;
public class ListHesapBilgileriDto
{
    public int toplamHesapSayisi { get; set; }
    public List<HesapBilgileriDto>? hesapBilgileri { get; set; }
}
