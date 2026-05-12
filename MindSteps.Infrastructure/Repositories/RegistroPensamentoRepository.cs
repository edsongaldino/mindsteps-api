using Microsoft.EntityFrameworkCore;
using MindSteps.Domain.Entities;
using MindSteps.Domain.Interfaces;
using MindSteps.Infrastructure.Data;

namespace MindSteps.Infrastructure.Repositories;

public class RegistroPensamentoRepository : IRegistroPensamentoRepository
{
	private readonly ApplicationDbContext _context;

	public RegistroPensamentoRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<RegistroPensamento>> ObterPorPacienteAsync(Guid pacienteId)
	{
		return await _context.RegistrosPensamentos
			.AsNoTracking()
			.Where(x => x.PacienteId == pacienteId)
			.OrderByDescending(x => x.CriadoEm)
			.ToListAsync();
	}

	public async Task<RegistroPensamento?> ObterPorIdAsync(Guid id)
	{
		return await _context.RegistrosPensamentos
			.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task AdicionarAsync(RegistroPensamento registro)
	{
		await _context.RegistrosPensamentos.AddAsync(registro);
	}

	public async Task SalvarAlteracoesAsync()
	{
		await _context.SaveChangesAsync();
	}
}