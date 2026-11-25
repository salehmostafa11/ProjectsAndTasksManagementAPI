using System.Linq.Expressions;
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

    public static readonly Expression<Func<ProjectTask, ProjectTaskResponse>> Projection = pt => new ProjectTaskResponse
    {
        Id = pt.Id,
        Name = pt.Name,
        Description = pt.Description,
        Status = pt.Status,
        CreatedAt = pt.CreatedAt,
        ProjectId = pt.ProjectId,
        AssignedUserId = pt.AssignedUserId
    };

    private static readonly Func<ProjectTask, ProjectTaskResponse> _compiledProjection = Projection.Compile();
    public static ProjectTaskResponse FromEntity(ProjectTask task) => _compiledProjection(task);
}