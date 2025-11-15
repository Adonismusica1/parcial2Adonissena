using Microsoft.EntityFrameworkCore;
using SchoolDomain.Core.Repositories;
using SchoolDomain.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configurar Kestrel explícitamente
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});

// Configuración de servicios
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseInMemoryDatabase("SchoolDb"));

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var appTask = app.RunAsync();

// Bloquear indefinidamente para mantener la aplicación en ejecución
while (true)
{
    System.Threading.Thread.Sleep(1000);
    if (appTask.IsCompleted)
    {
        Console.WriteLine("Aplicación completada.");
        break;
    }
}


