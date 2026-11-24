namespace Projects_And_Tasks_Management_WebAPI.Responses.Common;

public class PagedResponse<T> where T : class
{
    public IEnumerable<T> Items {get; set;} = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;
}