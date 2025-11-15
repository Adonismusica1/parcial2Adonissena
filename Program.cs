using Microsoft.EntityFrameworkCore;
using SchoolDomain.Contract;
using SchoolDomain.Core.Repositories;
using SchoolDomain.Infrastructure;
using SchoolDomain.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar Kestrel explícitamente
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});

// Configuración de servicios de base de datos
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseInMemoryDatabase("SchoolDb"));

// Registrar repositorios
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();

// Registrar servicios de negocio
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();

// Configuración de controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ComunidadApp v1");
    });
}

app.UseCors("AllowAll");
app.MapControllers();

Console.WriteLine("===========================================");
Console.WriteLine("ComunidadApp API - Iniciando");
Console.WriteLine("===========================================");

app.Run();



