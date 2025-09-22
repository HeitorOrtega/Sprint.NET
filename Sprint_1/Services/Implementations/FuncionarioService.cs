using Microsoft.EntityFrameworkCore;
using Sprint_1.Data;
using Sprint_1.Helpers;
using Sprint_1.Models;

public class FuncionarioService : IFuncionarioService
{
    private readonly AppDbContext _db;
    public FuncionarioService(AppDbContext db) => _db = db;

    public async Task<(IEnumerable<Funcionario> Items, int TotalCount)> GetAllAsync(QueryParameters parameters)
    {
        var query = _db.Funcionarios.AsQueryable();
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(f => f.Id)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Funcionario?> GetByIdAsync(long id) => await _db.Funcionarios.FindAsync(id);

    public async Task<Funcionario> CreateAsync(Funcionario f)
    {
        _db.Funcionarios.Add(f);
        await _db.SaveChangesAsync();
        return f;
    }

    public async Task<Funcionario?> UpdateAsync(long id, Funcionario f)
    {
        var existing = await _db.Funcionarios.FindAsync(id);
        if (existing == null) return null;

        existing.Nome = f.Nome;
        existing.Cpf = f.Cpf;
        existing.Email = f.Email;
        existing.Rg = f.Rg;
        existing.Telefone = f.Telefone;
        existing.PatioId = f.PatioId;

        await _db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existing = await _db.Funcionarios.FindAsync(id);
        if (existing == null) return false;
        _db.Funcionarios.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }
}