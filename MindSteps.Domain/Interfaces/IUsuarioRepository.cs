using MindSteps.Domain.Entities;

namespace MindSteps.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> ObterTodosAsync();
    Task<Usuario> ObterPorIdAsync(Guid id);
    Task<Usuario> CriarAsync(Usuario usuario);
    Task<Usuario> AtualizarAsync(Usuario usuario);
    Task<bool> RemoverAsync(Guid id);
    Task<Usuario> ObterPorEmailSenhaAsync(string email, string senha);
}