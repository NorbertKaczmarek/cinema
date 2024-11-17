namespace cinema.api.Models;

public class PageQuery
{
    public int Page { get; set; }
    public int Size { get; set; }
    public string? Phrase { get; set; }
}
