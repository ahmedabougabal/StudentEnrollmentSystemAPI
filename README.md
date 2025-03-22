# 🎓 Student Enrollment System API

![.NET 6+](https://img.shields.io/badge/.NET-6%2B-512BD4)
![FastEndpoints](https://img.shields.io/badge/FastEndpoints-✓-brightgreen)
![Last Updated](https://img.shields.io/badge/Last%20Updated-2025--03--22-blue)
![Author](https://img.shields.io/badge/Author-ahmedabougabal-green)
![License](https://img.shields.io/badge/License-MIT-yellow)

A modern, high-performance .NET-based Student Enrollment System API built with Clean Architecture principles and Domain-Driven Design patterns.

## Table of Contents
- [Architecture & Design Patterns](#-architecture--design-patterns)
- [Entity Relationships](#-entity-relationships)
- [Technical Stack](#-technical-stack)
- [API Structure](#-api-structure)
- [Deployment & Setup](#-deployment--setup)

## 🏗 Architecture & Design Patterns

### Clean Architecture Layers
```plaintext
┌─────────────────────────────┐
│    API (Features) Layer     │  → FastEndpoints for HTTP endpoints
├─────────────────────────────┤
│    Application Layer        │  → Business logic & use cases
├─────────────────────────────┤
│    Domain Layer            │  → Entities & business rules
├─────────────────────────────┤
│    Infrastructure Layer     │  → Data access & external services
└─────────────────────────────┘
```
---

## 🏗 Architecture Overview

The project follows a clean architecture pattern with distinct layers:

### 1. Domain Layer
- Contains core business entities:
  - `Student`
  - `Class`
  - `Enrollment`
  - `Mark`

### 2. Application Layer
- Implements business logic through services:
  - `StudentService`: Manages student operations
  - `ClassService`: Handles class management
  - `EnrollmentService`: Processes student enrollments
  - `MarkService`: Manages academic performance tracking

### 3. Infrastructure Layer
- Repository implementations for data persistence
- Uses repository pattern with interfaces:
  - `IStudentRepository`
  - `IClassRepository`
  - `IEnrollmentRepository`
  - `IMarkRepository`

### 4. API Layer (Features)
- Built using FastEndpoints for efficient API endpoint handling
- Organized by feature folders:
  - Students
  - Classes
  - Enrollments
  - Marks

## 🚀 Key Features

### Student Management
- Create, update, and delete student records
- View student details and academic history
- Generate comprehensive student reports
- Track student enrollments across multiple classes

### Class Management
- Create and manage classes
- Assign teachers to classes
- Track class enrollment statistics
- Calculate class average performance

### Enrollment System
- Enroll students in classes
- Prevent duplicate enrollments
- Track enrollment dates
- Manage class capacity

### Academic Performance Tracking
- Record and manage student marks
- Calculate individual and class averages
- Track examination and assignment scores
- Generate performance reports

## 🛠 Technical Implementation

### Modern Technology Stack
- **Framework**: .NET 6+
- **API Architecture**: REST with FastEndpoints
- **Documentation**: Swagger/OpenAPI
- **Authentication**: Built-in authorization support
- **Testing**: Includes PowerShell and Shell scripts for testing

### Best Practices

#### 1. Clean Architecture
- Clear separation of concerns
- Dependency injection
- Interface-based design

#### 2. RESTful API Design
- Consistent endpoint naming
- Proper HTTP method usage
- Standardized response formats

#### 3. Error Handling
- Custom exception types
- Meaningful error messages
- Global exception handling

#### 4. Performance Optimization
- Pagination support
- Lazy loading for related entities
- Efficient query optimization

#### 5. Data Validation
- Input validation
- Business rule enforcement
- Data consistency checks

## API Endpoints

### Students
```http
GET /api/students/{id}         # Get student details
GET /api/students/{id}/report  # Generate student report
POST /api/students            # Create new student
PUT /api/students/{id}        # Update student
DELETE /api/students/{id}     # Delete student
```

### Classes
```http
GET /api/classes/{id} - Get class details
GET /api/classes - List all classes
POST /api/classes - Create new class
PUT /api/classes/{id} - Update class
DELETE /api/classes/{id} - Delete class
```

### Enrollments
```http
POST /api/enrollments - Create new enrollment
GET /api/enrollments/{id} - Get enrollment details
GET /api/enrollments/student/{studentId} - Get student enrollments
GET /api/enrollments/class/{classId} - Get class enrollments
```


