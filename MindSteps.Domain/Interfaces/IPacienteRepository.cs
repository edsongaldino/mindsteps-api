using MindSteps.Domain.Entities;

namespace MindSteps.Domain.Interfaces;

public interface IPacienteRepository
{
	Task<List<Paciente>> ObterTodosAsync();
	Task<List<Paciente>> ObterPorPsicologoAsync(Guid psicologoId);
	Task<Paciente?> ObterPorIdAsync(Guid id);
	Task AdicionarAsync(Paciente paciente);
	Task SalvarAlteracoesAsync();
}