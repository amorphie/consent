namespace amorphie.consent.core.DTO.Contract;

public class ContractDocumentDto
{
    public string FileType { get; set; }
    public string FileContextType { get; set; }
    public string FileName { get; set; }
    public string DocumentCode { get; set; }
    public string DocumentVersion { get; set; }
    public string Reference { get; set; }
    public string Owner { get; set; }
    public string FileContext { get; set; } = string.Empty;
    public string FilePath { get; set; }
}