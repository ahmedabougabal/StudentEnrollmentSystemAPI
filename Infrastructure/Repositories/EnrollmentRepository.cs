using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UniversityEnrollmentSystem.Infrastructure.Repositories;

public class EnrollmentRepository : InMemoryRepository<Enrollment>, IEnrollmentRepository
{
    protected override int GetEntityId(Enrollment entity) => entity.Id;
    protected override void SetEntityId(Enrollment entity, int id) => entity.Id = id;

    public async Task<bool> IsStudentEnrolledInClassAsync(int studentId, int classId, CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            return _entities.Values.Any(e => 
                e.StudentId == studentId && 
                e.ClassId == classId);
        }, ct);
    }

    public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentIdAsync(
        int studentId, 
        int pageNumber = 1, 
        int pageSize = 10, 
        CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            return _entities.Values
                .Where(e => e.StudentId == studentId)
                .OrderByDescending(e => e.EnrollmentDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }, ct);
    }

    public async Task<IEnumerable<Enrollment>> GetEnrollmentsByClassIdAsync(
        int classId, 
        int pageNumber = 1, 
        int pageSize = 10, 
        CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            return _entities.Values
                .Where(e => e.ClassId == classId)
                .OrderByDescending(e => e.EnrollmentDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }, ct);
    }

    public async Task<DateTime?> GetEnrollmentDateAsync(int studentId, int classId, CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            var enrollment = _entities.Values
                .FirstOrDefault(e => 
                    e.StudentId == studentId && 
                    e.ClassId == classId);
            return enrollment?.EnrollmentDate;
        }, ct);
    }

    public async Task<int> GetTotalCountByStudentIdAsync(int studentId, CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            return _entities.Values.Count(e => e.StudentId == studentId);
        }, ct);
    }

    public async Task<int> GetTotalCountByClassIdAsync(int classId, CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            return _entities.Values.Count(e => e.ClassId == classId);
        }, ct);
    }
}