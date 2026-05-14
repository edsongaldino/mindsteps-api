using MindSteps.Domain.Entities;

namespace MindSteps.Domain.Interfaces;

public interface ICheckInEmocionalRepository
{
	Task<List<CheckInEmocional>> ObterPorPacienteAsync(Guid pacienteId);
	Task<CheckInEmocional?> ObterPorIdAsync(Guid id);
	Task<bool> JaFezCheckInHojeAsync(Guid pacienteId);
	Task AdicionarAsync(CheckInEmocional checkIn);
	Task SalvarAlteracoesAsync();
}