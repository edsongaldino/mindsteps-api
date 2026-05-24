namespace MindSteps.Application.DTOs;

public class MeResponseDto
{
	public Guid UsuarioId { get; set; }
	public Guid? PsicologoId { get; set; }
	public Guid? PacienteId { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string? Telefone { get; set; }
	public string Perfil { get; set; } = string.Empty;
	public int? Pontos { get; set; }
	public int? Nivel { get; set; }
	public string? FotoUrl { get; set; }
}