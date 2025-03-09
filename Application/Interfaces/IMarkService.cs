using UniversityEnrollmentSystem.Application.DTOs;
using UniversityEnrollmentSystem.Domain.Entities;

namespace UniversityEnrollmentSystem.Application.Interfaces;

public interface IMarkService
{
    Task<Mark?> GetMarkByIdAsync(int id);
    Task<PaginatedResult<Mark>> GetMarksByStudentIdAsync(int studentId, int page = 1, int pageSize = 10);
    Task<PaginatedResult<Mark>> GetMarksByClassIdAsync(int classId, int page = 1, int pageSize = 10);
    Task<Mark> CreateMarkAsync(Mark mark);
    Task UpdateMarkAsync(Mark mark);
    Task<bool> MarkExistsAsync(int id);
    Task<decimal> CalculateStudentAverageAsync(int studentId, int? classId = null);
}