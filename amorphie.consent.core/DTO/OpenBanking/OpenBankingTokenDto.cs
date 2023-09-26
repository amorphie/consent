using System.Text.Json.Serialization;
using amorphie.core.Base;

namespace amorphie.consent.core.DTO.OpenBanking
{
    public class OpenBankingTokenDto : EntityBaseWithOutId
    {
        public Guid Id { get; set; }
        public Guid ConsentId { get; set; }
        public string erisimBelirteci { get; set; }
        public int gecerlilikSuresi { get; set; }
        public string yenilemeBelirteci { get; set; }
        public int yenilemeBelirteciGecerlilikSuresi { get; set; }
    }
}
