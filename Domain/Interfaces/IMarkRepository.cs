using UniversityEnrollmentSystem.Domain.Entities;

namespace UniversityEnrollmentSystem.Domain.Interfaces;

public interface IMarkRepository : IRepository<Mark>
{
    Task<IEnumerable<Mark>> GetMarksByStudentIdAsync(int studentId, int page = 1, int pageSize = 10);
    Task<IEnumerable<Mark>> GetMarksByClassIdAsync(int classId, int page = 1, int pageSize = 10);
    Task<Mark?> GetStudentMarkInClassAsync(int studentId, int classId);
    Task<decimal> GetStudentAverageMarkAsync(int studentId);
    Task<decimal> GetClassAverageMarkAsync(int classId);
    Task<bool> HasExistingMarkAsync(int studentId, int classId);
    Task<int> GetTotalCountByStudentIdAsync(int studentId);
    Task<int> GetTotalCountByClassIdAsync(int classId);
}
