namespace amorphie.consent.core.DTO.Authorization;

public class SaveConsentForLoginDto
{
    public Guid RoleId { get; set; }
    public string ClientCode { get; set; }
    public long UserTCKN { get; set; }
    public string Scope { get; set; }
    
    public DateTime? LastValidAccessDate { get; set; }
}