using FastEndpoints;
using FluentValidation;
using UniversityEnrollmentSystem.Features.Students.Requests;

namespace UniversityEnrollmentSystem.Features.Students.Validators;

public class UpdateStudentValidator : Validator<UpdateStudentRequest>
{
    public UpdateStudentValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid student ID");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

        RuleFor(x => x.Age)
            .GreaterThan(0).WithMessage("Age must be greater than 0")
            .LessThan(150).WithMessage("Age must be less than 150");
    }
}
