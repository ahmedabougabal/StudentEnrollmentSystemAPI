using System.Collections.Concurrent;
using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Domain.Interfaces;

namespace UniversityEnrollmentSystem.Infrastructure.Repositories;

public class StudentRepository : InMemoryRepository<Student>, IStudentRepository
{
    protected override int GetEntityId(Student entity) => entity.Id;
    protected override void SetEntityId(Student entity, int id) => entity.Id = id;

    public async Task<IEnumerable<Student>> SearchAsync(
        string? searchTerm = null, 
        int? age = null, 
        int pageNumber = 1, 
        int pageSize = 10, 
        CancellationToken ct = default)
    {
        var query = _entities.Values.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(s => 
                s.FirstName.ToLower().Contains(searchTerm) || 
                s.LastName.ToLower().Contains(searchTerm));
        }

        if (age.HasValue)
        {
            query = query.Where(s => s.Age == age.Value);
        }

        // Apply sorting and pagination
        return await Task.FromResult(
            query.OrderBy(s => s.LastName)
                 .ThenBy(s => s.FirstName)
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToList());
    }

    public async Task<int> GetTotalCountAsync(string? searchTerm = null, CancellationToken ct = default)
    {
        var query = _entities.Values.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(s => 
                s.FirstName.ToLower().Contains(searchTerm) || 
                s.LastName.ToLower().Contains(searchTerm));
        }

        return await Task.FromResult(query.Count());
    }

    public async Task<IEnumerable<Student>> GetStudentsByClassIdAsync(int classId, CancellationToken ct = default)
    {
        return await Task.FromResult(
            _entities.Values
                .Where(s => s.Enrollments.Any(e => e.ClassId == classId))
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToList());
    }

    public async Task<int> GetTotalCountByClassIdAsync(int classId, CancellationToken ct = default)
    {
        return await Task.FromResult(
            _entities.Values.Count(s => s.Enrollments.Any(e => e.ClassId == classId)));
    }
}