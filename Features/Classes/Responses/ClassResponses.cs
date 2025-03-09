// Features/Classes/Responses/ClassResponses.cs
using System.Collections.Generic;
using System;

namespace UniversityEnrollmentSystem.Features.Classes.Responses;

public class ClassesResponse
{
    public IEnumerable<ClassResponse> Classes { get; set; } = new List<ClassResponse>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}

public class ClassResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Teacher { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int EnrollmentCount { get; set; }
    public int MarksCount { get; set; }
}

public class ClassDetailsResponse : ClassResponse
{
    public int EnrolledStudentsCount { get; set; }
    public decimal AverageMark { get; set; }
    public IEnumerable<StudentSummaryResponse> EnrolledStudents { get; set; } = new List<StudentSummaryResponse>();
}

public class StudentSummaryResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = default!;
    public decimal? Mark { get; set; }
}