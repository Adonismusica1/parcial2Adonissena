# 📤 INSTRUCCIONES PARA SUBIR A GIT

## 1. Inicializar Repositorio Git (si no existe)
```powershell
cd "c:\Users\Adonis Sena\ComunidadApp"
git init
git remote add origin <URL_DEL_REPOSITORIO>
```

## 2. Configurar identidad de Git
```powershell
git config user.name "Tu Nombre"
git config user.email "tu.email@example.com"
```

## 3. Crear una rama para el proyecto
```powershell
git checkout -b feature/school-domain-infraestructure
```

## 4. Ver estado de los cambios
```powershell
git status
```

## 5. Agregar todos los archivos
```powershell
git add .
```

## 6. Ver qué se va a commitar
```powershell
git diff --cached
```

## 7. Hacer commit del código
```powershell
git commit -m "feat: Implementar capa de Dominio e Infraestructura con API REST"
```

## 8. Subir cambios a GitHub
```powershell
git push origin feature/school-domain-infraestructure
```

## 9. Crear un Pull Request en GitHub
1. Ir a tu repositorio en GitHub
2. Click en "Compare & pull request"
3. Añadir descripción del PR:

```markdown
# Implementación de Capa de Dominio e Infraestructura

## Descripción
Se ha implementado una capa de Dominio e Infraestructura completa para la aplicación ComunidadApp con una API REST funcional.

## Cambios realizados

### ✅ Capa de Dominio (SchoolDomain/Core)
- Entidades: Department, Course, Student, Instructor
- BaseEntity: Clase base con propiedades Id, CreatedAt, UpdatedAt
- Relaciones Many-to-Many entre Student y Course
- Relaciones One-to-Many con Departments

### ✅ Capa de Infraestructura (SchoolDomain/Infrastructure)
- SchoolContext: DbContext configurado con Entity Framework Core 9.0
- Soporte para base de datos en memoria (InMemory)

### ✅ Capa de Repositorios (SchoolDomain/Core/Repositories)
- ICourseRepository y CourseRepository
- IDepartmentRepository y DepartmentRepository
- IStudentRepository y StudentRepository
- IInstructorRepository y InstructorRepository
- Métodos CRUD y búsquedas especializadas

### ✅ Capa de Presentación (Controllers)
- DepartmentsController
- CoursesController
- StudentsController
- InstructorsController

### ✅ API REST Endpoints
- GET /api/departments
- POST /api/departments
- PUT /api/departments/{id}
- DELETE /api/departments/{id}
- (Similar para Courses, Students, Instructors)

### ✅ Documentación
- README.md con instrucciones completas
- run-api.ps1 para ejecutar la API
- test-api.ps1 para probar los endpoints
- Swagger/OpenAPI integrado

## Tecnologías Utilizadas
- .NET 9.0
- Entity Framework Core 9.0
- ASP.NET Core Web API
- Swagger/OpenAPI
- Entity Framework In-Memory Database

## Cómo ejecutar
```powershell
cd "c:\Users\Adonis Sena\ComunidadApp"
$env:ASPNETCORE_ENVIRONMENT="Development"
dotnet run
```

API disponible en: http://localhost:5000

## Próximas mejoras
- Migración a SQL Server
- Autenticación y Autorización
- DTOs para Request/Response
- Validación con Fluent Validation
```

4. Click en "Create pull request"

---

## Alternativa: Hacer Push Directo a Main (no recomendado)
```powershell
# Cambiar a rama main
git checkout main

# Hacer pull para sincronizar
git pull origin main

# Hacer merge de tu rama
git merge feature/school-domain-infraestructure

# Hacer push a main
git push origin main
```

---

## Ver Histórico de Commits
```powershell
# Últimos 10 commits
git log --oneline -10

# Commits de una rama específica
git log feature/school-domain-infraestructure

# Commits que modificaron un archivo específico
git log Program.cs
```

---

## Verificar Rama Actual
```powershell
git branch
git branch -a  # Ver todas las ramas (local y remoto)
```

---

## Actualizar rama con cambios de main
```powershell
git fetch origin
git rebase origin/main
```

---

## Archivos a Ignorar (.gitignore)
Ya está configurado el proyecto para ignorar:
- `/bin` - Archivos compilados
- `/obj` - Archivos objeto
- `.vs` - Configuración de Visual Studio
- `*.user` - Configuración de usuario
- `appsettings.*.local.json` - Configuraciones locales

---

## Comandos Útiles
```powershell
# Ver cambios sin hacer commit
git diff

# Ver el estado resumido
git status -s

# Deshacer cambios en un archivo
git checkout -- <archivo>

# Deshacer últimos cambios (cuidado)
git reset --hard HEAD~1

# Crear tag para versión
git tag -a v1.0.0 -m "Primera versión de Dominio e Infraestructura"
git push origin v1.0.0
```

---

¡Listo! Tu código estará en la rama de feature para código review y posterior merge a main.
