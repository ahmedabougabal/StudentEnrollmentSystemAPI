namespace UniversityEnrollmentSystem.Features.Marks.Responses;

public class MarkResponse
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public string StudentName { get; set; } = default!;
    public string ClassName { get; set; } = default!;
    public decimal ExamMark { get; set; }
    public decimal AssignmentMark { get; set; }
    public decimal TotalMark { get; set; }
}