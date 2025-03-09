namespace UniversityEnrollmentSystem.Domain.Entities;

public class Enrollment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    
    // Navigation properties
    public virtual Student Student { get; set; } = default!;
    public virtual Class Class { get; set; } = default!;
}
