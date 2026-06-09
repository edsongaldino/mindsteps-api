using MindSteps.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace MindSteps.Application.Interfaces;

public interface IDashboardService
{
	Task<DashboardTerapeuticoDto> ObterDashboardPacienteAsync(Guid pacienteId);
}
