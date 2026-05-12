using Microsoft.EntityFrameworkCore;
using MindSteps.Domain.Entities;
using MindSteps.Domain.Interfaces;
using MindSteps.Infrastructure.Data;

namespace MindSteps.Infrastructure.Repositories;

public class CheckInEmocionalRepository : ICheckInEmocionalRepository
{
	private readonly ApplicationDbContext _context;

	public CheckInEmocionalRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<CheckInEmocional>> ObterPorPacienteAsync(Guid pacienteId)
	{
		return await _context.CheckInsEmocionais
			.AsNoTracking()
			.Where(x => x.PacienteId == pacienteId)
			.OrderByDescending(x => x.CriadoEm)
			.ToListAsync();
	}

	public async Task<CheckInEmocional?> ObterPorIdAsync(Guid id)
	{
		return await _context.CheckInsEmocionais
			.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task AdicionarAsync(CheckInEmocional checkIn)
	{
		await _context.CheckInsEmocionais.AddAsync(checkIn);
	}

	public async Task SalvarAlteracoesAsync()
	{
		await _context.SaveChangesAsync();
	}
}