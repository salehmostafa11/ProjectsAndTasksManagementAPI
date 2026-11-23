using Projects_And_Tasks_Management_WebAPI.Requests.ProjectTaskRequests;
using Projects_And_Tasks_Management_WebAPI.Responses;
using Projects_And_Tasks_Management_WebAPI.Responses.Common;

namespace Projects_And_Tasks_Management_WebAPI.IServices;

public interface IProjectTaskService
{
    Task<PagedResponse<ProjectTaskResponse>> GetTasksByProjectId(Guid projectId, CancellationToken ct = default);
    Task<ProjectTaskResponse> GetById(Guid taskId, CancellationToken ct = default);
    Task Create(CreateProjectTaskRequest request, Guid currentUserId, CancellationToken ct = default);
    Task Delete(Guid taskId, Guid currentUserId, CancellationToken ct = default);
    Task Edit(UpdateProjectTaskRequest request, Guid currentUserId, CancellationToken ct= default);
    Task UpdateStatus (Guid taskId, Guid currentUserId, CancellationToken ct= default);
    Task AssignTaskToUser(Guid taskId, Guid currentUserId, Guid asigneeId, CancellationToken ct= default);
}