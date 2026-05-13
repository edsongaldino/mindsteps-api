using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IAuthService _service;

	public AuthController(IAuthService service)
	{
		_service = service;
	}

	[AllowAnonymous]
	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto login)
	{
		var response = await _service.AutenticarAsync(login);

		if (response is null)
			return Unauthorized("Credenciais inválidas");

		return Ok(response);
	}

	[Authorize]
	[HttpGet("me")]
	public async Task<IActionResult> Me()
	{
		var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (string.IsNullOrWhiteSpace(usuarioIdClaim))
			return Unauthorized();

		var usuarioId = Guid.Parse(usuarioIdClaim);

		var response = await _service.ObterUsuarioLogadoAsync(usuarioId);

		if (response is null)
			return NotFound();

		return Ok(response);
	}
}