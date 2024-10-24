namespace cinema.api;

public class AuthenticationOptions
{
    public required string ValidIssuer { get; set; }
    public required string ValidAudience { get; set; }
    public required string IssuerSigningKey { get; set; }
}
