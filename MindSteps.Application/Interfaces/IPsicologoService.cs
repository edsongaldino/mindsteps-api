using MindSteps.Application.DTOs;

namespace MindSteps.Application.Interfaces;

public interface IPsicologoService
{
	Task<IEnumerable<PsicologoResponseDto>> ObterTodosAsync();
	Task<IEnumerable<PsicologoResponseDto>> ObterPendentesAsync();
	Task<PsicologoResponseDto?> ObterPorIdAsync(Guid id);
	Task<PsicologoResponseDto> CriarAsync(PsicologoCreateDto dto);
	Task<bool> AprovarAsync(Guid id);
	Task<bool> ReprovarAsync(Guid id);
}