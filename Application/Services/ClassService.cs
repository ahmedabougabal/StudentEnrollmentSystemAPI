using UniversityEnrollmentSystem.Application.DTOs;
using UniversityEnrollmentSystem.Application.Exceptions;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Domain.Interfaces;
using UniversityEnrollmentSystem.Features.Classes.Responses;

namespace UniversityEnrollmentSystem.Application.Services;

public class ClassService : IClassService
{
    private readonly IClassRepository _classRepository;
    private readonly IMarkRepository _markRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;

    public ClassService(
        IClassRepository classRepository,
        IMarkRepository markRepository,
        IEnrollmentRepository enrollmentRepository)
    {
        _classRepository = classRepository;
        _markRepository = markRepository;
        _enrollmentRepository = enrollmentRepository;
    }

    public async Task<ClassResponse?> GetClassByIdAsync(int id, CancellationToken ct = default)
    {
        var @class = await _classRepository.GetByIdAsync(id, ct);
        if (@class == null)
            return null;

        // Load related data
        @class.Enrollments = (await _enrollmentRepository.GetEnrollmentsByClassIdAsync(id, 1, 10, ct)).ToList();
        @class.Marks = (await _markRepository.GetMarksByClassIdAsync(id, 1, 10, ct)).ToList();
        
        return MapToResponse(@class);
    }

    public async Task<PaginatedResult<ClassResponse>> GetClassesAsync(
        string? name = null, 
        string? teacher = null, 
        int pageNumber = 1, 
        int pageSize = 10, 
        CancellationToken ct = default)
    {
        var classes = await _classRepository.SearchAsync(name, teacher, pageNumber, pageSize, ct);
        var totalCount = await _classRepository.GetTotalCountAsync(name, teacher, ct);
        
        return PaginatedResult<ClassResponse>.Create(
            classes.Select(MapToResponse),
            totalCount,
            pageNumber,
            pageSize);
    }

    public async Task<ClassResponse> CreateClassAsync(Class @class, CancellationToken ct = default)
    {
        ValidateClass(@class);
        await _classRepository.AddAsync(@class, ct);
        return MapToResponse(@class);
    }

    public async Task<ClassResponse> UpdateClassAsync(Class @class, CancellationToken ct = default)
    {
        if (!await _classRepository.ExistsAsync(@class.Id, ct))
            throw new NotFoundException(nameof(Class), @class.Id);
            
        ValidateClass(@class);
        await _classRepository.UpdateAsync(@class, ct);
        return MapToResponse(@class);
    }

    public async Task<bool> DeleteClassAsync(int id, CancellationToken ct = default)
    {
        var @class = await _classRepository.GetByIdAsync(id, ct);
        if (@class == null)
            return false;
            
        // Check if class has enrollments or marks
        var enrollments = await _enrollmentRepository.GetEnrollmentsByClassIdAsync(id, 1, 10, ct);
        var marks = await _markRepository.GetMarksByClassIdAsync(id, 1, 10, ct);
        
        if (enrollments.Any() || marks.Any())
            throw new ValidationException("Cannot delete class with existing enrollments or marks");
            
        await _classRepository.DeleteAsync(@class, ct);
        return true;
    }

    public async Task<bool> ClassExistsAsync(int id, CancellationToken ct = default)
        => await _classRepository.ExistsAsync(id, ct);

    public async Task<decimal> GetClassAverageMarkAsync(int classId, CancellationToken ct = default)
    {
        if (!await ClassExistsAsync(classId, ct))
            throw new NotFoundException(nameof(Class), classId);
            
        return await _markRepository.GetClassAverageMarkAsync(classId, ct);
    }

    public async Task<IEnumerable<Student>> GetEnrolledStudentsAsync(int classId, CancellationToken ct = default)
    {
        if (!await ClassExistsAsync(classId, ct))
            throw new NotFoundException(nameof(Class), classId);
            
        var enrollments = await _enrollmentRepository.GetEnrollmentsByClassIdAsync(classId, 1, 10, ct);
        return enrollments.Select(e => e.Student).Where(s => s != null)!;
    }

    private static void ValidateClass(Class @class)
    {
        if (string.IsNullOrWhiteSpace(@class.Name))
            throw new ValidationException("Class name is required");
        if (string.IsNullOrWhiteSpace(@class.Teacher))
            throw new ValidationException("Teacher name is required");
        if (string.IsNullOrWhiteSpace(@class.Description))
            throw new ValidationException("Class description is required");
    }

    private static ClassResponse MapToResponse(Class @class)
    {
        return new ClassResponse
        {
            Id = @class.Id,
            Name = @class.Name,
            Teacher = @class.Teacher,
            Description = @class.Description,
            EnrollmentCount = @class.Enrollments?.Count ?? 0,
            MarksCount = @class.Marks?.Count ?? 0
        };
    }
}