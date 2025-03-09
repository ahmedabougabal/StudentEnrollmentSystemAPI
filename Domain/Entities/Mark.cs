namespace UniversityEnrollmentSystem.Domain.Entities;

public class Mark
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public decimal ExamMark { get; set; }
    public decimal AssignmentMark { get; set; }
    
    // Computed property for total mark
    public decimal TotalMark => ExamMark + AssignmentMark;
    
    // Navigation properties
    public virtual Student Student { get; set; } = default!;
    public virtual Class Class { get; set; } = default!;
}
