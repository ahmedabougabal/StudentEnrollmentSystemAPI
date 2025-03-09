using FastEndpoints;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Features.Classes.Requests;
using UniversityEnrollmentSystem.Features.Classes.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace UniversityEnrollmentSystem.Features.Classes.Endpoints;

public class GetClassesEndpoint : Endpoint<ClassSearchRequest, ClassesResponse>
{
    private readonly IClassService _classService;

    public GetClassesEndpoint(IClassService classService)
    {
        _classService = classService;
    }

    public override void Configure()
    {
        Get("/classes");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Gets all classes with pagination and filtering";
            s.Description = "Retrieves a paginated list of classes with optional filtering by name and teacher";
            s.Response<ClassesResponse>(200, "Classes retrieved successfully");
        });
    }

    public override async Task HandleAsync(ClassSearchRequest req, CancellationToken ct)
    {
        var result = await _classService.GetClassesAsync(
            req.Name,
            req.Teacher,
            req.Page,
            req.PageSize,
            ct);

        await SendOkAsync(new ClassesResponse
        {
            Classes = result.Items,
            Page = req.Page,
            PageSize = req.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages
        }, ct);
    }
}
