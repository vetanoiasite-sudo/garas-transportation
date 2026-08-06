namespace GarasTransport.Domain.Helper;

/// <summary>Pagination result carried from repository to service (doc: Domain/Helper/PagedList.cs).</summary>
public class PagedList<T>
{
    public List<T> Items { get; set; } = new();
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)Math.Max(1, PageSize)));
}
