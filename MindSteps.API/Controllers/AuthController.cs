using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;

namespace MindSteps.API.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioService _service;

    public AuthController(IUsuarioService service)
    {
        _service = service;
    }

    [HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var token = await _service.AutenticarAsync(login);
        return token == null ? Unauthorized("Credenciais inválidas") : Ok(new { token });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Sair([FromBody] LoginDto login)
    {
        var token = await _service.AutenticarAsync(login);
        return token == null ? Unauthorized("Credenciais inválidas") : Ok(new { token });
    }
}