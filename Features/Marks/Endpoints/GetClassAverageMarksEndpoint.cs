using FastEndpoints;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Features.Marks.Responses;
using UniversityEnrollmentSystem.Features.Marks.Validators;

namespace UniversityEnrollmentSystem.Features.Marks.Endpoints;

public class GetClassAverageMarksEndpoint : Endpoint<GetClassAverageMarksRequest, ClassAverageMarksResponse>
{
    private readonly IMarkService _markService;
    private readonly IClassService _classService;

    public GetClassAverageMarksEndpoint(
        IMarkService markService,
        IClassService classService)
    {
        _markService = markService;
        _classService = classService;
    }

    public override void Configure()
    {
        Get("/classes/{ClassId}/average-marks");
        AllowAnonymous();
        Description(d => d
            .Produces<ClassAverageMarksResponse>(200)
            .Produces(404)
            .WithTags("Marks"));
    }

    public override async Task HandleAsync(GetClassAverageMarksRequest req, CancellationToken ct)
    {
        // Validate class exists
        var classEntity = await _classService.GetClassByIdAsync(req.ClassId, ct);
        if (classEntity == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        // Get all marks for the class
        var marks = await _markService.GetMarksByClassIdAsync(req.ClassId);
        
        if (!marks.Items.Any())
        {
            await SendOkAsync(new ClassAverageMarksResponse
            {
                ClassId = req.ClassId,
                ClassName = classEntity.Name,
                AverageExamMark = 0,
                AverageAssignmentMark = 0,
                AverageTotalMark = 0,
                StudentCount = 0
            }, ct);
            return;
        }

        // Calculate averages
        var averageExamMark = marks.Items.Average(m => m.ExamMark);
        var averageAssignmentMark = marks.Items.Average(m => m.AssignmentMark);
        var averageTotalMark = marks.Items.Average(m => m.ExamMark + m.AssignmentMark);

        var response = new ClassAverageMarksResponse
        {
            ClassId = req.ClassId,
            ClassName = classEntity.Name,
            AverageExamMark = Math.Round(averageExamMark, 2),
            AverageAssignmentMark = Math.Round(averageAssignmentMark, 2),
            AverageTotalMark = Math.Round(averageTotalMark, 2),
            StudentCount = marks.Items.Count()
        };

        await SendOkAsync(response, ct);
    }
}
