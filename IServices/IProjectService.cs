using Projects_And_Tasks_Management_WebAPI.Requests.ProjectTaskRequests;
using Projects_And_Tasks_Management_WebAPI.Requests.ProjectRequests;
using Projects_And_Tasks_Management_WebAPI.Requests;
using Projects_And_Tasks_Management_WebAPI.Responses;
using Projects_And_Tasks_Management_WebAPI.Responses.Common;

namespace Projects_And_Tasks_Management_WebAPI.IServices;

public interface IProjectService
{
    Task<PagedResponse<ProjectResponse>> GetAllWithTasksPagedAsync(PaginationRequest request, CancellationToken ct = default);
    Task<ProjectResponse> GetWithTasksById(Guid projectId, CancellationToken ct = default);
    Task<ProjectResponse> Create (CreateProjectRequest request, Guid currentUserId, CancellationToken ct = default);
    Task Delete (Guid projectId, Guid currentUserId, CancellationToken ct = default);
    Task Edit(Guid projectId, Guid currentUserId, UpdateProjectRequest request, CancellationToken ct = default);
    Task UpdateBudget(Guid projectId, Guid currentUserId, UpdateProjectBudgetRequest request, CancellationToken ct = default);
    Task EndProject(Guid projectId, Guid currentUserId, CancellationToken ct = default);
}