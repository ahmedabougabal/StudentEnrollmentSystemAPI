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

    public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentIdAsync(int studentId)
    {
        return await Task.FromResult(
            _entities.Values
                .Where(e => e.StudentId == studentId)
                .ToList());
    }

    public async Task<IEnumerable<Enrollment>> GetEnrollmentsByClassIdAsync(int classId)
    {
        return await Task.FromResult(
            _entities.Values
                .Where(e => e.ClassId == classId)
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
}