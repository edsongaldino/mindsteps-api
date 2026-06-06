using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindSteps.Domain.Entities;

namespace MindSteps.Infrastructure.Persistence.Configurations;

public class UsuarioDispositivoConfiguration : IEntityTypeConfiguration<UsuarioDispositivo>
{
	public void Configure(EntityTypeBuilder<UsuarioDispositivo> builder)
	{
		builder.ToTable("UsuariosDispositivos");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.DeviceToken)
			.IsRequired()
			.HasMaxLength(500);

		builder.Property(x => x.Plataforma)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(x => x.AtualizadoEm)
			.IsRequired();

		builder.HasOne(x => x.Usuario)
			.WithMany()
			.HasForeignKey(x => x.UsuarioId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasIndex(x => x.DeviceToken)
			.IsUnique();
	}
}
