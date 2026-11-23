using Projects_And_Tasks_Management_WebAPI.Entities;
using Projects_And_Tasks_Management_WebAPI.Enums;

namespace Projects_And_Tasks_Management_WebAPI.Responses;

public class ProjectTaskResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ProjectTaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid ProjectId { get; set; }
    public Guid AssignedUserId { get; set; }

    public static ProjectTaskResponse FromEntity(ProjectTask task)
    {
        return new ProjectTaskResponse
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            Status = task.Status,
            CreatedAt = task.CreatedAt,
            ProjectId = task.ProjectId,
            AssignedUserId = task.AssignedUserId
        };
    }
}