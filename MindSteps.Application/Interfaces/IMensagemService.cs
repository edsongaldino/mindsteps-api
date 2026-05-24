using MindSteps.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindSteps.Application.Interfaces;

public interface IMensagemService
{
	Task<MensagemResponseDto> EnviarMensagemAsync(Guid psicologoId, MensagemCreateDto dto);

	Task<IEnumerable<MensagemResponseDto>> ObterMinhasMensagensAsync(Guid pacienteId);

	Task<IEnumerable<MensagemResponseDto>> ObterHistoricoPacienteAsync(Guid psicologoId, Guid pacienteId);

	Task<bool> MarcarComoLidaAsync(Guid mensagemId, Guid pacienteId);
}
