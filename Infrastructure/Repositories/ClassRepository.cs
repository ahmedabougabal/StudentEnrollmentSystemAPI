using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Domain.Interfaces;

namespace UniversityEnrollmentSystem.Infrastructure.Repositories;

public class ClassRepository : InMemoryRepository<Class>, IClassRepository
{
    protected override int GetEntityId(Class entity) => entity.Id;
    protected override void SetEntityId(Class entity, int id) => entity.Id = id;

    public async Task<IEnumerable<Class>> SearchAsync(string? name = null, string? teacher = null, int page = 1, int pageSize = 10)
    {
        var query = _entities.Values.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            name = name.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(teacher))
        {
            teacher = teacher.ToLower();
            query = query.Where(c => c.Teacher.ToLower().Contains(teacher));
        }

        return await Task.FromResult(
            query.OrderBy(c => c.Name)
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .ToList());
    }

    public async Task<int> GetTotalCountAsync(string? name = null, string? teacher = null)
    {
        var query = _entities.Values.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            name = name.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(teacher))
        {
            teacher = teacher.ToLower();
            query = query.Where(c => c.Teacher.ToLower().Contains(teacher));
        }

        return await Task.FromResult(query.Count());
    }

    public async Task<IEnumerable<Class>> GetClassesByStudentIdAsync(int studentId)
    {
        return await Task.FromResult(
            _entities.Values
                .Where(c => c.Enrollments.Any(e => e.StudentId == studentId))
                .OrderBy(c => c.Name)
                .ToList());
    }

    public async Task<int> GetTotalCountByStudentIdAsync(int studentId)
    {
        return await Task.FromResult(
            _entities.Values.Count(c => c.Enrollments.Any(e => e.StudentId == studentId)));
    }

    public async Task<decimal> GetClassAverageMarkAsync(int classId)
    {
        var classMarks = _entities.Values
            .Where(c => c.Id == classId)
            .SelectMany(c => c.Marks)
            .ToList();

        if (!classMarks.Any())
            return await Task.FromResult(0m);

        return await Task.FromResult(classMarks.Average(m => m.TotalMark));
    }
}