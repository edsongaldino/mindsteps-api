using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MindSteps.Domain.Entities;

namespace MindSteps.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
	public void Configure(EntityTypeBuilder<Usuario> builder)
	{
		builder.ToTable("Usuarios");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Nome)
			.IsRequired()
			.HasMaxLength(150);

		builder.Property(x => x.Email)
			.IsRequired()
			.HasMaxLength(180);

		builder.Property(x => x.Telefone)
			.HasMaxLength(20);

		builder.HasIndex(x => x.Email)
			.IsUnique();

		builder.Property(x => x.SenhaHash)
			.IsRequired()
			.HasMaxLength(500);

		builder.Property(x => x.Perfil)
			.IsRequired()
			.HasConversion<int>();

		builder.Property(x => x.Ativo)
			.IsRequired();

		builder.Property(x => x.CriadoEm)
			.IsRequired();

		builder.Property(x => x.AtualizadoEm);
	}
}