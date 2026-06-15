namespace MindSteps.Domain.Entities;

public class Psicologo
{
	public Guid Id { get; set; } = Guid.NewGuid();

	public Guid UsuarioId { get; set; }

	public Usuario Usuario { get; set; } = null!;

	public string Crp { get; set; } = string.Empty;

	public string Documento { get; set; } = string.Empty;

	public string? AsaasCustomerId { get; set; }

	public string? AsaasSubscriptionId { get; set; }

	public bool Pago { get; set; } = false;

	public string? Plano { get; set; }

	public string? Bio { get; set; }

	public string? FotoUrl { get; set; }

	public bool Aprovado { get; set; } = false;

	public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

	public DateTime? AtualizadoEm { get; set; }

	public ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();

	public ICollection<Atividade> Atividades { get; set; } = new List<Atividade>();
}