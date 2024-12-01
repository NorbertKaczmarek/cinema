namespace cinema.api;

public class EmailOptions
{
    public required string Email { get; set; }
    public required string DisplayName { get; set; }
    public required string AppPassword { get; set; }
    public required string SmtpClientHost { get; set; }
    public int SmtpClientPort { get; set; }
    public required string WebsiteUrl { get; set; }
}
