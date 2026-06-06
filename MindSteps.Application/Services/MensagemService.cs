using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using MindSteps.Domain.Entities;
using MindSteps.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSteps.Application.Services;

public class MensagemService : IMensagemService
{
	private readonly IMensagemRepository _mensagemRepository;
	private readonly IPsicologoRepository _psicologoRepository;
	private readonly IPacienteRepository _pacienteRepository;
	private readonly INotificacaoService _notificacaoService;

	public MensagemService(
		IMensagemRepository mensagemRepository,
		IPsicologoRepository psicologoRepository,
		IPacienteRepository pacienteRepository,
		INotificacaoService notificacaoService)
	{
		_mensagemRepository = mensagemRepository;
		_psicologoRepository = psicologoRepository;
		_pacienteRepository = pacienteRepository;
		_notificacaoService = notificacaoService;
	}

	public async Task<MensagemResponseDto> EnviarMensagemAsync(Guid psicologoId, MensagemCreateDto dto)
	{
		var psicologo = await _psicologoRepository.ObterPorIdAsync(psicologoId);
		if (psicologo is null || !psicologo.Aprovado)
			throw new Exception("Psicólogo não encontrado ou ainda não aprovado.");

		var paciente = await _pacienteRepository.ObterPorIdAsync(dto.PacienteId);
		if (paciente is null || !paciente.Usuario.Ativo)
			throw new Exception("Paciente não encontrado ou inativo.");

		if (paciente.PsicologoId != psicologoId)
			throw new Exception("Este paciente não está sob seus cuidados.");

		if (string.IsNullOrWhiteSpace(dto.Conteudo))
			throw new Exception("O conteúdo da mensagem não pode ser vazio.");

		var mensagem = new Mensagem
		{
			PsicologoId = psicologoId,
			PacienteId = dto.PacienteId,
			Conteudo = dto.Conteudo.Trim(),
			Lida = false,
			CriadoEm = DateTime.UtcNow
		};

		await _mensagemRepository.AdicionarAsync(mensagem);
		await _mensagemRepository.SalvarAlteracoesAsync();

		mensagem.Psicologo = psicologo;
		mensagem.Paciente = paciente;

		try
		{
			var titulo = $"Nova mensagem de {psicologo.Usuario.Nome}";
			var corpo = mensagem.Conteudo.Length > 100 
				? mensagem.Conteudo.Substring(0, 97) + "..." 
				: mensagem.Conteudo;

			var dados = new Dictionary<string, string>
			{
				{ "type", "message" },
				{ "mensagemId", mensagem.Id.ToString() }
			};

			await _notificacaoService.EnviarNotificacaoUsuarioAsync(paciente.UsuarioId, titulo, corpo, dados);
		}
		catch
		{
			// Evita falhar o envio da mensagem se o serviço de notificação falhar
		}

		return MapToResponse(mensagem);
	}

	public async Task<IEnumerable<MensagemResponseDto>> ObterMinhasMensagensAsync(Guid pacienteId)
	{
		var mensagens = await _mensagemRepository.ObterMensagensPorPacienteAsync(pacienteId);
		return mensagens.Select(MapToResponse);
	}

	public async Task<IEnumerable<MensagemResponseDto>> ObterHistoricoPacienteAsync(Guid psicologoId, Guid pacienteId)
	{
		var mensagens = await _mensagemRepository.ObterMensagensEntrePsicologoEPacienteAsync(psicologoId, pacienteId);
		return mensagens.Select(MapToResponse);
	}

	public async Task<bool> MarcarComoLidaAsync(Guid mensagemId, Guid pacienteId)
	{
		var mensagem = await _mensagemRepository.ObterPorIdAsync(mensagemId);
		if (mensagem is null || mensagem.PacienteId != pacienteId)
			return false;

		mensagem.Lida = true;
		await _mensagemRepository.SalvarAlteracoesAsync();
		return true;
	}

	private static MensagemResponseDto MapToResponse(Mensagem mensagem)
	{
		return new MensagemResponseDto
		{
			Id = mensagem.Id,
			PsicologoId = mensagem.PsicologoId,
			PsicologoNome = mensagem.Psicologo?.Usuario?.Nome ?? "Psicólogo",
			PacienteId = mensagem.PacienteId,
			PacienteNome = mensagem.Paciente?.Usuario?.Nome ?? "Paciente",
			Conteudo = mensagem.Conteudo,
			Lida = mensagem.Lida,
			CriadoEm = mensagem.CriadoEm
		};
	}
}
