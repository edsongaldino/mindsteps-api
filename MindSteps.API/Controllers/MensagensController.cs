using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using MindSteps.Domain.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MindSteps.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MensagensController : ControllerBase
{
	private readonly IMensagemService _mensagemService;
	private readonly IUsuarioRepository _usuarioRepository;

	public MensagensController(
		IMensagemService mensagemService,
		IUsuarioRepository usuarioRepository)
	{
		_mensagemService = mensagemService;
		_usuarioRepository = usuarioRepository;
	}

	[HttpPost]
	[Authorize(Roles = "Psicologo")]
	public async Task<IActionResult> Enviar([FromBody] MensagemCreateDto dto)
	{
		try
		{
			var psicologoId = await ObterPsicologoIdLogadoAsync();
			if (psicologoId is null)
				return Unauthorized("Usuário logado não possui um perfil de Psicólogo associado.");

			var mensagem = await _mensagemService.EnviarMensagemAsync(psicologoId.Value, dto);
			return Ok(mensagem);
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpGet("paciente/{pacienteId:guid}")]
	[Authorize(Roles = "Psicologo")]
	public async Task<IActionResult> ObterHistoricoPaciente(Guid pacienteId)
	{
		try
		{
			var psicologoId = await ObterPsicologoIdLogadoAsync();
			if (psicologoId is null)
				return Unauthorized();

			var mensagens = await _mensagemService.ObterHistoricoPacienteAsync(psicologoId.Value, pacienteId);
			return Ok(mensagens);
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpGet("minhas")]
	[Authorize(Roles = "Paciente")]
	public async Task<IActionResult> ObterMinhas()
	{
		try
		{
			var pacienteId = await ObterPacienteIdLogadoAsync();
			if (pacienteId is null)
				return Unauthorized("Usuário logado não possui um perfil de Paciente associado.");

			var mensagens = await _mensagemService.ObterMinhasMensagensAsync(pacienteId.Value);
			return Ok(mensagens);
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpPatch("{id:guid}/ler")]
	[Authorize(Roles = "Paciente")]
	public async Task<IActionResult> MarcarComoLida(Guid id)
	{
		try
		{
			var pacienteId = await ObterPacienteIdLogadoAsync();
			if (pacienteId is null)
				return Unauthorized();

			var sucesso = await _mensagemService.MarcarComoLidaAsync(id, pacienteId.Value);
			if (!sucesso)
				return NotFound("Mensagem não encontrada ou não pertence a este paciente.");

			return Ok(new { message = "Mensagem marcada como lida com sucesso." });
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	private async Task<Guid?> ObterPsicologoIdLogadoAsync()
	{
		var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (string.IsNullOrWhiteSpace(usuarioIdClaim)) return null;

		var usuario = await _usuarioRepository.ObterComPerfisPorIdAsync(Guid.Parse(usuarioIdClaim));
		return usuario?.Psicologo?.Id;
	}

	private async Task<Guid?> ObterPacienteIdLogadoAsync()
	{
		var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (string.IsNullOrWhiteSpace(usuarioIdClaim)) return null;

		var usuario = await _usuarioRepository.ObterComPerfisPorIdAsync(Guid.Parse(usuarioIdClaim));
		return usuario?.Paciente?.Id;
	}
}
