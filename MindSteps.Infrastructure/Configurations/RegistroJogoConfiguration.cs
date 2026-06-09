using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindSteps.Domain.Entities;

namespace MindSteps.Infrastructure.Configurations;

public class RegistroJogoConfiguration : IEntityTypeConfiguration<RegistroJogo>
{
	public void Configure(EntityTypeBuilder<RegistroJogo> builder)
	{
		builder.ToTable("RegistrosJogos");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.JogoId)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(x => x.DataPlay)
			.IsRequired();

		builder.Property(x => x.DadosPlay)
			.IsRequired();

		builder.HasOne(x => x.Paciente)
			.WithMany(x => x.RegistrosJogos)
			.HasForeignKey(x => x.PacienteId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.AtividadePaciente)
			.WithMany()
			.HasForeignKey(x => x.AtividadePacienteId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}
