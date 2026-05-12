using MindSteps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSteps.Application.DTOs;
public class CheckInEmocionalCreateDto
{
	public Guid PacienteId { get; set; }
	public Humor Humor { get; set; }
	public int Intensidade { get; set; }
	public string? EmocaoPrincipal { get; set; }
	public string? Observacao { get; set; }
}

public class CheckInEmocionalResponseDto
{
	public Guid Id { get; set; }
	public Guid PacienteId { get; set; }
	public Humor Humor { get; set; }
	public int Intensidade { get; set; }
	public string? EmocaoPrincipal { get; set; }
	public string? Observacao { get; set; }
	public DateTime CriadoEm { get; set; }
}