using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSteps.Application.DTOs;

public class PsicologoCreateDto
{
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string? Telefone { get; set; }
	public string Senha { get; set; } = string.Empty;
	public string Crp { get; set; } = string.Empty;
	public string Documento { get; set; } = string.Empty;
	public string Plano { get; set; } = string.Empty;
	public string? Bio { get; set; }
}


public class PsicologoResponseDto
{
	public Guid Id { get; set; }
	public Guid UsuarioId { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string? Telefone { get; set; }
	public string Crp { get; set; } = string.Empty;
	public string Documento { get; set; } = string.Empty;
	public string? Plano { get; set; }
	public bool Pago { get; set; }
	public string? PaymentUrl { get; set; }
	public string? PixCopyPaste { get; set; }
	public string? Bio { get; set; }
	public string? FotoUrl { get; set; }
	public bool Aprovado { get; set; }
	public bool Ativo { get; set; }
}

public class PsicologoUpdateDto
{
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string? Telefone { get; set; }
	public string Crp { get; set; } = string.Empty;
	public string? Bio { get; set; }
	public string? Senha { get; set; }
}