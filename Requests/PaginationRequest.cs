namespace Projects_And_Tasks_Management_WebAPI.Requests;

public class PaginationRequest
{
    public int Page { get; set; } = 1;
    public int PageSize {get; set;} = 10;
}