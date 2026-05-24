using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using MindSteps.Domain.Entities;
using MindSteps.Domain.Enums;
using MindSteps.Domain.Interfaces;

namespace MindSteps.Application.Services;

public class PsicologoService : IPsicologoService
{
	private readonly IPsicologoRepository _psicologoRepository;
	private readonly IUsuarioRepository _usuarioRepository;

	public PsicologoService(
		IPsicologoRepository psicologoRepository,
		IUsuarioRepository usuarioRepository)
	{
		_psicologoRepository = psicologoRepository;
		_usuarioRepository = usuarioRepository;
	}

	public async Task<IEnumerable<PsicologoResponseDto>> ObterTodosAsync()
	{
		var psicologos = await _psicologoRepository.ObterTodosAsync();

		return psicologos.Select(x => new PsicologoResponseDto
		{
			Id = x.Id,
			UsuarioId = x.UsuarioId,
			Nome = x.Usuario.Nome,
			Email = x.Usuario.Email,
			Telefone = x.Usuario.Telefone,
			Crp = x.Crp,
			Bio = x.Bio,
			FotoUrl = x.FotoUrl,
			Aprovado = x.Aprovado,
			Ativo = x.Usuario.Ativo
		});
	}

	public async Task<IEnumerable<PsicologoResponseDto>> ObterPendentesAsync()
	{
		var psicologos = await _psicologoRepository.ObterPendentesAsync();

		return psicologos.Select(x => new PsicologoResponseDto
		{
			Id = x.Id,
			UsuarioId = x.UsuarioId,
			Nome = x.Usuario.Nome,
			Email = x.Usuario.Email,
			Telefone = x.Usuario.Telefone,
			Crp = x.Crp,
			Bio = x.Bio,
			FotoUrl = x.FotoUrl,
			Aprovado = x.Aprovado,
			Ativo = x.Usuario.Ativo
		});
	}

	public async Task<PsicologoResponseDto?> ObterPorIdAsync(Guid id)
	{
		var psicologo = await _psicologoRepository.ObterPorIdAsync(id);

		if (psicologo is null)
			return null;

		return new PsicologoResponseDto
		{
			Id = psicologo.Id,
			UsuarioId = psicologo.UsuarioId,
			Nome = psicologo.Usuario.Nome,
			Email = psicologo.Usuario.Email,
			Telefone = psicologo.Usuario.Telefone,
			Crp = psicologo.Crp,
			Bio = psicologo.Bio,
			FotoUrl = psicologo.FotoUrl,
			Aprovado = psicologo.Aprovado,
			Ativo = psicologo.Usuario.Ativo
		};
	}

	public async Task<PsicologoResponseDto> CriarAsync(PsicologoCreateDto dto)
	{
		var emailExiste = await _usuarioRepository.ExisteEmailAsync(dto.Email);

		if (emailExiste)
			throw new Exception("Já existe um usuário com este e-mail.");

		var crpExiste = await _psicologoRepository.ExisteCrpAsync(dto.Crp);

		if (crpExiste)
			throw new Exception("Já existe um psicólogo cadastrado com este CRP.");

		var usuario = new Usuario
		{
			Nome = dto.Nome,
			Email = dto.Email.ToLower().Trim(),
			Telefone = dto.Telefone,
			SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
			Perfil = PerfilUsuario.Psicologo,
			Ativo = true,
			CriadoEm = DateTime.UtcNow
		};

		var psicologo = new Psicologo
		{
			Usuario = usuario,
			Crp = dto.Crp,
			Bio = dto.Bio,
			Aprovado = false,
			CriadoEm = DateTime.UtcNow
		};

		await _psicologoRepository.AdicionarAsync(psicologo);
		await _psicologoRepository.SalvarAlteracoesAsync();

		return new PsicologoResponseDto
		{
			Id = psicologo.Id,
			UsuarioId = usuario.Id,
			Nome = usuario.Nome,
			Email = usuario.Email,
			Telefone = usuario.Telefone,
			Crp = psicologo.Crp,
			Bio = psicologo.Bio,
			FotoUrl = psicologo.FotoUrl,
			Aprovado = psicologo.Aprovado,
			Ativo = usuario.Ativo
		};
	}

	public async Task<bool> AprovarAsync(Guid id)
	{
		var psicologo = await _psicologoRepository.ObterPorIdAsync(id);

		if (psicologo is null)
			return false;

		psicologo.Aprovado = true;
		psicologo.AtualizadoEm = DateTime.UtcNow;

		await _psicologoRepository.SalvarAlteracoesAsync();

		return true;
	}

	public async Task<PsicologoResponseDto?> AtualizarAsync(Guid id, PsicologoUpdateDto dto)
	{
		var psicologo = await _psicologoRepository.ObterPorIdAsync(id);

		if (psicologo is null)
			return null;

		psicologo.Usuario.Nome = dto.Nome;
		psicologo.Usuario.Email = dto.Email.ToLower().Trim();
		psicologo.Usuario.Telefone = dto.Telefone;
		psicologo.Crp = dto.Crp;
		psicologo.Bio = dto.Bio;
		psicologo.AtualizadoEm = DateTime.UtcNow;
		psicologo.Usuario.AtualizadoEm = DateTime.UtcNow;

		await _psicologoRepository.SalvarAlteracoesAsync();

		return new PsicologoResponseDto
		{
			Id = psicologo.Id,
			UsuarioId = psicologo.UsuarioId,
			Nome = psicologo.Usuario.Nome,
			Email = psicologo.Usuario.Email,
			Telefone = psicologo.Usuario.Telefone,
			Crp = psicologo.Crp,
			Bio = psicologo.Bio,
			FotoUrl = psicologo.FotoUrl,
			Aprovado = psicologo.Aprovado,
			Ativo = psicologo.Usuario.Ativo
		};
	}

	public async Task<bool> ReprovarAsync(Guid id)
	{
		var psicologo = await _psicologoRepository.ObterPorIdAsync(id);

		if (psicologo is null)
			return false;

		psicologo.Aprovado = false;
		psicologo.Usuario.Ativo = false;
		psicologo.AtualizadoEm = DateTime.UtcNow;
		psicologo.Usuario.AtualizadoEm = DateTime.UtcNow;

		await _psicologoRepository.SalvarAlteracoesAsync();

		return true;
	}
}