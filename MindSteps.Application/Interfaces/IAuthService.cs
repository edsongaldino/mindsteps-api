using MindSteps.Application.DTOs;

namespace MindSteps.Application.Interfaces;

public interface IAuthService
{
	Task<AuthResponseDto?> AutenticarAsync(LoginDto dto);
	Task<MeResponseDto?> ObterUsuarioLogadoAsync(Guid usuarioId);
}