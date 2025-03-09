using FastEndpoints;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Features.Classes.Requests;
using UniversityEnrollmentSystem.Features.Classes.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace UniversityEnrollmentSystem.Features.Classes.Endpoints;

public class CreateClassEndpoint : Endpoint<CreateClassRequest, ClassResponse>
{
    private readonly IClassService _classService;

    public CreateClassEndpoint(IClassService classService)
    {
        _classService = classService;
    }

    public override void Configure()
    {
        Post("/api/classes");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Creates a new class";
            s.Description = "Creates a new class with the provided details";
            s.ExampleRequest = new CreateClassRequest
            {
                Name = "Computer Science 101",
                Teacher = "Dr. Smith",
                Description = "Introduction to Computer Science"
            };
            s.Response<ClassResponse>(201, "Class created successfully");
            s.Response(400, "Invalid input");
        });
    }

    public override async Task HandleAsync(CreateClassRequest req, CancellationToken ct)
    {
        var @class = new Class
        {
            Name = req.Name,
            Teacher = req.Teacher,
            Description = req.Description
        };

        var result = await _classService.CreateClassAsync(@class, ct);
        
        await SendCreatedAtAsync<GetClassEndpoint>(
            new { id = result.Id },
            result,
            cancellation: ct);
    }
}
