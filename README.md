# 🎓 ComunidadApp - API REST de Gestión Académica

## Descripción

API REST completamente funcional desarrollada con **ASP.NET Core 9.0** y arquitectura de capas (Dominio, Infraestructura y Presentación). Permite gestionar departamentos, cursos, estudiantes e instructores en una institución educativa.

---

##  Estructura del Proyecto

```
ComunidadApp/
│
├── SchoolDomain/              # Capa de Dominio e Infraestructura
│   ├── Core/
│   │   ├── BaseEntities.cs    # Clase base de entidades
│   │   ├── Entities/          # Entidades del dominio
│   │   │   ├── Courses/
│   │   │   ├── Departments/
│   │   │   ├── Instructors/
│   │   │   └── Students/
│   │   ├── Exceptions/        # Excepciones personalizadas
│   │   ├── Interfaces/        # Contratos de repositorios
│   │   └── Repositories/      # Implementaciones de repositorios
│   └── Infrastructure/
│       └── SchoolContext.cs   # DbContext de Entity Framework
│
├── Controllers/               # Controladores REST API
│   ├── DepartmentsController.cs
│   ├── CoursesController.cs
│   ├── StudentsController.cs
│   └── InstructorsController.cs
│
├── Program.cs                # Configuración de servicios y middleware
├── appsettings.json          # Configuración general
├── appsettings.Development.json # Configuración para desarrollo
└── Modelo.cs                 # Clases originales del proyecto
```

---

## 📋 Entidades Principales

### Department (Departamento)
```csharp
public class Department : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int? HeadInstructorId { get; set; }
    public string Building { get; set; }
    public string Budget { get; set; }
}
```

### Course (Curso)
```csharp
public class Course : BaseEntity
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public int Credits { get; set; }
    public int? DepartmentId { get; set; }
    public int? InstructorId { get; set; }
    public int? MaxStudents { get; set; }
}
```

### Student (Estudiante)
```csharp
public class Student : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string StudentId { get; set; }
    public int? SemesterLevel { get; set; }
    public DateTime EnrollmentDate { get; set; }
}
```

### Instructor (Docente)
```csharp
public class Instructor : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string EmployeeId { get; set; }
    public string Title { get; set; }
    public DateTime HireDate { get; set; }
}
```

---

## 🚀 Endpoints Disponibles

### Departamentos
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/departments` | Obtener todos los departamentos |
| GET | `/api/departments/{id}` | Obtener un departamento por ID |
| POST | `/api/departments` | Crear un nuevo departamento |
| PUT | `/api/departments/{id}` | Actualizar un departamento |
| DELETE | `/api/departments/{id}` | Eliminar un departamento |

### Cursos
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/courses` | Obtener todos los cursos |
| GET | `/api/courses/{id}` | Obtener un curso por ID |
| GET | `/api/courses/department/{departmentId}` | Obtener cursos por departamento |
| GET | `/api/courses/instructor/{instructorId}` | Obtener cursos por instructor |
| POST | `/api/courses` | Crear un nuevo curso |
| PUT | `/api/courses/{id}` | Actualizar un curso |
| DELETE | `/api/courses/{id}` | Eliminar un curso |

### Estudiantes
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/students` | Obtener todos los estudiantes |
| GET | `/api/students/{id}` | Obtener un estudiante por ID |
| GET | `/api/students/department/{departmentId}` | Obtener estudiantes por departamento |
| POST | `/api/students` | Crear un nuevo estudiante |
| PUT | `/api/students/{id}` | Actualizar un estudiante |
| DELETE | `/api/students/{id}` | Eliminar un estudiante |

### Instructores
| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/instructors` | Obtener todos los instructores |
| GET | `/api/instructors/{id}` | Obtener un instructor por ID |
| GET | `/api/instructors/department/{departmentId}` | Obtener instructores por departamento |
| POST | `/api/instructors` | Crear un nuevo instructor |
| PUT | `/api/instructors/{id}` | Actualizar un instructor |
| DELETE | `/api/instructors/{id}` | Eliminar un instructor |

---

## 🔧 Requisitos

- **.NET 9.0 SDK** o superior
- **PowerShell 5.0+** (en Windows)
- Conexión a internet (para descargar paquetes NuGet)

## 📦 Dependencias

- `Microsoft.EntityFrameworkCore` v9.0.0
- `Microsoft.EntityFrameworkCore.InMemory` v9.0.0
- `Microsoft.AspNetCore.OpenApi` v9.0.0
- `Swashbuckle.AspNetCore` v6.5.0

---

## ⚡ Instalación y Ejecución

### 1. Navegar a la carpeta del proyecto
```powershell
cd "c:\Users\Adonis Sena\ComunidadApp"
```

### 2. Restaurar dependencias (opcional, se hace automáticamente)
```powershell
dotnet restore
```

### 3. Compilar el proyecto
```powershell
dotnet build
```

### 4. Ejecutar la API
```powershell
$env:ASPNETCORE_ENVIRONMENT="Development"
dotnet run
```

### 5. Acceder a la API
- **URL Base**: `http://localhost:5000`
- **Swagger UI**: `http://localhost:5000/swagger` (en modo Development)

---

## 📝 Ejemplos de Uso

### Obtener todos los departamentos
```powershell
$response = Invoke-WebRequest -Uri "http://localhost:5000/api/departments" `
    -Method GET `
    -Headers @{"Content-Type"="application/json"}

$response.Content | ConvertFrom-Json | Format-Table -AutoSize
```

### Crear un nuevo departamento
```powershell
$body = @{
    name = "Ingeniería de Sistemas"
    description = "Departamento de Ingeniería"
    building = "Edificio Principal"
    budget = "1000000"
} | ConvertTo-Json

$response = Invoke-WebRequest -Uri "http://localhost:5000/api/departments" `
    -Method POST `
    -Headers @{"Content-Type"="application/json"} `
    -Body $body

$response.Content | ConvertFrom-Json | Format-Table -AutoSize
```

### Actualizar un departamento
```powershell
$body = @{
    name = "Ingeniería de Sistemas"
    description = "Departamento actualizado"
    building = "Edificio B"
    budget = "1200000"
} | ConvertTo-Json

$response = Invoke-WebRequest -Uri "http://localhost:5000/api/departments/1" `
    -Method PUT `
    -Headers @{"Content-Type"="application/json"} `
    -Body $body

$response.Content | Write-Host
```

### Eliminar un departamento
```powershell
$response = Invoke-WebRequest -Uri "http://localhost:5000/api/departments/1" `
    -Method DELETE

$response.Content | Write-Host
```

---

## 🗄️ Base de Datos

La aplicación utiliza **Entity Framework Core** con una base de datos **en memoria** (InMemory) que es perfecta para desarrollo y pruebas.

### Para migrar a SQL Server:

1. Instalar el paquete de SQL Server:
```powershell
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

2. Modificar `Program.cs`:
```csharp
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer("Server=localhost;Database=SchoolDb;Trusted_Connection=true;"));
```

3. Ejecutar migraciones:
```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## ✨ Características

✅ **Arquitectura de Capas**: Dominio, Infraestructura y Presentación

✅ **Entity Framework Core 9.0**: ORM moderno con LINQ

✅ **Repositorio Genérico**: Base reutilizable para acceso a datos

✅ **CRUD Completo**: Operaciones Create, Read, Update, Delete

✅ **Relaciones entre Entidades**: One-to-Many, Many-to-Many

✅ **Swagger/OpenAPI**: Documentación interactiva automática

✅ **CORS Habilitado**: Permite acceso desde cualquier origen

✅ **Validación de Datos**: Excepciones personalizadas

✅ **Métodos Asincronos**: Mejor rendimiento y escalabilidad

---

## 🐛 Resolución de Problemas

### La API no se ejecuta
- Verificar que el puerto 5000 está disponible
- Intentar con un puerto diferente: `$env:ASPNETCORE_URLS="http://localhost:5001"`
- Limpiar y reconstruir: `dotnet clean && dotnet build`

### Error de conexión a la base de datos
- Verificar que Entity Framework Core está instalado correctamente
- Intentar restaurar dependencias: `dotnet restore`

### Puertos ocupados
```powershell
# Encontrar qué proceso usa el puerto 5000
Get-NetTCPConnection -LocalPort 5000 | Select-Object -ExpandProperty OwningProcess

# Terminar el proceso
Stop-Process -Id <PID> -Force
```

---

## 📚 Recursos Adicionales

- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Swagger/OpenAPI](https://swagger.io/)

---

## 👨‍💼 Autor

Desarrollado como parte de un proyecto académico de arquitectura de software.

---

## 📄 Licencia

Este proyecto es de código abierto y puede ser utilizado libremente con propósitos educativos.

