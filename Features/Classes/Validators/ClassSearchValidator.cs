using FastEndpoints;
using FluentValidation;
using UniversityEnrollmentSystem.Features.Classes.Requests;

namespace UniversityEnrollmentSystem.Features.Classes.Validators;

public class ClassSearchValidator : Validator<ClassSearchRequest>
{
    public ClassSearchValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100");

        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Name search term must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Teacher)
            .MaximumLength(100).WithMessage("Teacher search term must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Teacher));
    }
}
