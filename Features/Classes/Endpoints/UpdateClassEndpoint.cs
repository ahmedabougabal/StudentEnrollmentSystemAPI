using FastEndpoints;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Features.Classes.Requests;
using UniversityEnrollmentSystem.Features.Classes.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace UniversityEnrollmentSystem.Features.Classes.Endpoints;

public class UpdateClassEndpoint : Endpoint<UpdateClassRequest, ClassResponse>
{
    private readonly IClassService _classService;

    public UpdateClassEndpoint(IClassService classService)
    {
        _classService = classService;
    }

    public override void Configure()
    {
        Put("/api/classes/{id}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Updates an existing class";
            s.Description = "Updates a class with the provided details";
            s.ExampleRequest = new UpdateClassRequest
            {
                Id = 1,
                Name = "Updated Computer Science 101",
                Teacher = "Dr. Johnson",
                Description = "Updated Introduction to Computer Science"
            };
            s.Response<ClassResponse>(200, "Class updated successfully");
            s.Response(400, "Invalid input");
            s.Response(404, "Class not found");
        });
    }

    public override async Task HandleAsync(UpdateClassRequest req, CancellationToken ct)
    {
        // Ensure route ID matches body ID
        var routeId = Route<int>("id");
        if (routeId != req.Id)
        {
            AddError(r => r.Id, "Route ID must match the ID in the request body");
            await SendErrorsAsync(400, ct);
            return;
        }

        var @class = new Class
        {
            Id = req.Id,
            Name = req.Name,
            Teacher = req.Teacher,
            Description = req.Description
        };

        var result = await _classService.UpdateClassAsync(@class, ct);
        
        if (result == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(result, ct);
    }
}
