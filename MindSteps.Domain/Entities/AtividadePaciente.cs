using MindSteps.Domain.Enums;

namespace MindSteps.Domain.Entities;

public class AtividadePaciente
{
	public Guid Id { get; set; } = Guid.NewGuid();

	public Guid AtividadeId { get; set; }

	public Atividade Atividade { get; set; } = null!;

	public Guid PacienteId { get; set; }

	public Paciente Paciente { get; set; } = null!;

	public StatusAtividadePaciente Status { get; set; } = StatusAtividadePaciente.Pendente;

	public DateTime DataEnvio { get; set; } = DateTime.UtcNow;

	public DateTime? DataLimite { get; set; }

	public DateTime? DataConclusao { get; set; }

	public string? RespostaTexto { get; set; }

	public int? NotaHumor { get; set; }

	public string? FeedbackPsicologo { get; set; }

	public DateTime? FeedbackEnviadoEm { get; set; }
}