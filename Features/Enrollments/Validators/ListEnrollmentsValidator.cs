using FastEndpoints;
using FluentValidation;
using UniversityEnrollmentSystem.Features.Enrollments.Requests;

namespace UniversityEnrollmentSystem.Features.Enrollments.Validators;

public class ListEnrollmentsValidator : Validator<ListEnrollmentsRequest>
{
    public ListEnrollmentsValidator()
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
