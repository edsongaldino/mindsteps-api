using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MindSteps.Domain.Enums;
using MindSteps.Domain.Interfaces;
using MindSteps.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MindSteps.API.Services;

public class VencimentoAtividadesWorker : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<VencimentoAtividadesWorker> _logger;
	private static readonly TimeSpan RunInterval = TimeSpan.FromHours(1); // Verifica a cada 1 hora

	public VencimentoAtividadesWorker(IServiceProvider serviceProvider, ILogger<VencimentoAtividadesWorker> logger)
	{
		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("VencimentoAtividadesWorker iniciado.");

		using var timer = new PeriodicTimer(RunInterval);

		// Executa uma primeira vez ao iniciar
		await ProcessarVencimentosAsync(stoppingToken);

		while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
		{
			await ProcessarVencimentosAsync(stoppingToken);
		}
	}

	private async Task ProcessarVencimentosAsync(CancellationToken stoppingToken)
	{
		try
		{
			_logger.LogInformation("Iniciando verificação de vencimento de atividades às {Time}...", DateTime.UtcNow);

			using var scope = _serviceProvider.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			var notificacaoService = scope.ServiceProvider.GetRequiredService<INotificacaoService>();

			var limiteVencimento = DateTime.UtcNow.AddHours(24);

			// Busca atividades pendentes que vencem nas próximas 24 horas e que ainda não foram notificadas
			var atividadesExpirando = await context.AtividadesPacientes
				.Include(ap => ap.Atividade)
				.Include(ap => ap.Paciente)
					.ThenInclude(p => p.Usuario)
				.Where(ap => ap.Status == StatusAtividadePaciente.Pendente
							 && ap.DataLimite.HasValue
							 && ap.DataLimite.Value > DateTime.UtcNow
							 && ap.DataLimite.Value <= limiteVencimento
							 && !ap.NotificacaoVencimentoEnviada)
				.ToListAsync(stoppingToken);

			if (!atividadesExpirando.Any())
			{
				_logger.LogInformation("Nenhuma atividade próxima do vencimento encontrada.");
				return;
			}

			_logger.LogInformation("Encontradas {Count} atividades próximas do vencimento para notificar.", atividadesExpirando.Count);

			foreach (var ap in atividadesExpirando)
			{
				if (stoppingToken.IsCancellationRequested)
					break;

				try
				{
					var paciente = ap.Paciente;
					var usuario = paciente.Usuario;

					var titulo = "Atividade próxima do vencimento! ⏳";
					var corpo = $"A atividade '{ap.Atividade.Titulo}' expira em breve. Não se esqueça de responder até {ap.DataLimite!.Value.ToLocalTime():dd/MM/yyyy HH:mm}!";
					
					var dados = new Dictionary<string, string>
					{
						{ "type", "activity_expiration" },
						{ "atividadePacienteId", ap.Id.ToString() }
					};

					_logger.LogInformation("Notificando paciente {PacienteId} sobre a atividade {AtividadePacienteId}.", paciente.Id, ap.Id);
					await notificacaoService.EnviarNotificacaoUsuarioAsync(usuario.Id, titulo, corpo, dados);

					// Marca como enviada para evitar duplicidade
					ap.NotificacaoVencimentoEnviada = true;
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Erro ao processar notificação de vencimento para a atividade {AtividadePacienteId}.", ap.Id);
				}
			}

			// Salva as marcações de envio no banco de dados
			await context.SaveChangesAsync(stoppingToken);
			_logger.LogInformation("Processamento de vencimento de atividades concluído.");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Erro crítico durante o processamento de vencimento de atividades.");
		}
	}
}
