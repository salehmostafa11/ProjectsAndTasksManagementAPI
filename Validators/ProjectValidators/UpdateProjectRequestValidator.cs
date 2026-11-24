using FluentValidation;
using Projects_And_Tasks_Management_WebAPI.Requests.ProjectRequests;

namespace Projects_And_Tasks_Management_WebAPI.Validators.ProjectValidators;

public class UpdateProjectRequestValidator : AbstractValidator<UpdateProjectRequest>
{
    public UpdateProjectRequestValidator()
    {
        RuleFor(x=> x.Name)
        .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Empty values not allowed.")
            .Length(3, 200).WithMessage("Project name must be between 3 and 200 characters.")
            .When(x=> x.Name is not null);
            // يعني لو مش باعت قيمة هعديها لان هعتبره مش عاوز يعدل ودي مهندلها في السيرفس
            // انما لو باعت قيمة لازم تكون قيمة فعلية مش مسافه او سترينج فاضي

        RuleFor(x=> x.Description)
            .MaximumLength(1000).WithMessage("Project descriptions can't exceed 1000 characters.");


        // will check on it on the service because we need a data from the database
        // RuleFor(x=>x.ExpectedStartDate)
        //     .Must(GraterThanorEqualToCreationDate).WithMessage("Start date must be today or day in the future.");
    }

}