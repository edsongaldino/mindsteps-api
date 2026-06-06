using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MindSteps.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MindSteps.Infrastructure.Services;

public class NotificacaoService : INotificacaoService
{
	private readonly IUsuarioRepository _usuarioRepository;
	private readonly ILogger<NotificacaoService> _logger;
	private readonly bool _fcmConfigurado;

	public NotificacaoService(IUsuarioRepository usuarioRepository, IConfiguration configuration, ILogger<NotificacaoService> logger)
	{
		_usuarioRepository = usuarioRepository;
		_logger = logger;

		try
		{
			if (FirebaseApp.DefaultInstance == null)
			{
				var jsonPath = configuration["Firebase:ServiceAccountKeyPath"] ?? "firebase-key.json";
				if (File.Exists(jsonPath))
				{
					FirebaseApp.Create(new AppOptions
					{
						Credential = GoogleCredential.FromFile(jsonPath)
					});
					_fcmConfigurado = true;
					_logger.LogInformation("Firebase App inicializado com sucesso usando o arquivo {Path}.", jsonPath);
				}
				else
				{
					_logger.LogWarning("Arquivo de credenciais do Firebase não encontrado em: {Path}. Notificações reais do FCM serão simuladas.", Path.GetFullPath(jsonPath));
					_fcmConfigurado = false;
				}
			}
			else
			{
				_fcmConfigurado = true;
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Erro ao inicializar o Firebase App. As notificações serão simuladas.");
			_fcmConfigurado = false;
		}
	}

	public async Task EnviarNotificacaoUsuarioAsync(Guid usuarioId, string titulo, string corpo, Dictionary<string, string>? dados = null)
	{
		var tokens = await _usuarioRepository.ObterDeviceTokensUsuarioAsync(usuarioId);

		if (tokens == null || !tokens.Any())
		{
			_logger.LogInformation("Nenhum token de dispositivo encontrado para o usuário {UsuarioId}. Notificação não enviada.", usuarioId);
			return;
		}

		_logger.LogInformation("Enviando notificação para o usuário {UsuarioId}. Título: '{Titulo}', Corpo: '{Corpo}'.", usuarioId, titulo, corpo);

		if (!_fcmConfigurado)
		{
			_logger.LogWarning("Notificação SIMULADA (FCM não configurado): Enviarei para {Count} dispositivos.", tokens.Count);
			return;
		}

		foreach (var token in tokens)
		{
			try
			{
				var message = new Message
				{
					Token = token,
					Notification = new Notification
					{
						Title = titulo,
						Body = corpo
					},
					Data = dados
				};

				var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
				_logger.LogInformation("Notificação enviada com sucesso para o token {Token}. FCM Response ID: {Response}", token, response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erro ao enviar notificação FCM para o token {Token}.", token);
			}
		}
	}
}
