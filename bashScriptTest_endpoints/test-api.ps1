#!/usr/bin/env pwsh
# API Testing Script for Student Enrollment System
# This script tests all endpoints and validates their responses

$baseUrl = "http://localhost:5260"
$studentId = 0
$classId = 0
$enrollmentId = 0
$markId = 0

# Helper function to make HTTP requests and display results
function Invoke-ApiRequest {
    param (
        [string]$Method,
        [string]$Endpoint,
        [object]$Body = $null,
        [string]$Description = ""
    )
    
    $url = "$baseUrl$Endpoint"
    Write-Host "Testing: $Description" -ForegroundColor Cyan
    Write-Host "$Method $url" -ForegroundColor Gray
    
    $params = @{
        Method = $Method
        Uri = $url
        ContentType = "application/json"
    }
    
    if ($Body) {
        $jsonBody = $Body | ConvertTo-Json -Depth 10
        $params.Body = $jsonBody
        Write-Host "Request Body: $jsonBody" -ForegroundColor Gray
    }
    
    try {
        $response = Invoke-RestMethod @params -ErrorVariable responseError
        Write-Host "Status: Success" -ForegroundColor Green
        Write-Host "Response: $($response | ConvertTo-Json -Depth 5)" -ForegroundColor Green
        return $response
    }
    catch {
        $statusCode = $_.Exception.Response.StatusCode.value__
        Write-Host "Status: Failed ($statusCode)" -ForegroundColor Red
        if ($responseError) {
            Write-Host "Error: $responseError" -ForegroundColor Red
        }
        return $null
    }
    finally {
        Write-Host "--------------------------------------------------" -ForegroundColor DarkGray
    }
}

# 1. Test Student Endpoints
Write-Host "TESTING STUDENT ENDPOINTS" -ForegroundColor Yellow

# 1.1 Create a student
$createStudentResponse = Invoke-ApiRequest -Method "POST" -Endpoint "/students" -Body @{
    FirstName = "John"
    LastName = "Doe"
    Age = 20
} -Description "Create a new student"

if ($createStudentResponse) {
    $studentId = $createStudentResponse.Id
    Write-Host "Created Student ID: $studentId" -ForegroundColor Magenta
}

# Create a second student for testing
$createStudentResponse2 = Invoke-ApiRequest -Method "POST" -Endpoint "/students" -Body @{
    FirstName = "Jane"
    LastName = "Doe"
    Age = 19
} -Description "Create a second student"

if ($createStudentResponse2) {
    $studentId2 = $createStudentResponse2.Id
    Write-Host "Created Second Student ID: $studentId2" -ForegroundColor Magenta
}

# 1.2 Get all students
Invoke-ApiRequest -Method "GET" -Endpoint "/students" -Description "Get all students"

# 1.3 Get student by ID
if ($studentId -gt 0) {
    Invoke-ApiRequest -Method "GET" -Endpoint "/students/$studentId" -Description "Get student by ID"
}

# 1.4 Update student
if ($studentId -gt 0) {
    Invoke-ApiRequest -Method "PUT" -Endpoint "/students/$studentId" -Body @{
        FirstName = "John"
        LastName = "Smith"
        Age = 21
    } -Description "Update student"
}

# 1.5 Test student filtering and pagination
Invoke-ApiRequest -Method "GET" -Endpoint "/students?pageNumber=1&pageSize=10&searchTerm=Smith" -Description "Get students with filtering and pagination"

# 2. Test Class Endpoints
Write-Host "TESTING CLASS ENDPOINTS" -ForegroundColor Yellow

# 2.1 Create a class
$createClassResponse = Invoke-ApiRequest -Method "POST" -Endpoint "/classes" -Body @{
    Name = "Mathematics 101"
    Teacher = "Prof. Einstein"
    Description = "Introduction to Mathematics"
} -Description "Create a new class"

if ($createClassResponse) {
    $classId = $createClassResponse.Id
    Write-Host "Created Class ID: $classId" -ForegroundColor Magenta
}

# Create a second class for testing
$createClassResponse2 = Invoke-ApiRequest -Method "POST" -Endpoint "/classes" -Body @{
    Name = "Physics 101"
    Teacher = "Prof. Newton"
    Description = "Introduction to Physics"
} -Description "Create a second class"

if ($createClassResponse2) {
    $classId2 = $createClassResponse2.Id
    Write-Host "Created Second Class ID: $classId2" -ForegroundColor Magenta
}

# 2.2 Get all classes
Invoke-ApiRequest -Method "GET" -Endpoint "/classes" -Description "Get all classes"

# 2.3 Get class by ID
if ($classId -gt 0) {
    Invoke-ApiRequest -Method "GET" -Endpoint "/classes/$classId" -Description "Get class by ID"
}

# 2.4 Test class filtering and pagination
Invoke-ApiRequest -Method "GET" -Endpoint "/classes?pageNumber=1&pageSize=10&searchTerm=Math" -Description "Get classes with filtering and pagination"

# 3. Test Enrollment Endpoints
Write-Host "TESTING ENROLLMENT ENDPOINTS" -ForegroundColor Yellow

# 3.1 Create enrollments
if ($studentId -gt 0 -and $classId -gt 0) {
    $createEnrollmentResponse = Invoke-ApiRequest -Method "POST" -Endpoint "/enrollments" -Body @{
        StudentId = $studentId
        ClassId = $classId
    } -Description "Create a new enrollment"
    
    if ($createEnrollmentResponse) {
        $enrollmentId = $createEnrollmentResponse.Id
        Write-Host "Created Enrollment ID: $enrollmentId" -ForegroundColor Magenta
    }
}

# Create a second enrollment
if ($studentId -gt 0 -and $classId2 -gt 0) {
    $createEnrollmentResponse2 = Invoke-ApiRequest -Method "POST" -Endpoint "/enrollments" -Body @{
        StudentId = $studentId
        ClassId = $classId2
    } -Description "Create a second enrollment"
    
    if ($createEnrollmentResponse2) {
        $enrollmentId2 = $createEnrollmentResponse2.Id
        Write-Host "Created Second Enrollment ID: $enrollmentId2" -ForegroundColor Magenta
    }
}

# Create a third enrollment
if ($studentId2 -gt 0 -and $classId -gt 0) {
    $createEnrollmentResponse3 = Invoke-ApiRequest -Method "POST" -Endpoint "/enrollments" -Body @{
        StudentId = $studentId2
        ClassId = $classId
    } -Description "Create a third enrollment"
    
    if ($createEnrollmentResponse3) {
        $enrollmentId3 = $createEnrollmentResponse3.Id
        Write-Host "Created Third Enrollment ID: $enrollmentId3" -ForegroundColor Magenta
    }
}

# 3.2 Get all enrollments
Invoke-ApiRequest -Method "GET" -Endpoint "/enrollments" -Description "Get all enrollments"

# 3.3 Test enrollment filtering and pagination
if ($studentId -gt 0) {
    Invoke-ApiRequest -Method "GET" -Endpoint "/enrollments?pageNumber=1&pageSize=10&studentId=$studentId" -Description "Get enrollments with student filtering"
}

if ($classId -gt 0) {
    Invoke-ApiRequest -Method "GET" -Endpoint "/enrollments?pageNumber=1&pageSize=10&classId=$classId" -Description "Get enrollments with class filtering"
}

# 4. Test Mark Endpoints
Write-Host "TESTING MARK ENDPOINTS" -ForegroundColor Yellow

# 4.1 Create marks
if ($studentId -gt 0 -and $classId -gt 0) {
    $createMarkResponse = Invoke-ApiRequest -Method "POST" -Endpoint "/marks" -Body @{
        StudentId = $studentId
        ClassId = $classId
        ExamMark = 85
        AssignmentMark = 90
    } -Description "Create a new mark"
    
    if ($createMarkResponse) {
        $markId = $createMarkResponse.Id
        Write-Host "Created Mark ID: $markId" -ForegroundColor Magenta
    }
}

# Create a second mark
if ($studentId -gt 0 -and $classId2 -gt 0) {
    $createMarkResponse2 = Invoke-ApiRequest -Method "POST" -Endpoint "/marks" -Body @{
        StudentId = $studentId
        ClassId = $classId2
        ExamMark = 75
        AssignmentMark = 80
    } -Description "Create a second mark"
    
    if ($createMarkResponse2) {
        $markId2 = $createMarkResponse2.Id
        Write-Host "Created Second Mark ID: $markId2" -ForegroundColor Magenta
    }
}

# Create a third mark
if ($studentId2 -gt 0 -and $classId -gt 0) {
    $createMarkResponse3 = Invoke-ApiRequest -Method "POST" -Endpoint "/marks" -Body @{
        StudentId = $studentId2
        ClassId = $classId
        ExamMark = 90
        AssignmentMark = 95
    } -Description "Create a third mark"
    
    if ($createMarkResponse3) {
        $markId3 = $createMarkResponse3.Id
        Write-Host "Created Third Mark ID: $markId3" -ForegroundColor Magenta
    }
}

# 4.2 Get all marks
Invoke-ApiRequest -Method "GET" -Endpoint "/marks" -Description "Get all marks"

# 4.3 Test mark filtering and pagination
if ($studentId -gt 0) {
    Invoke-ApiRequest -Method "GET" -Endpoint "/marks?pageNumber=1&pageSize=10&studentId=$studentId" -Description "Get marks with student filtering"
}

if ($classId -gt 0) {
    Invoke-ApiRequest -Method "GET" -Endpoint "/marks?pageNumber=1&pageSize=10&classId=$classId" -Description "Get marks with class filtering"
}

# 4.4 Get class average marks
if ($classId -gt 0) {
    Invoke-ApiRequest -Method "GET" -Endpoint "/classes/$classId/average-marks" -Description "Get class average marks"
}

if ($classId2 -gt 0) {
    Invoke-ApiRequest -Method "GET" -Endpoint "/classes/$classId2/average-marks" -Description "Get second class average marks"
}

# 5. Test Student Report
Write-Host "TESTING STUDENT REPORT" -ForegroundColor Yellow

# 5.1 Get student reports
if ($studentId -gt 0) {
    Invoke-ApiRequest -Method "GET" -Endpoint "/students/$studentId/report" -Description "Get student report"
}

if ($studentId2 -gt 0) {
    Invoke-ApiRequest -Method "GET" -Endpoint "/students/$studentId2/report" -Description "Get second student report"
}

# 6. Test Validation
Write-Host "TESTING VALIDATION" -ForegroundColor Yellow

# 6.1 Test invalid student ID
Invoke-ApiRequest -Method "GET" -Endpoint "/students/0/report" -Description "Test invalid student ID validation"

# 6.2 Test invalid class ID
Invoke-ApiRequest -Method "GET" -Endpoint "/classes/0/average-marks" -Description "Test invalid class ID validation"

# 6.3 Test invalid enrollment data
if ($classId -gt 0) {
    Invoke-ApiRequest -Method "POST" -Endpoint "/enrollments" -Body @{
        StudentId = 0
        ClassId = $classId
    } -Description "Test invalid student ID in enrollment"
}

if ($studentId -gt 0) {
    Invoke-ApiRequest -Method "POST" -Endpoint "/enrollments" -Body @{
        StudentId = $studentId
        ClassId = 0
    } -Description "Test invalid class ID in enrollment"
}

# 6.4 Test invalid mark data
if ($classId -gt 0) {
    Invoke-ApiRequest -Method "POST" -Endpoint "/marks" -Body @{
        StudentId = 0
        ClassId = $classId
        ExamMark = 85
        AssignmentMark = 90
    } -Description "Test invalid student ID in mark"
}

if ($studentId -gt 0) {
    Invoke-ApiRequest -Method "POST" -Endpoint "/marks" -Body @{
        StudentId = $studentId
        ClassId = 0
        ExamMark = 85
        AssignmentMark = 90
    } -Description "Test invalid class ID in mark"
    
    if ($classId -gt 0) {
        Invoke-ApiRequest -Method "POST" -Endpoint "/marks" -Body @{
            StudentId = $studentId
            ClassId = $classId
            ExamMark = -10
            AssignmentMark = 90
        } -Description "Test invalid exam mark"
        
        Invoke-ApiRequest -Method "POST" -Endpoint "/marks" -Body @{
            StudentId = $studentId
            ClassId = $classId
            ExamMark = 85
            AssignmentMark = 110
        } -Description "Test invalid assignment mark"
    }
}

# 7. Cleanup
Write-Host "CLEANUP" -ForegroundColor Yellow

# 7.1 Delete students
if ($studentId -gt 0) {
    Invoke-ApiRequest -Method "DELETE" -Endpoint "/students/$studentId" -Description "Delete student"
}

if ($studentId2 -gt 0) {
    Invoke-ApiRequest -Method "DELETE" -Endpoint "/students/$studentId2" -Description "Delete second student"
}

# 7.2 Delete classes
if ($classId -gt 0) {
    Invoke-ApiRequest -Method "DELETE" -Endpoint "/classes/$classId" -Description "Delete class"
}

if ($classId2 -gt 0) {
    Invoke-ApiRequest -Method "DELETE" -Endpoint "/classes/$classId2" -Description "Delete second class"
}

Write-Host "API TESTING COMPLETED" -ForegroundColor Yellow
