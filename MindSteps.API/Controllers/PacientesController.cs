using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;

namespace MindSteps.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
	private readonly IPacienteService _pacienteService;

	public PacientesController(IPacienteService pacienteService)
	{
		_pacienteService = pacienteService;
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
	[Authorize(Roles = "Psicologo")]
	public async Task<IActionResult> Atualizar(Guid id, [FromBody] PacienteUpdateDto dto)
	{
		try
		{
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
}