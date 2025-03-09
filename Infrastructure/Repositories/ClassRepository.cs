using System.Collections.Concurrent;
using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Domain.Interfaces;

namespace UniversityEnrollmentSystem.Infrastructure.Repositories;

public class ClassRepository : InMemoryRepository<Class>, IClassRepository
{
    private readonly IMarkRepository _markRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;

    public ClassRepository(
        IMarkRepository markRepository,
        IEnrollmentRepository enrollmentRepository)
    {
        _markRepository = markRepository;
        _enrollmentRepository = enrollmentRepository;
        // Initialize with some sample data
        InitializeSampleData();
    }

    protected override int GetEntityId(Class entity) => entity.Id;
    protected override void SetEntityId(Class entity, int id) => entity.Id = id;

    public async Task<IEnumerable<Class>> SearchAsync(
        string? name, 
        string? teacher, 
        int pageNumber, 
        int pageSize, 
        CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            
            var query = _entities.Values.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(teacher))
                query = query.Where(c => c.Teacher.Contains(teacher, StringComparison.OrdinalIgnoreCase));

            return query
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }, ct);
    }

    public async Task<int> GetTotalCountAsync(
        string? name, 
        string? teacher, 
        CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            
            var query = _entities.Values.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(teacher))
                query = query.Where(c => c.Teacher.Contains(teacher, StringComparison.OrdinalIgnoreCase));

            return query.Count();
        }, ct);
    }

    public async Task<IEnumerable<Class>> GetClassesByStudentIdAsync(
        int studentId, 
        CancellationToken ct = default)
    {
        var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId, 1, 10, ct);
        return enrollments
            .Select(e => _entities.GetValueOrDefault(e.ClassId))
            .Where(c => c != null)!;
    }

    public async Task<decimal> GetClassAverageMarkAsync(
        int classId, 
        CancellationToken ct = default)
    {
        var marks = await _markRepository.GetMarksByClassIdAsync(classId, 1, 10, ct);
        if (!marks.Any())
            return 0;

        return marks.Average(m => m.TotalMark);
    }

    private void InitializeSampleData()
    {
        var sampleClasses = new[]
        {
            new Class { Id = 1, Name = "Mathematics 101", Teacher = "Dr. Smith", Description = "Introduction to Mathematics" },
            new Class { Id = 2, Name = "Physics 101", Teacher = "Dr. Johnson", Description = "Introduction to Physics" },
            new Class { Id = 3, Name = "Chemistry 101", Teacher = "Dr. Brown", Description = "Introduction to Chemistry" }
        };

        foreach (var @class in sampleClasses)
        {
            _entities.TryAdd(@class.Id, @class);
        }
    }
}