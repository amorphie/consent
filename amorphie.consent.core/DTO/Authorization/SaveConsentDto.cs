namespace amorphie.consent.core.DTO.Authorization;

public class SaveConsentDto
{
    public Guid RoleId { get; set; }
    public string ClientCode { get; set; }
    public long UserTCKN { get; set; }
    public string Scope { get; set; }
    public string ConsentType { get; set; }
    public DateTime? LastValidAccessDate { get; set; }
}