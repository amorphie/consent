using amorphie.core.Base;

public class ConsentDefinitionDTO : DtoBase
{
    public string Name { get; set; }
    public string RoleAssignment { get; set; }
    public string[] Scope { get; set; }
    public string[] ClientId { get; set; }
}