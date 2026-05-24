using Microsoft.AspNetCore.Mvc;
using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MindSteps.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
	private readonly IUsuarioService _usuarioService;
	private readonly IWebHostEnvironment _env;

	public UsuariosController(IUsuarioService usuarioService, IWebHostEnvironment env)
	{
		_usuarioService = usuarioService;
		_env = env;
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

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Atualizar(Guid id, [FromBody] UsuarioUpdateDto dto)
	{
		try
		{
			var usuario = await _usuarioService.AtualizarAsync(id, dto);

			if (usuario is null)
				return NotFound();

			return Ok(usuario);
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

	[HttpPost("{id:guid}/foto")]
	[Consumes("multipart/form-data")]
	public async Task<IActionResult> UploadFoto(Guid id, IFormFile file)
	{
		if (file == null || file.Length == 0)
			return BadRequest(new { message = "Nenhum arquivo enviado." });

		var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
		var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
		if (!allowedExtensions.Contains(ext))
			return BadRequest(new { message = "Formato de arquivo não suportado. Use JPG ou PNG." });

		if (file.Length > 5 * 1024 * 1024) // 5 MB
			return BadRequest(new { message = "O arquivo não pode ser maior que 5MB." });

		var uploadsRoot = Path.Combine(_env.WebRootPath ?? "wwwroot", "images", "users");
		if (!Directory.Exists(uploadsRoot))
			Directory.CreateDirectory(uploadsRoot);

		var fileName = $"{id}.png"; // Standardized to PNG
		var filePath = Path.Combine(uploadsRoot, fileName);

		var thumbFileName = $"{id}_thumb.png";
		var thumbFilePath = Path.Combine(uploadsRoot, thumbFileName);

		try
		{
			using (var stream = file.OpenReadStream())
			{
				// Load original image using ImageSharp
				using (var image = await SixLabors.ImageSharp.Image.LoadAsync(stream))
				{
					// Save original as PNG
					await image.SaveAsPngAsync(filePath);

					// Generate thumbnail (150x150 crop)
					image.Mutate(x => x.Resize(new SixLabors.ImageSharp.Processing.ResizeOptions
					{
						Size = new SixLabors.ImageSharp.Size(150, 150),
						Mode = SixLabors.ImageSharp.Processing.ResizeMode.Crop
					}));
					await image.SaveAsPngAsync(thumbFilePath);
				}
			}

			var fotoUrl = $"/images/users/{fileName}";
			var result = await _usuarioService.AtualizarFotoAsync(id, fotoUrl);

			if (result is null)
				return NotFound(new { message = "Usuário não encontrado." });

			return Ok(new { fotoUrl });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = $"Erro ao salvar a imagem: {ex.Message}" });
		}
	}
}