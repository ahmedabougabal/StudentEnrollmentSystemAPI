using UniversityEnrollmentSystem.Application.DTOs;
using UniversityEnrollmentSystem.Features.Students.Requests;
using UniversityEnrollmentSystem.Features.Students.Responses;

namespace UniversityEnrollmentSystem.Application.Interfaces;

public interface IStudentService
{
    Task<StudentResponse?> GetStudentByIdAsync(int id, CancellationToken ct = default);
    Task<PaginatedResult<StudentResponse>> GetStudentsAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, CancellationToken ct = default);
    Task<StudentResponse> CreateStudentAsync(CreateStudentRequest request, CancellationToken ct = default);
    Task<StudentResponse> UpdateStudentAsync(UpdateStudentRequest request, CancellationToken ct = default);
    Task<bool> DeleteStudentAsync(int id, CancellationToken ct = default);
}
