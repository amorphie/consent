namespace amorphie.consent.core.DTO.Contract;

public class ContractResponseDto
{
    public bool IsAuthorized { get; set; } = true;
    public ICollection<ContractDocumentDto> ContractDocuments { get; set; }
}