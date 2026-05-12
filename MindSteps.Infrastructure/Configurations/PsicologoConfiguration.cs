using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindSteps.Domain.Entities;

namespace MindSteps.Infrastructure.Configurations;

public class PsicologoConfiguration : IEntityTypeConfiguration<Psicologo>
{
	public void Configure(EntityTypeBuilder<Psicologo> builder)
	{
		builder.ToTable("Psicologos");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Crp)
			.IsRequired()
			.HasMaxLength(30);

		builder.Property(x => x.Bio)
			.HasMaxLength(1000);

		builder.Property(x => x.FotoUrl)
			.HasMaxLength(500);

		builder.Property(x => x.Aprovado)
			.IsRequired();

		builder.Property(x => x.CriadoEm)
			.IsRequired();

		builder.HasOne(x => x.Usuario)
			.WithOne(x => x.Psicologo)
			.HasForeignKey<Psicologo>(x => x.UsuarioId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasIndex(x => x.UsuarioId)
			.IsUnique();

		builder.HasIndex(x => x.Crp)
			.IsUnique();
	}
}