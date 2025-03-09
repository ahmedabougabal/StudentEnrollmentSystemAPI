using FastEndpoints;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Features.Students.Responses;
using UniversityEnrollmentSystem.Features.Students.Validators;

namespace UniversityEnrollmentSystem.Features.Students.Endpoints;

public class GetStudentReportEndpoint : Endpoint<GetStudentReportRequest, StudentReportResponse>
{
    private readonly IStudentService _studentService;
    private readonly IEnrollmentService _enrollmentService;
    private readonly IClassService _classService;
    private readonly IMarkService _markService;

    public GetStudentReportEndpoint(
        IStudentService studentService,
        IEnrollmentService enrollmentService,
        IClassService classService,
        IMarkService markService)
    {
        _studentService = studentService;
        _enrollmentService = enrollmentService;
        _classService = classService;
        _markService = markService;
    }

    public override void Configure()
    {
        Get("/students/{StudentId}/report");
        AllowAnonymous();
        Description(d => d
            .Produces<StudentReportResponse>(200)
            .Produces(404)
            .WithTags("Students"));
    }

    public override async Task HandleAsync(GetStudentReportRequest req, CancellationToken ct)
    {
        // Validate student exists
        var student = await _studentService.GetStudentByIdAsync(req.StudentId, ct);
        if (student == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        // Get all enrollments for the student
        var enrollments = await _enrollmentService.GetEnrollmentsByStudentIdAsync(req.StudentId);
        
        var classReports = new List<ClassReportItem>();
        decimal overallAverage = 0;
        
        if (enrollments.Items.Any())
        {
            foreach (var enrollment in enrollments.Items)
            {
                // Get class details
                var classEntity = await _classService.GetClassByIdAsync(enrollment.ClassId, ct);
                if (classEntity == null) continue;
                
                // Get marks for this student in this class
                var marks = await _markService.GetMarksByClassIdAsync(enrollment.ClassId);
                var studentMark = marks.Items.FirstOrDefault(m => m.StudentId == req.StudentId);
                
                var classReport = new ClassReportItem
                {
                    ClassId = classEntity.Id,
                    ClassName = classEntity.Name,
                    Teacher = classEntity.Teacher,
                    EnrollmentDate = enrollment.EnrollmentDate
                };
                
                if (studentMark != null)
                {
                    classReport.ExamMark = studentMark.ExamMark;
                    classReport.AssignmentMark = studentMark.AssignmentMark;
                    classReport.TotalMark = studentMark.ExamMark + studentMark.AssignmentMark;
                }
                
                classReports.Add(classReport);
            }
            
            // Calculate overall average if there are marks
            if (classReports.Any(c => c.TotalMark.HasValue))
            {
                overallAverage = classReports
                    .Where(c => c.TotalMark.HasValue)
                    .Average(c => c.TotalMark.Value);
            }
        }
        
        var response = new StudentReportResponse
        {
            StudentId = student.Id,
            StudentName = $"{student.FirstName} {student.LastName}",
            Age = student.Age,
            EnrolledClasses = classReports,
            ClassCount = classReports.Count,
            OverallAverage = Math.Round(overallAverage, 2)
        };
        
        await SendOkAsync(response, ct);
    }
}
