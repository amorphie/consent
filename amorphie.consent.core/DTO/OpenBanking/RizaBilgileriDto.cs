using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using amorphie.consent.core.Helper;
using Newtonsoft.Json;

namespace amorphie.consent.core.DTO.OpenBanking
{
    public class RizaBilgileriDto
    {
        public string rizaNo { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime olusZmn { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime gnclZmn { get; set; }
        public string rizaDrm { get; set; }
        public string rizaIptDtyKod { get; set; }
    }
}
