using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using MindSteps.Domain.Entities;
using MindSteps.Domain.Interfaces;

namespace MindSteps.Application.Services;

public class RegistroPensamentoService : IRegistroPensamentoService
{
	private readonly IRegistroPensamentoRepository _registroRepository;
	private readonly IPacienteRepository _pacienteRepository;
	private readonly IAtividadeRepository _atividadeRepository;

	public RegistroPensamentoService(
		IRegistroPensamentoRepository registroRepository,
		IPacienteRepository pacienteRepository,
		IAtividadeRepository atividadeRepository)
	{
		_registroRepository = registroRepository;
		_pacienteRepository = pacienteRepository;
		_atividadeRepository = atividadeRepository;
	}

	public async Task<IEnumerable<RegistroPensamentoResponseDto>> ObterPorPacienteAsync(Guid pacienteId)
	{
		var registros = await _registroRepository.ObterPorPacienteAsync(pacienteId);

		return registros.Select(MapToResponse);
	}

	public async Task<RegistroPensamentoResponseDto?> ObterPorIdAsync(Guid id)
	{
		var registro = await _registroRepository.ObterPorIdAsync(id);

		if (registro is null)
			return null;

		return MapToResponse(registro);
	}

	public async Task<RegistroPensamentoResponseDto> CriarAsync(RegistroPensamentoCreateDto dto)
	{
		var paciente = await _pacienteRepository.ObterPorIdAsync(dto.PacienteId);

		if (paciente is null || !paciente.Usuario.Ativo)
			throw new Exception("Paciente não encontrado ou inativo.");

		if (dto.IntensidadeEmocao < 0 || dto.IntensidadeEmocao > 10)
			throw new Exception("A intensidade da emoção deve estar entre 0 e 10.");

		if (dto.IntensidadeFinal.HasValue && (dto.IntensidadeFinal < 0 || dto.IntensidadeFinal > 10))
			throw new Exception("A intensidade final deve estar entre 0 e 10.");

		if (dto.AtividadePacienteId.HasValue)
		{
			var atividadePaciente = await _atividadeRepository.ObterAtividadePacientePorIdAsync(dto.AtividadePacienteId.Value);

			if (atividadePaciente is null || atividadePaciente.PacienteId != dto.PacienteId)
				throw new Exception("Atividade do paciente inválida.");
		}

		var registro = new RegistroPensamento
		{
			PacienteId = dto.PacienteId,
			AtividadePacienteId = dto.AtividadePacienteId,
			Situacao = dto.Situacao,
			PensamentoAutomatico = dto.PensamentoAutomatico,
			Emocao = dto.Emocao,
			IntensidadeEmocao = dto.IntensidadeEmocao,
			EvidenciasAFavor = dto.EvidenciasAFavor,
			EvidenciasContra = dto.EvidenciasContra,
			PensamentoAlternativo = dto.PensamentoAlternativo,
			IntensidadeFinal = dto.IntensidadeFinal,
			CriadoEm = DateTime.UtcNow
		};

		await _registroRepository.AdicionarAsync(registro);

		// Gamificação: conceder 15 XP caso seja um registro de pensamentos avulso (independente de atividade)
		if (!dto.AtividadePacienteId.HasValue)
		{
			paciente.Pontos += 15;
			var novoNivel = (paciente.Pontos / 100) + 1;
			if (novoNivel > paciente.Nivel)
			{
				paciente.Nivel = novoNivel;
			}
		}

		await _registroRepository.SalvarAlteracoesAsync();

		return MapToResponse(registro);
	}

	private static RegistroPensamentoResponseDto MapToResponse(RegistroPensamento registro)
	{
		return new RegistroPensamentoResponseDto
		{
			Id = registro.Id,
			PacienteId = registro.PacienteId,
			AtividadePacienteId = registro.AtividadePacienteId,
			Situacao = registro.Situacao,
			PensamentoAutomatico = registro.PensamentoAutomatico,
			Emocao = registro.Emocao,
			IntensidadeEmocao = registro.IntensidadeEmocao,
			EvidenciasAFavor = registro.EvidenciasAFavor,
			EvidenciasContra = registro.EvidenciasContra,
			PensamentoAlternativo = registro.PensamentoAlternativo,
			IntensidadeFinal = registro.IntensidadeFinal,
			CriadoEm = registro.CriadoEm
		};
	}
}