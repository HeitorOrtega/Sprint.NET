using Microsoft.EntityFrameworkCore;
using Sprint.Data;
using Sprint.Helpers;
using Sprint.Models;

public class PatioService : IPatioService
{
    private readonly AppDbContext _db;
    public PatioService(AppDbContext db) => _db = db;

    public async Task<(IEnumerable<Patio> Items, int TotalCount)> GetAllAsync(QueryParameters parameters)
    {
        var query = _db.Patios.AsQueryable();
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(p => p.Id)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Patio?> GetByIdAsync(long id) => await _db.Patios.FindAsync(id);

    public async Task<Patio> CreateAsync(Patio p)
    {
        _db.Patios.Add(p);
        await _db.SaveChangesAsync();
        return p;
    }

    public async Task<Patio?> UpdateAsync(long id, Patio p)
    {
        var existing = await _db.Patios.FindAsync(id);
        if (existing == null) return null;
        existing.Localizacao = p.Localizacao;
        await _db.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existing = await _db.Patios.FindAsync(id);
        if (existing == null) return false;
        _db.Patios.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }
}