using FastEndpoints;
using UniversityEnrollmentSystem.Application.Interfaces;

namespace UniversityEnrollmentSystem.Features.Students.Endpoints;

public class DeleteStudentEndpoint : EndpointWithoutRequest
{
    private readonly IStudentService _studentService;

    public DeleteStudentEndpoint(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public override void Configure()
    {
        Delete("/students/{id}");
        AllowAnonymous();
        Description(d => d
            .Produces(204)
            .Produces(404)
            .WithTags("Students"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var success = await _studentService.DeleteStudentAsync(id, ct);
        
        if (!success)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendNoContentAsync(ct);
    }
}
