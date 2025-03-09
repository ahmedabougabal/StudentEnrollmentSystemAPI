namespace UniversityEnrollmentSystem.Domain.Entities;

public class Class
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Teacher { get; set; } = default!;
    public string Description { get; set; } = default!;
    
    // Navigation properties
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
}
