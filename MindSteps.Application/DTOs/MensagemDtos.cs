using System;

namespace MindSteps.Application.DTOs;

public class MensagemCreateDto
{
	public Guid PacienteId { get; set; }
	public string Conteudo { get; set; } = string.Empty;
}

public class MensagemResponseDto
{
	public Guid Id { get; set; }
	public Guid PsicologoId { get; set; }
	public string PsicologoNome { get; set; } = string.Empty;
	public Guid PacienteId { get; set; }
	public string PacienteNome { get; set; } = string.Empty;
	public string Conteudo { get; set; } = string.Empty;
	public bool Lida { get; set; }
	public DateTime CriadoEm { get; set; }
}
