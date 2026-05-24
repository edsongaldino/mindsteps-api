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

	// Novos campos da Wizard
	public string? TipoResposta { get; set; }
	public bool AtividadeObrigatoria { get; set; } = true;
	public bool PermitirAnexos { get; set; }
	public string? FeedbackAutomatico { get; set; }
	public string? CategoriaEmocional { get; set; }
	public string? NivelSugerido { get; set; }
	public int Nivel { get; set; } = 1;
	public string? Frequencia { get; set; }
	public string? DiasSemana { get; set; }
	public string? HorarioSugerido { get; set; }
	public string? PrazoConclusao { get; set; }
	public bool NotificarPush { get; set; } = true;
	public bool NotificarEmail { get; set; } = true;
	public bool LembreteSuave { get; set; }

	public bool Ativo { get; set; } = true;

	public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

	public DateTime? AtualizadoEm { get; set; }

	public ICollection<AtividadePaciente> Pacientes { get; set; } = new List<AtividadePaciente>();
}