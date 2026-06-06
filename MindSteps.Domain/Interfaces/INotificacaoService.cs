using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindSteps.Domain.Interfaces;

public interface INotificacaoService
{
	Task EnviarNotificacaoUsuarioAsync(Guid usuarioId, string titulo, string corpo, Dictionary<string, string>? dados = null);
}
