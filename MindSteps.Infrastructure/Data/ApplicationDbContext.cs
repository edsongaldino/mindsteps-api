using Microsoft.EntityFrameworkCore;
using MindSteps.Domain.Entities;

namespace MindSteps.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public DbSet<Usuario> Usuarios => Set<Usuario>();
	public DbSet<Psicologo> Psicologos => Set<Psicologo>();
	public DbSet<Paciente> Pacientes => Set<Paciente>();
	public DbSet<Atividade> Atividades => Set<Atividade>();
	public DbSet<AtividadePaciente> AtividadesPacientes => Set<AtividadePaciente>();
	public DbSet<CheckInEmocional> CheckInsEmocionais => Set<CheckInEmocional>();
	public DbSet<RegistroPensamento> RegistrosPensamentos => Set<RegistroPensamento>();
	public DbSet<Mensagem> Mensagens => Set<Mensagem>();
	public DbSet<UsuarioDispositivo> UsuariosDispositivos => Set<UsuarioDispositivo>();
	public DbSet<RegistroJogo> RegistrosJogos => Set<RegistroJogo>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
		base.OnModelCreating(modelBuilder);
	}
}