using UniversityEnrollmentSystem.Domain.Entities;

namespace UniversityEnrollmentSystem.Domain.Interfaces;

public interface IMarkRepository : IRepository<Mark>
{
    Task<IEnumerable<Mark>> GetMarksByStudentIdAsync(int studentId);
    Task<IEnumerable<Mark>> GetMarksByClassIdAsync(int classId);
    Task<Mark?> GetStudentMarkInClassAsync(int studentId, int classId);
    Task<decimal> GetStudentAverageMarkAsync(int studentId);
    Task<decimal> GetClassAverageMarkAsync(int classId);
    Task<bool> HasExistingMarkAsync(int studentId, int classId);
}
