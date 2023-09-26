using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking.HHS
{
    public class UpdatePCForAuthorizationDto
    {
        public Guid Id { get; set; }
        public string State { get; set; }
        public GonderenHesapDto? SenderAccount { get; set; }
    }
}
