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
	public TipoAtividade Tipo { get; set; }
	public StatusAtividadePaciente Status { get; set; }
	public DateTime DataEnvio { get; set; }
	public DateTime? DataLimite { get; set; }
	public DateTime? DataConclusao { get; set; }
	public string? RespostaTexto { get; set; }
}

public class ResponderAtividadeDto
{
	public Guid AtividadePacienteId { get; set; }
	public string? RespostaTexto { get; set; }
	public int? NotaHumor { get; set; }
}