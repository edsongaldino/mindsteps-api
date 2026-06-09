using Microsoft.EntityFrameworkCore;
using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using MindSteps.Infrastructure.Data;
using MindSteps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MindSteps.Infrastructure.Services;

public class DashboardService : IDashboardService
{
	private readonly ApplicationDbContext _context;

	public DashboardService(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<DashboardTerapeuticoDto> ObterDashboardPacienteAsync(Guid pacienteId)
	{
		var dto = new DashboardTerapeuticoDto();

		// 1. Obter Registros de Pensamentos
		var rpList = await _context.RegistrosPensamentos
			.Where(rp => rp.PacienteId == pacienteId)
			.ToListAsync();

		// 2. Obter Check-Ins Emocionais
		var checkins = await _context.CheckInsEmocionais
			.Where(c => c.PacienteId == pacienteId)
			.ToListAsync();

		// 3. Obter Jogos Registrados
		var jogos = await _context.RegistrosJogos
			.Where(j => j.PacienteId == pacienteId)
			.ToListAsync();

		// --- Métrica: Ansiedade ---
		int catastroficosCount = rpList.Count(rp => 
			(rp.PensamentoAutomatico != null && rp.PensamentoAutomatico.Contains("catas", StringComparison.OrdinalIgnoreCase)) ||
			(rp.Situacao != null && rp.Situacao.Contains("catas", StringComparison.OrdinalIgnoreCase)));

		double somaIntensidade = checkins.Sum(c => c.Intensidade);
		double intensidadeMedia = checkins.Any() ? (double)somaIntensidade / checkins.Count : 0.0;

		// --- Métrica: Autoestima (Crenças negativas) ---
		var crencasFrequentes = new Dictionary<string, int>();

		// --- Métrica: Habilidades Sociais (Semáforo Emocional) ---
		int assertivas = 0;
		int passivas = 0;
		int agressivas = 0;

		// --- Métrica: Exposição (Missão Coragem) ---
		int concluidos = 0;
		int recusados = 0;
		int adiados = 0;

		// --- Métrica: Gatilhos (Caçador de Gatilhos) ---
		var mapaGatilhos = new Dictionary<string, int>();
		var rankingGatilhos = new Dictionary<string, int>();

		// --- Métrica: Sabotadores ---
		var frequenciaSabotadores = new Dictionary<string, int>();

		// --- Métrica: Escape Room ---
		int salasDesbloqueadas = 0;
		var distorcoesIdentificadas = new Dictionary<string, int>();

		// --- Métrica: O Monstro da Ansiedade ---
		string monstroNome = "";
		string monstroCor = "";
		var medosMonstro = new List<string>();

		// --- Métrica: Ilha das Emoções ---
		var regioesVisitadas = new Dictionary<string, int>();

		foreach (var jogo in jogos)
		{
			try
			{
				using var doc = JsonDocument.Parse(jogo.DadosPlay);
				var root = doc.RootElement;

				switch (jogo.JogoId.ToLower())
				{
					case "detetive":
						if (root.TryGetProperty("pensamento", out var pensProp))
						{
							var p = pensProp.GetString();
							if (!string.IsNullOrEmpty(p))
							{
								crencasFrequentes[p] = crencasFrequentes.GetValueOrDefault(p) + 1;
							}
						}
						if (root.TryGetProperty("intensidade", out var intProp))
						{
							somaIntensidade += intProp.GetInt32();
						}
						break;

					case "tribunal":
						if (root.TryGetProperty("pensamento", out var pensTribProp))
						{
							var p = pensTribProp.GetString();
							if (!string.IsNullOrEmpty(p))
							{
								crencasFrequentes[p] = crencasFrequentes.GetValueOrDefault(p) + 1;
							}
						}
						break;

					case "gatilhos":
						if (root.TryGetProperty("gatilho", out var gatProp))
						{
							var g = gatProp.GetString();
							if (!string.IsNullOrEmpty(g))
							{
								mapaGatilhos[g] = mapaGatilhos.GetValueOrDefault(g) + 1;
							}
						}
						if (root.TryGetProperty("situacao", out var sitProp))
						{
							var s = sitProp.GetString();
							if (!string.IsNullOrEmpty(s))
							{
								rankingGatilhos[s] = rankingGatilhos.GetValueOrDefault(s) + 1;
							}
						}
						break;

					case "coragem":
						if (root.TryGetProperty("status", out var statProp))
						{
							var status = statProp.GetString()?.ToLower();
							if (status == "concluido") concluidos++;
							else if (status == "recusado") recusados++;
							else if (status == "adiado") adiados++;
						}
						break;

					case "monstro":
						if (root.TryGetProperty("nome", out var nomeProp)) monstroNome = nomeProp.GetString() ?? "";
						if (root.TryGetProperty("cor", out var corProp)) monstroCor = corProp.GetString() ?? "";
						if (root.TryGetProperty("medo", out var medoProp))
						{
							var m = medoProp.GetString();
							if (!string.IsNullOrEmpty(m)) medosMonstro.Add(m);
						}
						break;

					case "ilha":
						if (root.TryGetProperty("regiao", out var regProp))
						{
							var r = regProp.GetString();
							if (!string.IsNullOrEmpty(r))
							{
								regioesVisitadas[r] = regioesVisitadas.GetValueOrDefault(r) + 1;
							}
						}
						break;

					case "semaforo":
						if (root.TryGetProperty("classificacao", out var classProp))
						{
							var c = classProp.GetString()?.ToLower();
							if (c == "assertiva") assertivas++;
							else if (c == "passiva") passivas++;
							else if (c == "agressiva") agressivas++;
						}
						break;

					case "sabotadores":
						if (root.TryGetProperty("sabotador", out var sabProp))
						{
							var s = sabProp.GetString();
							if (!string.IsNullOrEmpty(s))
							{
								frequenciaSabotadores[s] = frequenciaSabotadores.GetValueOrDefault(s) + 1;
							}
						}
						break;

					case "escape":
						salasDesbloqueadas++;
						if (root.TryGetProperty("distorcao", out var distProp))
						{
							var d = distProp.GetString();
							if (!string.IsNullOrEmpty(d))
							{
								distorcoesIdentificadas[d] = distorcoesIdentificadas.GetValueOrDefault(d) + 1;
								if (d.Contains("catas", StringComparison.OrdinalIgnoreCase))
								{
									catastroficosCount++;
								}
							}
						}
						break;
				}
			}
			catch
			{
			}
		}

		dto.Ansiedade = new AnsiedadeMetricsDto
		{
			FrequenciaPensamentosCatastroficos = catastroficosCount,
			IntensidadeMedia = Math.Round(intensidadeMedia, 1)
		};

		dto.Autoestima = new AutoestimaMetricsDto
		{
			CrencasMaisEscolhidas = crencasFrequentes
				.Select(kv => new CrencaNegativaDto { Crenca = kv.Key, Quantidade = kv.Value })
				.OrderByDescending(c => c.Quantidade)
				.Take(5)
				.ToList()
		};

		dto.HabilidadesSociais = new HabilidadesSociaisMetricsDto
		{
			RespostasAssertivas = assertivas,
			RespostasPassivas = passivas,
			RespostasAgressivas = agressivas
		};

		int totalExposicao = concluidos + recusados + adiados;
		dto.Exposicao = new ExposicaoMetricsDto
		{
			DesafiosConcluidos = concluidos,
			DesafiosRecusados = recusados,
			DesafiosAdiados = adiados,
			TaxaDesistencia = totalExposicao > 0 ? Math.Round((double)recusados / totalExposicao * 100, 1) : 0.0
		};

		dto.Gatilhos = new GatilhosMetricsDto
		{
			MapaGatilhos = mapaGatilhos,
			RankingGatilhos = rankingGatilhos
				.Select(kv => new GatilhoRankingDto { Gatilho = kv.Key, Frequencia = kv.Value })
				.OrderByDescending(g => g.Frequencia)
				.Take(5)
				.ToList()
		};

		string sabotadorMaisFreq = frequenciaSabotadores.Any() 
			? frequenciaSabotadores.OrderByDescending(x => x.Value).First().Key 
			: "Nenhum";

		dto.Sabotadores = new SabotadoresMetricsDto
		{
			SabotadorMaisFrequente = sabotadorMaisFreq,
			FrequenciaSabotadores = frequenciaSabotadores
		};

		dto.EscapeRoom = new EscapeRoomMetricsDto
		{
			SalasDesbloqueadas = salasDesbloqueadas,
			DistorcoesIdentificadas = distorcoesIdentificadas
		};

		dto.Monstro = new MonstroAnsiedadeMetricsDto
		{
			Nome = monstroNome,
			Cor = monstroCor,
			MedosAparecem = medosMonstro.Distinct().ToList(),
			FrequenciaMedos = medosMonstro.Count
		};

		dto.RegioesIlhaMaisVisitadas = regioesVisitadas
			.OrderByDescending(x => x.Value)
			.Select(x => x.Key)
			.Take(3)
			.ToList();

		return dto;
	}
}
