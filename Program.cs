using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using UniversityEnrollmentSystem.Application.Interfaces;
using UniversityEnrollmentSystem.Application.Services;
using UniversityEnrollmentSystem.Domain.Interfaces;
using UniversityEnrollmentSystem.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add FastEndpoints
builder.Services.AddFastEndpoints();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Authorization services
builder.Services.AddAuthorization();

// Register Services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IMarkService, MarkService>();

// Register Repositories
builder.Services.AddSingleton<IStudentRepository, StudentRepository>();
builder.Services.AddSingleton<IClassRepository, ClassRepository>();
builder.Services.AddSingleton<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddSingleton<IMarkRepository, MarkRepository>();

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDefaultExceptionHandler();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Use FastEndpoints
app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
    c.Serializer.Options.PropertyNamingPolicy = null;
});

app.Run();
