using FastEndpoints;
using FluentValidation;
using UniversityEnrollmentSystem.Features.Marks.Requests;

namespace UniversityEnrollmentSystem.Features.Marks.Validators;

public class ListMarksValidator : Validator<ListMarksRequest>
{
    public ListMarksValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must be between 1 and 100");

        RuleFor(x => x.StudentId)
            .GreaterThan(0)
            .When(x => x.StudentId.HasValue)
            .WithMessage("Student ID must be greater than 0");

        RuleFor(x => x.ClassId)
            .GreaterThan(0)
            .When(x => x.ClassId.HasValue)
            .WithMessage("Class ID must be greater than 0");
    }
}
