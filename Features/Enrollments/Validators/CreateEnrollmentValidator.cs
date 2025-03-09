using FastEndpoints;
using FluentValidation;
using UniversityEnrollmentSystem.Features.Enrollments.Requests;

namespace UniversityEnrollmentSystem.Features.Enrollments.Validators;

public class CreateEnrollmentValidator : Validator<CreateEnrollmentRequest>
{
    public CreateEnrollmentValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0)
            .WithMessage("Student ID must be greater than 0");

        RuleFor(x => x.ClassId)
            .GreaterThan(0)
            .WithMessage("Class ID must be greater than 0");
    }
}
