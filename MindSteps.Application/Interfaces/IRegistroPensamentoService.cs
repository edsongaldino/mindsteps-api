using MindSteps.Application.DTOs;

namespace MindSteps.Application.Interfaces;

public interface IRegistroPensamentoService
{
	Task<IEnumerable<RegistroPensamentoResponseDto>> ObterPorPacienteAsync(Guid pacienteId);
	Task<RegistroPensamentoResponseDto?> ObterPorIdAsync(Guid id);
	Task<RegistroPensamentoResponseDto> CriarAsync(RegistroPensamentoCreateDto dto);
}