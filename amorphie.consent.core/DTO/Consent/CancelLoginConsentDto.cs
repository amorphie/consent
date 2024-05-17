using System.ComponentModel.DataAnnotations;

namespace amorphie.consent.core.DTO.Consent;

public class CancelLoginConsentDto
{
    [Required]
    public string ClientCode { get; set; }
    [Required]
    public long UserTCKN { get; set; }
    [Required]
    public long ScopeTCKN { get; set; }
}