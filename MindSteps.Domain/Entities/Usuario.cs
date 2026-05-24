using MindSteps.Domain.Enums;

namespace MindSteps.Domain.Entities;

public class Usuario
{
	public Guid Id { get; set; } = Guid.NewGuid();

	public string Nome { get; set; } = string.Empty;

	public string Email { get; set; } = string.Empty;

	public string? Telefone { get; set; }

	public string SenhaHash { get; set; } = string.Empty;

	public PerfilUsuario Perfil { get; set; }

	public bool Ativo { get; set; } = true;

	public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

	public DateTime? AtualizadoEm { get; set; }

	public Psicologo? Psicologo { get; set; }

	public Paciente? Paciente { get; set; }
}