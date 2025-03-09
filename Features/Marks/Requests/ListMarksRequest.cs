namespace UniversityEnrollmentSystem.Features.Marks.Requests;

public class ListMarksRequest
{
    public int? StudentId { get; set; }
    public int? ClassId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
