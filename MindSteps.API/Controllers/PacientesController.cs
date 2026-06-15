using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using System.Security.Claims;

namespace MindSteps.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
	private readonly IPacienteService _pacienteService;
	private readonly IPsicologoService _psicologoService;
	private readonly IIaService _iaService;

	public PacientesController(
		IPacienteService pacienteService,
		IPsicologoService psicologoService,
		IIaService iaService)
	{
		_pacienteService = pacienteService;
		_psicologoService = psicologoService;
		_iaService = iaService;
	}

	[HttpGet]
	[Authorize(Roles = "Administrador")]
	public async Task<IActionResult> ObterTodos()
	{
		return Ok(await _pacienteService.ObterTodosAsync());
	}

	[HttpGet("psicologo/{psicologoId:guid}")]
	[Authorize(Roles = "Psicologo")]
	public async Task<IActionResult> ObterPorPsicologo(Guid psicologoId)
	{
		return Ok(await _pacienteService.ObterPorPsicologoAsync(psicologoId));
	}

	[HttpGet("{id:guid}")]
	[Authorize(Roles = "Psicologo")]
	public async Task<IActionResult> ObterPorId(Guid id)
	{
		var paciente = await _pacienteService.ObterPorIdAsync(id);

		if (paciente is null)
			return NotFound();

		return Ok(paciente);
	}

	[HttpGet("{id:guid}/ia-insights")]
	[Authorize(Roles = "Psicologo")]
	public async Task<IActionResult> ObterInsightsIa(Guid id)
	{
		var paciente = await _pacienteService.ObterPorIdAsync(id);
		if (paciente is null)
			return NotFound(new { message = "Paciente não encontrado." });

		var psicologo = await _psicologoService.ObterPorIdAsync(paciente.PsicologoId);
		if (psicologo is null)
			return NotFound(new { message = "Psicólogo associado não encontrado." });

		var plano = psicologo.Plano ?? "Starter";
		if (plano.ToLower() != "profissional" && plano.ToLower() != "clinica")
		{
			return StatusCode(403, new { message = "Insights por IA estão disponíveis apenas nos planos Profissional e Clínica. Faça o upgrade de seu plano." });
		}

		var insights = await _iaService.GerarInsightsClinicosAsync(id);
		return Ok(insights);
	}

	[HttpPost]
	[Authorize(Roles = "Psicologo")]
	public async Task<IActionResult> Criar([FromBody] PacienteCreateDto dto)
	{
		try
		{
			var paciente = await _pacienteService.CriarAsync(dto);
			return CreatedAtAction(nameof(ObterPorId), new { id = paciente.Id }, paciente);
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpPut("{id:guid}")]
	[Authorize(Roles = "Psicologo,Paciente")]
	public async Task<IActionResult> Atualizar(Guid id, [FromBody] PacienteUpdateDto dto)
	{
		try
		{
			if (User.IsInRole("Paciente"))
			{
				var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (!string.IsNullOrWhiteSpace(usuarioIdClaim))
				{
					var usuarioId = Guid.Parse(usuarioIdClaim);
					var pacienteExistente = await _pacienteService.ObterPorIdAsync(id);
					if (pacienteExistente == null || pacienteExistente.UsuarioId != usuarioId)
					{
						return StatusCode(403, new { message = "Você só pode atualizar o seu próprio perfil." });
					}
				}
			}

			var paciente = await _pacienteService.AtualizarAsync(id, dto);

			if (paciente is null)
				return NotFound();

			return Ok(paciente);
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpDelete("{id:guid}")]
	[Authorize(Roles = "Psicologo")]
	public async Task<IActionResult> Desativar(Guid id)
	{
		var sucesso = await _pacienteService.DesativarAsync(id);

		if (!sucesso)
			return NotFound();

		return NoContent();
	}

	[HttpPatch("{id:guid}/anotacoes")]
	[Authorize(Roles = "Psicologo")]
	public async Task<IActionResult> AtualizarAnotacoes(Guid id, [FromBody] PacienteAnotacoesDto dto)
	{
		var sucesso = await _pacienteService.AtualizarAnotacoesAsync(id, dto.Anotacoes);

		if (!sucesso)
			return NotFound();

		return Ok(new { message = "Anotações particulares atualizadas com sucesso." });
	}
}