using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using MindSteps.Domain.Interfaces;

namespace MindSteps.Application.Services;

public class AuthService : IAuthService
{
	private readonly IUsuarioRepository _usuarioRepository;
	private readonly IConfiguration _configuration;

	public AuthService(
		IUsuarioRepository usuarioRepository,
		IConfiguration configuration)
	{
		_usuarioRepository = usuarioRepository;
		_configuration = configuration;
	}

	public async Task<AuthResponseDto?> AutenticarAsync(LoginDto dto)
	{
		var usuario = await _usuarioRepository.ObterPorEmailAsync(dto.Email);

		if (usuario is null || !usuario.Ativo)
			return null;

		var senhaValida = BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash);

		if (!senhaValida)
			return null;

		var token = GerarToken(usuario);

		return new AuthResponseDto
		{
			Token = token,
			UsuarioId = usuario.Id,
			Nome = usuario.Nome,
			Email = usuario.Email,
			Perfil = usuario.Perfil.ToString()
		};
	}

	private string GerarToken(Domain.Entities.Usuario usuario)
	{
		var jwtKey = _configuration["JwtSettings:Secret"];

		if (string.IsNullOrWhiteSpace(jwtKey))
			throw new Exception("Chave JWT não configurada.");

		var claims = new[]
		{
			new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
			new Claim(ClaimTypes.Name, usuario.Nome),
			new Claim(ClaimTypes.Email, usuario.Email),
			new Claim(ClaimTypes.Role, usuario.Perfil.ToString())
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _configuration["JwtSettings:Issuer"],
			audience: _configuration["JwtSettings:Audience"],
			claims: claims,
			expires: DateTime.UtcNow.AddHours(8),
			signingCredentials: credentials
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	public async Task<MeResponseDto?> ObterUsuarioLogadoAsync(Guid usuarioId)
	{
		var usuario = await _usuarioRepository.ObterComPerfisPorIdAsync(usuarioId);

		if (usuario is null)
			return null;

		return new MeResponseDto
		{
			UsuarioId = usuario.Id,
			PsicologoId = usuario.Psicologo?.Id,
			PacienteId = usuario.Paciente?.Id,
			Nome = usuario.Nome,
			Email = usuario.Email,
			Perfil = usuario.Perfil.ToString()
		};
	}
}