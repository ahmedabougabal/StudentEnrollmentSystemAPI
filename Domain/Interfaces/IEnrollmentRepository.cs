using UniversityEnrollmentSystem.Domain.Entities;

namespace UniversityEnrollmentSystem.Domain.Interfaces;

public interface IEnrollmentRepository : IRepository<Enrollment>
{
    Task<bool> IsStudentEnrolledInClassAsync(int studentId, int classId);
    Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentIdAsync(int studentId);
    Task<IEnumerable<Enrollment>> GetEnrollmentsByClassIdAsync(int classId);
    Task<DateTime?> GetEnrollmentDateAsync(int studentId, int classId);
};