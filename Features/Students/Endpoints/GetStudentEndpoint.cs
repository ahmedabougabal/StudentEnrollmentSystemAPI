using FastEndpoints;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Features.Students.Responses;

namespace UniversityEnrollmentSystem.Features.Students.Endpoints;

public class GetStudentEndpoint : EndpointWithoutRequest<StudentResponse>
{
    private readonly IStudentService _studentService;

    public GetStudentEndpoint(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public override void Configure()
    {
        Get("/students/{id}");
        AllowAnonymous();
        Description(d => d
            .Produces<StudentResponse>(200)
            .Produces(404)
            .WithTags("Students"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var student = await _studentService.GetStudentByIdAsync(id, ct);
        if (student == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        await SendOkAsync(student, ct);
    }
}
