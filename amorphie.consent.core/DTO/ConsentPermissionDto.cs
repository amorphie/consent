using amorphie.core.Base;

public class ConsentPermissionDto : DtoBase
{
    public Guid ConsentId { get; set; }
    public string Permission { get; set; }
}