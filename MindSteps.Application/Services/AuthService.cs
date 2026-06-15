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
			Perfil = usuario.Perfil.ToString(),
			Aprovado = usuario.Perfil != MindSteps.Domain.Enums.PerfilUsuario.Psicologo || (usuario.Psicologo != null && usuario.Psicologo.Aprovado),
			FotoUrl = usuario.Paciente?.FotoUrl ?? usuario.Psicologo?.FotoUrl
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
			Telefone = usuario.Telefone,
			Perfil = usuario.Perfil.ToString(),
			Pontos = usuario.Paciente?.Pontos,
			Nivel = usuario.Paciente?.Nivel,
			FotoUrl = usuario.Paciente?.FotoUrl ?? usuario.Psicologo?.FotoUrl,
			Aprovado = usuario.Perfil != MindSteps.Domain.Enums.PerfilUsuario.Psicologo || (usuario.Psicologo != null && usuario.Psicologo.Aprovado),
			Plano = usuario.Psicologo?.Plano
		};
	}

	public async Task<bool> RecuperarSenhaAsync(string email)
	{
		var usuario = await _usuarioRepository.ObterPorEmailAsync(email);
		if (usuario is null)
		{
			throw new System.Exception("Nenhum usuário cadastrado com este e-mail.");
		}

		var subject = "MindSteps - Recuperação de Senha";
		var body = $@"Olá {usuario.Nome},

Recebemos uma solicitação de recuperação de senha para sua conta MindSteps.
Seu token de recuperação simulado é: {System.Guid.NewGuid().ToString().Substring(0, 8)}

Para redefinir sua senha, acesse o link de recuperação.
Caso não tenha solicitado a alteração, desconsidere este e-mail.

Atenciosamente,
Equipe MindSteps";

		System.Console.WriteLine("==================================================");
		System.Console.WriteLine($"[EMAIL ENVIADO] Para: {email}");
		System.Console.WriteLine($"Assunto: {subject}");
		System.Console.WriteLine("Corpo do e-mail:");
		System.Console.WriteLine(body);
		System.Console.WriteLine("==================================================");

		try
		{
			var logPath = @"C:\Projects\mindsteps-api\recovery_email_log.txt";
			var logContent = $"Data/Hora: {System.DateTime.Now}\nPara: {email}\nAssunto: {subject}\n\n{body}\n\n==================================================\n\n";
			await System.IO.File.AppendAllTextAsync(logPath, logContent);
		}
		catch (System.Exception ex)
		{
			System.Console.WriteLine($"Erro ao gravar log de e-mail de recuperação: {ex.Message}");
		}

		return true;
	}
}