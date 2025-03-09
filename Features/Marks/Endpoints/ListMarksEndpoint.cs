using FastEndpoints;
using UniversityEnrollmentSystem.Application.DTOs;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Features.Marks.Requests;
using UniversityEnrollmentSystem.Features.Marks.Responses;

namespace UniversityEnrollmentSystem.Features.Marks.Endpoints;

public class ListMarksEndpoint : Endpoint<ListMarksRequest, PaginatedResult<MarkResponse>>
{
    private readonly IMarkService _markService;
    private readonly IStudentService _studentService;
    private readonly IClassService _classService;

    public ListMarksEndpoint(
        IMarkService markService,
        IStudentService studentService,
        IClassService classService)
    {
        _markService = markService;
        _studentService = studentService;
        _classService = classService;
    }

    public override void Configure()
    {
        Get("/marks");
        AllowAnonymous();
        Description(d => d
            .Produces<PaginatedResult<MarkResponse>>(200)
            .WithTags("Marks"));
    }

    public override async Task HandleAsync(ListMarksRequest req, CancellationToken ct)
    {
        PaginatedResult<Domain.Entities.Mark> marks;
        
        if (req.StudentId.HasValue)
        {
            marks = await _markService.GetMarksByStudentIdAsync(
                req.StudentId.Value, 
                req.PageNumber, 
                req.PageSize);
        }
        else if (req.ClassId.HasValue)
        {
            marks = await _markService.GetMarksByClassIdAsync(
                req.ClassId.Value, 
                req.PageNumber, 
                req.PageSize);
        }
        else
        {
            // If no filters are provided, return an empty result
            // This would require adding a method to get all marks to the service
            await SendOkAsync(new PaginatedResult<MarkResponse>
            {
                Items = new List<MarkResponse>(),
                PageNumber = req.PageNumber,
                PageSize = req.PageSize,
                TotalCount = 0
            }, ct);
            return;
        }

        // Map to response objects
        var responseItems = new List<MarkResponse>();
        foreach (var mark in marks.Items)
        {
            var student = await _studentService.GetStudentByIdAsync(mark.StudentId, ct);
            var classEntity = await _classService.GetClassByIdAsync(mark.ClassId, ct);
            
            if (student != null && classEntity != null)
            {
                responseItems.Add(new MarkResponse
                {
                    Id = mark.Id,
                    StudentId = mark.StudentId,
                    ClassId = mark.ClassId,
                    ExamMark = mark.ExamMark,
                    AssignmentMark = mark.AssignmentMark,
                    TotalMark = mark.ExamMark + mark.AssignmentMark,
                    StudentName = $"{student.FirstName} {student.LastName}",
                    ClassName = classEntity.Name
                });
            }
        }

        var result = new PaginatedResult<MarkResponse>
        {
            Items = responseItems,
            TotalCount = marks.TotalCount,
            PageNumber = req.PageNumber,
            PageSize = req.PageSize
        };

        await SendOkAsync(result, ct);
    }
}
