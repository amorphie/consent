public class Token:EntityBase
{
    [ForeignKey("Consent")]
    public Guid ConsentId { get; set; }
    public string Token { get; set; }
    public int TokenType { get; set; }
    public int ExpireTime { get; set; }
    public Consent Consent { get; set; }
}