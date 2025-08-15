using MindSteps.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MindSteps.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
}