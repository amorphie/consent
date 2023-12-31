﻿namespace amorphie.consent.core.DTO.OpenBanking.YOS
{
    public class HesapBilgisiRizaIstegiDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public RizaBilgileriDto rzBlg { get; set; }
        public KimlikDto kmlk { get; set; }
        public KatilimciBilgisiDto katilimciBlg { get; set; }
        public GkdDto gkd { get; set; }
        public HesapBilgisiDto hspBlg { get; set; }
        public string? Description { get; set; }
        public string XGroupId { get; set; }
    }
}
