using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;

namespace MindSteps.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckInsEmocionaisController : ControllerBase
{
	private readonly ICheckInEmocionalService _checkInService;

	public CheckInsEmocionaisController(ICheckInEmocionalService checkInService)
	{
		_checkInService = checkInService;
	}

	[HttpGet("paciente/{pacienteId:guid}")]
	public async Task<IActionResult> ObterPorPaciente(Guid pacienteId)
	{
		return Ok(await _checkInService.ObterPorPacienteAsync(pacienteId));
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> ObterPorId(Guid id)
	{
		var checkIn = await _checkInService.ObterPorIdAsync(id);

		if (checkIn is null)
			return NotFound();

		return Ok(checkIn);
	}

	[HttpPost]
	[Authorize(Roles = "Paciente")]
	public async Task<IActionResult> Criar([FromBody] CheckInEmocionalCreateDto dto)
	{
		try
		{
			var checkIn = await _checkInService.CriarAsync(dto);
			return CreatedAtAction(nameof(ObterPorId), new { id = checkIn.Id }, checkIn);
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}
}