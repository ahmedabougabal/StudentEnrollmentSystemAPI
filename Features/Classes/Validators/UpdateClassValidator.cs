using FastEndpoints;
using FluentValidation;
using UniversityEnrollmentSystem.Features.Classes.Requests;

namespace UniversityEnrollmentSystem.Features.Classes.Validators;

public class UpdateClassValidator : Validator<UpdateClassRequest>
{
    public UpdateClassValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Class ID must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Class name is required")
            .MaximumLength(100).WithMessage("Class name must not exceed 100 characters");

        RuleFor(x => x.Teacher)
            .NotEmpty().WithMessage("Teacher name is required")
            .MaximumLength(100).WithMessage("Teacher name must not exceed 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters");
    }
}
