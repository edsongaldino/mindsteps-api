using MindSteps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSteps.Application.DTOs;

public class AtividadeCreateDto
{
	public Guid PsicologoId { get; set; }
	public string Titulo { get; set; } = string.Empty;
	public string? Descricao { get; set; }
	public TipoAtividade Tipo { get; set; }
	public string? Conteudo { get; set; }
	public string? AudioUrl { get; set; }
	public string? ArquivoUrl { get; set; }

	// Novos campos
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
}

public class AtividadeResponseDto
{
	public Guid Id { get; set; }
	public Guid PsicologoId { get; set; }
	public string Titulo { get; set; } = string.Empty;
	public string? Descricao { get; set; }
	public TipoAtividade Tipo { get; set; }
	public string? Conteudo { get; set; }
	public string? AudioUrl { get; set; }
	public string? ArquivoUrl { get; set; }
	public bool Ativo { get; set; }
	public DateTime CriadoEm { get; set; }

	// Novos campos
	public string? TipoResposta { get; set; }
	public bool AtividadeObrigatoria { get; set; }
	public bool PermitirAnexos { get; set; }
	public string? FeedbackAutomatico { get; set; }
	public string? CategoriaEmocional { get; set; }
	public string? NivelSugerido { get; set; }
	public int Nivel { get; set; }
	public string? Frequencia { get; set; }
	public string? DiasSemana { get; set; }
	public string? HorarioSugerido { get; set; }
	public string? PrazoConclusao { get; set; }
	public bool NotificarPush { get; set; }
	public bool NotificarEmail { get; set; }
	public bool LembreteSuave { get; set; }
}

public class EnviarAtividadeDto
{
	public Guid AtividadeId { get; set; }
	public Guid PacienteId { get; set; }
	public DateTime? DataLimite { get; set; }
}

public class AtividadePacienteResponseDto
{
	public Guid Id { get; set; }
	public Guid AtividadeId { get; set; }
	public Guid PacienteId { get; set; }
	public string Titulo { get; set; } = string.Empty;
	public string? Descricao { get; set; }
	public string? Conteudo { get; set; }
	public TipoAtividade Tipo { get; set; }
	public StatusAtividadePaciente Status { get; set; }
	public int Nivel { get; set; }
	public DateTime DataEnvio { get; set; }
	public DateTime? DataLimite { get; set; }
	public DateTime? DataConclusao { get; set; }
	public string? RespostaTexto { get; set; }
	public int? NotaHumor { get; set; }
}

public class ResponderAtividadeDto
{
	public Guid AtividadePacienteId { get; set; }
	public string? RespostaTexto { get; set; }
	public int? NotaHumor { get; set; }
}