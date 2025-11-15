global using SchoolDomain.Core.Entities;
global using SchoolDomain.Core.Exceptions;
global using SchoolDomain.Core.Repositories;
global using SchoolDomain.Infrastructure;

namespace SchoolDomain.Core;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
