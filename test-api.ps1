#!/usr/bin/env pwsh
<#
.SYNOPSIS
Script para probar los endpoints de la API ComunidadApp

.DESCRIPTION
Ejecuta requests de prueba contra los endpoints de la API

.EXAMPLE
./test-api.ps1
#>

$BASE_URL = "http://localhost:5000/api"

function Test-Endpoint {
    param(
        [string]$Method,
        [string]$Endpoint,
        [string]$Description,
        [hashtable]$Body = $null
    )
    
    Write-Host ""
    Write-Host "→ $Description" -ForegroundColor Yellow
    Write-Host "  $Method $Endpoint" -ForegroundColor Gray
    
    try {
        $uri = "$BASE_URL$Endpoint"
        $params = @{
            Uri         = $uri
            Method      = $Method
            Headers     = @{"Content-Type" = "application/json"}
            ErrorAction = "Stop"
        }
        
        if ($Body) {
            $params.Body = $Body | ConvertTo-Json
        }
        
        $response = Invoke-WebRequest @params
        Write-Host "  Status: $($response.StatusCode)" -ForegroundColor Green
        
        if ($response.Content) {
            $content = $response.Content | ConvertFrom-Json
            Write-Host "  Response: " -ForegroundColor Green -NoNewline
            if ($content -is [array]) {
                Write-Host "Array con $($content.Count) items"
            }
            else {
                Write-Host ($content | ConvertTo-Json -Depth 1)
            }
        }
    }
    catch {
        Write-Host "  ✗ Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "===================================" -ForegroundColor Cyan
Write-Host "Pruebas de API - ComunidadApp" -ForegroundColor Green
Write-Host "===================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Base URL: $BASE_URL" -ForegroundColor Yellow
Write-Host ""

# Pruebas de Departments
Write-Host "📚 TESTING DEPARTMENTS" -ForegroundColor Magenta
Test-Endpoint -Method "GET" -Endpoint "/departments" -Description "Obtener todos los departamentos"

# Crear un departamento
$deptBody = @{
    name = "Ingeniería de Sistemas"
    description = "Departamento de Ingeniería"
    building = "Edificio Principal"
    budget = "1000000"
}
Test-Endpoint -Method "POST" -Endpoint "/departments" -Description "Crear nuevo departamento" -Body $deptBody

# Pruebas de Courses
Write-Host ""
Write-Host "📖 TESTING COURSES" -ForegroundColor Magenta
Test-Endpoint -Method "GET" -Endpoint "/courses" -Description "Obtener todos los cursos"

# Crear un curso
$courseBody = @{
    name = "Programación Avanzada"
    code = "CS201"
    description = "Curso avanzado de programación"
    credits = 4
    departmentId = 1
    maxStudents = 30
}
Test-Endpoint -Method "POST" -Endpoint "/courses" -Description "Crear nuevo curso" -Body $courseBody

# Pruebas de Students
Write-Host ""
Write-Host "👨‍🎓 TESTING STUDENTS" -ForegroundColor Magenta
Test-Endpoint -Method "GET" -Endpoint "/students" -Description "Obtener todos los estudiantes"

# Crear un estudiante
$studentBody = @{
    firstName = "Juan"
    lastName = "Pérez"
    email = "juan.perez@university.com"
    studentId = "STU2024001"
    semesterLevel = 3
    enrollmentDate = (Get-Date).ToString("yyyy-MM-dd")
}
Test-Endpoint -Method "POST" -Endpoint "/students" -Description "Crear nuevo estudiante" -Body $studentBody

# Pruebas de Instructors
Write-Host ""
Write-Host "👨‍🏫 TESTING INSTRUCTORS" -ForegroundColor Magenta
Test-Endpoint -Method "GET" -Endpoint "/instructors" -Description "Obtener todos los instructores"

# Crear un instructor
$instructorBody = @{
    firstName = "Dr. Carlos"
    lastName = "García"
    email = "carlos.garcia@university.com"
    employeeId = "EMP001"
    title = "Profesor Titular"
    hireDate = (Get-Date).ToString("yyyy-MM-dd")
}
Test-Endpoint -Method "POST" -Endpoint "/instructors" -Description "Crear nuevo instructor" -Body $instructorBody

Write-Host ""
Write-Host "===================================" -ForegroundColor Cyan
Write-Host "✓ Pruebas completadas" -ForegroundColor Green
Write-Host "===================================" -ForegroundColor Cyan
