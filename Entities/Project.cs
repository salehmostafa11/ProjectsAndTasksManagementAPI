namespace Projects_And_Tasks_Management_WebAPI.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Budget { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpectedStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public Guid OwnerId { get; set; }

    public ICollection<ProjectTask> Tasks { get; set; } = [];
}