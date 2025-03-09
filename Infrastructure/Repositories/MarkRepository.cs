using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UniversityEnrollmentSystem.Infrastructure.Repositories;

public class MarkRepository : InMemoryRepository<Mark>, IMarkRepository
{
    protected override int GetEntityId(Mark entity) => entity.Id;
    protected override void SetEntityId(Mark entity, int id) => entity.Id = id;

    public async Task<IEnumerable<Mark>> GetMarksByStudentIdAsync(
        int studentId, 
        int pageNumber = 1, 
        int pageSize = 10, 
        CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            return _entities.Values
                .Where(m => m.StudentId == studentId)
                .OrderByDescending(m => m.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }, ct);
    }

    public async Task<IEnumerable<Mark>> GetMarksByClassIdAsync(
        int classId, 
        int pageNumber = 1, 
        int pageSize = 10, 
        CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            return _entities.Values
                .Where(m => m.ClassId == classId)
                .OrderByDescending(m => m.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }, ct);
    }

    public async Task<Mark?> GetStudentMarkInClassAsync(int studentId, int classId, CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            return _entities.Values
                .FirstOrDefault(m => 
                    m.StudentId == studentId && 
                    m.ClassId == classId);
        }, ct);
    }

    public async Task<decimal> GetStudentAverageMarkAsync(int studentId, CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            var marks = _entities.Values.Where(m => m.StudentId == studentId);
            return marks.Any() ? marks.Average(m => m.TotalMark) : 0;
        }, ct);
    }

    public async Task<decimal> GetClassAverageMarkAsync(int classId, CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            var marks = _entities.Values.Where(m => m.ClassId == classId);
            return marks.Any() ? marks.Average(m => m.TotalMark) : 0;
        }, ct);
    }

    public async Task<bool> HasExistingMarkAsync(int studentId, int classId, CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            return _entities.Values.Any(m => 
                m.StudentId == studentId && 
                m.ClassId == classId);
        }, ct);
    }

    public async Task<int> GetTotalCountByStudentIdAsync(int studentId, CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            return _entities.Values.Count(m => m.StudentId == studentId);
        }, ct);
    }

    public async Task<int> GetTotalCountByClassIdAsync(int classId, CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            return _entities.Values.Count(m => m.ClassId == classId);
        }, ct);
    }

    private void InitializeSampleData()
    {
        var sampleMarks = new[]
        {
            new Mark { Id = 1, StudentId = 1, ClassId = 1, ExamMark = 85, AssignmentMark = 90 },
            new Mark { Id = 2, StudentId = 1, ClassId = 2, ExamMark = 78, AssignmentMark = 88 },
            new Mark { Id = 3, StudentId = 2, ClassId = 1, ExamMark = 92, AssignmentMark = 95 }
        };

        foreach (var mark in sampleMarks)
        {
            _entities.TryAdd(mark.Id, mark);
        }
    }
}