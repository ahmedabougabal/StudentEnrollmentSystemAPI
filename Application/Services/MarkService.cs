using UniversityEnrollmentSystem.Application.DTOs;
using UniversityEnrollmentSystem.Application.Exceptions;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Domain.Interfaces;

namespace UniversityEnrollmentSystem.Application.Services;

public class MarkService : IMarkService
{
    private readonly IMarkRepository _markRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IClassRepository _classRepository;

    public MarkService(
        IMarkRepository markRepository,
        IEnrollmentRepository enrollmentRepository,
        IStudentRepository studentRepository,
        IClassRepository classRepository)
    {
        _markRepository = markRepository;
        _enrollmentRepository = enrollmentRepository;
        _studentRepository = studentRepository;
        _classRepository = classRepository;
    }

    public async Task<Mark?> GetMarkByIdAsync(int id)
    {
        var mark = await _markRepository.GetByIdAsync(id);
        if (mark == null)
            throw new NotFoundException(nameof(Mark), id);

        await LoadMarkRelations(mark);
        return mark;
    }

    public async Task<PaginatedResult<Mark>> GetMarksByStudentIdAsync(int studentId, int page = 1, int pageSize = 10)
    {
        if (!await _studentRepository.ExistsAsync(studentId))
            throw new NotFoundException(nameof(Student), studentId);

        var marks = await _markRepository.GetMarksByStudentIdAsync(studentId, page, pageSize);
        var totalCount = await _markRepository.GetTotalCountByStudentIdAsync(studentId);

        foreach (var mark in marks)
        {
            await LoadMarkRelations(mark);
        }

        return new PaginatedResult<Mark>
        {
            Items = marks,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<PaginatedResult<Mark>> GetMarksByClassIdAsync(int classId, int page = 1, int pageSize = 10)
    {
        if (!await _classRepository.ExistsAsync(classId))
            throw new NotFoundException(nameof(Class), classId);

        var marks = await _markRepository.GetMarksByClassIdAsync(classId, page, pageSize);
        var totalCount = await _markRepository.GetTotalCountByClassIdAsync(classId);

        foreach (var mark in marks)
        {
            await LoadMarkRelations(mark);
        }

        return new PaginatedResult<Mark>
        {
            Items = marks,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<Mark> CreateMarkAsync(Mark mark)
    {
        await ValidateMarkAsync(mark);
        await _markRepository.AddAsync(mark);
        await LoadMarkRelations(mark);
        return mark;
    }

    public async Task UpdateMarkAsync(Mark mark)
    {
        if (!await _markRepository.ExistsAsync(mark.Id))
            throw new NotFoundException(nameof(Mark), mark.Id);

        await ValidateMarkAsync(mark);
        await _markRepository.UpdateAsync(mark);
    }

    public async Task<bool> MarkExistsAsync(int id)
        => await _markRepository.ExistsAsync(id);

    public async Task<decimal> CalculateStudentAverageAsync(int studentId, int? classId = null)
    {
        if (!await _studentRepository.ExistsAsync(studentId))
            throw new NotFoundException(nameof(Student), studentId);

        if (classId.HasValue)
        {
            if (!await _classRepository.ExistsAsync(classId.Value))
                throw new NotFoundException(nameof(Class), classId.Value);

            var mark = await _markRepository.GetStudentMarkInClassAsync(studentId, classId.Value);
            return mark?.TotalMark ?? 0;
        }

        return await _markRepository.GetStudentAverageMarkAsync(studentId);
    }

    private async Task ValidateMarkAsync(Mark mark)
    {
        if (!await _studentRepository.ExistsAsync(mark.StudentId))
            throw new NotFoundException(nameof(Student), mark.StudentId);

        if (!await _classRepository.ExistsAsync(mark.ClassId))
            throw new NotFoundException(nameof(Class), mark.ClassId);

        if (!await _enrollmentRepository.IsStudentEnrolledInClassAsync(mark.StudentId, mark.ClassId))
            throw new ValidationException($"Student {mark.StudentId} is not enrolled in class {mark.ClassId}");

        if (mark.Id == 0 && await _markRepository.HasExistingMarkAsync(mark.StudentId, mark.ClassId))
            throw new ValidationException($"Mark already exists for student {mark.StudentId} in class {mark.ClassId}");

        ValidateMarkRange(mark.ExamMark, "Exam");
        ValidateMarkRange(mark.AssignmentMark, "Assignment");
    }

    private static void ValidateMarkRange(decimal mark, string type)
    {
        if (mark < 0 || mark > 100)
            throw new ValidationException($"{type} mark must be between 0 and 100");
    }

    private async Task LoadMarkRelations(Mark mark)
    {
        mark.Student = await _studentRepository.GetByIdAsync(mark.StudentId);
        mark.Class = await _classRepository.GetByIdAsync(mark.ClassId);
    }
}
