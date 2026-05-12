using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindSteps.Domain.Entities;

namespace MindSteps.Infrastructure.Configurations;

public class CheckInEmocionalConfiguration : IEntityTypeConfiguration<CheckInEmocional>
{
	public void Configure(EntityTypeBuilder<CheckInEmocional> builder)
	{
		builder.ToTable("CheckInsEmocionais");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Humor)
			.IsRequired()
			.HasConversion<int>();

		builder.Property(x => x.Intensidade)
			.IsRequired();

		builder.Property(x => x.EmocaoPrincipal)
			.HasMaxLength(100);

		builder.Property(x => x.Observacao)
			.HasMaxLength(1000);

		builder.Property(x => x.CriadoEm)
			.IsRequired();

		builder.HasOne(x => x.Paciente)
			.WithMany(x => x.CheckInsEmocionais)
			.HasForeignKey(x => x.PacienteId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}