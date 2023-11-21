using System.Text.Json.Serialization;
using amorphie.core.Base;

namespace amorphie.consent.core.DTO.OpenBanking;
public class OpenBankingTokenDto : EntityBaseWithOutId
{
    //TODO:Ekip DTOBaseWithOutId class could not be used. The codes inside is commented.
    //If it is open, that class will be used.
    public Guid Id { get; set; }
    public Guid ConsentId { get; set; }
    public string erisimBelirteci { get; set; }
    public int gecerlilikSuresi { get; set; }
    public string yenilemeBelirteci { get; set; }
    public int yenilemeBelirteciGecerlilikSuresi { get; set; }
}