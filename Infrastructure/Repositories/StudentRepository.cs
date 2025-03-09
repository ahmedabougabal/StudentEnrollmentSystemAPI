// Infrastructure/Repositories/StudentRepository.cs
using System.Collections.Concurrent;
using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Domain.Interfaces;

namespace UniversityEnrollmentSystem.Infrastructure.Repositories;

public class StudentRepository : InMemoryRepository<Student>, IStudentRepository
{
    protected override int GetEntityId(Student entity) => entity.Id;
    protected override void SetEntityId(Student entity, int id) => entity.Id = id;

    public async Task<IEnumerable<Student>> SearchAsync(string? name = null, int? age = null, int page = 1, int pageSize = 10)
    {
        var query = _entities.Values.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(name))
        {
            name = name.ToLower();
            query = query.Where(s => 
                s.FirstName.ToLower().Contains(name) || 
                s.LastName.ToLower().Contains(name));
        }

        if (age.HasValue)
        {
            query = query.Where(s => s.Age == age.Value);
        }

        // Apply pagination
        return await Task.FromResult(
            query.Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .ToList());
    }

    public async Task<int> GetTotalCountAsync(string? name = null, int? age = null)
    {
        var query = _entities.Values.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            name = name.ToLower();
            query = query.Where(s => 
                s.FirstName.ToLower().Contains(name) || 
                s.LastName.ToLower().Contains(name));
        }

        if (age.HasValue)
        {
            query = query.Where(s => s.Age == age.Value);
        }

        return await Task.FromResult(query.Count());
    }

    public async Task<IEnumerable<Student>> GetStudentsByClassIdAsync(int classId)
    {
        return await Task.FromResult(
            _entities.Values
                .Where(s => s.Enrollments.Any(e => e.ClassId == classId))
                .ToList());
    }
}