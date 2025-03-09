using UniversityEnrollmentSystem.Application.DTOs;
using UniversityEnrollmentSystem.Application.Exceptions;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Domain.Interfaces;

namespace UniversityEnrollmentSystem.Application.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMarkRepository _markRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;

    public StudentService(
        IStudentRepository studentRepository, 
        IMarkRepository markRepository,
        IEnrollmentRepository enrollmentRepository)
    {
        _studentRepository = studentRepository;
        _markRepository = markRepository;
        _enrollmentRepository = enrollmentRepository;
    }

    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null)
            throw new NotFoundException(nameof(Student), id);
        
        // Load related data
        student.Marks = (await _markRepository.GetMarksByStudentIdAsync(id)).ToList();
        student.Enrollments = (await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(id)).ToList();
        
        return student;
    }

    public async Task<PaginatedResult<Student>> GetStudentsAsync(string? name = null, int? age = null, int page = 1, int pageSize = 10)
    {
        var students = await _studentRepository.SearchAsync(name, age, page, pageSize);
        var totalCount = await _studentRepository.GetTotalCountAsync(name, age);
        
        return new PaginatedResult<Student>
        {
            Items = students,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<Student> CreateStudentAsync(Student student)
    {
        ValidateStudent(student);
        await _studentRepository.AddAsync(student);
        return student;
    }

    public async Task UpdateStudentAsync(Student student)
    {
        if (!await _studentRepository.ExistsAsync(student.Id))
            throw new NotFoundException(nameof(Student), student.Id);
            
        ValidateStudent(student);
        await _studentRepository.UpdateAsync(student);
    }

    public async Task DeleteStudentAsync(int id)
    {
        var student = await GetStudentByIdAsync(id) 
            ?? throw new NotFoundException(nameof(Student), id);
            
        // Check if student has enrollments or marks
        var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(id);
        var marks = await _markRepository.GetMarksByStudentIdAsync(id);
        
        if (enrollments.Any() || marks.Any())
            throw new ValidationException("Cannot delete student with existing enrollments or marks");
            
        await _studentRepository.DeleteAsync(student);
    }

    public async Task<bool> StudentExistsAsync(int id)
        => await _studentRepository.ExistsAsync(id);

    public async Task<IEnumerable<Student>> GetStudentsByClassIdAsync(int classId)
        => await _studentRepository.GetStudentsByClassIdAsync(classId);

    private static void ValidateStudent(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.FirstName))
            throw new ValidationException("First name is required");
        if (string.IsNullOrWhiteSpace(student.LastName))
            throw new ValidationException("Last name is required");
        if (student.Age < 16 || student.Age > 100)
            throw new ValidationException("Age must be between 16 and 100");
    }
}