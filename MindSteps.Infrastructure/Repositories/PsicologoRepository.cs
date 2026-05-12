using Microsoft.EntityFrameworkCore;
using MindSteps.Domain.Entities;
using MindSteps.Domain.Interfaces;
using MindSteps.Infrastructure.Data;

namespace MindSteps.Infrastructure.Repositories;

public class PsicologoRepository : IPsicologoRepository
{
	private readonly ApplicationDbContext _context;

	public PsicologoRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<Psicologo>> ObterTodosAsync()
	{
		return await _context.Psicologos
			.AsNoTracking()
			.Include(x => x.Usuario)
			.ToListAsync();
	}

	public async Task<List<Psicologo>> ObterPendentesAsync()
	{
		return await _context.Psicologos
			.AsNoTracking()
			.Include(x => x.Usuario)
			.Where(x => !x.Aprovado)
			.ToListAsync();
	}

	public async Task<Psicologo?> ObterPorIdAsync(Guid id)
	{
		return await _context.Psicologos
			.Include(x => x.Usuario)
			.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task<bool> ExisteCrpAsync(string crp)
	{
		return await _context.Psicologos
			.AnyAsync(x => x.Crp == crp);
	}

	public async Task AdicionarAsync(Psicologo psicologo)
	{
		await _context.Psicologos.AddAsync(psicologo);
	}

	public async Task SalvarAlteracoesAsync()
	{
		await _context.SaveChangesAsync();
	}
}