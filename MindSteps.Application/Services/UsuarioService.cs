using MindSteps.Application.DTOs;
using MindSteps.Application.Interfaces;
using MindSteps.Domain.Entities;
using MindSteps.Domain.Interfaces;
using MindSteps.SharedKernel.Security;

namespace MindSteps.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repo;
    private readonly ITokenService _tokenService;

    public UsuarioService(IUsuarioRepository repo, ITokenService tokenService)
    {
        _repo = repo;
        _tokenService = tokenService;
    }

    public async Task<IEnumerable<UsuarioDto>> ObterTodosAsync() =>
        (await _repo.ObterTodosAsync()).Select(u => new UsuarioDto {
            Id = u.Id, Nome = u.Nome, Email = u.Email, Telefone = u.Telefone
        });

    public async Task<UsuarioDto> ObterPorIdAsync(Guid id)
    {
        var u = await _repo.ObterPorIdAsync(id);
        return u == null ? null : new UsuarioDto { Id = u.Id, Nome = u.Nome, Email = u.Email, Telefone = u.Telefone };
    }

    public async Task<UsuarioDto> CriarAsync(UsuarioDto dto)
    {
        var u = new Usuario {
            Id = Guid.NewGuid(),
            Nome = dto.Nome,
            Email = dto.Email,
            Telefone = dto.Telefone,
            Senha = "123456" // Idealmente usar hash e vir da API
        };
        await _repo.CriarAsync(u);
        dto.Id = u.Id;
        return dto;
    }

    public async Task<UsuarioDto> AtualizarAsync(Guid id, UsuarioDto dto)
    {
        var u = await _repo.ObterPorIdAsync(id);
        if (u == null) return null;

        u.Nome = dto.Nome;
        u.Email = dto.Email;
        u.Telefone = dto.Telefone;
        await _repo.AtualizarAsync(u);
        return dto;
    }

    public async Task<bool> RemoverAsync(Guid id) => await _repo.RemoverAsync(id);

    public async Task<string> AutenticarAsync(LoginDto login)
    {
        var usuario = await _repo.ObterPorEmailSenhaAsync(login.Email, login.Senha);
        return usuario != null ? _tokenService.GerarToken(usuario) : null;
    }
}