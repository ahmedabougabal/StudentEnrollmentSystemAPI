using FastEndpoints;
using FluentValidation;
using UniversityEnrollmentSystem.Features.Students.Endpoints;

namespace UniversityEnrollmentSystem.Features.Students.Validators;

// Create a simple request class for validation
public class GetStudentReportRequest
{
    public int StudentId { get; set; }
}

public class GetStudentReportValidator : Validator<GetStudentReportRequest>
{
    public GetStudentReportValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0)
            .WithMessage("Student ID must be greater than 0");
    }
}
