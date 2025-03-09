using UniversityEnrollmentSystem.Domain.Entities;

namespace UniversityEnrollmentSystem.Domain.Interfaces;

public interface IEnrollmentRepository : IRepository<Enrollment>
{
    Task<bool> IsStudentEnrolledInClassAsync(int studentId, int classId);
    Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentIdAsync(int studentId, int page = 1, int pageSize = 10);
    Task<IEnumerable<Enrollment>> GetEnrollmentsByClassIdAsync(int classId, int page = 1, int pageSize = 10);
    Task<DateTime?> GetEnrollmentDateAsync(int studentId, int classId);
    Task<int> GetTotalCountByStudentIdAsync(int studentId);
    Task<int> GetTotalCountByClassIdAsync(int classId);
}