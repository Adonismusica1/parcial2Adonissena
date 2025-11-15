# ⚡ REFERENCIA RÁPIDA - ComunidadApp

## 🚀 Iniciar la API

```powershell
cd "c:\Users\Adonis Sena\ComunidadApp"
$env:ASPNETCORE_ENVIRONMENT="Development"
dotnet run
```

**URL**: `http://localhost:5000`

---

## 📋 Endpoints Principales

### Departments
```
GET    /api/departments              # Obtener todos
GET    /api/departments/{id}         # Obtener uno
POST   /api/departments              # Crear
PUT    /api/departments/{id}         # Actualizar
DELETE /api/departments/{id}         # Eliminar
```

### Courses
```
GET    /api/courses                           # Obtener todos
GET    /api/courses/{id}                      # Obtener uno
GET    /api/courses/department/{deptId}       # Por departamento
GET    /api/courses/instructor/{instructorId} # Por instructor
POST   /api/courses                           # Crear
PUT    /api/courses/{id}                      # Actualizar
DELETE /api/courses/{id}                      # Eliminar
```

### Students
```
GET    /api/students                          # Obtener todos
GET    /api/students/{id}                     # Obtener uno
GET    /api/students/department/{deptId}      # Por departamento
POST   /api/students                          # Crear
PUT    /api/students/{id}                     # Actualizar
DELETE /api/students/{id}                     # Eliminar
```

### Instructors
```
GET    /api/instructors                       # Obtener todos
GET    /api/instructors/{id}                  # Obtener uno
GET    /api/instructors/department/{deptId}   # Por departamento
POST   /api/instructors                       # Crear
PUT    /api/instructors/{id}                  # Actualizar
DELETE /api/instructors/{id}                  # Eliminar
```

---

## 💻 Ejemplos de Uso (PowerShell)

### GET - Obtener todos los departamentos
```powershell
$response = Invoke-WebRequest -Uri "http://localhost:5000/api/departments" `
    -Method GET -Headers @{"Content-Type"="application/json"}
$response.Content | ConvertFrom-Json | Format-Table
```

### POST - Crear un departamento
```powershell
$body = @{
    name = "Ingeniería"
    description = "Dept de Ingeniería"
    building = "Edificio A"
    budget = "1000000"
} | ConvertTo-Json

$response = Invoke-WebRequest -Uri "http://localhost:5000/api/departments" `
    -Method POST `
    -Headers @{"Content-Type"="application/json"} `
    -Body $body

$response.Content | ConvertFrom-Json
```

### GET - Por ID
```powershell
Invoke-WebRequest -Uri "http://localhost:5000/api/departments/1" `
    -Method GET | Select-Object -ExpandProperty Content | ConvertFrom-Json
```

### PUT - Actualizar
```powershell
$body = @{
    name = "Ingeniería Actualizada"
    description = "Actualizado"
    building = "Edificio B"
    budget = "1200000"
} | ConvertTo-Json

Invoke-WebRequest -Uri "http://localhost:5000/api/departments/1" `
    -Method PUT `
    -Headers @{"Content-Type"="application/json"} `
    -Body $body
```

### DELETE
```powershell
Invoke-WebRequest -Uri "http://localhost:5000/api/departments/1" -Method DELETE
```

---

## 📦 Estructura de Carpetas

```
ComunidadApp/
├── SchoolDomain/
│   ├── Core/
│   │   ├── Entities/
│   │   │   ├── Courses/
│   │   │   ├── Departments/
│   │   │   ├── Instructors/
│   │   │   └── Students/
│   │   ├── Interfaces/
│   │   └── Repositories/
│   └── Infrastructure/
├── Controllers/
│   ├── CoursesController.cs
│   ├── DepartmentsController.cs
│   ├── StudentsController.cs
│   └── InstructorsController.cs
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
├── README.md
├── ARCHITECTURE.md
├── GIT_INSTRUCTIONS.md
├── run-api.ps1
└── test-api.ps1
```

---

## 🛠️ Comandos Útiles

### Compilar
```powershell
dotnet build
dotnet build -c Release
```

### Limpiar
```powershell
dotnet clean
```

### Restaurar paquetes
```powershell
dotnet restore
```

### Ver versión de .NET
```powershell
dotnet --version
```

### Agregar paquete NuGet
```powershell
dotnet add package NombreDelPaquete
```

### Ejecutar pruebas
```powershell
# Crear nuevo proyecto de tests
dotnet new xunit -n ComunidadApp.Tests

# Ejecutar tests
dotnet test
```

---

## 📚 Archivos de Documentación

- **README.md** - Guía completa del proyecto
- **ARCHITECTURE.md** - Detalles de arquitectura y patrones
- **GIT_INSTRUCTIONS.md** - Cómo subir a GitHub

---

## 🔌 Swagger/OpenAPI

```
http://localhost:5000/swagger
```

Interfaz interactiva para probar endpoints

---

## 🗄️ Base de Datos

**Actual**: Entity Framework In-Memory (perfecta para desarrollo)

**Para migrar a SQL Server**:
1. Instalar paquete: `dotnet add package Microsoft.EntityFrameworkCore.SqlServer`
2. Cambiar DbContext en Program.cs
3. Crear migraciones: `dotnet ef migrations add InitialCreate`
4. Actualizar BD: `dotnet ef database update`

---

## ⚠️ Solución de Problemas

### Puerto 5000 en uso
```powershell
# Encontrar proceso
Get-NetTCPConnection -LocalPort 5000 | Select-Object OwningProcess

# Terminar proceso (reemplazar PID)
Stop-Process -Id <PID> -Force

# O usar puerto diferente
$env:ASPNETCORE_URLS="http://localhost:5001"
```

### No compila
```powershell
dotnet clean
dotnet restore
dotnet build
```

### Error de dependencias
```powershell
dotnet restore --force
```

---

## 📝 Scripts Disponibles

### Ejecutar API
```powershell
./run-api.ps1
```

### Probar endpoints
```powershell
./test-api.ps1
```

---

## 🎯 Checklist de Entrega

- ✅ Entidades de Dominio creadas
- ✅ Capa de Infraestructura implementada
- ✅ Repositorios con CRUD completo
- ✅ Controladores REST API funcionales
- ✅ 4+ Endpoints por entidad
- ✅ Swagger/OpenAPI integrado
- ✅ CORS habilitado
- ✅ Manejo de errores
- ✅ Documentación completa
- ✅ Scripts de prueba
- ✅ Instrucciones de Git

---

## 🚀 Próximos Pasos

1. **Pruebas Unitarias** - xUnit + Moq
2. **DTOs** - AutoMapper
3. **Validación** - Fluent Validation
4. **Autenticación** - JWT
5. **Logging** - Serilog
6. **SQL Server** - Migraciones
7. **CI/CD** - GitHub Actions
8. **Docker** - Containerización

---

## 📞 Soporte

Para dudas o problemas, referir a:
- `README.md` - Instrucciones generales
- `ARCHITECTURE.md` - Detalles técnicos
- `GIT_INSTRUCTIONS.md` - Control de versiones

---

**Estado**: ✅ Listo para producción (con mejoras futuras recomendadas)

**Última actualización**: 15 de Noviembre de 2025
