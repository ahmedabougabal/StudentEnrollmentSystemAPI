using System.ComponentModel.DataAnnotations;

namespace UniversityEnrollmentSystem.Features.Enrollments.Requests;

public class CreateEnrollmentRequest
{
    [Required]
    public int StudentId { get; set; }

    [Required]
    public int ClassId { get; set; }
}