using FastEndpoints;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Features.Students.Requests;
using UniversityEnrollmentSystem.Features.Students.Responses;

namespace UniversityEnrollmentSystem.Features.Students.Endpoints;

public class CreateStudentEndpoint : Endpoint<CreateStudentRequest, StudentResponse>
{
    private readonly IStudentService _studentService;

    public CreateStudentEndpoint(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public override void Configure()
    {
        Post("/students");
        AllowAnonymous();
        Description(d => d
            .Produces<StudentResponse>(201)
            .Produces(400)
            .WithTags("Students"));
    }

    public override async Task HandleAsync(CreateStudentRequest req, CancellationToken ct)
    {
        var student = await _studentService.CreateStudentAsync(req, ct);
        await SendCreatedAtAsync<GetStudentEndpoint>(
            new { id = student.Id },
            new StudentResponse
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Age = student.Age
            },
            cancellation: ct);
    }
}
