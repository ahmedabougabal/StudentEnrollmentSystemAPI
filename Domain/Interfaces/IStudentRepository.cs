namespace UniversityEnrollmentSystem.Domain.Interfaces;

using UniversityEnrollmentSystem.Domain.Entities;

public interface IStudentRepository : IRepository<Student>
{
    Task<IEnumerable<Student>> SearchAsync(string? name = null, int? age = null, int page = 1, int pageSize = 10);
    Task<int> GetTotalCountAsync(string? name = null, int? age = null);
    Task<IEnumerable<Student>> GetStudentsByClassIdAsync(int classId);
}