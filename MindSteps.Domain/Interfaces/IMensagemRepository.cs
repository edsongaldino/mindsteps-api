using MindSteps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindSteps.Domain.Interfaces;

public interface IMensagemRepository
{
	Task<Mensagem?> ObterPorIdAsync(Guid id);

	Task<List<Mensagem>> ObterMensagensPorPacienteAsync(Guid pacienteId);

	Task<List<Mensagem>> ObterMensagensEntrePsicologoEPacienteAsync(Guid psicologoId, Guid pacienteId);

	Task AdicionarAsync(Mensagem mensagem);

	Task SalvarAlteracoesAsync();
}
