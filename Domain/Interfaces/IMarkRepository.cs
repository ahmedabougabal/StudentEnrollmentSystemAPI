using UniversityEnrollmentSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UniversityEnrollmentSystem.Domain.Interfaces;

public interface IMarkRepository : IRepository<Mark>
{
    Task<IEnumerable<Mark>> GetMarksByStudentIdAsync(int studentId, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);
    Task<IEnumerable<Mark>> GetMarksByClassIdAsync(int classId, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);
    Task<Mark?> GetStudentMarkInClassAsync(int studentId, int classId, CancellationToken ct = default);
    Task<decimal> GetStudentAverageMarkAsync(int studentId, CancellationToken ct = default);
    Task<decimal> GetClassAverageMarkAsync(int classId, CancellationToken ct = default);
    Task<bool> HasExistingMarkAsync(int studentId, int classId, CancellationToken ct = default);
    Task<int> GetTotalCountByStudentIdAsync(int studentId, CancellationToken ct = default);
    Task<int> GetTotalCountByClassIdAsync(int classId, CancellationToken ct = default);
}
