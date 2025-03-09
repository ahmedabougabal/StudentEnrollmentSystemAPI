namespace UniversityEnrollmentSystem.Domain.Interfaces;

using UniversityEnrollmentSystem.Domain.Entities;

public interface IClassRepository : IRepository<Class>
{
    Task<IEnumerable<Class>> SearchAsync(string? name = null, string? teacher = null, int page = 1, int pageSize = 10);
    Task<int> GetTotalCountAsync(string? name = null, string? teacher = null);
    Task<IEnumerable<Class>> GetClassesByStudentIdAsync(int studentId);
    Task<decimal> GetClassAverageMarkAsync(int classId);
}