namespace MindSteps.Application.DTOs;

public class MeResponseDto
{
	public Guid UsuarioId { get; set; }
	public Guid? PsicologoId { get; set; }
	public Guid? PacienteId { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Perfil { get; set; } = string.Empty;
}