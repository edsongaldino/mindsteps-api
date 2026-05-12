using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using MindSteps.Domain.Entities;
using MindSteps.Domain.Enums;
using MindSteps.Domain.Interfaces;

namespace MindSteps.Application.Services;

public class PacienteService : IPacienteService
{
	private readonly IPacienteRepository _pacienteRepository;
	private readonly IPsicologoRepository _psicologoRepository;
	private readonly IUsuarioRepository _usuarioRepository;

	public PacienteService(
		IPacienteRepository pacienteRepository,
		IPsicologoRepository psicologoRepository,
		IUsuarioRepository usuarioRepository)
	{
		_pacienteRepository = pacienteRepository;
		_psicologoRepository = psicologoRepository;
		_usuarioRepository = usuarioRepository;
	}

	public async Task<IEnumerable<PacienteResponseDto>> ObterTodosAsync()
	{
		var pacientes = await _pacienteRepository.ObterTodosAsync();

		return pacientes.Select(x => new PacienteResponseDto
		{
			Id = x.Id,
			UsuarioId = x.UsuarioId,
			PsicologoId = x.PsicologoId,
			Nome = x.Usuario.Nome,
			Email = x.Usuario.Email,
			DataNascimento = x.DataNascimento,
			Genero = x.Genero,
			FotoUrl = x.FotoUrl,
			Ativo = x.Usuario.Ativo
		});
	}

	public async Task<IEnumerable<PacienteResponseDto>> ObterPorPsicologoAsync(Guid psicologoId)
	{
		var pacientes = await _pacienteRepository.ObterPorPsicologoAsync(psicologoId);

		return pacientes.Select(x => new PacienteResponseDto
		{
			Id = x.Id,
			UsuarioId = x.UsuarioId,
			PsicologoId = x.PsicologoId,
			Nome = x.Usuario.Nome,
			Email = x.Usuario.Email,
			DataNascimento = x.DataNascimento,
			Genero = x.Genero,
			FotoUrl = x.FotoUrl,
			Ativo = x.Usuario.Ativo
		});
	}

	public async Task<PacienteResponseDto?> ObterPorIdAsync(Guid id)
	{
		var paciente = await _pacienteRepository.ObterPorIdAsync(id);

		if (paciente is null)
			return null;

		return new PacienteResponseDto
		{
			Id = paciente.Id,
			UsuarioId = paciente.UsuarioId,
			PsicologoId = paciente.PsicologoId,
			Nome = paciente.Usuario.Nome,
			Email = paciente.Usuario.Email,
			DataNascimento = paciente.DataNascimento,
			Genero = paciente.Genero,
			FotoUrl = paciente.FotoUrl,
			Ativo = paciente.Usuario.Ativo
		};
	}

	public async Task<PacienteResponseDto> CriarAsync(PacienteCreateDto dto)
	{
		var psicologo = await _psicologoRepository.ObterPorIdAsync(dto.PsicologoId);

		if (psicologo is null || !psicologo.Aprovado)
			throw new Exception("Psicólogo não encontrado ou ainda não aprovado.");

		var emailExiste = await _usuarioRepository.ExisteEmailAsync(dto.Email);

		if (emailExiste)
			throw new Exception("Já existe um usuário com este e-mail.");

		var usuario = new Usuario
		{
			Nome = dto.Nome,
			Email = dto.Email.ToLower().Trim(),
			SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
			Perfil = PerfilUsuario.Paciente,
			Ativo = true,
			CriadoEm = DateTime.UtcNow
		};

		var paciente = new Paciente
		{
			Usuario = usuario,
			PsicologoId = dto.PsicologoId,
			DataNascimento = dto.DataNascimento,
			Genero = dto.Genero,
			CriadoEm = DateTime.UtcNow
		};

		await _pacienteRepository.AdicionarAsync(paciente);
		await _pacienteRepository.SalvarAlteracoesAsync();

		return new PacienteResponseDto
		{
			Id = paciente.Id,
			UsuarioId = usuario.Id,
			PsicologoId = paciente.PsicologoId,
			Nome = usuario.Nome,
			Email = usuario.Email,
			DataNascimento = paciente.DataNascimento,
			Genero = paciente.Genero,
			FotoUrl = paciente.FotoUrl,
			Ativo = usuario.Ativo
		};
	}

	public async Task<bool> DesativarAsync(Guid id)
	{
		var paciente = await _pacienteRepository.ObterPorIdAsync(id);

		if (paciente is null)
			return false;

		paciente.Usuario.Ativo = false;
		paciente.AtualizadoEm = DateTime.UtcNow;
		paciente.Usuario.AtualizadoEm = DateTime.UtcNow;

		await _pacienteRepository.SalvarAlteracoesAsync();

		return true;
	}
}