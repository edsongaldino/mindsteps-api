namespace MindSteps.Application.DTOs;

public class LoginDto
{
	public string Email { get; set; } = string.Empty;
	public string Senha { get; set; } = string.Empty;
}

public class AuthResponseDto
{
	public string Token { get; set; } = string.Empty;
	public Guid UsuarioId { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Perfil { get; set; } = string.Empty;
	public string? FotoUrl { get; set; }
	public bool Aprovado { get; set; } = true;
}

public class RecuperarSenhaDto
{
	public string Email { get; set; } = string.Empty;
}