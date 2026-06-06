using MindSteps.Domain.Enums;

namespace MindSteps.Application.DTOs;

public class UsuarioCreateDto
{
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string? Telefone { get; set; }
	public string Senha { get; set; } = string.Empty;
	public PerfilUsuario Perfil { get; set; }
}

public class UsuarioResponseDto
{
	public Guid Id { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string? Telefone { get; set; }
	public PerfilUsuario Perfil { get; set; }
	public bool Ativo { get; set; }
	public string? FotoUrl { get; set; }
}

public class UsuarioUpdateDto
{
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string? Telefone { get; set; }
	public PerfilUsuario Perfil { get; set; }
}

public class RegistrarDeviceTokenDto
{
	public string DeviceToken { get; set; } = string.Empty;
	public string Plataforma { get; set; } = string.Empty;
}