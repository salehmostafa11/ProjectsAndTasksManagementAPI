using Projects_And_Tasks_Management_WebAPI.Requests;
using Projects_And_Tasks_Management_WebAPI.Requests.ProjectTaskRequests;
using Projects_And_Tasks_Management_WebAPI.Responses;
using Projects_And_Tasks_Management_WebAPI.Responses.Common;

namespace Projects_And_Tasks_Management_WebAPI.IServices;

public interface IProjectTaskService
{
    Task<PagedResponse<ProjectTaskResponse>> GetTasksByProjectId(Guid projectId, PaginationRequest request, CancellationToken ct = default);
    Task<ProjectTaskResponse> GetTaskById(Guid taskId, CancellationToken ct = default);
    Task<ProjectTaskResponse> Create(Guid projectId, Guid currentUserId, CreateProjectTaskRequest request, CancellationToken ct = default);
    Task Delete(Guid taskId, Guid currentUserId, CancellationToken ct = default);
    Task Edit(Guid taskId, Guid currentUserId, UpdateProjectTaskRequest request, CancellationToken ct = default);
    Task UpdateStatus (Guid taskId, Guid currentUserId, UpdateProjectTaskStatusRequest request, CancellationToken ct = default);
    Task AssignTaskToUser(Guid taskId, Guid currentUserId, AssignProjectTaskToAssigneeRequest request, CancellationToken ct = default);
}