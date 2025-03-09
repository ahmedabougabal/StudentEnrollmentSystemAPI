using System.ComponentModel.DataAnnotations;

namespace UniversityEnrollmentSystem.Features.Students.Requests;

public class CreateStudentRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public int Age { get; set; }
}

public class UpdateStudentRequest
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public int Age { get; set; }
}

public class ListStudentsRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
}