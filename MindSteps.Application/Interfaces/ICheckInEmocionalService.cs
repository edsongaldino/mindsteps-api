using MindSteps.Application.DTOs;

namespace MindSteps.Application.Interfaces;

public interface ICheckInEmocionalService
{
	Task<IEnumerable<CheckInEmocionalResponseDto>> ObterPorPacienteAsync(Guid pacienteId);
	Task<CheckInEmocionalResponseDto?> ObterPorIdAsync(Guid id);
	Task<CheckInEmocionalResponseDto> CriarAsync(CheckInEmocionalCreateDto dto);
}