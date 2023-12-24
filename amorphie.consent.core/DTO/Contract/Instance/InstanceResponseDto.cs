using Newtonsoft.Json;

namespace amorphie.consent.core.DTO.Contract;

public class InstanceResponseDto
{
    public string id { get; set; }
    public string status { get; set; }
    public IList<DocumentInfoDto> document { get; set; }
}



