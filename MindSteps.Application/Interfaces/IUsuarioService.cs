using MindSteps.Application.DTOs;

namespace MindSteps.Application.Interfaces;

public interface IUsuarioService
{
	Task<IEnumerable<UsuarioResponseDto>> ObterTodosAsync();
	Task<UsuarioResponseDto?> ObterPorIdAsync(Guid id);
	Task<UsuarioResponseDto> CriarAsync(UsuarioCreateDto dto);
	Task<UsuarioResponseDto?> AtualizarAsync(Guid id, UsuarioUpdateDto dto);
	Task<bool> DesativarAsync(Guid id);
	Task<string?> AtualizarFotoAsync(Guid id, string fotoUrl);
}