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
	private readonly IEmailService _emailService;

	public AuthService(
		IUsuarioRepository usuarioRepository,
		IConfiguration configuration,
		IEmailService emailService)
	{
		_usuarioRepository = usuarioRepository;
		_configuration = configuration;
		_emailService = emailService;
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
		var usuario = await _usuarioRepository.ObterPorEmailAsync(email.ToLower().Trim());
		if (usuario is null)
		{
			throw new System.Exception("Nenhum usuário cadastrado com este e-mail.");
		}

		var codigoValidacao = new Random().Next(100000, 999999).ToString();
		usuario.CodigoVerificacao = codigoValidacao;
		usuario.CodigoVerificacaoExpiracao = DateTime.UtcNow.AddMinutes(30);

		await _usuarioRepository.SalvarAlteracoesAsync();

		var emailBody = MindSteps.Application.Utils.EmailTemplates.GetPasswordResetEmail(usuario.Nome, codigoValidacao);

		await _emailService.SendEmailAsync(
			usuario.Email,
			"MindSteps - Recuperação de Senha",
			emailBody
		);

		return true;
	}

	public async Task<AuthResponseDto> RedefinirSenhaAsync(RedefinirSenhaDto dto)
	{
		var usuario = await _usuarioRepository.ObterPorEmailAsync(dto.Email.ToLower().Trim());
		
		if (usuario == null || usuario.CodigoVerificacao != dto.Codigo)
		{
			throw new Exception("Código de verificação inválido.");
		}
		if (usuario.CodigoVerificacaoExpiracao < DateTime.UtcNow)
		{
			throw new Exception("Código de verificação expirado.");
		}

		usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);
		usuario.CodigoVerificacao = null;
		usuario.CodigoVerificacaoExpiracao = null;

		await _usuarioRepository.SalvarAlteracoesAsync();

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

	public async Task<bool> ValidarCodigoRecuperacaoAsync(string email, string codigo)
	{
		var usuario = await _usuarioRepository.ObterPorEmailAsync(email.ToLower().Trim());
		
		if (usuario == null || usuario.CodigoVerificacao != codigo)
		{
			throw new Exception("Código de verificação inválido.");
		}
		if (usuario.CodigoVerificacaoExpiracao < DateTime.UtcNow)
		{
			throw new Exception("Código de verificação expirado.");
		}

		return true;
	}
}