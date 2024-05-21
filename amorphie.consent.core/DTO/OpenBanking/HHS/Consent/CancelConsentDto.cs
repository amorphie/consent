using System.ComponentModel.DataAnnotations;
using amorphie.core.Base;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class CancelConsentDto
{
    public Guid ConsentId { get; set; }
    public string CancelDetailCode { get; set; }
}