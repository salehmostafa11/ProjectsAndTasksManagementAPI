using Microsoft.EntityFrameworkCore;
using Projects_And_Tasks_Management_WebAPI.Data;
using Projects_And_Tasks_Management_WebAPI.Entities;
using Projects_And_Tasks_Management_WebAPI.Enums;
using Projects_And_Tasks_Management_WebAPI.Exceptions;
using Projects_And_Tasks_Management_WebAPI.IServices;
using Projects_And_Tasks_Management_WebAPI.Requests;
using Projects_And_Tasks_Management_WebAPI.Requests.ProjectTaskRequests;
using Projects_And_Tasks_Management_WebAPI.Responses;
using Projects_And_Tasks_Management_WebAPI.Responses.Common;

namespace Projects_And_Tasks_Management_WebAPI.Services;

public class ProjectTaskService(AppDbContext _context) : IProjectTaskService
{
    public Task AssignTaskToUser(Guid taskId, Guid currentUserId, AssignProjectTaskToAssigneeRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ProjectTaskResponse> Create(Guid projectId, Guid currentUserId, CreateProjectTaskRequest request, CancellationToken ct = default)
    {
        var project = await _context.Projects.AsNoTracking()
                .Where(p=> p.Id == projectId)
                .Select(p=> new {
                    p.OwnerId,
                    p.ActualEndDate})
                    .FirstOrDefaultAsync(ct)
            ??throw new BussinessRuleException($"Project with id = \"{projectId}\" not found.", StatusCodes.Status404NotFound);

        if(project.OwnerId != currentUserId)
            throw new BussinessRuleException("Only the project owner can add tasks to the project.", StatusCodes.Status403Forbidden);
        
        if(project.ActualEndDate.HasValue)
            throw new BussinessRuleException("Can't add tasks to an ended project.", StatusCodes.Status409Conflict);
        
        
        // TODO(identity): verify that request.AssigneeId belongs to an existing user.

        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Status = ProjectTaskStatus.NotStarted,
            CreatedAt = DateTime.UtcNow,
            ProjectId = projectId,
            AssignedUserId = request.AssigneeId
        };

        //add async not important here, because i don't use value generators that based on DB
        _context.ProjectTasks.Add(task);
        await _context.SaveChangesAsync(ct);

        return ProjectTaskResponse.FromEntity(task);
    }

    public Task Delete(Guid taskId, Guid currentUserId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task Edit(Guid taskId, Guid currentUserId, UpdateProjectTaskRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ProjectTaskResponse> GetTaskById(Guid taskId, CancellationToken ct = default)
    {
        var task = await _context.ProjectTasks
                        .AsNoTracking()
                        .Where(pt=> pt.Id == taskId)
                        .Select(ProjectTaskResponse.Projection)
                        .SingleOrDefaultAsync(ct)
            ?? throw new BussinessRuleException($"Task with id = \"{taskId}\" not found.", StatusCodes.Status404NotFound);
        
        return task;
    }

    public async Task<PagedResponse<ProjectTaskResponse>> GetTasksByProjectId(Guid projectId, PaginationRequest request, CancellationToken ct = default)
    {
        if(!await _context.Projects.AnyAsync(p=> p.Id == projectId, ct))
            throw new BussinessRuleException($"Project with id = \"{projectId}\" not found.", StatusCodes.Status404NotFound);
        

        var query = _context.ProjectTasks
                .AsNoTracking()
                .Where(t=> t.ProjectId == projectId);
            
        var tasksCount = await query.CountAsync(ct);
        
        var tasks = await query
                    .OrderBy(t=> t.CreatedAt).ThenBy(t=> t.Id)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(ProjectTaskResponse.Projection)
                    .ToListAsync(ct);
                
        return new PagedResponse<ProjectTaskResponse>
        {
            Items = tasks,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = tasksCount
        };
    }

    public Task UpdateStatus(Guid taskId, Guid currentUserId, UpdateProjectTaskStatusRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}