using Microsoft.EntityFrameworkCore;
using SafeVaultExample.Models;

namespace SafeVaultExample.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Alumno> Alumnos => Set<Alumno>();
}