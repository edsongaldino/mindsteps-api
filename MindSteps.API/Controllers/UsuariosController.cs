using Microsoft.AspNetCore.Mvc;
using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;

namespace MindSteps.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
	private readonly IUsuarioService _usuarioService;

	public UsuariosController(IUsuarioService usuarioService)
	{
		_usuarioService = usuarioService;
	}

	[HttpGet]
	public async Task<IActionResult> ObterTodos()
	{
		var usuarios = await _usuarioService.ObterTodosAsync();
		return Ok(usuarios);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> ObterPorId(Guid id)
	{
		var usuario = await _usuarioService.ObterPorIdAsync(id);

		if (usuario is null)
			return NotFound();

		return Ok(usuario);
	}

	[HttpPost]
	public async Task<IActionResult> Criar([FromBody] UsuarioCreateDto dto)
	{
		try
		{
			var usuario = await _usuarioService.CriarAsync(dto);
			return CreatedAtAction(nameof(ObterPorId), new { id = usuario.Id }, usuario);
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Desativar(Guid id)
	{
		var sucesso = await _usuarioService.DesativarAsync(id);

		if (!sucesso)
			return NotFound();

		return NoContent();
	}
}