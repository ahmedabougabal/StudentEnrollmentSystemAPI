namespace UniversityEnrollmentSystem.Features.Enrollments.Responses;

public class EnrollmentResponse
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public string StudentName { get; set; } = default!;
    public string ClassName { get; set; } = default!;
}