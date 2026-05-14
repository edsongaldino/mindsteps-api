using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSteps.Application.DTOs;

public class PacienteCreateDto
{
	public Guid PsicologoId { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Senha { get; set; } = string.Empty;
	public DateTime? DataNascimento { get; set; }
	public string? Genero { get; set; }
}

public class PacienteResponseDto
{
	public Guid Id { get; set; }
	public Guid UsuarioId { get; set; }
	public Guid PsicologoId { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public DateTime? DataNascimento { get; set; }
	public string? Genero { get; set; }
	public string? FotoUrl { get; set; }
	public bool Ativo { get; set; }
}

public class PacienteUpdateDto
{
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public DateTime? DataNascimento { get; set; }
	public string? Genero { get; set; }
}
