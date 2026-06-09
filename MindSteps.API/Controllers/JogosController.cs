using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using MindSteps.Domain.Entities;
using MindSteps.Domain.Enums;
using MindSteps.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace MindSteps.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JogosController : ControllerBase
{
	private readonly ApplicationDbContext _context;
	private readonly IDashboardService _dashboardService;

	public JogosController(ApplicationDbContext context, IDashboardService dashboardService)
	{
		_context = context;
		_dashboardService = dashboardService;
	}

	[HttpPost("registrar")]
	public async Task<IActionResult> Registrar([FromBody] RegistrarJogoDto dto)
	{
		var paciente = await _context.Pacientes
			.Include(p => p.Usuario)
			.FirstOrDefaultAsync(p => p.Id == dto.PacienteId);

		if (paciente == null)
			return NotFound(new { message = "Paciente não encontrado." });

		var registro = new RegistroJogo
		{
			PacienteId = dto.PacienteId,
			JogoId = dto.JogoId,
			DadosPlay = dto.DadosPlay,
			AtividadePacienteId = dto.AtividadePacienteId,
			DataPlay = DateTime.UtcNow
		};

		_context.RegistrosJogos.Add(registro);

		int pontosGanhos = 15; // Pontuação padrão para jogo livre

		if (dto.AtividadePacienteId.HasValue)
		{
			var atividadePaciente = await _context.AtividadesPacientes
				.Include(ap => ap.Atividade)
				.FirstOrDefaultAsync(ap => ap.Id == dto.AtividadePacienteId.Value);

			if (atividadePaciente != null && atividadePaciente.Status != StatusAtividadePaciente.Concluida)
			{
				atividadePaciente.Status = StatusAtividadePaciente.Concluida;
				atividadePaciente.DataConclusao = DateTime.UtcNow;
				atividadePaciente.RespostaTexto = dto.DadosPlay;

				// XP baseado na atividade
				var nivel = atividadePaciente.Atividade.Nivel > 0 ? atividadePaciente.Atividade.Nivel : 1;
				pontosGanhos = nivel * 15;
			}
		}

		paciente.Pontos += pontosGanhos;
		var novoNivel = (paciente.Pontos / 100) + 1;
		if (novoNivel > paciente.Nivel)
		{
			paciente.Nivel = novoNivel;
		}

		await _context.SaveChangesAsync();

		return Ok(new
		{
			message = "Jogo registrado com sucesso.",
			pontosGanhos,
			novoTotalPontos = paciente.Pontos,
			novoNivel = paciente.Nivel
		});
	}

	[HttpGet("dashboard/{pacienteId:guid}")]
	public async Task<IActionResult> ObterDashboard(Guid pacienteId)
	{
		try
		{
			var dashboard = await _dashboardService.ObterDashboardPacienteAsync(pacienteId);
			return Ok(dashboard);
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}
}

public class RegistrarJogoDto
{
	public Guid PacienteId { get; set; }
	public string JogoId { get; set; } = string.Empty;
	public string DadosPlay { get; set; } = string.Empty;
	public Guid? AtividadePacienteId { get; set; }
}
