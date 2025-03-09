#!/bin/bash
# API Testing Script for Student Enrollment System
# This script tests all endpoints and validates their responses

BASE_URL="http://localhost:5260"
STUDENT_ID=0
CLASS_ID=0
ENROLLMENT_ID=0
MARK_ID=0

# Helper function to make HTTP requests and display results
function invoke_api_request() {
    local method=$1
    local endpoint=$2
    local body=$3
    local description=$4
    
    local url="${BASE_URL}${endpoint}"
    echo -e "\e[36mTesting: $description\e[0m"
    echo -e "\e[90m$method $url\e[0m"
    
    if [ ! -z "$body" ]; then
        echo -e "\e[90mRequest Body: $body\e[0m"
        response=$(curl -s -X $method -H "Content-Type: application/json" -d "$body" $url)
        status=$?
    else
        response=$(curl -s -X $method $url)
        status=$?
    fi
    
    if [ $status -eq 0 ]; then
        echo -e "\e[32mStatus: Success\e[0m"
        echo -e "\e[32mResponse: $response\e[0m"
        echo $response
    else
        echo -e "\e[31mStatus: Failed\e[0m"
        echo -e "\e[31mError: $response\e[0m"
        echo ""
    fi
    
    echo -e "\e[90m--------------------------------------------------\e[0m"
}

# 1. Test Student Endpoints
echo -e "\e[33mTESTING STUDENT ENDPOINTS\e[0m"

# 1.1 Create a student
create_student_response=$(invoke_api_request "POST" "/students" '{"FirstName": "John", "LastName": "Doe", "Age": 20}' "Create a new student")
STUDENT_ID=$(echo $create_student_response | grep -o '"Id":[0-9]*' | grep -o '[0-9]*')

if [ ! -z "$STUDENT_ID" ]; then
    echo -e "\e[35mCreated Student ID: $STUDENT_ID\e[0m"
fi

# 1.2 Get all students
invoke_api_request "GET" "/students" "" "Get all students"

# 1.3 Get student by ID
if [ $STUDENT_ID -gt 0 ]; then
    invoke_api_request "GET" "/students/$STUDENT_ID" "" "Get student by ID"
fi

# 1.4 Update student
if [ $STUDENT_ID -gt 0 ]; then
    invoke_api_request "PUT" "/students/$STUDENT_ID" '{"FirstName": "John", "LastName": "Smith", "Age": 21}' "Update student"
fi

# Create a second student for testing
create_student_response=$(invoke_api_request "POST" "/students" '{"FirstName": "Jane", "LastName": "Doe", "Age": 19}' "Create a second student")
STUDENT_ID2=$(echo $create_student_response | grep -o '"Id":[0-9]*' | grep -o '[0-9]*')

if [ ! -z "$STUDENT_ID2" ]; then
    echo -e "\e[35mCreated Second Student ID: $STUDENT_ID2\e[0m"
fi

# 2. Test Class Endpoints
echo -e "\e[33mTESTING CLASS ENDPOINTS\e[0m"

# 2.1 Create a class
create_class_response=$(invoke_api_request "POST" "/classes" '{"Name": "Mathematics 101", "Teacher": "Prof. Einstein", "Description": "Introduction to Mathematics"}' "Create a new class")
CLASS_ID=$(echo $create_class_response | grep -o '"Id":[0-9]*' | grep -o '[0-9]*')

if [ ! -z "$CLASS_ID" ]; then
    echo -e "\e[35mCreated Class ID: $CLASS_ID\e[0m"
fi

# Create a second class for testing
create_class_response=$(invoke_api_request "POST" "/classes" '{"Name": "Physics 101", "Teacher": "Prof. Newton", "Description": "Introduction to Physics"}' "Create a second class")
CLASS_ID2=$(echo $create_class_response | grep -o '"Id":[0-9]*' | grep -o '[0-9]*')

if [ ! -z "$CLASS_ID2" ]; then
    echo -e "\e[35mCreated Second Class ID: $CLASS_ID2\e[0m"
fi

# 2.2 Get all classes
invoke_api_request "GET" "/classes" "" "Get all classes"

# 2.3 Get class by ID
if [ $CLASS_ID -gt 0 ]; then
    invoke_api_request "GET" "/classes/$CLASS_ID" "" "Get class by ID"
fi

# 2.4 Test class filtering and pagination
invoke_api_request "GET" "/classes?pageNumber=1&pageSize=10&searchTerm=Math" "" "Get classes with filtering and pagination"

# 3. Test Enrollment Endpoints
echo -e "\e[33mTESTING ENROLLMENT ENDPOINTS\e[0m"

# 3.1 Create enrollments
if [ $STUDENT_ID -gt 0 ] && [ $CLASS_ID -gt 0 ]; then
    create_enrollment_response=$(invoke_api_request "POST" "/enrollments" '{"StudentId": '$STUDENT_ID', "ClassId": '$CLASS_ID'}' "Create a new enrollment")
    ENROLLMENT_ID=$(echo $create_enrollment_response | grep -o '"Id":[0-9]*' | grep -o '[0-9]*')
    
    if [ ! -z "$ENROLLMENT_ID" ]; then
        echo -e "\e[35mCreated Enrollment ID: $ENROLLMENT_ID\e[0m"
    fi
fi

# Create a second enrollment
if [ $STUDENT_ID -gt 0 ] && [ $CLASS_ID2 -gt 0 ]; then
    create_enrollment_response=$(invoke_api_request "POST" "/enrollments" '{"StudentId": '$STUDENT_ID', "ClassId": '$CLASS_ID2'}' "Create a second enrollment")
    ENROLLMENT_ID2=$(echo $create_enrollment_response | grep -o '"Id":[0-9]*' | grep -o '[0-9]*')
    
    if [ ! -z "$ENROLLMENT_ID2" ]; then
        echo -e "\e[35mCreated Second Enrollment ID: $ENROLLMENT_ID2\e[0m"
    fi
fi

# Create a third enrollment
if [ $STUDENT_ID2 -gt 0 ] && [ $CLASS_ID -gt 0 ]; then
    create_enrollment_response=$(invoke_api_request "POST" "/enrollments" '{"StudentId": '$STUDENT_ID2', "ClassId": '$CLASS_ID'}' "Create a third enrollment")
    ENROLLMENT_ID3=$(echo $create_enrollment_response | grep -o '"Id":[0-9]*' | grep -o '[0-9]*')
    
    if [ ! -z "$ENROLLMENT_ID3" ]; then
        echo -e "\e[35mCreated Third Enrollment ID: $ENROLLMENT_ID3\e[0m"
    fi
fi

# 3.2 Get all enrollments
invoke_api_request "GET" "/enrollments" "" "Get all enrollments"

# 3.3 Test enrollment filtering and pagination
invoke_api_request "GET" "/enrollments?pageNumber=1&pageSize=10&studentId=$STUDENT_ID" "" "Get enrollments with student filtering"
invoke_api_request "GET" "/enrollments?pageNumber=1&pageSize=10&classId=$CLASS_ID" "" "Get enrollments with class filtering"

# 4. Test Mark Endpoints
echo -e "\e[33mTESTING MARK ENDPOINTS\e[0m"

# 4.1 Create marks
if [ $STUDENT_ID -gt 0 ] && [ $CLASS_ID -gt 0 ]; then
    create_mark_response=$(invoke_api_request "POST" "/marks" '{"StudentId": '$STUDENT_ID', "ClassId": '$CLASS_ID', "ExamMark": 85, "AssignmentMark": 90}' "Create a new mark")
    MARK_ID=$(echo $create_mark_response | grep -o '"Id":[0-9]*' | grep -o '[0-9]*')
    
    if [ ! -z "$MARK_ID" ]; then
        echo -e "\e[35mCreated Mark ID: $MARK_ID\e[0m"
    fi
fi

# Create a second mark
if [ $STUDENT_ID -gt 0 ] && [ $CLASS_ID2 -gt 0 ]; then
    create_mark_response=$(invoke_api_request "POST" "/marks" '{"StudentId": '$STUDENT_ID', "ClassId": '$CLASS_ID2', "ExamMark": 75, "AssignmentMark": 80}' "Create a second mark")
    MARK_ID2=$(echo $create_mark_response | grep -o '"Id":[0-9]*' | grep -o '[0-9]*')
    
    if [ ! -z "$MARK_ID2" ]; then
        echo -e "\e[35mCreated Second Mark ID: $MARK_ID2\e[0m"
    fi
fi

# Create a third mark
if [ $STUDENT_ID2 -gt 0 ] && [ $CLASS_ID -gt 0 ]; then
    create_mark_response=$(invoke_api_request "POST" "/marks" '{"StudentId": '$STUDENT_ID2', "ClassId": '$CLASS_ID', "ExamMark": 90, "AssignmentMark": 95}' "Create a third mark")
    MARK_ID3=$(echo $create_mark_response | grep -o '"Id":[0-9]*' | grep -o '[0-9]*')
    
    if [ ! -z "$MARK_ID3" ]; then
        echo -e "\e[35mCreated Third Mark ID: $MARK_ID3\e[0m"
    fi
fi

# 4.2 Get all marks
invoke_api_request "GET" "/marks" "" "Get all marks"

# 4.3 Test mark filtering and pagination
invoke_api_request "GET" "/marks?pageNumber=1&pageSize=10&studentId=$STUDENT_ID" "" "Get marks with student filtering"
invoke_api_request "GET" "/marks?pageNumber=1&pageSize=10&classId=$CLASS_ID" "" "Get marks with class filtering"

# 4.4 Get class average marks
if [ $CLASS_ID -gt 0 ]; then
    invoke_api_request "GET" "/classes/$CLASS_ID/average-marks" "" "Get class average marks"
fi

if [ $CLASS_ID2 -gt 0 ]; then
    invoke_api_request "GET" "/classes/$CLASS_ID2/average-marks" "" "Get second class average marks"
fi

# 5. Test Student Report
echo -e "\e[33mTESTING STUDENT REPORT\e[0m"

# 5.1 Get student reports
if [ $STUDENT_ID -gt 0 ]; then
    invoke_api_request "GET" "/students/$STUDENT_ID/report" "" "Get student report"
fi

if [ $STUDENT_ID2 -gt 0 ]; then
    invoke_api_request "GET" "/students/$STUDENT_ID2/report" "" "Get second student report"
fi

# 6. Test Validation
echo -e "\e[33mTESTING VALIDATION\e[0m"

# 6.1 Test invalid student ID
invoke_api_request "GET" "/students/0/report" "" "Test invalid student ID validation"

# 6.2 Test invalid class ID
invoke_api_request "GET" "/classes/0/average-marks" "" "Test invalid class ID validation"

# 6.3 Test invalid enrollment data
invoke_api_request "POST" "/enrollments" '{"StudentId": 0, "ClassId": '$CLASS_ID'}' "Test invalid student ID in enrollment"
invoke_api_request "POST" "/enrollments" '{"StudentId": '$STUDENT_ID', "ClassId": 0}' "Test invalid class ID in enrollment"

# 6.4 Test invalid mark data
invoke_api_request "POST" "/marks" '{"StudentId": 0, "ClassId": '$CLASS_ID', "ExamMark": 85, "AssignmentMark": 90}' "Test invalid student ID in mark"
invoke_api_request "POST" "/marks" '{"StudentId": '$STUDENT_ID', "ClassId": 0, "ExamMark": 85, "AssignmentMark": 90}' "Test invalid class ID in mark"
invoke_api_request "POST" "/marks" '{"StudentId": '$STUDENT_ID', "ClassId": '$CLASS_ID', "ExamMark": -10, "AssignmentMark": 90}' "Test invalid exam mark"
invoke_api_request "POST" "/marks" '{"StudentId": '$STUDENT_ID', "ClassId": '$CLASS_ID', "ExamMark": 85, "AssignmentMark": 110}' "Test invalid assignment mark"

# 7. Cleanup
echo -e "\e[33mCLEANUP\e[0m"

# 7.1 Delete students
if [ $STUDENT_ID -gt 0 ]; then
    invoke_api_request "DELETE" "/students/$STUDENT_ID" "" "Delete student"
fi

if [ $STUDENT_ID2 -gt 0 ]; then
    invoke_api_request "DELETE" "/students/$STUDENT_ID2" "" "Delete second student"
fi

# 7.2 Delete classes
if [ $CLASS_ID -gt 0 ]; then
    invoke_api_request "DELETE" "/classes/$CLASS_ID" "" "Delete class"
fi

if [ $CLASS_ID2 -gt 0 ]; then
    invoke_api_request "DELETE" "/classes/$CLASS_ID2" "" "Delete second class"
fi

echo -e "\e[33mAPI TESTING COMPLETED\e[0m"
