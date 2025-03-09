using FastEndpoints;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Features.Classes.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace UniversityEnrollmentSystem.Features.Classes.Endpoints;

public class GetClassEndpoint : EndpointWithoutRequest<ClassResponse>
{
    private readonly IClassService _classService;

    public GetClassEndpoint(IClassService classService)
    {
        _classService = classService;
    }

    public override void Configure()
    {
        Get("/api/classes/{id}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Gets a class by ID";
            s.Description = "Retrieves a specific class by its unique identifier";
            s.Response<ClassResponse>(200, "Class found");
            s.Response(404, "Class not found");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var result = await _classService.GetClassByIdAsync(id, ct);

        if (result == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(result, ct);
    }
}
