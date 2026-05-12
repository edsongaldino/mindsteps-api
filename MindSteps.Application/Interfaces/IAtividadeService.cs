using MindSteps.Application.DTOs;

namespace MindSteps.Application.Interfaces;

public interface IAtividadeService
{
	Task<IEnumerable<AtividadeResponseDto>> ObterTodasAsync();
	Task<IEnumerable<AtividadeResponseDto>> ObterPorPsicologoAsync(Guid psicologoId);
	Task<AtividadeResponseDto?> ObterPorIdAsync(Guid id);
	Task<AtividadeResponseDto> CriarAsync(AtividadeCreateDto dto);
	Task<AtividadePacienteResponseDto> EnviarParaPacienteAsync(EnviarAtividadeDto dto);
	Task<IEnumerable<AtividadePacienteResponseDto>> ObterAtividadesPorPacienteAsync(Guid pacienteId);
	Task<AtividadePacienteResponseDto?> ResponderAtividadeAsync(ResponderAtividadeDto dto);

}