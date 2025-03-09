using FastEndpoints;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Features.Enrollments.Requests;
using UniversityEnrollmentSystem.Features.Enrollments.Responses;

namespace UniversityEnrollmentSystem.Features.Enrollments.Endpoints;

public class CreateEnrollmentEndpoint : Endpoint<CreateEnrollmentRequest, EnrollmentResponse>
{
    private readonly IEnrollmentService _enrollmentService;
    private readonly IStudentService _studentService;
    private readonly IClassService _classService;

    public CreateEnrollmentEndpoint(
        IEnrollmentService enrollmentService,
        IStudentService studentService,
        IClassService classService)
    {
        _enrollmentService = enrollmentService;
        _studentService = studentService;
        _classService = classService;
    }

    public override void Configure()
    {
        Post("/enrollments");
        AllowAnonymous();
        Description(d => d
            .Produces<EnrollmentResponse>(201)
            .Produces(400)
            .Produces(404)
            .WithTags("Enrollments"));
    }

    public override async Task HandleAsync(CreateEnrollmentRequest req, CancellationToken ct)
    {
        // Validate student exists
        var student = await _studentService.GetStudentByIdAsync(req.StudentId, ct);
        if (student == null)
        {
            AddError(r => r.StudentId, "Student not found");
            await SendErrorsAsync(404, ct);
            return;
        }

        // Validate class exists
        var classEntity = await _classService.GetClassByIdAsync(req.ClassId, ct);
        if (classEntity == null)
        {
            AddError(r => r.ClassId, "Class not found");
            await SendErrorsAsync(404, ct);
            return;
        }

        // Check if student is already enrolled in the class
        var isEnrolled = await _enrollmentService.IsStudentEnrolledInClassAsync(req.StudentId, req.ClassId);
        if (isEnrolled)
        {
            AddError("Student is already enrolled in this class");
            await SendErrorsAsync(400, ct);
            return;
        }

        // Create enrollment
        var enrollment = new Enrollment
        {
            StudentId = req.StudentId,
            ClassId = req.ClassId,
            EnrollmentDate = DateTime.UtcNow
        };

        var createdEnrollment = await _enrollmentService.CreateEnrollmentAsync(enrollment);

        // Return response
        var response = new EnrollmentResponse
        {
            Id = createdEnrollment.Id,
            StudentId = createdEnrollment.StudentId,
            ClassId = createdEnrollment.ClassId,
            EnrollmentDate = createdEnrollment.EnrollmentDate
        };

        await SendCreatedAtAsync<CreateEnrollmentEndpoint>(
            new { id = createdEnrollment.Id },
            response,
            generateAbsoluteUrl: true,
            cancellation: ct);
    }
}
