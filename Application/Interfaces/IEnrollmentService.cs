using UniversityEnrollmentSystem.Application.DTOs;
using UniversityEnrollmentSystem.Domain.Entities;

namespace UniversityEnrollmentSystem.Application.Interfaces;

public interface IEnrollmentService
{
    Task<Enrollment?> GetEnrollmentByIdAsync(int id);
    Task<PaginatedResult<Enrollment>> GetEnrollmentsByStudentIdAsync(int studentId, int page = 1, int pageSize = 10);
    Task<PaginatedResult<Enrollment>> GetEnrollmentsByClassIdAsync(int classId, int page = 1, int pageSize = 10);
    Task<Enrollment> CreateEnrollmentAsync(Enrollment enrollment);
    Task<bool> IsStudentEnrolledInClassAsync(int studentId, int classId);
    Task<bool> EnrollmentExistsAsync(int id);
}