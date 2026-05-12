using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSteps.Application.DTOs;

public class RegistroPensamentoCreateDto
{
	public Guid PacienteId { get; set; }
	public Guid? AtividadePacienteId { get; set; }

	public string Situacao { get; set; } = string.Empty;
	public string PensamentoAutomatico { get; set; } = string.Empty;
	public string Emocao { get; set; } = string.Empty;
	public int IntensidadeEmocao { get; set; }

	public string? EvidenciasAFavor { get; set; }
	public string? EvidenciasContra { get; set; }
	public string? PensamentoAlternativo { get; set; }
	public int? IntensidadeFinal { get; set; }
}

public class RegistroPensamentoResponseDto
{
	public Guid Id { get; set; }
	public Guid PacienteId { get; set; }
	public Guid? AtividadePacienteId { get; set; }

	public string Situacao { get; set; } = string.Empty;
	public string PensamentoAutomatico { get; set; } = string.Empty;
	public string Emocao { get; set; } = string.Empty;
	public int IntensidadeEmocao { get; set; }

	public string? EvidenciasAFavor { get; set; }
	public string? EvidenciasContra { get; set; }
	public string? PensamentoAlternativo { get; set; }
	public int? IntensidadeFinal { get; set; }

	public DateTime CriadoEm { get; set; }
}