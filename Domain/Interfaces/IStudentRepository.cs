using UniversityEnrollmentSystem.Domain.Entities;

namespace UniversityEnrollmentSystem.Domain.Interfaces;

public interface IStudentRepository : IRepository<Student>
{
    Task<IEnumerable<Student>> SearchAsync(string? searchTerm = null, int? age = null, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);
    Task<int> GetTotalCountAsync(string? searchTerm = null, CancellationToken ct = default);
    Task<IEnumerable<Student>> GetStudentsByClassIdAsync(int classId, CancellationToken ct = default);
    Task<int> GetTotalCountByClassIdAsync(int classId, CancellationToken ct = default);
}