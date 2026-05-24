using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindSteps.Domain.Entities;

namespace MindSteps.Infrastructure.Configurations;

public class MensagemConfiguration : IEntityTypeConfiguration<Mensagem>
{
	public void Configure(EntityTypeBuilder<Mensagem> builder)
	{
		builder.ToTable("Mensagens");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Conteudo)
			.IsRequired()
			.HasMaxLength(2000);

		builder.Property(x => x.Lida)
			.IsRequired()
			.HasDefaultValue(false);

		builder.Property(x => x.CriadoEm)
			.IsRequired();

		builder.HasOne(x => x.Psicologo)
			.WithMany()
			.HasForeignKey(x => x.PsicologoId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.Paciente)
			.WithMany()
			.HasForeignKey(x => x.PacienteId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
