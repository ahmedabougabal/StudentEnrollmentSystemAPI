using UniversityEnrollmentSystem.Application.DTOs;
using UniversityEnrollmentSystem.Domain.Entities;

namespace UniversityEnrollmentSystem.Application.Interfaces;

public interface IClassService
{
    Task<Class?> GetClassByIdAsync(int id);
    Task<PaginatedResult<Class>> GetClassesAsync(string? name = null, string? teacher = null, int page = 1, int pageSize = 10);
    Task<Class> CreateClassAsync(Class @class);
    Task UpdateClassAsync(Class @class);
    Task DeleteClassAsync(int id);
    Task<bool> ClassExistsAsync(int id);
    Task<decimal> GetClassAverageMarkAsync(int classId);
    Task<IEnumerable<Student>> GetEnrolledStudentsAsync(int classId);
}