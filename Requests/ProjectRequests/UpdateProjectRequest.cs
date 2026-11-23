namespace Projects_And_Tasks_Management_WebAPI.Requests.ProjectRequests;

public class UpdateProjectRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? ExpectedStartDate { get; set; }
}