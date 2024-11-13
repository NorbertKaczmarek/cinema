namespace cinema.api.Models;

public class PageResult<T>
{
    public List<T> Content { get; set; }
    public int TotalElements { get; set; }
    public int TotalPages { get; set; }

    public PageResult(List<T> content, int totalElements, int size)
    {
        this.Content = content;
        this.TotalElements = totalElements;
        this.TotalPages = (int)Math.Ceiling(totalElements / (double)size);
    }
}
