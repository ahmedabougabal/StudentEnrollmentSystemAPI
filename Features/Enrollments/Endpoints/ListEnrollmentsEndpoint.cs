using FastEndpoints;
using UniversityEnrollmentSystem.Application.DTOs;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Features.Enrollments.Requests;
using UniversityEnrollmentSystem.Features.Enrollments.Responses;

namespace UniversityEnrollmentSystem.Features.Enrollments.Endpoints;

public class ListEnrollmentsEndpoint : Endpoint<ListEnrollmentsRequest, PaginatedResult<EnrollmentResponse>>
{
    private readonly IEnrollmentService _enrollmentService;
    private readonly IStudentService _studentService;
    private readonly IClassService _classService;

    public ListEnrollmentsEndpoint(
        IEnrollmentService enrollmentService,
        IStudentService studentService,
        IClassService classService)
    {
        _enrollmentService = enrollmentService;
        _studentService = studentService;
        _classService = classService;
    }

    public override void Configure()
    {
        Get("/enrollments");
        AllowAnonymous();
        Description(d => d
            .Produces<PaginatedResult<EnrollmentResponse>>(200)
            .WithTags("Enrollments"));
    }

    public override async Task HandleAsync(ListEnrollmentsRequest req, CancellationToken ct)
    {
        PaginatedResult<Domain.Entities.Enrollment> enrollments;
        
        if (req.StudentId.HasValue)
        {
            enrollments = await _enrollmentService.GetEnrollmentsByStudentIdAsync(
                req.StudentId.Value, 
                req.PageNumber, 
                req.PageSize);
        }
        else if (req.ClassId.HasValue)
        {
            enrollments = await _enrollmentService.GetEnrollmentsByClassIdAsync(
                req.ClassId.Value, 
                req.PageNumber, 
                req.PageSize);
        }
        else
        {
            // If no filters are provided, return all enrollments
            // This would require adding a method to the service
            // For now, we'll return an empty result
            await SendOkAsync(new PaginatedResult<EnrollmentResponse>
            {
                Items = new List<EnrollmentResponse>(),
                PageNumber = req.PageNumber,
                PageSize = req.PageSize,
                TotalCount = 0
            }, ct);
            return;
        }

        // Map to response objects
        var responseItems = new List<EnrollmentResponse>();
        foreach (var enrollment in enrollments.Items)
        {
            var student = await _studentService.GetStudentByIdAsync(enrollment.StudentId, ct);
            var classEntity = await _classService.GetClassByIdAsync(enrollment.ClassId, ct);
            
            if (student != null && classEntity != null)
            {
                responseItems.Add(new EnrollmentResponse
                {
                    Id = enrollment.Id,
                    StudentId = enrollment.StudentId,
                    ClassId = enrollment.ClassId,
                    EnrollmentDate = enrollment.EnrollmentDate,
                    StudentName = $"{student.FirstName} {student.LastName}",
                    ClassName = classEntity.Name
                });
            }
        }

        var result = new PaginatedResult<EnrollmentResponse>
        {
            Items = responseItems,
            TotalCount = enrollments.TotalCount,
            PageNumber = req.PageNumber,
            PageSize = req.PageSize
        };

        await SendOkAsync(result, ct);
    }
}
