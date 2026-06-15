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

		builder.Property(x => x.Documento)
			.IsRequired()
			.HasMaxLength(20);

		builder.Property(x => x.AsaasCustomerId)
			.HasMaxLength(50);

		builder.Property(x => x.AsaasSubscriptionId)
			.HasMaxLength(50);

		builder.Property(x => x.Pago)
			.IsRequired();

		builder.Property(x => x.Plano)
			.HasMaxLength(50);

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