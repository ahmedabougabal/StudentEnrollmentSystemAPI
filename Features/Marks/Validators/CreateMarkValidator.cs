using FastEndpoints;
using FluentValidation;
using UniversityEnrollmentSystem.Features.Marks.Requests;

namespace UniversityEnrollmentSystem.Features.Marks.Validators;

public class CreateMarkValidator : Validator<CreateMarkRequest>
{
    public CreateMarkValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0)
            .WithMessage("Student ID must be greater than 0");

        RuleFor(x => x.ClassId)
            .GreaterThan(0)
            .WithMessage("Class ID must be greater than 0");

        RuleFor(x => x.ExamMark)
            .InclusiveBetween(0, 100)
            .WithMessage("Exam mark must be between 0 and 100");

        RuleFor(x => x.AssignmentMark)
            .InclusiveBetween(0, 100)
            .WithMessage("Assignment mark must be between 0 and 100");
    }
}
