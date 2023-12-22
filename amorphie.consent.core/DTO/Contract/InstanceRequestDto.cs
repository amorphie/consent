namespace amorphie.consent.core.DTO.Contract;

public class InstanceRequestDto
{
    public InstanceRequestDto()
    {
        contractName = "logindocs";
        process = new ProcessDto();
    }
    public string contractName { get; set; }
    public string reference { get; set; }
    public string owner { get; set; }
    public ProcessDto process { get; set; }
}
