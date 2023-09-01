using amorphie.core.Base;

public class ConsentDataDto : DtoBase
{
    // Consent verileri
    public string ConsentType { get; set; }
    public string State { get; set; }
    public string AdditionalData { get; set; }

    // ConsentDefinition verileri
    public string ConsentDefinitionName { get; set; }
    public string RoleAssignment { get; set; }
    public string[] Scope { get; set; }
    public string[] ClientId { get; set; }

    // ConsentPermission verileri
    public string Permission { get; set; }
}