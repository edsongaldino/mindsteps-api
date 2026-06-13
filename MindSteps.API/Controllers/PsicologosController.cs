using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using System.Security.Claims;

namespace MindSteps.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PsicologosController : ControllerBase
{
	private readonly IPsicologoService _psicologoService;

	public PsicologosController(IPsicologoService psicologoService)
	{
		_psicologoService = psicologoService;
	}

	[HttpGet]
	public async Task<IActionResult> ObterTodos()
	{
		return Ok(await _psicologoService.ObterTodosAsync());
	}

	[HttpGet("pendentes")]
	public async Task<IActionResult> ObterPendentes()
	{
		return Ok(await _psicologoService.ObterPendentesAsync());
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> ObterPorId(Guid id)
	{
		var psicologo = await _psicologoService.ObterPorIdAsync(id);

		if (psicologo is null)
			return NotFound();

		return Ok(psicologo);
	}

	[HttpPost]
	[Authorize(Roles = "Administrador")]
	public async Task<IActionResult> Criar([FromBody] PsicologoCreateDto dto)
	{
		try
		{
			var psicologo = await _psicologoService.CriarAsync(dto);
			return CreatedAtAction(nameof(ObterPorId), new { id = psicologo.Id }, psicologo);
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpPost("registrar")]
	[AllowAnonymous]
	public async Task<IActionResult> Registrar([FromBody] PsicologoCreateDto dto)
	{
		try
		{
			var psicologo = await _psicologoService.CriarAsync(dto);
			return CreatedAtAction(nameof(ObterPorId), new { id = psicologo.Id }, psicologo);
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpPut("{id:guid}")]
	[Authorize(Roles = "Administrador,Psicologo")]
	public async Task<IActionResult> Atualizar(Guid id, [FromBody] PsicologoUpdateDto dto)
	{
		try
		{
			if (User.IsInRole("Psicologo"))
			{
				var usuarioIdClaim = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
				if (!string.IsNullOrWhiteSpace(usuarioIdClaim))
				{
					var usuarioId = Guid.Parse(usuarioIdClaim);
					var psicologoExistente = await _psicologoService.ObterPorIdAsync(id);
					if (psicologoExistente == null || psicologoExistente.UsuarioId != usuarioId)
					{
						return StatusCode(403, new { message = "Você só pode atualizar o seu próprio perfil." });
					}
				}
			}

			var psicologo = await _psicologoService.AtualizarAsync(id, dto);

			if (psicologo is null)
				return NotFound();

			return Ok(psicologo);
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpPatch("{id:guid}/aprovar")]
	[Authorize(Roles = "Administrador")]
	public async Task<IActionResult> Aprovar(Guid id)
	{
		var sucesso = await _psicologoService.AprovarAsync(id);

		if (!sucesso)
			return NotFound();

		return NoContent();
	}

	[HttpPatch("{id:guid}/reprovar")]
	[Authorize(Roles = "Administrador")]
	public async Task<IActionResult> Reprovar(Guid id)
	{
		var sucesso = await _psicologoService.ReprovarAsync(id);

		if (!sucesso)
			return NotFound();

		return NoContent();
	}
}