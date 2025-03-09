namespace UniversityEnrollmentSystem.Domain.Entities;

public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public int Age { get; set; }
    
    // Navigation properties
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
}
