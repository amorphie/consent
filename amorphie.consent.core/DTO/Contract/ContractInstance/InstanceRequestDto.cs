namespace amorphie.consent.core.DTO.Contract.ContractInstance;

public class InstanceRequestDto
{
    public InstanceRequestDto(string userTCKN)
    {
        contractName = "logindocs";
        process = new ProcessDto();
        reference = userTCKN;
        owner = userTCKN;
    }
    public string contractName { get; set; }
    public string reference { get; set; }
    public string owner { get; set; }
    public ProcessDto process { get; set; }
}
