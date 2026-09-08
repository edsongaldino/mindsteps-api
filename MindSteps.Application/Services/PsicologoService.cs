using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using MindSteps.Domain.Entities;
using MindSteps.Domain.Enums;
using MindSteps.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSteps.Application.Services;

public class PsicologoService : IPsicologoService
{
	private readonly IPsicologoRepository _psicologoRepository;
	private readonly IUsuarioRepository _usuarioRepository;
	private readonly IAsaasService _asaasService;

	public PsicologoService(
		IPsicologoRepository psicologoRepository,
		IUsuarioRepository usuarioRepository,
		IAsaasService asaasService)
	{
		_psicologoRepository = psicologoRepository;
		_usuarioRepository = usuarioRepository;
		_asaasService = asaasService;
	}

	public async Task<IEnumerable<PsicologoResponseDto>> ObterTodosAsync()
	{
		var psicologos = await _psicologoRepository.ObterTodosAsync();

		return psicologos.Select(x => new PsicologoResponseDto
		{
			Id = x.Id,
			UsuarioId = x.UsuarioId,
			Nome = x.Usuario.Nome,
			Email = x.Usuario.Email,
			Telefone = x.Usuario.Telefone,
			Crp = x.Crp,
			Documento = x.Documento,
			Plano = x.Plano,
			Pago = x.Pago,
			Bio = x.Bio,
			FotoUrl = x.FotoUrl,
			Aprovado = x.Aprovado,
			Ativo = x.Usuario.Ativo
		});
	}

	public async Task<IEnumerable<PsicologoResponseDto>> ObterPendentesAsync()
	{
		var psicologos = await _psicologoRepository.ObterPendentesAsync();

		return psicologos.Select(x => new PsicologoResponseDto
		{
			Id = x.Id,
			UsuarioId = x.UsuarioId,
			Nome = x.Usuario.Nome,
			Email = x.Usuario.Email,
			Telefone = x.Usuario.Telefone,
			Crp = x.Crp,
			Documento = x.Documento,
			Plano = x.Plano,
			Pago = x.Pago,
			Bio = x.Bio,
			FotoUrl = x.FotoUrl,
			Aprovado = x.Aprovado,
			Ativo = x.Usuario.Ativo
		});
	}

	public async Task<PsicologoResponseDto?> ObterPorIdAsync(Guid id)
	{
		var psicologo = await _psicologoRepository.ObterPorIdAsync(id);

		if (psicologo is null)
			return null;

		return new PsicologoResponseDto
		{
			Id = psicologo.Id,
			UsuarioId = psicologo.UsuarioId,
			Nome = psicologo.Usuario.Nome,
			Email = psicologo.Usuario.Email,
			Telefone = psicologo.Usuario.Telefone,
			Crp = psicologo.Crp,
			Documento = psicologo.Documento,
			Plano = psicologo.Plano,
			Pago = psicologo.Pago,
			Bio = psicologo.Bio,
			FotoUrl = psicologo.FotoUrl,
			Aprovado = psicologo.Aprovado,
			Ativo = psicologo.Usuario.Ativo
		};
	}

	public async Task<PsicologoResponseDto> CriarAsync(PsicologoCreateDto dto)
	{
		var emailExiste = await _usuarioRepository.ExisteEmailAsync(dto.Email);

		if (emailExiste)
			throw new Exception("Já existe um usuário com este e-mail.");

		var crpExiste = await _psicologoRepository.ExisteCrpAsync(dto.Crp);

		if (crpExiste)
			throw new Exception("Já existe um psicólogo cadastrado com este CRP.");

		// Determina o valor da assinatura baseada no plano selecionado
		double valorPlano = dto.Plano.ToLower() switch
		{
			"starter" => 39.90,
			"essencial" => 89.00,
			"profissional" => 149.00,
			"clinica" => 299.00,
			_ => 149.00
		};

		// Integração ASAAS: Criação de Cliente
		string asaasCustId;
		try
		{
			asaasCustId = await _asaasService.CreateCustomerAsync(dto.Nome, dto.Email, dto.Documento, dto.Telefone);
		}
		catch (Exception ex)
		{
			throw new Exception($"Falha ao registrar cliente no gateway de pagamentos: {ex.Message}");
		}

		// Integração ASAAS: Criação de Assinatura
		string subId;
		string paymentUrl;
		string pixCopyPaste;
		try
		{
			(subId, paymentUrl, pixCopyPaste) = await _asaasService.CreateSubscriptionAsync(asaasCustId, dto.Plano, valorPlano);
		}
		catch (Exception ex)
		{
			throw new Exception($"Falha ao criar assinatura no gateway de pagamentos: {ex.Message}");
		}

		var usuario = new Usuario
		{
			Nome = dto.Nome,
			Email = dto.Email.ToLower().Trim(),
			Telefone = dto.Telefone,
			SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
			Perfil = PerfilUsuario.Psicologo,
			Ativo = true,
			CriadoEm = DateTime.UtcNow
		};

		var psicologo = new Psicologo
		{
			Usuario = usuario,
			Crp = dto.Crp,
			Documento = dto.Documento,
			Plano = dto.Plano,
			AsaasCustomerId = asaasCustId,
			AsaasSubscriptionId = subId,
			Pago = false,
			Aprovado = false, // Apenas aprovado após o pagamento ou aprovação manual
			Bio = dto.Bio,
			CriadoEm = DateTime.UtcNow
		};

		await _psicologoRepository.AdicionarAsync(psicologo);
		await _psicologoRepository.SalvarAlteracoesAsync();

		return new PsicologoResponseDto
		{
			Id = psicologo.Id,
			UsuarioId = usuario.Id,
			Nome = usuario.Nome,
			Email = usuario.Email,
			Telefone = usuario.Telefone,
			Crp = psicologo.Crp,
			Documento = psicologo.Documento,
			Plano = psicologo.Plano,
			Pago = psicologo.Pago,
			PaymentUrl = paymentUrl,
			PixCopyPaste = pixCopyPaste,
			Bio = psicologo.Bio,
			FotoUrl = psicologo.FotoUrl,
			Aprovado = psicologo.Aprovado,
			Ativo = usuario.Ativo
		};
	}

	public async Task<bool> AprovarAsync(Guid id)
	{
		var psicologo = await _psicologoRepository.ObterPorIdAsync(id);

		if (psicologo is null)
			return false;

		psicologo.Aprovado = true;
		psicologo.AtualizadoEm = DateTime.UtcNow;

		await _psicologoRepository.SalvarAlteracoesAsync();

		return true;
	}

	public async Task<PsicologoResponseDto?> AtualizarAsync(Guid id, PsicologoUpdateDto dto)
	{
		var psicologo = await _psicologoRepository.ObterPorIdAsync(id);

		if (psicologo is null)
			return null;

		psicologo.Usuario.Nome = dto.Nome;
		psicologo.Usuario.Email = dto.Email.ToLower().Trim();
		psicologo.Usuario.Telefone = dto.Telefone;
		psicologo.Crp = dto.Crp;
		psicologo.Bio = dto.Bio;
		
		if (!string.IsNullOrWhiteSpace(dto.Senha))
		{
			psicologo.Usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);
		}

		psicologo.AtualizadoEm = DateTime.UtcNow;
		psicologo.Usuario.AtualizadoEm = DateTime.UtcNow;

		await _psicologoRepository.SalvarAlteracoesAsync();

		return new PsicologoResponseDto
		{
			Id = psicologo.Id,
			UsuarioId = psicologo.UsuarioId,
			Nome = psicologo.Usuario.Nome,
			Email = psicologo.Usuario.Email,
			Telefone = psicologo.Usuario.Telefone,
			Crp = psicologo.Crp,
			Documento = psicologo.Documento,
			Plano = psicologo.Plano,
			Pago = psicologo.Pago,
			Bio = psicologo.Bio,
			FotoUrl = psicologo.FotoUrl,
			Aprovado = psicologo.Aprovado,
			Ativo = psicologo.Usuario.Ativo
		};
	}

	public async Task<bool> ReprovarAsync(Guid id)
	{
		var psicologo = await _psicologoRepository.ObterPorIdAsync(id);

		if (psicologo is null)
			return false;

		psicologo.Aprovado = false;
		psicologo.Usuario.Ativo = false;
		psicologo.AtualizadoEm = DateTime.UtcNow;
		psicologo.Usuario.AtualizadoEm = DateTime.UtcNow;

		await _psicologoRepository.SalvarAlteracoesAsync();

		return true;
	}

	public async Task<bool> AtualizarStatusPagamentoAsync(string subscriptionId, bool pago)
	{
		var psicologo = await _psicologoRepository.ObterPorSubscriptionIdAsync(subscriptionId);

		if (psicologo is null)
			return false;

		psicologo.Pago = pago;
		if (pago)
		{
			psicologo.Aprovado = true; // Auto-aprova ao confirmar o pagamento
		}
		psicologo.AtualizadoEm = DateTime.UtcNow;

		await _psicologoRepository.SalvarAlteracoesAsync();
		return true;
	}
}