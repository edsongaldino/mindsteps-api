using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindSteps.Domain.Entities;

namespace MindSteps.Infrastructure.Configurations;

public class AtividadeConfiguration : IEntityTypeConfiguration<Atividade>
{
	public void Configure(EntityTypeBuilder<Atividade> builder)
	{
		builder.ToTable("Atividades");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Titulo)
			.IsRequired()
			.HasMaxLength(150);

		builder.Property(x => x.Descricao)
			.HasMaxLength(1000);

		builder.Property(x => x.Tipo)
			.IsRequired()
			.HasConversion<int>();

		builder.Property(x => x.Conteudo);

		builder.Property(x => x.AudioUrl)
			.HasMaxLength(500);

		builder.Property(x => x.ArquivoUrl)
			.HasMaxLength(500);

		builder.Property(x => x.Ativo)
			.IsRequired();

		builder.Property(x => x.CriadoEm)
			.IsRequired();

		builder.Property(x => x.AtualizadoEm);

		builder.HasOne(x => x.Psicologo)
			.WithMany(x => x.Atividades)
			.HasForeignKey(x => x.PsicologoId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}