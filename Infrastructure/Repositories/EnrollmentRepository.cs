using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Domain.Interfaces;

namespace UniversityEnrollmentSystem.Infrastructure.Repositories;

public class EnrollmentRepository : InMemoryRepository<Enrollment>, IEnrollmentRepository
{
    protected override int GetEntityId(Enrollment entity) => entity.Id;
    protected override void SetEntityId(Enrollment entity, int id) => entity.Id = id;

    public async Task<bool> IsStudentEnrolledInClassAsync(int studentId, int classId)
    {
        return await Task.FromResult(
            _entities.Values.Any(e => 
                e.StudentId == studentId && 
                e.ClassId == classId));
    }

    public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentIdAsync(int studentId, int page = 1, int pageSize = 10)
    {
        return await Task.FromResult(
            _entities.Values
                .Where(e => e.StudentId == studentId)
                .OrderByDescending(e => e.EnrollmentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList());
    }

    public async Task<IEnumerable<Enrollment>> GetEnrollmentsByClassIdAsync(int classId, int page = 1, int pageSize = 10)
    {
        return await Task.FromResult(
            _entities.Values
                .Where(e => e.ClassId == classId)
                .OrderByDescending(e => e.EnrollmentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList());
    }

    public async Task<DateTime?> GetEnrollmentDateAsync(int studentId, int classId)
    {
        var enrollment = _entities.Values
            .FirstOrDefault(e => 
                e.StudentId == studentId && 
                e.ClassId == classId);

        return await Task.FromResult(enrollment?.EnrollmentDate);
    }

    public async Task<int> GetTotalCountByStudentIdAsync(int studentId)
    {
        return await Task.FromResult(
            _entities.Values.Count(e => e.StudentId == studentId));
    }

    public async Task<int> GetTotalCountByClassIdAsync(int classId)
    {
        return await Task.FromResult(
            _entities.Values.Count(e => e.ClassId == classId));
    }
}