using System.Data;
using FluentValidation;
using Projects_And_Tasks_Management_WebAPI.Requests.ProjectTaskRequests;

namespace Projects_And_Tasks_Management_WebAPI.Validators.ProjectTaskValidators;

public class UpdateProjectTaskRequestValidator : AbstractValidator<UpdateProjectTaskRequest>
{
    public UpdateProjectTaskRequestValidator()
    {
        RuleFor(x=> x.Name)
        .NotEmpty().WithMessage("Task name is required.")
        .Length(3,200).WithMessage("Task name must be 3 to 200 characters.")
        .When(x=> x.Name is not null);
        // يعني لو مديلها قيمة مهما كانت طبق عليها الكلام دا عشان لو مبعتش قيمة يبقا نسيب القيمة القديمة


        RuleFor(x=> x.Description)
        .MaximumLength(1000).WithMessage("Task description cannot exceed 1000 characters.");
        //هسمح انه يسيبه فاضي وفي السيرفس هتأكد انه مش نال ساعتها هحدث عادي واسمع بالمسافات

    }

}