using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;

namespace MindSteps.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AtividadesController : ControllerBase
{
	private readonly IAtividadeService _atividadeService;
	private readonly IWebHostEnvironment _env;

	public AtividadesController(IAtividadeService atividadeService, IWebHostEnvironment env)
	{
		_atividadeService = atividadeService;
		_env = env;
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

	[HttpPost("upload-media")]
	[Consumes("multipart/form-data")]
	public async Task<IActionResult> UploadMedia(IFormFile file)
	{
		if (file == null || file.Length == 0)
			return BadRequest(new { message = "Nenhum arquivo enviado." });

		var allowedExtensions = new[] { ".mp3", ".m4a", ".wav", ".aac", ".ogg", ".mp4", ".mov", ".avi", ".pdf", ".png", ".jpg", ".jpeg" };
		var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
		if (!allowedExtensions.Contains(ext))
			return BadRequest(new { message = "Formato de arquivo não suportado." });

		if (file.Length > 50 * 1024 * 1024) // 50 MB
			return BadRequest(new { message = "O arquivo não pode ser maior que 50MB." });

		var uploadsRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", "atividades");
		if (!Directory.Exists(uploadsRoot))
			Directory.CreateDirectory(uploadsRoot);

		var uniqueFileName = $"{Guid.NewGuid()}{ext}";
		var filePath = Path.Combine(uploadsRoot, uniqueFileName);

		using (var stream = new FileStream(filePath, FileMode.Create))
		{
			await file.CopyToAsync(stream);
		}

		var publicUrl = $"/uploads/atividades/{uniqueFileName}";
		return Ok(new { url = publicUrl, fileName = file.FileName, size = file.Length });
	}
}