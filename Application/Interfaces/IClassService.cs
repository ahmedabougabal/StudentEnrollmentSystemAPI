using UniversityEnrollmentSystem.Application.DTOs;
using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Features.Classes.Responses;
using System.Threading;

namespace UniversityEnrollmentSystem.Application.Interfaces;

public interface IClassService
{
    Task<ClassResponse?> GetClassByIdAsync(int id, CancellationToken ct = default);
    Task<PaginatedResult<ClassResponse>> GetClassesAsync(string? name = null, string? teacher = null, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);
    Task<ClassResponse> CreateClassAsync(Class @class, CancellationToken ct = default);
    Task<ClassResponse> UpdateClassAsync(Class @class, CancellationToken ct = default);
    Task<bool> DeleteClassAsync(int id, CancellationToken ct = default);
    Task<bool> ClassExistsAsync(int id, CancellationToken ct = default);
    Task<decimal> GetClassAverageMarkAsync(int classId, CancellationToken ct = default);
    Task<IEnumerable<Student>> GetEnrolledStudentsAsync(int classId, CancellationToken ct = default);
}