using Microsoft.EntityFrameworkCore;
using Sprint.Data;
using Sprint.Helpers;
using Sprint.Models;

public class MotoService : IMotoService
{
    private readonly AppDbContext _db;
    public MotoService(AppDbContext db) => _db = db;

    public async Task<(IEnumerable<Moto> Items, int TotalCount)> GetAllAsync(QueryParameters parameters)
    {
        var query = _db.Motos.AsQueryable();
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(m => m.Id)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Moto?> GetByIdAsync(long id) => await _db.Motos.FindAsync(id);

    public async Task<Moto> CreateAsync(Moto m)
    {
        _db.Motos.Add(m);
        await _db.SaveChangesAsync();
        return m;
    }

    public async Task<Moto?> UpdateAsync(long id, Moto m)
    {
        var existing = await _db.Motos.FindAsync(id);
        if (existing == null) return null;

        existing.Cor = m.Cor;
        existing.Placa = m.Placa;
        existing.DataFabricacao = m.DataFabricacao;

        await _db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existing = await _db.Motos.FindAsync(id);
        if (existing == null) return false;
        _db.Motos.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }
}