using MindSteps.Domain.Entities;

namespace MindSteps.Domain.Interfaces;

public interface IUsuarioRepository
{
	Task<List<Usuario>> ObterTodosAsync();
	Task<Usuario?> ObterPorIdAsync(Guid id);
	Task<Usuario?> ObterPorEmailAsync(string email);
	Task<bool> ExisteEmailAsync(string email);
	Task AdicionarAsync(Usuario usuario);
	Task SalvarAlteracoesAsync();
	Task<Usuario?> ObterComPerfisPorIdAsync(Guid id);
	Task RegistrarDeviceTokenAsync(Guid usuarioId, string deviceToken, string plataforma);
	Task<List<string>> ObterDeviceTokensUsuarioAsync(Guid usuarioId);
}