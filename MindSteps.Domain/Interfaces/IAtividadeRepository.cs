using MindSteps.Domain.Entities;

namespace MindSteps.Domain.Interfaces;

public interface IAtividadeRepository
{
	Task<List<Atividade>> ObterTodasAsync();
	Task<List<Atividade>> ObterPorPsicologoAsync(Guid psicologoId);
	Task<Atividade?> ObterPorIdAsync(Guid id);
	Task AdicionarAsync(Atividade atividade);
	Task SalvarAlteracoesAsync();

	Task AdicionarAtividadePacienteAsync(AtividadePaciente atividadePaciente);
	Task<List<AtividadePaciente>> ObterAtividadesPorPacienteAsync(Guid pacienteId);
	Task<AtividadePaciente?> ObterAtividadePacientePorIdAsync(Guid id);
}