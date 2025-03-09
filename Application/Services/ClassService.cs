using UniversityEnrollmentSystem.Application.DTOs;
using UniversityEnrollmentSystem.Application.Exceptions;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Domain.Interfaces;

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

    public async Task<Class?> GetClassByIdAsync(int id)
    {
        var @class = await _classRepository.GetByIdAsync(id);
        if (@class == null)
            throw new NotFoundException(nameof(Class), id);

        // Load related data
        var enrollments = await _enrollmentRepository.GetEnrollmentsByClassIdAsync(id);
        var marks = await _markRepository.GetMarksByClassIdAsync(id);
        
        @class.Enrollments = enrollments.ToList();
        @class.Marks = marks.ToList();
        
        return @class;
    }

    public async Task<PaginatedResult<Class>> GetClassesAsync(string? name = null, string? teacher = null, int page = 1, int pageSize = 10)
    {
        var classes = await _classRepository.SearchAsync(name, teacher, page, pageSize);
        var totalCount = await _classRepository.GetTotalCountAsync(name, teacher);
        
        return new PaginatedResult<Class>
        {
            Items = classes,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<Class> CreateClassAsync(Class @class)
    {
        ValidateClass(@class);
        await _classRepository.AddAsync(@class);
        return @class;
    }

    public async Task UpdateClassAsync(Class @class)
    {
        if (!await _classRepository.ExistsAsync(@class.Id))
            throw new NotFoundException(nameof(Class), @class.Id);
            
        ValidateClass(@class);
        await _classRepository.UpdateAsync(@class);
    }

    public async Task DeleteClassAsync(int id)
    {
        var @class = await GetClassByIdAsync(id) 
            ?? throw new NotFoundException(nameof(Class), id);
            
        // Check if class has enrollments or marks
        var enrollments = await _enrollmentRepository.GetEnrollmentsByClassIdAsync(id);
        var marks = await _markRepository.GetMarksByClassIdAsync(id);
        
        if (enrollments.Any() || marks.Any())
            throw new ValidationException("Cannot delete class with existing enrollments or marks");
            
        await _classRepository.DeleteAsync(@class);
    }

    public async Task<bool> ClassExistsAsync(int id)
        => await _classRepository.ExistsAsync(id);

    public async Task<decimal> GetClassAverageMarkAsync(int classId)
    {
        if (!await ClassExistsAsync(classId))
            throw new NotFoundException(nameof(Class), classId);
            
        return await _markRepository.GetClassAverageMarkAsync(classId);
    }

    public async Task<IEnumerable<Student>> GetEnrolledStudentsAsync(int classId)
    {
        if (!await ClassExistsAsync(classId))
            throw new NotFoundException(nameof(Class), classId);
            
        var enrollments = await _enrollmentRepository.GetEnrollmentsByClassIdAsync(classId);
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
}