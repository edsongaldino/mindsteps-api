using MindSteps.Domain.Enums;

namespace MindSteps.Domain.Entities;

public class Atividade
{
	public Guid Id { get; set; } = Guid.NewGuid();

	public Guid PsicologoId { get; set; }

	public Psicologo Psicologo { get; set; } = null!;

	public string Titulo { get; set; } = string.Empty;

	public string? Descricao { get; set; }

	public TipoAtividade Tipo { get; set; }

	public string? Conteudo { get; set; }

	public string? AudioUrl { get; set; }

	public string? ArquivoUrl { get; set; }

	public bool Ativo { get; set; } = true;

	public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

	public DateTime? AtualizadoEm { get; set; }

	public ICollection<AtividadePaciente> Pacientes { get; set; } = new List<AtividadePaciente>();
}