using amorphie.core.Base;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class SaveAccountReferenceDto: DtoBase
{
    public List<string> AccountReferences { get; set; }
}