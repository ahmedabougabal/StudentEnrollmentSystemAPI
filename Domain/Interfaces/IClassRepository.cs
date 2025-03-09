using UniversityEnrollmentSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UniversityEnrollmentSystem.Domain.Interfaces;

public interface IClassRepository : IRepository<Class>
{
    new Task<Class?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Class>> SearchAsync(string? name, string? teacher, int pageNumber, int pageSize, CancellationToken ct = default);
    Task<int> GetTotalCountAsync(string? name = null, string? teacher = null, CancellationToken ct = default);
    new Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    new Task AddAsync(Class entity, CancellationToken ct = default);
    new Task UpdateAsync(Class entity, CancellationToken ct = default);
    new Task DeleteAsync(Class entity, CancellationToken ct = default);
    Task<IEnumerable<Class>> GetClassesByStudentIdAsync(int studentId, CancellationToken ct = default);
    Task<decimal> GetClassAverageMarkAsync(int classId, CancellationToken ct = default);
}