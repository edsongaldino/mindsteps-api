using System;
using System.Collections.Generic;

namespace MindSteps.Application.DTOs;

public class DashboardTerapeuticoDto
{
	public AnsiedadeMetricsDto Ansiedade { get; set; } = new();
	public AutoestimaMetricsDto Autoestima { get; set; } = new();
	public HabilidadesSociaisMetricsDto HabilidadesSociais { get; set; } = new();
	public ExposicaoMetricsDto Exposicao { get; set; } = new();
	public GatilhosMetricsDto Gatilhos { get; set; } = new();
	public SabotadoresMetricsDto Sabotadores { get; set; } = new();
	public EscapeRoomMetricsDto EscapeRoom { get; set; } = new();
	public MonstroAnsiedadeMetricsDto Monstro { get; set; } = new();
	public List<string> RegioesIlhaMaisVisitadas { get; set; } = new();
}

public class AnsiedadeMetricsDto
{
	public int FrequenciaPensamentosCatastroficos { get; set; }
	public double IntensidadeMedia { get; set; }
}

public class AutoestimaMetricsDto
{
	public List<CrencaNegativaDto> CrencasMaisEscolhidas { get; set; } = new();
}

public class CrencaNegativaDto
{
	public string Crenca { get; set; } = string.Empty;
	public int Quantidade { get; set; }
}

public class HabilidadesSociaisMetricsDto
{
	public int RespostasAssertivas { get; set; }
	public int RespostasPassivas { get; set; }
	public int RespostasAgressivas { get; set; }
}

public class ExposicaoMetricsDto
{
	public int DesafiosConcluidos { get; set; }
	public int DesafiosRecusados { get; set; }
	public int DesafiosAdiados { get; set; }
	public double TaxaDesistencia { get; set; }
}

public class GatilhosMetricsDto
{
	public Dictionary<string, int> MapaGatilhos { get; set; } = new(); // Categoria -> Quantidade
	public List<GatilhoRankingDto> RankingGatilhos { get; set; } = new();
}

public class GatilhoRankingDto
{
	public string Gatilho { get; set; } = string.Empty;
	public int Frequencia { get; set; }
}

public class SabotadoresMetricsDto
{
	public string SabotadorMaisFrequente { get; set; } = "Nenhum";
	public Dictionary<string, int> FrequenciaSabotadores { get; set; } = new();
}

public class EscapeRoomMetricsDto
{
	public int SalasDesbloqueadas { get; set; }
	public Dictionary<string, int> DistorcoesIdentificadas { get; set; } = new();
}

public class MonstroAnsiedadeMetricsDto
{
	public string Nome { get; set; } = string.Empty;
	public string Cor { get; set; } = string.Empty;
	public List<string> MedosAparecem { get; set; } = new();
	public int FrequenciaMedos { get; set; }
}
