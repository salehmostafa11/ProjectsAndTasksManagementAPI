using System.Data;
using FluentValidation;
using Projects_And_Tasks_Management_WebAPI.Requests.ProjectTaskRequests;

namespace Projects_And_Tasks_Management_WebAPI.Validators.ProjectTaskValidators;

public class AssignProjectTaskToAssigneeRequestValidator : AbstractValidator<AssignProjectTaskToAssigneeRequest>
{
    public AssignProjectTaskToAssigneeRequestValidator()
    {
        RuleFor(x=> x.AssigneeId)
        .NotEmpty().WithMessage("Assignee id is required.");
    }

}