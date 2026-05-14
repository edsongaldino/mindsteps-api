using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using MindSteps.Domain.Entities;
using MindSteps.Domain.Interfaces;

namespace MindSteps.Application.Services;

public class UsuarioService : IUsuarioService
{
	private readonly IUsuarioRepository _usuarioRepository;

	public UsuarioService(IUsuarioRepository usuarioRepository)
	{
		_usuarioRepository = usuarioRepository;
	}

	public async Task<IEnumerable<UsuarioResponseDto>> ObterTodosAsync()
	{
		var usuarios = await _usuarioRepository.ObterTodosAsync();

		return usuarios.Select(x => new UsuarioResponseDto
		{
			Id = x.Id,
			Nome = x.Nome,
			Email = x.Email,
			Perfil = x.Perfil,
			Ativo = x.Ativo
		});
	}

	public async Task<UsuarioResponseDto?> ObterPorIdAsync(Guid id)
	{
		var usuario = await _usuarioRepository.ObterPorIdAsync(id);

		if (usuario is null)
			return null;

		return new UsuarioResponseDto
		{
			Id = usuario.Id,
			Nome = usuario.Nome,
			Email = usuario.Email,
			Perfil = usuario.Perfil,
			Ativo = usuario.Ativo
		};
	}

	public async Task<UsuarioResponseDto> CriarAsync(UsuarioCreateDto dto)
	{
		var emailExiste = await _usuarioRepository.ExisteEmailAsync(dto.Email);

		if (emailExiste)
			throw new Exception("Já existe um usuário com este e-mail.");

		var usuario = new Usuario
		{
			Nome = dto.Nome,
			Email = dto.Email.ToLower().Trim(),
			SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
			Perfil = dto.Perfil,
			Ativo = true,
			CriadoEm = DateTime.UtcNow
		};

		await _usuarioRepository.AdicionarAsync(usuario);
		await _usuarioRepository.SalvarAlteracoesAsync();

		return new UsuarioResponseDto
		{
			Id = usuario.Id,
			Nome = usuario.Nome,
			Email = usuario.Email,
			Perfil = usuario.Perfil,
			Ativo = usuario.Ativo
		};
	}

	public async Task<UsuarioResponseDto?> AtualizarAsync(Guid id, UsuarioUpdateDto dto)
	{
		var usuario = await _usuarioRepository.ObterPorIdAsync(id);

		if (usuario is null)
			return null;

		usuario.Nome = dto.Nome;
		usuario.Email = dto.Email.ToLower().Trim();
		usuario.Perfil = dto.Perfil;
		usuario.AtualizadoEm = DateTime.UtcNow;

		await _usuarioRepository.SalvarAlteracoesAsync();

		return new UsuarioResponseDto
		{
			Id = usuario.Id,
			Nome = usuario.Nome,
			Email = usuario.Email,
			Perfil = usuario.Perfil,
			Ativo = usuario.Ativo
		};
	}

	public async Task<bool> DesativarAsync(Guid id)
	{
		var usuario = await _usuarioRepository.ObterPorIdAsync(id);

		if (usuario is null)
			return false;

		usuario.Ativo = false;
		usuario.AtualizadoEm = DateTime.UtcNow;

		await _usuarioRepository.SalvarAlteracoesAsync();

		return true;
	}
}
