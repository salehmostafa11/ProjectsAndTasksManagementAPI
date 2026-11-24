using System.Data;
using FluentValidation;
using Projects_And_Tasks_Management_WebAPI.Requests.ProjectTaskRequests;

namespace Projects_And_Tasks_Management_WebAPI.Validators.ProjectTaskValidators;

public class CreateProjectTaskRequestValidator : AbstractValidator<CreateProjectTaskRequest>
{
    public CreateProjectTaskRequestValidator()
    {
        RuleFor(x=> x.Name).Cascade(CascadeMode.Stop)
        .NotEmpty().WithMessage("Task name is required.")
        .Length(3,200).WithMessage("Task name must be 3 to 200 characters.");

        RuleFor(x=> x.Description)
        .MaximumLength(1000).WithMessage("Task description cannot exceed 1000 characters.");

        RuleFor(x=> x.AssigneeId)
        .NotEmpty().WithMessage("Assignee id is required.");
    }

}