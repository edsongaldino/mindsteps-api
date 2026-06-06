using Microsoft.EntityFrameworkCore;
using MindSteps.Domain.Entities;
using MindSteps.Infrastructure.Data;
using MindSteps.Domain.Interfaces;

namespace MindSteps.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
	private readonly ApplicationDbContext _context;

	public UsuarioRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<Usuario>> ObterTodosAsync()
	{
		return await _context.Usuarios
			.AsNoTracking()
			.Include(x => x.Paciente)
			.Include(x => x.Psicologo)
			.ToListAsync();
	}

	public async Task<Usuario?> ObterPorIdAsync(Guid id)
	{
		return await _context.Usuarios
			.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task<bool> ExisteEmailAsync(string email)
	{
		var emailNormalizado = email.ToLower().Trim();

		return await _context.Usuarios
			.AnyAsync(x => x.Email == emailNormalizado);
	}

	public async Task AdicionarAsync(Usuario usuario)
	{
		await _context.Usuarios.AddAsync(usuario);
	}

	public async Task<Usuario?> ObterPorEmailAsync(string email)
	{
		var emailNormalizado = email.ToLower().Trim();

		return await _context.Usuarios
			.Include(x => x.Paciente)
			.Include(x => x.Psicologo)
			.FirstOrDefaultAsync(x => x.Email == emailNormalizado);
	}

	public async Task SalvarAlteracoesAsync()
	{
		await _context.SaveChangesAsync();
	}

	public async Task<Usuario?> ObterComPerfisPorIdAsync(Guid id)
	{
		return await _context.Usuarios
			.Include(x => x.Psicologo)
			.Include(x => x.Paciente)
			.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task RegistrarDeviceTokenAsync(Guid usuarioId, string deviceToken, string plataforma)
	{
		var tokensExistentes = await _context.UsuariosDispositivos
			.Where(x => x.DeviceToken == deviceToken)
			.ToListAsync();

		if (tokensExistentes.Any())
		{
			_context.UsuariosDispositivos.RemoveRange(tokensExistentes);
		}

		var novoDispositivo = new UsuarioDispositivo
		{
			UsuarioId = usuarioId,
			DeviceToken = deviceToken,
			Plataforma = plataforma,
			AtualizadoEm = DateTime.UtcNow
		};

		await _context.UsuariosDispositivos.AddAsync(novoDispositivo);
		await _context.SaveChangesAsync();
	}

	public async Task<List<string>> ObterDeviceTokensUsuarioAsync(Guid usuarioId)
	{
		return await _context.UsuariosDispositivos
			.Where(x => x.UsuarioId == usuarioId)
			.Select(x => x.DeviceToken)
			.ToListAsync();
	}
}