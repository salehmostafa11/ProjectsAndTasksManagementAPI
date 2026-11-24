using System.Data;
using FluentValidation;
using Projects_And_Tasks_Management_WebAPI.Requests.ProjectTaskRequests;

namespace Projects_And_Tasks_Management_WebAPI.Validators.ProjectTaskValidators;

public class UpdateProjectTaskStatusRequestValidator : AbstractValidator<UpdateProjectTaskStatusRequest>
{
    public UpdateProjectTaskStatusRequestValidator()
    {
        RuleFor(x=> x.Status)
        .Cascade(CascadeMode.Stop)
        .NotNull().WithMessage("Project Status is required.")
        .IsInEnum().WithMessage("Invalid status value.");
    }
}