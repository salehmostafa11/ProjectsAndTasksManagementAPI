using Microsoft.EntityFrameworkCore;
using Projects_And_Tasks_Management_WebAPI.Data;
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

    public Task Create(Guid projectId, Guid currentUserId, CreateProjectTaskRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid taskId, Guid currentUserId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task Edit(Guid taskId, Guid currentUserId, UpdateProjectTaskRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectTaskResponse> GetTaskById(Guid taskId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
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
                    .Skip((request.Page -1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(t=> new ProjectTaskResponse
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description,
                        Status = t.Status,
                        CreatedAt = t.CreatedAt,
                        ProjectId = t.ProjectId,
                        AssignedUserId = t.AssignedUserId
                    }).ToListAsync(ct);
                
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