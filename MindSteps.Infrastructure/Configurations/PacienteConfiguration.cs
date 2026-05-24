using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindSteps.Domain.Entities;

namespace MindSteps.Infrastructure.Configurations;

public class PacienteConfiguration : IEntityTypeConfiguration<Paciente>
{
	public void Configure(EntityTypeBuilder<Paciente> builder)
	{
		builder.ToTable("Pacientes");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Genero)
			.HasMaxLength(50);

		builder.Property(x => x.FotoUrl)
			.HasMaxLength(500);

		builder.Property(x => x.Pontos)
			.HasDefaultValue(0);

		builder.Property(x => x.Nivel)
			.HasDefaultValue(1);

		builder.Property(x => x.CriadoEm)
			.IsRequired();

		builder.HasOne(x => x.Usuario)
			.WithOne(x => x.Paciente)
			.HasForeignKey<Paciente>(x => x.UsuarioId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.Psicologo)
			.WithMany(x => x.Pacientes)
			.HasForeignKey(x => x.PsicologoId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasIndex(x => x.UsuarioId)
			.IsUnique();
	}
}