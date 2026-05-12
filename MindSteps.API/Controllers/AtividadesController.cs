using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;

namespace MindSteps.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AtividadesController : ControllerBase
{
	private readonly IAtividadeService _atividadeService;

	public AtividadesController(IAtividadeService atividadeService)
	{
		_atividadeService = atividadeService;
	}

	[HttpGet]
	public async Task<IActionResult> ObterTodas()
	{
		return Ok(await _atividadeService.ObterTodasAsync());
	}

	[HttpGet("psicologo/{psicologoId:guid}")]
	public async Task<IActionResult> ObterPorPsicologo(Guid psicologoId)
	{
		return Ok(await _atividadeService.ObterPorPsicologoAsync(psicologoId));
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> ObterPorId(Guid id)
	{
		var atividade = await _atividadeService.ObterPorIdAsync(id);

		if (atividade is null)
			return NotFound();

		return Ok(atividade);
	}

	[HttpPost]
	public async Task<IActionResult> Criar([FromBody] AtividadeCreateDto dto)
	{
		try
		{
			var atividade = await _atividadeService.CriarAsync(dto);
			return CreatedAtAction(nameof(ObterPorId), new { id = atividade.Id }, atividade);
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpPost("enviar")]
	public async Task<IActionResult> EnviarParaPaciente([FromBody] EnviarAtividadeDto dto)
	{
		try
		{
			var envio = await _atividadeService.EnviarParaPacienteAsync(dto);
			return Ok(envio);
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpGet("paciente/{pacienteId:guid}")]
	public async Task<IActionResult> ObterAtividadesPorPaciente(Guid pacienteId)
	{
		return Ok(await _atividadeService.ObterAtividadesPorPacienteAsync(pacienteId));
	}

	[Authorize(Roles = "Paciente")]
	[HttpPatch("responder")]
	public async Task<IActionResult> Responder([FromBody] ResponderAtividadeDto dto)
	{
		try
		{
			var response = await _atividadeService.ResponderAtividadeAsync(dto);

			if (response is null)
				return NotFound();

			return Ok(response);
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}
}