using FastEndpoints;
using FluentValidation;
using UniversityEnrollmentSystem.Features.Students.Requests;

namespace UniversityEnrollmentSystem.Features.Students.Validators;

public class ListStudentsValidator : Validator<ListStudentsRequest>
{
    public ListStudentsValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100 items");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(50).WithMessage("Search term cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.SearchTerm));
    }
}
