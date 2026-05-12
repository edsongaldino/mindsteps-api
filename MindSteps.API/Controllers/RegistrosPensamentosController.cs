using Microsoft.AspNetCore.Mvc;
using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;

namespace MindSteps.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrosPensamentosController : ControllerBase
{
	private readonly IRegistroPensamentoService _registroService;

	public RegistrosPensamentosController(IRegistroPensamentoService registroService)
	{
		_registroService = registroService;
	}

	[HttpGet("paciente/{pacienteId:guid}")]
	public async Task<IActionResult> ObterPorPaciente(Guid pacienteId)
	{
		return Ok(await _registroService.ObterPorPacienteAsync(pacienteId));
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> ObterPorId(Guid id)
	{
		var registro = await _registroService.ObterPorIdAsync(id);

		if (registro is null)
			return NotFound();

		return Ok(registro);
	}

	[HttpPost]
	public async Task<IActionResult> Criar([FromBody] RegistroPensamentoCreateDto dto)
	{
		try
		{
			var registro = await _registroService.CriarAsync(dto);
			return CreatedAtAction(nameof(ObterPorId), new { id = registro.Id }, registro);
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}
}