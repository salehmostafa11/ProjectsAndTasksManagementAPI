using FluentValidation;
using Projects_And_Tasks_Management_WebAPI.Requests.ProjectRequests;

namespace Projects_And_Tasks_Management_WebAPI.Validators.ProjectValidators;

public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectRequestValidator()
    {
        RuleFor(x=> x.Name)
            .NotEmpty().WithMessage("Project name can't be empty.")
            .Length(3, 200).WithMessage("Project name must be between 3 and 200 characters.");

        RuleFor(x=> x.Description)
            .MaximumLength(1000).WithMessage("Project descriptions can't exceed 1000 characters.");
        
        RuleFor(x=> x.Budget) // using greater than and not empty is a redunduncy of the same thing because the default value of empty decimal is zero.
            .GreaterThan(0).WithMessage("Project budget must be greater than 0.");

        RuleFor(x=>x.ExpectedStartDate)
            .NotEmpty().WithMessage("Project start date is required.")
            .Must(GraterThanorEqualToTodatay).WithMessage("Start date must be today or day in the future.");
    }

    private bool GraterThanorEqualToTodatay(DateTime date)
    {
        return date.Date >= DateTime.Today;
    }
}