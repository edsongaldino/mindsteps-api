using MindSteps.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MindSteps.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Usuario>(b =>
		{
			b.ToTable("usuarios"); // nome da tabela em snake_case

			b.Property(x => x.Id).HasColumnName("id");
			b.Property(x => x.Nome).HasColumnName("nome");
			b.Property(x => x.Email).HasColumnName("email");
			b.Property(x => x.Telefone).HasColumnName("telefone");
			b.Property(x => x.Senha).HasColumnName("senha");

			// índice único para email
			b.HasIndex(x => x.Email).IsUnique();
		});
	}

}