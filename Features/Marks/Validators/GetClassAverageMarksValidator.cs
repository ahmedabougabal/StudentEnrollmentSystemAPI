using FastEndpoints;
using FluentValidation;
using UniversityEnrollmentSystem.Features.Marks.Endpoints;

namespace UniversityEnrollmentSystem.Features.Marks.Validators;

// Create a simple request class for validation
public class GetClassAverageMarksRequest
{
    public int ClassId { get; set; }
}

public class GetClassAverageMarksValidator : Validator<GetClassAverageMarksRequest>
{
    public GetClassAverageMarksValidator()
    {
        RuleFor(x => x.ClassId)
            .GreaterThan(0)
            .WithMessage("Class ID must be greater than 0");
    }
}
