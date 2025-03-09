using UniversityEnrollmentSystem.Application.DTOs;
using UniversityEnrollmentSystem.Application.Exceptions;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Domain.Interfaces;

namespace UniversityEnrollmentSystem.Application.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IClassRepository _classRepository;

    public EnrollmentService(
        IEnrollmentRepository enrollmentRepository,
        IStudentRepository studentRepository,
        IClassRepository classRepository)
    {
        _enrollmentRepository = enrollmentRepository;
        _studentRepository = studentRepository;
        _classRepository = classRepository;
    }

    public async Task<Enrollment?> GetEnrollmentByIdAsync(int id)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(id);
        if (enrollment == null)
            throw new NotFoundException(nameof(Enrollment), id);

        await LoadEnrollmentRelations(enrollment);
        return enrollment;
    }

    public async Task<PaginatedResult<Enrollment>> GetEnrollmentsByStudentIdAsync(int studentId, int page = 1, int pageSize = 10)
    {
        if (!await _studentRepository.ExistsAsync(studentId))
            throw new NotFoundException(nameof(Student), studentId);

        var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(studentId, page, pageSize);
        var totalCount = await _enrollmentRepository.GetTotalCountByStudentIdAsync(studentId);

        foreach (var enrollment in enrollments)
        {
            await LoadEnrollmentRelations(enrollment);
        }

        return new PaginatedResult<Enrollment>
        {
            Items = enrollments,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<PaginatedResult<Enrollment>> GetEnrollmentsByClassIdAsync(int classId, int page = 1, int pageSize = 10)
    {
        if (!await _classRepository.ExistsAsync(classId))
            throw new NotFoundException(nameof(Class), classId);

        var enrollments = await _enrollmentRepository.GetEnrollmentsByClassIdAsync(classId, page, pageSize);
        var totalCount = await _enrollmentRepository.GetTotalCountByClassIdAsync(classId);

        foreach (var enrollment in enrollments)
        {
            await LoadEnrollmentRelations(enrollment);
        }

        return new PaginatedResult<Enrollment>
        {
            Items = enrollments,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<Enrollment> CreateEnrollmentAsync(Enrollment enrollment)
    {
        await ValidateEnrollmentAsync(enrollment);
        
        enrollment.EnrollmentDate = DateTime.UtcNow;
        await _enrollmentRepository.AddAsync(enrollment);
        
        await LoadEnrollmentRelations(enrollment);
        return enrollment;
    }

    public async Task<bool> IsStudentEnrolledInClassAsync(int studentId, int classId)
    {
        if (!await _studentRepository.ExistsAsync(studentId))
            throw new NotFoundException(nameof(Student), studentId);
            
        if (!await _classRepository.ExistsAsync(classId))
            throw new NotFoundException(nameof(Class), classId);
            
        return await _enrollmentRepository.IsStudentEnrolledInClassAsync(studentId, classId);
    }

    public async Task<bool> EnrollmentExistsAsync(int id)
        => await _enrollmentRepository.ExistsAsync(id);

    private async Task ValidateEnrollmentAsync(Enrollment enrollment)
    {
        if (!await _studentRepository.ExistsAsync(enrollment.StudentId))
            throw new NotFoundException(nameof(Student), enrollment.StudentId);

        if (!await _classRepository.ExistsAsync(enrollment.ClassId))
            throw new NotFoundException(nameof(Class), enrollment.ClassId);

        if (await _enrollmentRepository.IsStudentEnrolledInClassAsync(enrollment.StudentId, enrollment.ClassId))
            throw new ValidationException($"Student {enrollment.StudentId} is already enrolled in class {enrollment.ClassId}");

        var existingEnrollmentDate = await _enrollmentRepository.GetEnrollmentDateAsync(enrollment.StudentId, enrollment.ClassId);
        if (existingEnrollmentDate.HasValue)
            throw new ValidationException($"Student was previously enrolled in this class on {existingEnrollmentDate.Value:d}");
    }

    private async Task LoadEnrollmentRelations(Enrollment enrollment)
    {
        enrollment.Student = await _studentRepository.GetByIdAsync(enrollment.StudentId);
        enrollment.Class = await _classRepository.GetByIdAsync(enrollment.ClassId);
    }
}
