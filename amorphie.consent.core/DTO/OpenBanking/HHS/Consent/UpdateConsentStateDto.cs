using System.ComponentModel.DataAnnotations;
using amorphie.core.Base;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class UpdateConsentStateDto : DtoBase
{
    [Required]
    public string State { get; set; }
}