using UniversityEnrollmentSystem.Application.DTOs;
using UniversityEnrollmentSystem.Application.Exceptions;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Domain.Entities;
using UniversityEnrollmentSystem.Domain.Interfaces;
using UniversityEnrollmentSystem.Features.Students.Requests;
using UniversityEnrollmentSystem.Features.Students.Responses;

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

    public async Task<StudentResponse?> GetStudentByIdAsync(int id, CancellationToken ct = default)
    {
        var student = await _studentRepository.GetByIdAsync(id, ct);
        if (student == null)
            return null;
        
        // Load related data
        student.Marks = (await _markRepository.GetMarksByStudentIdAsync(id, 1, 10, ct)).ToList();
        student.Enrollments = (await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(id, 1, 10, ct)).ToList();
        
        return MapToResponse(student);
    }

    public async Task<PaginatedResult<StudentResponse>> GetStudentsAsync(
        int pageNumber = 1, 
        int pageSize = 10, 
        string? searchTerm = null, 
        CancellationToken ct = default)
    {
        var students = await _studentRepository.SearchAsync(searchTerm, null, pageNumber, pageSize, ct);
        var totalCount = await _studentRepository.GetTotalCountAsync(searchTerm, ct);
        
        return PaginatedResult<StudentResponse>.Create(
            students.Select(MapToResponse),
            totalCount,
            pageNumber,
            pageSize);
    }

    public async Task<StudentResponse> CreateStudentAsync(CreateStudentRequest request, CancellationToken ct = default)
    {
        var student = new Student
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Age = request.Age
        };

        ValidateStudent(student);
        await _studentRepository.AddAsync(student, ct);
        return MapToResponse(student);
    }

    public async Task<StudentResponse> UpdateStudentAsync(UpdateStudentRequest request, CancellationToken ct = default)
    {
        var student = await _studentRepository.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException(nameof(Student), request.Id);

        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        student.Age = request.Age;
            
        ValidateStudent(student);
        await _studentRepository.UpdateAsync(student, ct);
        return MapToResponse(student);
    }

    public async Task<bool> DeleteStudentAsync(int id, CancellationToken ct = default)
    {
        var student = await _studentRepository.GetByIdAsync(id, ct);
        if (student == null)
            return false;
            
        // Check if student has enrollments or marks
        var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAsync(id, 1, 10, ct);
        var marks = await _markRepository.GetMarksByStudentIdAsync(id, 1, 10, ct);
        
        if (enrollments.Any() || marks.Any())
            throw new ValidationException("Cannot delete student with existing enrollments or marks");
            
        await _studentRepository.DeleteAsync(student, ct);
        return true;
    }

    private static StudentResponse MapToResponse(Student student)
    {
        return new StudentResponse
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Age = student.Age
        };
    }

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