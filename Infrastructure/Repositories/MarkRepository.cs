using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Domain.Interfaces;

namespace UniversityEnrollmentSystem.Infrastructure.Repositories;

public class MarkRepository : InMemoryRepository<Mark>, IMarkRepository
{
    protected override int GetEntityId(Mark entity) => entity.Id;
    protected override void SetEntityId(Mark entity, int id) => entity.Id = id;

    public async Task<IEnumerable<Mark>> GetMarksByStudentIdAsync(int studentId)
    {
        return await Task.FromResult(
            _entities.Values
                .Where(m => m.StudentId == studentId)
                .ToList());
    }

    public async Task<IEnumerable<Mark>> GetMarksByClassIdAsync(int classId)
    {
        return await Task.FromResult(
            _entities.Values
                .Where(m => m.ClassId == classId)
                .ToList());
    }

    public async Task<Mark?> GetStudentMarkInClassAsync(int studentId, int classId)
    {
        return await Task.FromResult(
            _entities.Values
                .FirstOrDefault(m => 
                    m.StudentId == studentId && 
                    m.ClassId == classId));
    }

    public async Task<decimal> GetStudentAverageMarkAsync(int studentId)
    {
        var studentMarks = _entities.Values
            .Where(m => m.StudentId == studentId)
            .ToList();

        if (!studentMarks.Any())
            return await Task.FromResult(0m);

        return await Task.FromResult(studentMarks.Average(m => m.TotalMark));
    }

    public async Task<decimal> GetClassAverageMarkAsync(int classId)
    {
        var classMarks = _entities.Values
            .Where(m => m.ClassId == classId)
            .ToList();

        if (!classMarks.Any())
            return await Task.FromResult(0m);

        return await Task.FromResult(classMarks.Average(m => m.TotalMark));
    }

    public async Task<bool> HasExistingMarkAsync(int studentId, int classId)
    {
        return await Task.FromResult(
            _entities.Values.Any(m => 
                m.StudentId == studentId && 
                m.ClassId == classId));
    }
}