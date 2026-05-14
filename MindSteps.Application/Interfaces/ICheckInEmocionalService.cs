using MindSteps.Application.DTOs;

namespace MindSteps.Application.Interfaces;

public interface ICheckInEmocionalService
{
	Task<IEnumerable<CheckInEmocionalResponseDto>> ObterPorPacienteAsync(Guid pacienteId);
	Task<CheckInEmocionalResponseDto?> ObterPorIdAsync(Guid id);
	Task<bool> VerificarCheckInHojeAsync(Guid pacienteId);
	Task<CheckInEmocionalResponseDto> CriarAsync(CheckInEmocionalCreateDto dto);
}