using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MindSteps.API.Controllers;

[ApiController]
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