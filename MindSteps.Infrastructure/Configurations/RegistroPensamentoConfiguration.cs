using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindSteps.Domain.Entities;

namespace MindSteps.Infrastructure.Configurations;

public class RegistroPensamentoConfiguration : IEntityTypeConfiguration<RegistroPensamento>
{
	public void Configure(EntityTypeBuilder<RegistroPensamento> builder)
	{
		builder.ToTable("RegistrosPensamentos");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Situacao)
			.IsRequired()
			.HasMaxLength(2000);

		builder.Property(x => x.PensamentoAutomatico)
			.IsRequired()
			.HasMaxLength(2000);

		builder.Property(x => x.Emocao)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(x => x.IntensidadeEmocao)
			.IsRequired();

		builder.Property(x => x.EvidenciasAFavor)
			.HasMaxLength(2000);

		builder.Property(x => x.EvidenciasContra)
			.HasMaxLength(2000);

		builder.Property(x => x.PensamentoAlternativo)
			.HasMaxLength(2000);

		builder.Property(x => x.IntensidadeFinal);

		builder.Property(x => x.CriadoEm)
			.IsRequired();

		builder.HasOne(x => x.Paciente)
			.WithMany(x => x.RegistrosPensamentos)
			.HasForeignKey(x => x.PacienteId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.AtividadePaciente)
			.WithMany()
			.HasForeignKey(x => x.AtividadePacienteId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}