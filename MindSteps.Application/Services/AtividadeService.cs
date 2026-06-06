using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using MindSteps.Domain.Entities;
using MindSteps.Domain.Enums;
using MindSteps.Domain.Interfaces;

namespace MindSteps.Application.Services;

public class AtividadeService : IAtividadeService
{
	private readonly IAtividadeRepository _atividadeRepository;
	private readonly IPsicologoRepository _psicologoRepository;
	private readonly IPacienteRepository _pacienteRepository;
	private readonly INotificacaoService _notificacaoService;

	public AtividadeService(
		IAtividadeRepository atividadeRepository,
		IPsicologoRepository psicologoRepository,
		IPacienteRepository pacienteRepository,
		INotificacaoService notificacaoService)
	{
		_atividadeRepository = atividadeRepository;
		_psicologoRepository = psicologoRepository;
		_pacienteRepository = pacienteRepository;
		_notificacaoService = notificacaoService;
	}

	public async Task<IEnumerable<AtividadeResponseDto>> ObterTodasAsync()
	{
		var atividades = await _atividadeRepository.ObterTodasAsync();

		return atividades.Select(MapToResponse);
	}

	public async Task<IEnumerable<AtividadeResponseDto>> ObterPorPsicologoAsync(Guid psicologoId)
	{
		var atividades = await _atividadeRepository.ObterPorPsicologoAsync(psicologoId);

		return atividades.Select(MapToResponse);
	}

	public async Task<AtividadeResponseDto?> ObterPorIdAsync(Guid id)
	{
		var atividade = await _atividadeRepository.ObterPorIdAsync(id);

		if (atividade is null)
			return null;

		return MapToResponse(atividade);
	}

	public async Task<AtividadeResponseDto> CriarAsync(AtividadeCreateDto dto)
	{
		var psicologo = await _psicologoRepository.ObterPorIdAsync(dto.PsicologoId);

		if (psicologo is null || !psicologo.Aprovado)
			throw new Exception("Psicólogo não encontrado ou ainda não aprovado.");

		var atividade = new Atividade
		{
			PsicologoId = dto.PsicologoId,
			Titulo = dto.Titulo,
			Descricao = dto.Descricao,
			Tipo = dto.Tipo,
			Conteudo = dto.Conteudo,
			AudioUrl = dto.AudioUrl,
			ArquivoUrl = dto.ArquivoUrl,
			Ativo = true,
			CriadoEm = DateTime.UtcNow,
			TipoResposta = dto.TipoResposta,
			AtividadeObrigatoria = dto.AtividadeObrigatoria,
			PermitirAnexos = dto.PermitirAnexos,
			FeedbackAutomatico = dto.FeedbackAutomatico,
			CategoriaEmocional = dto.CategoriaEmocional,
			NivelSugerido = dto.NivelSugerido,
			Nivel = dto.Nivel > 0 ? dto.Nivel : 1,
			Frequencia = dto.Frequencia,
			DiasSemana = dto.DiasSemana,
			HorarioSugerido = dto.HorarioSugerido,
			PrazoConclusao = dto.PrazoConclusao,
			NotificarPush = dto.NotificarPush,
			NotificarEmail = dto.NotificarEmail,
			LembreteSuave = dto.LembreteSuave
		};

		await _atividadeRepository.AdicionarAsync(atividade);
		await _atividadeRepository.SalvarAlteracoesAsync();

		return MapToResponse(atividade);
	}

	public async Task<AtividadePacienteResponseDto> EnviarParaPacienteAsync(EnviarAtividadeDto dto)
	{
		var atividade = await _atividadeRepository.ObterPorIdAsync(dto.AtividadeId);

		if (atividade is null || !atividade.Ativo)
			throw new Exception("Atividade não encontrada ou inativa.");

		var paciente = await _pacienteRepository.ObterPorIdAsync(dto.PacienteId);

		if (paciente is null || !paciente.Usuario.Ativo)
			throw new Exception("Paciente não encontrado ou inativo.");

		if (paciente.PsicologoId != atividade.PsicologoId)
			throw new Exception("Este paciente não pertence ao psicólogo responsável pela atividade.");

		if (paciente.Nivel < atividade.Nivel)
			throw new Exception($"O paciente está no nível {paciente.Nivel} e esta atividade exige o nível {atividade.Nivel}. Estimule a participação do paciente para liberar!");

		var atividadePaciente = new AtividadePaciente
		{
			AtividadeId = dto.AtividadeId,
			PacienteId = dto.PacienteId,
			Status = StatusAtividadePaciente.Pendente,
			DataEnvio = DateTime.UtcNow,
			DataLimite = dto.DataLimite.HasValue ? DateTime.SpecifyKind(dto.DataLimite.Value, DateTimeKind.Utc) : null
		};

		await _atividadeRepository.AdicionarAtividadePacienteAsync(atividadePaciente);
		await _atividadeRepository.SalvarAlteracoesAsync();

		atividadePaciente.Atividade = atividade;

		try
		{
			var titulo = "Nova atividade recebida";
			var corpo = $"Seu psicólogo te enviou a atividade: {atividade.Titulo}";
			var dados = new Dictionary<string, string>
			{
				{ "type", "activity" },
				{ "atividadePacienteId", atividadePaciente.Id.ToString() }
			};

			await _notificacaoService.EnviarNotificacaoUsuarioAsync(paciente.UsuarioId, titulo, corpo, dados);
		}
		catch
		{
			// Evita falhar o envio da atividade se o serviço de notificação falhar
		}

		return MapToAtividadePacienteResponse(atividadePaciente);
	}

	public async Task<IEnumerable<AtividadePacienteResponseDto>> ObterAtividadesPorPacienteAsync(Guid pacienteId)
	{
		var atividades = await _atividadeRepository.ObterAtividadesPorPacienteAsync(pacienteId);

		return atividades.Select(MapToAtividadePacienteResponse);
	}

	private static AtividadeResponseDto MapToResponse(Atividade atividade)
	{
		return new AtividadeResponseDto
		{
			Id = atividade.Id,
			PsicologoId = atividade.PsicologoId,
			Titulo = atividade.Titulo,
			Descricao = atividade.Descricao,
			Tipo = atividade.Tipo,
			Conteudo = atividade.Conteudo,
			AudioUrl = atividade.AudioUrl,
			ArquivoUrl = atividade.ArquivoUrl,
			Ativo = atividade.Ativo,
			CriadoEm = atividade.CriadoEm,
			TipoResposta = atividade.TipoResposta,
			AtividadeObrigatoria = atividade.AtividadeObrigatoria,
			PermitirAnexos = atividade.PermitirAnexos,
			FeedbackAutomatico = atividade.FeedbackAutomatico,
			CategoriaEmocional = atividade.CategoriaEmocional,
			NivelSugerido = atividade.NivelSugerido,
			Nivel = atividade.Nivel,
			Frequencia = atividade.Frequencia,
			DiasSemana = atividade.DiasSemana,
			HorarioSugerido = atividade.HorarioSugerido,
			PrazoConclusao = atividade.PrazoConclusao,
			NotificarPush = atividade.NotificarPush,
			NotificarEmail = atividade.NotificarEmail,
			LembreteSuave = atividade.LembreteSuave
		};
	}

	private static AtividadePacienteResponseDto MapToAtividadePacienteResponse(AtividadePaciente item)
	{
		return new AtividadePacienteResponseDto
		{
			Id = item.Id,
			AtividadeId = item.AtividadeId,
			PacienteId = item.PacienteId,
			Titulo = item.Atividade.Titulo,
			Descricao = item.Atividade.Descricao,
			Conteudo = item.Atividade.Conteudo,
			Tipo = item.Atividade.Tipo,
			Status = item.Status,
			Nivel = item.Atividade.Nivel,
			DataEnvio = item.DataEnvio,
			DataLimite = item.DataLimite,
			DataConclusao = item.DataConclusao,
			RespostaTexto = item.RespostaTexto,
			NotaHumor = item.NotaHumor
		};
	}

	public async Task<AtividadePacienteResponseDto?> ResponderAtividadeAsync(ResponderAtividadeDto dto)
	{
		var atividadePaciente = await _atividadeRepository
			.ObterAtividadePacientePorIdAsync(dto.AtividadePacienteId);

		if (atividadePaciente is null)
			return null;

		if (dto.NotaHumor.HasValue && (dto.NotaHumor < 0 || dto.NotaHumor > 10))
			throw new Exception("A nota de humor deve estar entre 0 e 10.");

		atividadePaciente.RespostaTexto = dto.RespostaTexto;
		atividadePaciente.NotaHumor = dto.NotaHumor;
		atividadePaciente.Status = StatusAtividadePaciente.Concluida;
		atividadePaciente.DataConclusao = DateTime.UtcNow;

		var pontosGanhos = (atividadePaciente.Atividade.Nivel > 0 ? atividadePaciente.Atividade.Nivel : 1) * 10;
		var paciente = await _pacienteRepository.ObterPorIdAsync(atividadePaciente.PacienteId);
		if (paciente is not null)
		{
			paciente.Pontos += pontosGanhos;
			var novoNivel = (paciente.Pontos / 100) + 1;
			if (novoNivel > paciente.Nivel)
			{
				paciente.Nivel = novoNivel;
			}
		}

		await _atividadeRepository.SalvarAlteracoesAsync();

		return MapToAtividadePacienteResponse(atividadePaciente);
	}
}