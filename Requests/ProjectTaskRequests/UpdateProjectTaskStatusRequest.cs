using Projects_And_Tasks_Management_WebAPI.Enums;

namespace Projects_And_Tasks_Management_WebAPI.Requests.ProjectTaskRequests;

public class UpdateProjectTaskStatusRequest
{
    public ProjectTaskStatus? Status {get; set;}
}