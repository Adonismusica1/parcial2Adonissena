# 🏗️ ARQUITECTURA Y DISEÑO DEL PROYECTO

## Arquitectura de Capas

```
┌─────────────────────────────────────────────┐
│         Capa de Presentación (Web)          │
│  Controllers: Departments, Courses, ...     │
│  Swagger/OpenAPI Documentation              │
└────────────────┬────────────────────────────┘
                 │ Depends on
┌────────────────▼────────────────────────────┐
│    Capa de Aplicación (Services)            │
│  - Lógica de negocio                        │
│  - Validaciones                             │
│  - DTOs (futuros)                           │
└────────────────┬────────────────────────────┘
                 │ Depends on
┌────────────────▼────────────────────────────┐
│       Capa de Dominio (Domain)              │
│  - Entidades: Department, Course, ...       │
│  - Interfaces: IRepository                  │
│  - Excepciones: CourseException, ...        │
└────────────────┬────────────────────────────┘
                 │ Depends on
┌────────────────▼────────────────────────────┐
│  Capa de Infraestructura (Infrastructure)   │
│  - Implementaciones de Repositorios         │
│  - Entity Framework Core DbContext          │
│  - Database: In-Memory (SQL Server ready)   │
└─────────────────────────────────────────────┘
```

---

## Patrones de Diseño Utilizados

### 1. **Repository Pattern**
```csharp
// Interfaz en Dominio
public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(int id);
    Task<IEnumerable<Course>> GetAllAsync();
    Task AddAsync(Course course);
    Task UpdateAsync(Course course);
    Task DeleteAsync(int id);
}

// Implementación en Infraestructura
public class CourseRepository : ICourseRepository
{
    private readonly SchoolContext _context;
    // ...
}
```

### 2. **Dependency Injection**
```csharp
// En Program.cs
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
```

### 3. **Unit of Work Pattern** (implícito con DbContext)
```csharp
// DbContext maneja transacciones
await _context.SaveChangesAsync();
```

### 4. **DTO Pattern** (Recomendado para futuro)
```csharp
// Separar modelos de base de datos de modelos API
public class CourseCreateDto
{
    public string Name { get; set; }
    public string Code { get; set; }
    // ... solo propiedades necesarias
}
```

---

## Relaciones entre Entidades

### One-to-Many
```
Department ←── 1:N ──→ Course
Department ←── 1:N ──→ Student
Department ←── 1:N ──→ Instructor
Course ←── 1:N ──→ Instructor (asignado)
```

### Many-to-Many
```
Student ←── M:N ──→ Course
   (a través de tabla intermedia CourseStudent)
```

### Entity Relationships en Code
```csharp
// Department tiene muchos Courses
public virtual ICollection<Course> Courses { get; set; }

// Course pertenece a un Department
public int? DepartmentId { get; set; }
public virtual Department? Department { get; set; }

// Many-to-Many
public virtual ICollection<Student> Students { get; set; }
```

---

## Flujo de una Request

```
1. HTTP Request → API Controller
   ↓
2. Controller desserializa JSON → Entidad
   ↓
3. Controller valida → Si error: Exception manejada
   ↓
4. Controller inyecta Repository
   ↓
5. Repository llama a DbContext
   ↓
6. DbContext ejecuta SQL/In-Memory query
   ↓
7. DbContext retorna Entidad
   ↓
8. Repository retorna Entidad
   ↓
9. Controller serializa → JSON
   ↓
10. HTTP Response (200, 201, 400, 404, 500)
```

---

## Métodos Asincronos

Todo está implementado con `async/await` para mejor rendimiento:

```csharp
public async Task<Course?> GetByIdAsync(int id)
{
    return await _context.Courses
        .FirstOrDefaultAsync(c => c.Id == id);
}

public async Task AddAsync(Course course)
{
    _context.Courses.Add(course);
    await _context.SaveChangesAsync();
}
```

---

## Validaciones

### En Repository
```csharp
public async Task AddAsync(Course course)
{
    if (string.IsNullOrEmpty(course.Code))
        throw new CourseException("El código del curso es requerido");
    
    _context.Courses.Add(course);
    await _context.SaveChangesAsync();
}
```

### En Controller
```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] Course course)
{
    if (course == null)
        return BadRequest(new { message = "Curso no puede ser nulo" });

    try
    {
        await _repository.AddAsync(course);
        return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = ex.Message });
    }
}
```

---

## Configuración Entity Framework

### OnModelCreating
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Relaciones One-to-Many
    modelBuilder.Entity<Department>()
        .HasMany(d => d.Courses)
        .WithOne(c => c.Department)
        .HasForeignKey(c => c.DepartmentId)
        .OnDelete(DeleteBehavior.Restrict);

    // Relaciones Many-to-Many
    modelBuilder.Entity<Course>()
        .HasMany(c => c.Students)
        .WithMany(s => s.Courses)
        .UsingEntity(j => j.ToTable("CourseStudent"));
}
```

---

## Transiciones de Base de Datos

### Actualmente: In-Memory
```csharp
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseInMemoryDatabase("SchoolDb")
);
```

### Futuro: SQL Server
```csharp
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
);
```

### Futuro: PostgreSQL
```csharp
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
);
```

---

## Manejo de Errores

### Excepciones Personalizadas
```csharp
public class CourseException : Exception
{
    public CourseException(string message) : base(message) { }
}

public class DepartmentException : Exception
{
    public DepartmentException(string message) : base(message) { }
}
```

### Try-Catch en Controllers
```csharp
try
{
    await _repository.AddAsync(course);
    return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
}
catch (CourseException ex)
{
    return BadRequest(new { message = ex.Message });
}
catch (Exception ex)
{
    return StatusCode(500, new { message = ex.Message });
}
```

---

## HTTP Status Codes Utilizados

| Código | Significado | Ejemplo |
|--------|-------------|---------|
| 200 | OK | GET exitoso |
| 201 | Created | POST exitoso |
| 204 | No Content | DELETE exitoso |
| 400 | Bad Request | Datos inválidos |
| 404 | Not Found | Recurso no existe |
| 500 | Internal Server Error | Error en servidor |

---

## CORS (Cross-Origin Resource Sharing)

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()     // *.com
                   .AllowAnyMethod()     // GET, POST, PUT, DELETE, etc
                   .AllowAnyHeader();    // Content-Type, Authorization, etc
        });
});

// Aplicar en middleware
app.UseCors("AllowAll");
```

---

## Swagger/OpenAPI

Generación automática de documentación:

```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// En desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

Disponible en: `http://localhost:5000/swagger`

---

## Próximas Mejoras Recomendadas

1. **AutoMapper**: Para mapeo de DTOs
```csharp
builder.Services.AddAutoMapper(typeof(Program));
```

2. **Fluent Validation**: Para validaciones complejas
```csharp
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
```

3. **Logging**: Para auditoría
```csharp
builder.Services.AddSerilog();
```

4. **Authentication**: JWT o OAuth2
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { /* configurar */ });
```

5. **Unit Tests**: Pruebas unitarias con xUnit
6. **Integration Tests**: Pruebas de integración
7. **API Versioning**: v1, v2 endpoints

---

## Diagrama de Clases (Relaciones)

```
┌─────────────────────┐
│     Department      │
├─────────────────────┤
│ - Id                │
│ - Name              │
│ - Description       │◄─────┐
│ - Building          │      │
│ - Budget            │      │
│ - HeadInstructorId  │      │
├─────────────────────┤      │
│ + Courses []        │      │
│ + Students []       │      │
│ + Instructors []    │      │
└─────────────────────┘      │
         △                    │
         │ 1:N               │
         │                    │ 1:N
    ┌────┴──────────┐         │
    │               │         │
┌───▼──────┐   ┌───▼─────────┴──┐
│  Course  │   │  Instructor    │
├──────────┤   ├────────────────┤
│ - Id     │   │ - Id           │
│ - Name   │   │ - FirstName    │
│ - Code   │   │ - LastName     │
│ - Credits│   │ - Email        │
│ - MaxStu │   │ - EmployeeId   │
│ - DeptId │   │ - Title        │
│ - InstId │   │ - HireDate     │
├──────────┤   ├────────────────┤
│ + Students   │ + Courses []   │
│  (M:N)   │   │ + Department   │
└────┬─────┘   └────────────────┘
     │
     │ M:N
     │
     ▼
 ┌─────────────┐
 │   Student   │
 ├─────────────┤
 │ - Id        │
 │ - FirstName │
 │ - LastName  │
 │ - Email     │
 │ - StudentId │
 │ - Semester  │
 │ - DeptId    │
 ├─────────────┤
 │ + Courses[] │
 │ + Department│
 └─────────────┘
```

---

¡Tu arquitectura está lista para escalar y crecer! 🚀
