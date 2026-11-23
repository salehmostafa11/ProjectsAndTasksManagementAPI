using Projects_And_Tasks_Management_WebAPI.Enums;

namespace Projects_And_Tasks_Management_WebAPI.Entities;
public class ProjectTask
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ProjectTaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid ProjectId { get; set; }
    public Guid AssignedUserId { get; set; }
    public Project Project { get; set; } = null!;
}