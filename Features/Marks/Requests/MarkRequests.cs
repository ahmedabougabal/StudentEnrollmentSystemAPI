using System.ComponentModel.DataAnnotations;

namespace UniversityEnrollmentSystem.Features.Marks.Requests;

public class CreateMarkRequest
{
    [Required]
    public int StudentId { get; set; }

    [Required]
    public int ClassId { get; set; }

    [Range(0, 100)]
    public decimal ExamMark { get; set; }

    [Range(0, 100)]
    public decimal AssignmentMark { get; set; }
}

public class UpdateMarkRequest : CreateMarkRequest
{
    [Required]
    public int Id { get; set; }
}