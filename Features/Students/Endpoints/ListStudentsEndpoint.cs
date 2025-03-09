using FastEndpoints;
using UniversityEnrollmentSystem.Application.DTOs;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Features.Students.Requests;
using UniversityEnrollmentSystem.Features.Students.Responses;

namespace UniversityEnrollmentSystem.Features.Students.Endpoints;

public class ListStudentsEndpoint : Endpoint<ListStudentsRequest, PaginatedResult<StudentResponse>>
{
    private readonly IStudentService _studentService;

    public ListStudentsEndpoint(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public override void Configure()
    {
        Get("/students");
        AllowAnonymous();
        Description(d => d
            .Produces<PaginatedResult<StudentResponse>>(200)
            .WithTags("Students"));
    }

    public override async Task HandleAsync(ListStudentsRequest req, CancellationToken ct)
    {
        var result = await _studentService.GetStudentsAsync(
            req.PageNumber, 
            req.PageSize, 
            req.SearchTerm,
            ct);
            
        await SendOkAsync(result, ct);
    }
}
