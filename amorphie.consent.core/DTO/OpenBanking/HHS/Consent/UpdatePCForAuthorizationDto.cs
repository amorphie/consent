using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using amorphie.core.Base;

namespace amorphie.consent.core.DTO.OpenBanking.HHS
{
    public class UpdatePCForAuthorizationDto : DtoBase
    {
        public GonderenHesapDto? SenderAccount { get; set; }
    }
}
