using FastEndpoints;
using UniversityEnrollmentSystem.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace UniversityEnrollmentSystem.Features.Classes.Endpoints;

public class DeleteClassEndpoint : EndpointWithoutRequest
{
    private readonly IClassService _classService;

    public DeleteClassEndpoint(IClassService classService)
    {
        _classService = classService;
    }

    public override void Configure()
    {
        Delete("/api/classes/{id}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Deletes a class";
            s.Description = "Deletes a class by its unique identifier";
            s.Response(204, "Class deleted successfully");
            s.Response(404, "Class not found");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var success = await _classService.DeleteClassAsync(id, ct);

        if (!success)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendNoContentAsync(ct);
    }
}
