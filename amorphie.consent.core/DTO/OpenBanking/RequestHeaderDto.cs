namespace amorphie.consent.core.DTO.OpenBanking
{
    public class RequestHeaderDto
    {
        public string ContentType { get; set; }
        public string XRequestID { get; set; }
        public string XGroupID { get; set; }
        public string XASPSPCode { get; set; }
        public string XTPPCode { get; set; }
        public string PSUInitiated { get; set; }

    }

}