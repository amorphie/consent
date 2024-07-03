using System.ComponentModel.DataAnnotations;
using amorphie.core.Base;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class UpdateConsentUserTcknDto : DtoBase
{
    [Required]
    public long UserTckn { get; set; }
}