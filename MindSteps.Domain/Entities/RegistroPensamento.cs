namespace MindSteps.Domain.Entities;

public class RegistroPensamento
{
	public Guid Id { get; set; } = Guid.NewGuid();

	public Guid PacienteId { get; set; }

	public Paciente Paciente { get; set; } = null!;

	public Guid? AtividadePacienteId { get; set; }

	public AtividadePaciente? AtividadePaciente { get; set; }

	public string Situacao { get; set; } = string.Empty;

	public string PensamentoAutomatico { get; set; } = string.Empty;

	public string Emocao { get; set; } = string.Empty;

	public int IntensidadeEmocao { get; set; }

	public string? EvidenciasAFavor { get; set; }

	public string? EvidenciasContra { get; set; }

	public string? PensamentoAlternativo { get; set; }

	public int? IntensidadeFinal { get; set; }

	public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}