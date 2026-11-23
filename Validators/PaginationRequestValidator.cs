using System.Data;
using FluentValidation;
using Projects_And_Tasks_Management_WebAPI.Requests;

namespace Projects_And_Tasks_Management_WebAPI.Validators;

public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
{
    public PaginationRequestValidator()
    {
        RuleFor(p=> p.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page must be greater than or equal 1.");

        RuleFor(p=> p.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}