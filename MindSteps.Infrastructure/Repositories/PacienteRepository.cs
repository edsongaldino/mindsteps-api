using Microsoft.EntityFrameworkCore;
using MindSteps.Domain.Entities;
using MindSteps.Domain.Interfaces;
using MindSteps.Infrastructure.Data;

namespace MindSteps.Infrastructure.Repositories;

public class PacienteRepository : IPacienteRepository
{
	private readonly ApplicationDbContext _context;

	public PacienteRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<Paciente>> ObterTodosAsync()
	{
		return await _context.Pacientes
			.AsNoTracking()
			.Include(x => x.Usuario)
			.Include(x => x.Psicologo)
				.ThenInclude(x => x.Usuario)
			.ToListAsync();
	}

	public async Task<List<Paciente>> ObterPorPsicologoAsync(Guid psicologoId)
	{
		return await _context.Pacientes
			.AsNoTracking()
			.Include(x => x.Usuario)
			.Where(x => x.PsicologoId == psicologoId)
			.ToListAsync();
	}

	public async Task<Paciente?> ObterPorIdAsync(Guid id)
	{
		return await _context.Pacientes
			.Include(x => x.Usuario)
			.Include(x => x.Psicologo)
				.ThenInclude(x => x.Usuario)
			.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task AdicionarAsync(Paciente paciente)
	{
		await _context.Pacientes.AddAsync(paciente);
	}

	public async Task SalvarAlteracoesAsync()
	{
		await _context.SaveChangesAsync();
	}
}