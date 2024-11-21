namespace cinema.api.Helpers.EmailSender;

public class TicketInfo
{
    public required string MovieName { get; set; }
    public required string Date { get; set; }
    public required string Time { get; set; }
    public required string SeatNumbers { get; set; }
    public required string WebsiteUrl { get; set; }
    public required string Code { get; set; }
}