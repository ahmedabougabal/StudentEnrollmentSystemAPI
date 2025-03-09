namespace UniversityEnrollmentSystem.Features.Students.Responses;

public class StudentReportResponse
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = default!;
    public int Age { get; set; }
    public List<ClassReportItem> EnrolledClasses { get; set; } = new List<ClassReportItem>();
    public int ClassCount { get; set; }
    public decimal OverallAverage { get; set; }
}

public class ClassReportItem
{
    public int ClassId { get; set; }
    public string ClassName { get; set; } = default!;
    public string Teacher { get; set; } = default!;
    public DateTime EnrollmentDate { get; set; }
    public decimal? ExamMark { get; set; }
    public decimal? AssignmentMark { get; set; }
    public decimal? TotalMark { get; set; }
}
