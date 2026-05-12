using MindSteps.Domain.Entities;

namespace MindSteps.Domain.Interfaces;

public interface IPsicologoRepository
{
	Task<List<Psicologo>> ObterTodosAsync();
	Task<List<Psicologo>> ObterPendentesAsync();
	Task<Psicologo?> ObterPorIdAsync(Guid id);
	Task<bool> ExisteCrpAsync(string crp);
	Task AdicionarAsync(Psicologo psicologo);
	Task SalvarAlteracoesAsync();
}