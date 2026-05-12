using Microsoft.EntityFrameworkCore;
using MindSteps.Domain.Entities;
using MindSteps.Domain.Interfaces;
using MindSteps.Infrastructure.Data;

namespace MindSteps.Infrastructure.Repositories;

public class AtividadeRepository : IAtividadeRepository
{
	private readonly ApplicationDbContext _context;

	public AtividadeRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<Atividade>> ObterTodasAsync()
	{
		return await _context.Atividades
			.AsNoTracking()
			.Include(x => x.Psicologo)
				.ThenInclude(x => x.Usuario)
			.ToListAsync();
	}

	public async Task<List<Atividade>> ObterPorPsicologoAsync(Guid psicologoId)
	{
		return await _context.Atividades
			.AsNoTracking()
			.Where(x => x.PsicologoId == psicologoId)
			.ToListAsync();
	}

	public async Task<Atividade?> ObterPorIdAsync(Guid id)
	{
		return await _context.Atividades
			.Include(x => x.Psicologo)
				.ThenInclude(x => x.Usuario)
			.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task AdicionarAsync(Atividade atividade)
	{
		await _context.Atividades.AddAsync(atividade);
	}

	public async Task AdicionarAtividadePacienteAsync(AtividadePaciente atividadePaciente)
	{
		await _context.AtividadesPacientes.AddAsync(atividadePaciente);
	}

	public async Task<List<AtividadePaciente>> ObterAtividadesPorPacienteAsync(Guid pacienteId)
	{
		return await _context.AtividadesPacientes
			.AsNoTracking()
			.Include(x => x.Atividade)
			.Where(x => x.PacienteId == pacienteId)
			.ToListAsync();
	}

	public async Task<AtividadePaciente?> ObterAtividadePacientePorIdAsync(Guid id)
	{
		return await _context.AtividadesPacientes
			.Include(x => x.Atividade)
			.Include(x => x.Paciente)
				.ThenInclude(x => x.Usuario)
			.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task SalvarAlteracoesAsync()
	{
		await _context.SaveChangesAsync();
	}
}