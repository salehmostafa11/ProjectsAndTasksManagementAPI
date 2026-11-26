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
    public async Task AssignTaskToUser(Guid taskId, Guid currentUserId, AssignProjectTaskToAssigneeRequest request, CancellationToken ct = default)
    {
        var data = await _context.ProjectTasks
                .Where(pt=> pt.Id == taskId)
                .Select(pt=> new{Task = pt, pt.Project.ActualEndDate, pt.Project.OwnerId })
                .FirstOrDefaultAsync(ct)
                ?? throw new BussinessRuleException($"Task with id = \"{taskId}\" not found.", StatusCodes.Status404NotFound);

        if(currentUserId != data.OwnerId)
            throw new BussinessRuleException("Only the project owner can assign tasks to users.", StatusCodes.Status403Forbidden);
        
        if(data.ActualEndDate.HasValue)
            throw new BussinessRuleException("Can't assign ended project task to any user.", StatusCodes.Status409Conflict);
        
        if(data.Task.Status is ProjectTaskStatus.Completed or ProjectTaskStatus.Cancelled)
            throw new BussinessRuleException("Can't assign a task to user while it is completed or cancelled.", StatusCodes.Status409Conflict);
        
        //TODO(identity): verify that request.AssigneeId belongs to an existing user.
        data.Task.AssignedUserId = request.AssigneeId;
        await _context.SaveChangesAsync(ct);
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

    public async Task Delete(Guid taskId, Guid currentUserId, CancellationToken ct = default)
    {
        var data = await _context.ProjectTasks
                .Where(pt=> pt.Id == taskId)
                .Select(pt=> new{Task = pt, pt.Project.ActualEndDate, pt.Project.OwnerId })
                .FirstOrDefaultAsync(ct)
                ?? throw new BussinessRuleException($"Task with id = \"{taskId}\" not found.", StatusCodes.Status404NotFound);

        if(currentUserId != data.OwnerId)
            throw new BussinessRuleException("Only the project owner can delete the tasks.", StatusCodes.Status403Forbidden);
        
        if(data.ActualEndDate.HasValue)
            throw new BussinessRuleException("Can't delete ended project task.", StatusCodes.Status409Conflict);
        
        if(data.Task.Status is not ( ProjectTaskStatus.Completed or ProjectTaskStatus.Cancelled))
            throw new BussinessRuleException("Can't delete a task unless it is completed or cancelled.", StatusCodes.Status409Conflict);
        
        _context.ProjectTasks.Remove(data.Task);
        await _context.SaveChangesAsync(ct);
    }

    public async Task Edit(Guid taskId, Guid currentUserId, UpdateProjectTaskRequest request, CancellationToken ct = default)
    {
        var data = await _context.ProjectTasks
                .Where(pt=> pt.Id == taskId)
                .Select(t=> new {Task =t, t.Project.OwnerId, t.Project.ActualEndDate})
                .FirstOrDefaultAsync(ct)
                ?? throw new BussinessRuleException($"Task with id = \"{taskId}\" not found.", StatusCodes.Status404NotFound);
        
        if(data.OwnerId != currentUserId)
            throw new BussinessRuleException("Only the project owner can edit the tasks.", StatusCodes.Status403Forbidden);
        
        if(data.ActualEndDate.HasValue)
            throw new BussinessRuleException("Can't edit ended project tasks.", StatusCodes.Status409Conflict);
        
        if(data.Task.Status is ProjectTaskStatus.Cancelled or ProjectTaskStatus.Completed)
            throw new BussinessRuleException("Can't edit completed or cancelled tasks.", StatusCodes.Status409Conflict);
        
        if(request.Name is not null)
            data.Task.Name = request.Name;
        
        if(request.Description is not null)
            data.Task.Description = request.Description;
        
        await _context.SaveChangesAsync(ct);
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

    public async Task UpdateStatus(Guid taskId, Guid currentUserId, UpdateProjectTaskStatusRequest request, CancellationToken ct = default)
    {
        var data = await _context.ProjectTasks
                    .Where(pt=> pt.Id == taskId)
                    .Select(pt => new {Task = pt, pt.Project.OwnerId, pt.Project.ActualEndDate})
                    .FirstOrDefaultAsync(ct)
                    ?? throw new BussinessRuleException($"Task with id = \"{taskId}\" not found.", StatusCodes.Status404NotFound);
        
        if(data.OwnerId != currentUserId && data.Task.AssignedUserId != currentUserId)
            throw new BussinessRuleException("Only the project owner or assigned user can update the task status.", StatusCodes.Status403Forbidden);
        
        if(data.ActualEndDate.HasValue)
            throw new BussinessRuleException("Can't update ended project task.", StatusCodes.Status409Conflict);
        
        if(data.Task.Status is ProjectTaskStatus.Cancelled or ProjectTaskStatus.Completed)
            throw new BussinessRuleException("Can't update completed or cancelled task status.", StatusCodes.Status409Conflict);
        
        data.Task.Status = request.Status!.Value;
        await _context.SaveChangesAsync(ct);
    }
}