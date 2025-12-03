using FastPayApi.Dtos;
using FastPayApi.Models;
using FastPayApi.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(opts =>
{
    opts.SerializerOptions.PropertyNamingPolicy = null;
});

// register repositories (interfaces + implementations)
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddSingleton<ITransactionRepository, InMemoryTransactionRepository>();

builder.Services.AddCors(opt => opt.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseCors("AllowAll");

app.MapGet("/", () => Results.Ok(new { Message = "Fast-Pay API running" }));

app.MapPost("/api/users", async (CreateUserDto dto, IUserRepository users) =>
{
    // basic validation
    if (string.IsNullOrWhiteSpace(dto.Phone) || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Pin))
        return Results.BadRequest(new { Error = "Name, phone and pin are required" });

    var existing = await users.GetByPhoneAsync(dto.Phone);
    if (existing != null)
        return Results.Conflict(new { Error = "Phone already registered" });

    var user = new User { Name = dto.Name, Phone = dto.Phone, Email = dto.Email, Pin = dto.Pin, Balance = 0m };
    await users.AddAsync(user);

    return Results.Created($"/api/users/{user.Id}", user);
});

app.MapGet("/api/users/{phone}", async (string phone, IUserRepository users) =>
{
    var user = await users.GetByPhoneAsync(phone);
    if (user == null) return Results.NotFound();
    return Results.Ok(user);
});

app.MapGet("/api/users", async (IUserRepository users) => await users.GetAllAsync());

app.MapPost("/api/payments/send", async (SendPaymentDto dto, IUserRepository users, ITransactionRepository txRepo) =>
{
    // validate
    if (dto.Amount <= 0) return Results.BadRequest(new { Error = "Amount must be > 0" });
    var from = await users.GetByPhoneAsync(dto.FromPhone);
    var to = await users.GetByPhoneAsync(dto.ToPhone);
    if (from == null || to == null) return Results.NotFound(new { Error = "Sender or recipient not found" });
    if (from.Pin != dto.Pin) return Results.Unauthorized();
    if (from.Balance < dto.Amount) return Results.BadRequest(new { Error = "Insufficient funds" });

    // perform
    from.Balance -= dto.Amount;
    to.Balance += dto.Amount;
    await users.UpdateAsync(from);
    await users.UpdateAsync(to);

    var tx = new Transaction { FromUserId = from.Id, ToUserId = to.Id, Amount = dto.Amount, Note = dto.Note };
    await txRepo.AddAsync(tx);

    return Results.Ok(new { Message = "Payment completed", Transaction = tx });
});

app.MapGet("/api/transactions/{phone}", async (string phone, IUserRepository users, ITransactionRepository txRepo) =>
{
    var user = await users.GetByPhoneAsync(phone);
    if (user == null) return Results.NotFound();
    var list = await txRepo.GetByUserAsync(user.Id);
    return Results.Ok(list);
});

app.Run();
