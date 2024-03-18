namespace amorphie.consent.core.DTO.OpenBanking.Event;

public class EventApiResultDto
{
    public int? StatusCode { get; set; }
    public bool ContinueTry { get; set; } = true;
}