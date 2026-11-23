namespace Projects_And_Tasks_Management_WebAPI.Requests.ProjectRequests;

public class CreateProjectRequest
{

    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Budget { get; set; }
    public DateTime ExpectedStartDate { get; set; }
}