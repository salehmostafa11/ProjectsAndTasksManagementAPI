using FluentValidation;
using Projects_And_Tasks_Management_WebAPI.Requests.ProjectRequests;

namespace Projects_And_Tasks_Management_WebAPI.Validators.ProjectValidators;

public class UpdateProjectBudgetRequestValidator : AbstractValidator<UpdateProjectBudgetRequest>
{
    public UpdateProjectBudgetRequestValidator()
    {
        
        RuleFor(x=>x.Budget)
            .NotNull().WithMessage("Project budget cannot be null.")
            .GreaterThan(0).WithMessage("Project budget must be greater than 0.");
    }
}