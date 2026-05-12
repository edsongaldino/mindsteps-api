namespace MindSteps.Domain.Entities;

public class Paciente
{
	public Guid Id { get; set; } = Guid.NewGuid();

	public Guid UsuarioId { get; set; }

	public Usuario Usuario { get; set; } = null!;

	public Guid PsicologoId { get; set; }

	public Psicologo Psicologo { get; set; } = null!;

	public DateTime? DataNascimento { get; set; }

	public string? Genero { get; set; }

	public string? FotoUrl { get; set; }

	public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

	public DateTime? AtualizadoEm { get; set; }

	public ICollection<AtividadePaciente> AtividadesRecebidas { get; set; } = new List<AtividadePaciente>();

	public ICollection<CheckInEmocional> CheckInsEmocionais { get; set; } = new List<CheckInEmocional>();

	public ICollection<RegistroPensamento> RegistrosPensamentos { get; set; } = new List<RegistroPensamento>();
}