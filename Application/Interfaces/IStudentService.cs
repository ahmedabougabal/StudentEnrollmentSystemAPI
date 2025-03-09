using UniversityEnrollmentSystem.Application.DTOs;
using UniversityEnrollmentSystem.Domain.Entities;

namespace UniversityEnrollmentSystem.Application.Interfaces;

public interface IStudentService
{
    Task<Student?> GetStudentByIdAsync(int id);
    Task<PaginatedResult<Student>> GetStudentsAsync(string? name = null, int? age = null, int page = 1, int pageSize = 10);
    Task<Student> CreateStudentAsync(Student student);
    Task UpdateStudentAsync(Student student);
    Task DeleteStudentAsync(int id);
    Task<bool> StudentExistsAsync(int id);
    Task<IEnumerable<Student>> GetStudentsByClassIdAsync(int classId);
}
