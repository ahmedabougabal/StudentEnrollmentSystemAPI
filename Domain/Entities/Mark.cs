namespace UniversityEnrollmentSystem.Domain.Entities;

public class Mark
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public decimal ExamMark { get; set; }
    public decimal AssignmentMark { get; set; }
    
    // this is the total mark (60% exam, 40% assignment)
    public decimal TotalMark => (ExamMark * 0.6M) + (AssignmentMark * 0.4M);
    
    // Navigation properties
    public virtual Student Student { get; set; } = default!;
    public virtual Class Class { get; set; } = default!;
}
