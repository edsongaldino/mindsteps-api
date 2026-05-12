using MindSteps.Domain.Enums;

namespace MindSteps.Domain.Entities;

public class CheckInEmocional
{
	public Guid Id { get; set; } = Guid.NewGuid();

	public Guid PacienteId { get; set; }

	public Paciente Paciente { get; set; } = null!;

	public Humor Humor { get; set; }

	public int Intensidade { get; set; }

	public string? EmocaoPrincipal { get; set; }

	public string? Observacao { get; set; }

	public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}