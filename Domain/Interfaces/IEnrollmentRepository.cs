using UniversityEnrollmentSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UniversityEnrollmentSystem.Domain.Interfaces;

public interface IEnrollmentRepository : IRepository<Enrollment>
{
    Task<bool> IsStudentEnrolledInClassAsync(int studentId, int classId, CancellationToken ct = default);
    Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentIdAsync(int studentId, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);
    Task<IEnumerable<Enrollment>> GetEnrollmentsByClassIdAsync(int classId, int pageNumber = 1, int pageSize = 10, CancellationToken ct = default);
    Task<DateTime?> GetEnrollmentDateAsync(int studentId, int classId, CancellationToken ct = default);
    Task<int> GetTotalCountByStudentIdAsync(int studentId, CancellationToken ct = default);
    Task<int> GetTotalCountByClassIdAsync(int classId, CancellationToken ct = default);
}