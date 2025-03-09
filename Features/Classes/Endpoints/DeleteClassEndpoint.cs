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
        Delete("/classes/{id}");
        AllowAnonymous();
        Description(d => d
            .Produces(204)
            .Produces(404)
            .WithTags("Classes"));
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
