using System;

namespace MindSteps.Domain.Entities;

public class Mensagem
{
	public Guid Id { get; set; } = Guid.NewGuid();

	public Guid PsicologoId { get; set; }

	public Psicologo Psicologo { get; set; } = null!;

	public Guid PacienteId { get; set; }

	public Paciente Paciente { get; set; } = null!;

	public string Conteudo { get; set; } = string.Empty;

	public bool Lida { get; set; } = false;

	public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
