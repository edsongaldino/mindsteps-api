using MindSteps.Application.DTOs;

namespace MindSteps.Application.Interfaces;

public interface IUsuarioService
{
	Task<IEnumerable<UsuarioResponseDto>> ObterTodosAsync();
	Task<UsuarioResponseDto?> ObterPorIdAsync(Guid id);
	Task<UsuarioResponseDto> CriarAsync(UsuarioCreateDto dto);
	Task<bool> DesativarAsync(Guid id);
}