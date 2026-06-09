using System;

namespace MindSteps.Domain.Entities;

public class RegistroJogo
{
	public Guid Id { get; set; } = Guid.NewGuid();

	public Guid PacienteId { get; set; }

	public Paciente Paciente { get; set; } = null!;

	public string JogoId { get; set; } = string.Empty; // "detetive", "tribunal", etc.

	public DateTime DataPlay { get; set; } = DateTime.UtcNow;

	public string DadosPlay { get; set; } = string.Empty; // JSON contendo escolhas da rodada

	public Guid? AtividadePacienteId { get; set; }

	public AtividadePaciente? AtividadePaciente { get; set; }
}
