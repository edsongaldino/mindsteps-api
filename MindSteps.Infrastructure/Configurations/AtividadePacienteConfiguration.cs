using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindSteps.Domain.Entities;

namespace MindSteps.Infrastructure.Configurations;

public class AtividadePacienteConfiguration : IEntityTypeConfiguration<AtividadePaciente>
{
	public void Configure(EntityTypeBuilder<AtividadePaciente> builder)
	{
		builder.ToTable("AtividadesPacientes");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Status)
			.IsRequired()
			.HasConversion<int>();

		builder.Property(x => x.DataEnvio)
			.IsRequired();

		builder.Property(x => x.RespostaTexto)
			.HasMaxLength(3000);

		builder.Property(x => x.NotaHumor);

		builder.Property(x => x.FeedbackPsicologo)
			.HasMaxLength(3000);

		builder.Property(x => x.FeedbackEnviadoEm);

		builder.HasOne(x => x.Atividade)
			.WithMany(x => x.Pacientes)
			.HasForeignKey(x => x.AtividadeId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.Paciente)
			.WithMany(x => x.AtividadesRecebidas)
			.HasForeignKey(x => x.PacienteId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}