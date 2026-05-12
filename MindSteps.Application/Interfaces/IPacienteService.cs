using MindSteps.Application.DTOs;

namespace MindSteps.Application.Interfaces;

public interface IPacienteService
{
	Task<IEnumerable<PacienteResponseDto>> ObterTodosAsync();
	Task<IEnumerable<PacienteResponseDto>> ObterPorPsicologoAsync(Guid psicologoId);
	Task<PacienteResponseDto?> ObterPorIdAsync(Guid id);
	Task<PacienteResponseDto> CriarAsync(PacienteCreateDto dto);
	Task<bool> DesativarAsync(Guid id);
}