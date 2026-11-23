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

    public static ProjectResponse FromEntity(Project project)
    {
        return new ProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Budget = project.Budget,
            StartDate= project.ExpectedStartDate,
            ActualEndDate = project.ActualEndDate,
            CreatedAt = project.CreatedAt,
            OwnerId = project.OwnerId,
            Tasks = project.Tasks.Select(pt=> ProjectTaskResponse.FromEntity(pt)).ToList()
        };
    }
}