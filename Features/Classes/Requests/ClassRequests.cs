using System.ComponentModel.DataAnnotations;

namespace UniversityEnrollmentSystem.Features.Classes.Requests;

public class CreateClassRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = default!;

    [Required]
    [StringLength(100)]
    public string Teacher { get; set; } = default!;

    [StringLength(500)]
    public string Description { get; set; } = default!;
}

public class UpdateClassRequest : CreateClassRequest
{
    [Required]
    public int Id { get; set; }
}

public class ClassSearchRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Name { get; set; }
    public string? Teacher { get; set; }
}