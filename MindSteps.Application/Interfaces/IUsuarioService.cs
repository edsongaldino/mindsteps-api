using MindSteps.Application.DTOs;

namespace MindSteps.Application.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> ObterTodosAsync();
    Task<UsuarioDto> ObterPorIdAsync(Guid id);
    Task<UsuarioDto> CriarAsync(UsuarioDto dto);
    Task<UsuarioDto> AtualizarAsync(Guid id, UsuarioDto dto);
    Task<bool> RemoverAsync(Guid id);
    Task<string> AutenticarAsync(LoginDto login);
}