using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using MindSteps.Domain.Entities;
using MindSteps.Domain.Enums;
using System;

namespace MindSteps.Infrastructure.Data.Seed;

public static class DbSeeder
{
	public static async Task SeedAsync(ApplicationDbContext context)
	{
		if (await context.Usuarios.AnyAsync())
			return;

		var agora = DateTime.UtcNow;

		var adminUsuario = new Usuario
		{
			Id = Guid.NewGuid(),
			Nome = "Administrador MindSteps",
			Email = "admin@mindsteps.com",
			SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
			Perfil = PerfilUsuario.Administrador,
			Ativo = true,
			CriadoEm = agora
		};

		var psicologoUsuario = new Usuario
		{
			Id = Guid.NewGuid(),
			Nome = "Dra. Camila Rocha",
			Email = "psicologo@mindsteps.com",
			SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
			Perfil = PerfilUsuario.Psicologo,
			Ativo = true,
			CriadoEm = agora
		};

		var pacienteUsuario = new Usuario
		{
			Id = Guid.NewGuid(),
			Nome = "Ana Clara Müller",
			Email = "paciente@mindsteps.com",
			SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456"),
			Perfil = PerfilUsuario.Paciente,
			Ativo = true,
			CriadoEm = agora
		};

		var psicologo = new Psicologo
		{
			Id = Guid.NewGuid(),
			UsuarioId = psicologoUsuario.Id,
			Crp = "18/12345",
			Bio = "Psicóloga clínica com atuação em Terapia Cognitivo-Comportamental.",
			FotoUrl = null,
			Aprovado = true,
			CriadoEm = agora
		};

		var paciente = new Paciente
		{
			Id = Guid.NewGuid(),
			UsuarioId = pacienteUsuario.Id,
			PsicologoId = psicologo.Id,
			DataNascimento = new DateTime(2012, 5, 10, 0, 0, 0, DateTimeKind.Utc),
			Genero = "Feminino",
			FotoUrl = null,
			CriadoEm = agora
		};

		var atividade = new Atividade
		{
			Id = Guid.NewGuid(),
			PsicologoId = psicologo.Id,
			Titulo = "Registro de pensamentos automáticos",
			Descricao = "Atividade de TCC para identificar situação, pensamento, emoção e resposta alternativa.",
			Tipo = TipoAtividade.RegistroPensamentos,
			Conteudo = "Descreva a situação, o pensamento automático, a emoção sentida e uma resposta mais equilibrada.",
			Ativo = true,
			CriadoEm = agora
		};

		var atividadePaciente = new AtividadePaciente
		{
			Id = Guid.NewGuid(),
			AtividadeId = atividade.Id,
			PacienteId = paciente.Id,
			Status = StatusAtividadePaciente.Concluida,
			DataEnvio = agora.AddDays(-2),
			DataLimite = agora.AddDays(5),
			DataConclusao = agora.AddDays(-1),
			RespostaTexto = "Fiquei nervosa quando pensei que tinha feito algo errado.",
			NotaHumor = 7,
			FeedbackPsicologo = "Ótimo registro. Vamos trabalhar a resposta alternativa na próxima sessão.",
			FeedbackEnviadoEm = agora
		};

		var checkins = new List<CheckInEmocional>
		{
			new()
			{
				Id = Guid.NewGuid(),
				PacienteId = paciente.Id,
				Humor = Humor.Bom,
				Intensidade = 6,
				EmocaoPrincipal = "Ansiedade",
				Observacao = "Fiquei preocupada com a escola.",
				CriadoEm = agora.AddDays(-6)
			},
			new()
			{
				Id = Guid.NewGuid(),
				PacienteId = paciente.Id,
				Humor = Humor.MuitoBom,
				Intensidade = 5,
				EmocaoPrincipal = "Tranquilidade",
				Observacao = "Consegui respirar antes de responder.",
				CriadoEm = agora.AddDays(-3)
			},
			new()
			{
				Id = Guid.NewGuid(),
				PacienteId = paciente.Id,
				Humor = Humor.MuitoBom,
				Intensidade = 4,
				EmocaoPrincipal = "Alegria",
				Observacao = "Me senti melhor depois da atividade.",
				CriadoEm = agora.AddDays(-1)
			}
		};

		var registroPensamento = new RegistroPensamento
		{
			Id = Guid.NewGuid(),
			PacienteId = paciente.Id,
			AtividadePacienteId = atividadePaciente.Id,
			Situacao = "Minha mãe falou sério comigo.",
			PensamentoAutomatico = "Ela está brava comigo.",
			Emocao = "Ansiedade",
			IntensidadeEmocao = 8,
			EvidenciasAFavor = "Ela estava com o rosto sério.",
			EvidenciasContra = "Ela não gritou e depois conversou normalmente.",
			PensamentoAlternativo = "Talvez ela estivesse cansada, não necessariamente brava comigo.",
			IntensidadeFinal = 4,
			CriadoEm = agora
		};

		await context.Usuarios.AddRangeAsync(adminUsuario, psicologoUsuario, pacienteUsuario);
		await context.Psicologos.AddAsync(psicologo);
		await context.Pacientes.AddAsync(paciente);
		await context.Atividades.AddAsync(atividade);
		await context.AtividadesPacientes.AddAsync(atividadePaciente);
		await context.CheckInsEmocionais.AddRangeAsync(checkins);
		await context.RegistrosPensamentos.AddAsync(registroPensamento);

		await context.SaveChangesAsync();
	}
}