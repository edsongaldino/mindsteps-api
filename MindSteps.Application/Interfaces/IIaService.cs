using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindSteps.Application.Interfaces;

public interface IIaService
{
	Task<List<string>> GerarInsightsClinicosAsync(Guid pacienteId);
}
