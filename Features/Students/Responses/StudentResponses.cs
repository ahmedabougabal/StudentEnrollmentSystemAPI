namespace UniversityEnrollmentSystem.Features.Students.Responses;

public class StudentResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public int Age { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}

public class StudentDetailsResponse : StudentResponse
{
    public IEnumerable<EnrollmentSummaryResponse> Enrollments { get; set; } = new List<EnrollmentSummaryResponse>();
    public IEnumerable<MarkSummaryResponse> Marks { get; set; } = new List<MarkSummaryResponse>();
    public decimal AverageMark { get; set; }
}

public class EnrollmentSummaryResponse
{
    public int ClassId { get; set; }
    public string ClassName { get; set; } = default!;
    public DateTime EnrollmentDate { get; set; }
}

public class MarkSummaryResponse
{
    public int ClassId { get; set; }
    public string ClassName { get; set; } = default!;
    public decimal TotalMark { get; set; }
}