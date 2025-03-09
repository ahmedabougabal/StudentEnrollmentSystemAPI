namespace UniversityEnrollmentSystem.Features.Enrollments.Requests;

public class ListEnrollmentsRequest
{
    public int? StudentId { get; set; }
    public int? ClassId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
