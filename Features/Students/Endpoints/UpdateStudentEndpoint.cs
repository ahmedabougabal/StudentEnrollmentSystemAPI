using FastEndpoints;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Features.Students.Requests;
using UniversityEnrollmentSystem.Features.Students.Responses;

namespace UniversityEnrollmentSystem.Features.Students.Endpoints;

public class UpdateStudentEndpoint : Endpoint<UpdateStudentRequest, StudentResponse>
{
    private readonly IStudentService _studentService;

    public UpdateStudentEndpoint(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public override void Configure()
    {
        Put("/students/{id}");
        AllowAnonymous();
        Description(d => d
            .Produces<StudentResponse>(200)
            .Produces(404)
            .Produces(400)
            .WithTags("Students"));
    }

    public override async Task HandleAsync(UpdateStudentRequest req, CancellationToken ct)
    {
        var id = Route<int>("id");
        if (id != req.Id)
        {
            ThrowError("Id in route must match Id in request body");
        }

        var updatedStudent = await _studentService.UpdateStudentAsync(req, ct);
        if (updatedStudent == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(updatedStudent, ct);
    }
}
