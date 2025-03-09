using System.ComponentModel.DataAnnotations;

namespace UniversityEnrollmentSystem.Features.Students.Requests;

public class CreateStudentRequest
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = default!;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = default!;

    [Range(16, 100)]
    public int Age { get; set; }
}

public class UpdateStudentRequest : CreateStudentRequest
{
    [Required]
    public int Id { get; set; }
}

public class StudentSearchRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Name { get; set; }
    public int? Age { get; set; }
}