// Features/Classes/Responses/ClassResponses.cs
namespace UniversityEnrollmentSystem.Features.Classes.Responses;

public class ClassResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Teacher { get; set; } = default!;
    public string Description { get; set; } = default!;
}

public class ClassDetailsResponse : ClassResponse
{
    public int EnrolledStudentsCount { get; set; }
    public decimal AverageMark { get; set; }
    public IEnumerable<StudentSummaryResponse> EnrolledStudents { get; set; } = new List<StudentSummaryResponse>();
}

public class StudentSummaryResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = default!;
    public decimal? Mark { get; set; }
}