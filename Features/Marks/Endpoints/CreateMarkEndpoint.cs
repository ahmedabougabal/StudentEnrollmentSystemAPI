using FastEndpoints;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Features.Marks.Requests;
using UniversityEnrollmentSystem.Features.Marks.Responses;

namespace UniversityEnrollmentSystem.Features.Marks.Endpoints;

public class CreateMarkEndpoint : Endpoint<CreateMarkRequest, MarkResponse>
{
    private readonly IMarkService _markService;
    private readonly IEnrollmentService _enrollmentService;

    public CreateMarkEndpoint(
        IMarkService markService,
        IEnrollmentService enrollmentService)
    {
        _markService = markService;
        _enrollmentService = enrollmentService;
    }

    public override void Configure()
    {
        Post("/marks");
        AllowAnonymous();
        Description(d => d
            .Produces<MarkResponse>(201)
            .Produces(400)
            .Produces(404)
            .WithTags("Marks"));
    }

    public override async Task HandleAsync(CreateMarkRequest req, CancellationToken ct)
    {
        // Validate student is enrolled in the class
        var isEnrolled = await _enrollmentService.IsStudentEnrolledInClassAsync(req.StudentId, req.ClassId);
        if (!isEnrolled)
        {
            AddError("Student is not enrolled in this class");
            await SendErrorsAsync(400, ct);
            return;
        }

        // Create mark
        var mark = new Mark
        {
            StudentId = req.StudentId,
            ClassId = req.ClassId,
            ExamMark = req.ExamMark,
            AssignmentMark = req.AssignmentMark
        };

        var createdMark = await _markService.CreateMarkAsync(mark);

        // Return response
        var response = new MarkResponse
        {
            Id = createdMark.Id,
            StudentId = createdMark.StudentId,
            ClassId = createdMark.ClassId,
            ExamMark = createdMark.ExamMark,
            AssignmentMark = createdMark.AssignmentMark,
            TotalMark = createdMark.ExamMark + createdMark.AssignmentMark
        };

        await SendCreatedAtAsync<CreateMarkEndpoint>(
            new { id = createdMark.Id },
            response,
            generateAbsoluteUrl: true,
            cancellation: ct);
    }
}
