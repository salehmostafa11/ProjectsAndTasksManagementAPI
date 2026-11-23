using Microsoft.EntityFrameworkCore;
using Projects_And_Tasks_Management_WebAPI.Data;
using Projects_And_Tasks_Management_WebAPI.Entities;
using Projects_And_Tasks_Management_WebAPI.Enums;
using Projects_And_Tasks_Management_WebAPI.Exceptions;
using Projects_And_Tasks_Management_WebAPI.IServices;
using Projects_And_Tasks_Management_WebAPI.Requests.ProjectRequests;
using Projects_And_Tasks_Management_WebAPI.Requests;
using Projects_And_Tasks_Management_WebAPI.Responses;
using Projects_And_Tasks_Management_WebAPI.Responses.Common;

namespace Projects_And_Tasks_Management_WebAPI.Services;

public class ProjectService(AppDbContext _context) : IProjectService
{
    public async Task<ProjectResponse> Create(CreateProjectRequest request, Guid currentUserId, CancellationToken ct = default)
    {
        // The current user's ID is extracted from the authenticated user's claims
        // in the controller and passed to the service.
        var project = new Entities.Project
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Budget = request.Budget,
            CreatedAt = DateTime.UtcNow,
            ExpectedStartDate = request.ExpectedStartDate,
            OwnerId = currentUserId
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync(ct);

        return ProjectResponse.FromEntity(project);
    }

    public async Task Delete(Guid projectId, Guid currentUserId, CancellationToken ct = default)
    {
        var project = await _context.Projects.Include(p=> p.Tasks).FirstOrDefaultAsync(p=> p.Id == projectId, ct)
            ?? throw new BussinessRuleException($"Project with id = \"{projectId}\" not found.", StatusCodes.Status404NotFound);
        
        if (project.OwnerId != currentUserId)
            throw new BussinessRuleException("Only the project owner can delete the project.", StatusCodes.Status403Forbidden);
        
        if (project.ActualEndDate.HasValue)
            throw new BussinessRuleException("Can't delete an ended project.", StatusCodes.Status409Conflict);
        
        if(!project.Tasks.All(t=> t.Status == ProjectTaskStatus.Completed || t.Status == ProjectTaskStatus.Cancelled))
            throw new BussinessRuleException("Can't delete project with active tasks.", StatusCodes.Status409Conflict);
        
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync(ct);
    }

    public async Task Edit(Guid projectId, Guid currentUserId, UpdateProjectRequest request, CancellationToken ct = default)
    {
        var project = await _context.Projects.FindAsync(new object[]{projectId}, ct)
        ?? throw new BussinessRuleException($"Project with id = \"{projectId}\" not found.", StatusCodes.Status404NotFound);

        if (project.OwnerId != currentUserId)
            throw new BussinessRuleException("Only the project owner can update the project.", StatusCodes.Status403Forbidden);

        if (project.ActualEndDate.HasValue)
            throw new BussinessRuleException("Can't modify an ended project", StatusCodes.Status409Conflict);

        if (request.ExpectedStartDate.HasValue)
        {
            if (request.ExpectedStartDate.Value.Date < project.ExpectedStartDate.Date)
                throw new BussinessRuleException("Expected start date cannot be earlier than its current value.", StatusCodes.Status409Conflict);

            project.ExpectedStartDate = request.ExpectedStartDate.Value;
        }


        if (request.Name is not null)
            project.Name = request.Name;

        if (request.Description is not null)// allows white space" ",   ""
            project.Description = request.Description;

        await _context.SaveChangesAsync(ct);
    }

    public async Task EndProject(Guid projectId, Guid currentUserId, CancellationToken ct = default)
    {
        var project = await _context.Projects.Include(p=>p.Tasks).FirstOrDefaultAsync(p=> p.Id == projectId, ct)
        ?? throw new BussinessRuleException($"Project with id = \"{projectId}\" not found.", StatusCodes.Status404NotFound);

        if (project.OwnerId != currentUserId)
            throw new BussinessRuleException("Only the project owner can end the project.", StatusCodes.Status403Forbidden);

        if (project.ActualEndDate.HasValue)
            throw new BussinessRuleException("Can't end an ended project.", StatusCodes.Status409Conflict);
        
        if(!project.Tasks.Any())
            throw new BussinessRuleException("Can't end a project with no tasks.", StatusCodes.Status409Conflict);
        
        if(!project.Tasks.All(t=> t.Status is ProjectTaskStatus.Completed or ProjectTaskStatus.Cancelled))
            throw new BussinessRuleException("Can't end project with active tasks.", StatusCodes.Status409Conflict);
        
        project.ActualEndDate = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
    }

    public async Task<PagedResponse<ProjectResponse>> GetAllWithTasksPagedAsync(PaginationRequest request, CancellationToken ct = default)
    {
        var query = _context.Projects.AsNoTracking();
        var projectCount = await query.CountAsync(ct);

        /*using projection(Select) instead of (Include) to retrieve only the required columns from the database 
            rather than loading the complete entities into the memory
            -reduces data transfer and memory consumption
            */
        var projects = await query
                    .OrderBy(p => p.Id)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(p => new ProjectResponse
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Budget = p.Budget,
                        StartDate = p.ExpectedStartDate,
                        ActualEndDate = p.ActualEndDate,
                        CreatedAt = p.CreatedAt,
                        OwnerId = p.OwnerId,
                        Tasks = p.Tasks.Select(t => new ProjectTaskResponse
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Description = t.Description,
                            Status = t.Status,
                            CreatedAt = t.CreatedAt,
                            ProjectId = t.ProjectId,
                            AssignedUserId = t.AssignedUserId
                        })
                    }).ToListAsync(ct);

        return new PagedResponse<ProjectResponse>
        {
            Items = projects,
            TotalCount = projectCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public async Task<ProjectResponse> GetWithTasksById(Guid projectId, CancellationToken ct = default)
    {
        // This is a read-only query, so no tracking is needed.
        var project = await _context.Projects
                        .AsNoTracking()
                        .Where(p => p.Id == projectId)
                        .Select(p => new ProjectResponse
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            Budget = p.Budget,
                            StartDate = p.ExpectedStartDate,
                            ActualEndDate = p.ActualEndDate,
                            CreatedAt = p.CreatedAt,
                            OwnerId = p.OwnerId,
                            Tasks = p.Tasks.Select(t => new ProjectTaskResponse
                            {
                                Id = t.Id,
                                Name = t.Name,
                                Description = t.Description,
                                Status = t.Status,
                                CreatedAt = t.CreatedAt,
                                ProjectId = t.ProjectId,
                                AssignedUserId = t.AssignedUserId
                            }).ToList()
                        })
                        .SingleOrDefaultAsync(ct)
                        ?? throw new BussinessRuleException($"Project with id: \"{projectId}\" not found.", StatusCodes.Status404NotFound);

        return project;
    }

    public async Task UpdateBudget(Guid projectId, Guid currentUserId, UpdateProjectBudgetRequest request, CancellationToken ct = default)
    {
        var project = await _context.Projects.FindAsync(new object[]{projectId}, ct);

        if(project is null)
            throw new BussinessRuleException($"Project with id = \"{projectId}\" not found.", StatusCodes.Status404NotFound);
        
        if(currentUserId != project.OwnerId)
            throw new BussinessRuleException("Only the project owner can update the project.", StatusCodes.Status403Forbidden);
        
        if(project.ActualEndDate.HasValue)
            throw new BussinessRuleException("Can't modify an ended project", StatusCodes.Status409Conflict);
        
        project.Budget = request.Budget;

        await _context.SaveChangesAsync(ct);
    }
}