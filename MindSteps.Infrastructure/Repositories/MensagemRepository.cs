using Microsoft.EntityFrameworkCore;
using MindSteps.Domain.Entities;
using MindSteps.Domain.Interfaces;
using MindSteps.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSteps.Infrastructure.Repositories;

public class MensagemRepository : IMensagemRepository
{
	private readonly ApplicationDbContext _context;

	public MensagemRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Mensagem?> ObterPorIdAsync(Guid id)
	{
		return await _context.Mensagens
			.Include(x => x.Psicologo)
				.ThenInclude(x => x.Usuario)
			.Include(x => x.Paciente)
				.ThenInclude(x => x.Usuario)
			.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task<List<Mensagem>> ObterMensagensPorPacienteAsync(Guid pacienteId)
	{
		return await _context.Mensagens
			.AsNoTracking()
			.Include(x => x.Psicologo)
				.ThenInclude(x => x.Usuario)
			.Where(x => x.PacienteId == pacienteId)
			.OrderByDescending(x => x.CriadoEm)
			.ToListAsync();
	}

	public async Task<List<Mensagem>> ObterMensagensEntrePsicologoEPacienteAsync(Guid psicologoId, Guid pacienteId)
	{
		return await _context.Mensagens
			.AsNoTracking()
			.Include(x => x.Psicologo)
				.ThenInclude(x => x.Usuario)
			.Where(x => x.PsicologoId == psicologoId && x.PacienteId == pacienteId)
			.OrderByDescending(x => x.CriadoEm)
			.ToListAsync();
	}

	public async Task AdicionarAsync(Mensagem mensagem)
	{
		await _context.Mensagens.AddAsync(mensagem);
	}

	public async Task SalvarAlteracoesAsync()
	{
		await _context.SaveChangesAsync();
	}
}
