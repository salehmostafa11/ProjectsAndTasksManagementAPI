using Projects_And_Tasks_Management_WebAPI.Enums;

namespace Projects_And_Tasks_Management_WebAPI.Requests.ProjectTask;

public class UpdateProjectTaskStatusRequest
{
    public ProjectTaskStatus Status {get; set;}
}