using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using MindSteps.Domain.Entities;
using MindSteps.Domain.Interfaces;

namespace MindSteps.Application.Services;

public class CheckInEmocionalService : ICheckInEmocionalService
{
	private readonly ICheckInEmocionalRepository _checkInRepository;
	private readonly IPacienteRepository _pacienteRepository;

	public CheckInEmocionalService(
		ICheckInEmocionalRepository checkInRepository,
		IPacienteRepository pacienteRepository)
	{
		_checkInRepository = checkInRepository;
		_pacienteRepository = pacienteRepository;
	}

	public async Task<IEnumerable<CheckInEmocionalResponseDto>> ObterPorPacienteAsync(Guid pacienteId)
	{
		var checkIns = await _checkInRepository.ObterPorPacienteAsync(pacienteId);

		return checkIns.Select(MapToResponse);
	}

	public async Task<CheckInEmocionalResponseDto?> ObterPorIdAsync(Guid id)
	{
		var checkIn = await _checkInRepository.ObterPorIdAsync(id);

		if (checkIn is null)
			return null;

		return MapToResponse(checkIn);
	}

	public async Task<bool> VerificarCheckInHojeAsync(Guid pacienteId)
	{
		return await _checkInRepository.JaFezCheckInHojeAsync(pacienteId);
	}

	public async Task<CheckInEmocionalResponseDto> CriarAsync(CheckInEmocionalCreateDto dto)
	{
		var paciente = await _pacienteRepository.ObterPorIdAsync(dto.PacienteId);

		if (paciente is null || !paciente.Usuario.Ativo)
			throw new Exception("Paciente não encontrado ou inativo.");

		if (await _checkInRepository.JaFezCheckInHojeAsync(dto.PacienteId))
			throw new Exception("Você já realizou seu check-in emocional hoje.");

		if (dto.Intensidade < 0 || dto.Intensidade > 10)
			throw new Exception("A intensidade deve estar entre 0 e 10.");

		var checkIn = new CheckInEmocional
		{
			PacienteId = dto.PacienteId,
			Humor = dto.Humor,
			Intensidade = dto.Intensidade,
			EmocaoPrincipal = dto.EmocaoPrincipal,
			Observacao = dto.Observacao,
			CriadoEm = DateTime.UtcNow
		};

		await _checkInRepository.AdicionarAsync(checkIn);
		await _checkInRepository.SalvarAlteracoesAsync();

		return MapToResponse(checkIn);
	}

	private static CheckInEmocionalResponseDto MapToResponse(CheckInEmocional checkIn)
	{
		return new CheckInEmocionalResponseDto
		{
			Id = checkIn.Id,
			PacienteId = checkIn.PacienteId,
			Humor = checkIn.Humor,
			Intensidade = checkIn.Intensidade,
			EmocaoPrincipal = checkIn.EmocaoPrincipal,
			Observacao = checkIn.Observacao,
			CriadoEm = checkIn.CriadoEm
		};
	}
}