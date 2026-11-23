namespace Projects_And_Tasks_Management_WebAPI.Requests.ProjectTaskRequests;

public class CreateProjectTaskRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid AsigneeId { get; set; }
}