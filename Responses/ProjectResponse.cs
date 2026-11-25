using System.Linq.Expressions;
using Projects_And_Tasks_Management_WebAPI.Entities;

namespace Projects_And_Tasks_Management_WebAPI.Responses;

public  class ProjectResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Budget { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid OwnerId { get; set; }
    public IEnumerable<ProjectTaskResponse> Tasks { get; set; } = [];

    public static readonly Expression<Func<Project, ProjectResponse>> Projection = p => new ProjectResponse
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Budget = p.Budget,
        StartDate= p.ExpectedStartDate,
        ActualEndDate = p.ActualEndDate,
        CreatedAt = p.CreatedAt,
        OwnerId = p.OwnerId,
        Tasks = p.Tasks.AsQueryable().Select(ProjectTaskResponse.Projection).ToList()
    };
    private static readonly Func<Project, ProjectResponse> _compiledProjection = Projection.Compile();
    public static ProjectResponse FromEntity(Project project) => _compiledProjection(project);

}