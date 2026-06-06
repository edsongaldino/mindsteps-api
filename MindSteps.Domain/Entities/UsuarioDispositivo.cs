using System;

namespace MindSteps.Domain.Entities;

public class UsuarioDispositivo
{
	public Guid Id { get; set; } = Guid.NewGuid();

	public Guid UsuarioId { get; set; }

	public Usuario Usuario { get; set; } = null!;

	public string DeviceToken { get; set; } = string.Empty;

	public string Plataforma { get; set; } = string.Empty;

	public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;
}
