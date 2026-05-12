using MindSteps.Domain.Entities;

namespace MindSteps.Domain.Interfaces;

public interface IRegistroPensamentoRepository
{
	Task<List<RegistroPensamento>> ObterPorPacienteAsync(Guid pacienteId);
	Task<RegistroPensamento?> ObterPorIdAsync(Guid id);
	Task AdicionarAsync(RegistroPensamento registro);
	Task SalvarAlteracoesAsync();
}