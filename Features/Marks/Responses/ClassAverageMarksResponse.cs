namespace UniversityEnrollmentSystem.Features.Marks.Responses;

public class ClassAverageMarksResponse
{
    public int ClassId { get; set; }
    public string ClassName { get; set; } = default!;
    public decimal AverageExamMark { get; set; }
    public decimal AverageAssignmentMark { get; set; }
    public decimal AverageTotalMark { get; set; }
    public int StudentCount { get; set; }
}
