using MindSteps.Domain.Entities;
using MindSteps.Domain.Interfaces;
using MindSteps.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MindSteps.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationDbContext _context;

    public UsuarioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Usuario>> ObterTodosAsync() =>
        await _context.Usuarios.ToListAsync();

    public async Task<Usuario> ObterPorIdAsync(Guid id) =>
        await _context.Usuarios.FindAsync(id);

    public async Task<Usuario> CriarAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<Usuario> AtualizarAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<bool> RemoverAsync(Guid id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return false;

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Usuario> ObterPorEmailSenhaAsync(string email, string senha) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);
}